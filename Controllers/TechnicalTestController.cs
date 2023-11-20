using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnicalTestAPI.Models;

namespace TechnicalTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TechnicalTestController : ControllerBase
    {        
        private readonly ILogger<TechnicalTestController> _logger;

        private TechnicalTestContext _context;

        public TechnicalTestController(ILogger<TechnicalTestController> logger, TechnicalTestContext context)
        {
            _logger = logger;
            _context = context;
        }

        // define a function to add a new debtor. 
        // parameter: string debtorName. 
        // parameter: string countryCode. 
        // returns Debtor Id if succesful. 
        // returns error message if unsuccessful. 
        [HttpPost("AddDebtor")]
        public async Task<ActionResult<string>> AddDebtor([FromBody] Debtor debtorToAdd)
        {
            try
            {
                // check against debtorName if already exists. 
                var debtor = _context.Debtors.FirstOrDefault(d => d.Name == debtorToAdd.Name);
                if (debtor != null)
                {
                    throw new Exception("This debtor has already been added"); 
                }

                // add the debtor to context. 
                await _context.Debtors.AddAsync(debtorToAdd);
                await _context.SaveChangesAsync();
                return Ok(debtorToAdd.Id.ToString());

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex); 
            }
        }

        [HttpPut("UpdateDebtor")]
        public async Task<ActionResult<Debtor>> UpdateDebtor([FromBody] Debtor debtorToUpdate)
        {
            try
            {
                // check against debtorName if it exists. 
                var debtor = _context.Debtors.FirstOrDefault(d => d.Name == debtorToUpdate.Name);
                if (debtor == null)
                {
                    throw new Exception("This debtor doesn't exist");
                }

                var updatedDebtor = new Debtor()
                {
                    Id = debtor.Id,
                    CountryCode = debtorToUpdate.CountryCode,
                    Address1 = debtorToUpdate.Address1,
                    Address2 = debtorToUpdate.Address2,
                    Town = debtorToUpdate.Town,
                    State = debtorToUpdate.State,
                    ZipCode = debtorToUpdate.ZipCode,
                    RegistrationNumber = debtorToUpdate.RegistrationNumber
                }; 

                _context.Entry<Debtor>(debtor).CurrentValues.SetValues(updatedDebtor);
                await _context.SaveChangesAsync();
                var actualUpdatedDebtor = _context.Debtors.Where(d => d.Id == updatedDebtor.Id).FirstOrDefault();
                return Ok(actualUpdatedDebtor);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        // define a function to store receivables. 
        // if debtorReference not found, create one and store it in Debtor table before storing receivable. 
        // should return the receivable ID if successful.
        // should return the error message if unsuccessful. 
        [HttpPost("AddReceivables")]
        public async Task<ActionResult<List<Receivable>>> AddReceivables([FromBody] List<Receivable> receivables)
        {
            try
            {
                List<Receivable> successfulReceivables = new List<Receivable>();
                // loop through each receivable. 
                foreach(Receivable r in receivables)
                {                    
                    // check against the debtor reference/ID. 
                    var debtor = _context.Debtors.FirstOrDefault(d => d.Id.ToString() == r.DebtorId && d.Name == r.DebtorName && d.CountryCode == r.DebtorCountryCode); 
                    if (debtor == null)
                    {
                        // create new debtor first. 
                        debtor = new Debtor()
                        {
                            Id = Guid.NewGuid(),
                            Name = r.DebtorName,
                            CountryCode = r.DebtorCountryCode
                        };

                        // add new debtor to context. 
                        await _context.Debtors.AddAsync(debtor); 
                        await _context.SaveChangesAsync();
                    }

                    // add new receivable. 
                    await _context.Receivables.AddAsync(r); 
                    await _context.SaveChangesAsync();

                    // if successful, add to list of receivables successfully added to context. 
                    successfulReceivables.Add(r);
                }
                return Ok(successfulReceivables); 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex); 
            }
        }

        // define a function to update a specific receivable. 
        // parameter: Receivable object. 
        // return string message (list of values updated OR any error message). 
        [HttpPut("UpdateReceivables")]
        public async Task<ActionResult<Receivable>> UpdateReceivable([FromBody] Receivable receivableToUpdate)
        {
            try
            {
                // check against receivable ID if it exists. 
                var receivable = await _context.Receivables.Where(r => r.Id == receivableToUpdate.Id).FirstOrDefaultAsync();
                if (receivable == null)
                {
                    throw new Exception("This receivable doesn't exist");
                }

                var updatedReceivable = new Receivable()
                {                    
                    CurrencyCode = receivableToUpdate.CurrencyCode,
                    PaidValue = receivableToUpdate.PaidValue,

                    // other fields cannot be modified after initial add to context.
                    // refer to original receivable data. 

                    Id = receivable.Id,
                    IssueDate = receivable.IssueDate,
                    OpeningValue = receivable.OpeningValue,
                    DueDate = receivable.DueDate,
                    DebtorId = receivable.DebtorId,
                    DebtorName = receivable.DebtorName,
                    DebtorCountryCode = receivable.DebtorCountryCode
                };

                updatedReceivable.IsCancelled = receivable.IsCancelled == true ? true : receivableToUpdate.IsCancelled;
                updatedReceivable.ClosedDate = updatedReceivable.IsCancelled ? DateTime.UtcNow.ToString() : receivable.ClosedDate;

                _context.Entry<Receivable>(receivable).CurrentValues.SetValues(updatedReceivable);
                await _context.SaveChangesAsync();
                return Ok(updatedReceivable);
            }

            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }

        // define a function get all receivables for individual debtor. 
        // parameter: debtor ID/reference. 
        // return list of all receivables for individual debtor. 
        [HttpGet("GetAllReceivables")]
        public List<Receivable> GetAllReceivables(Guid debtorReference)
        {
            return _context.Receivables.Where(r => r.DebtorId == debtorReference.ToString()).ToList();
        }

        // define a function get all open receivables for individual debtor. 
        // parameter: debtor ID/reference. 
        // return list of open receivables for individual debtor. 
        [HttpGet("GetOpenReceivables")]
        public List<Receivable> GetOpenReceivables(Guid debtorReference)
        {
            return _context.Receivables.Where(r => r.DebtorId == debtorReference.ToString() && r.ClosedDate == null).ToList();
        }

        // define a function get all receivables for individual debtor. 
        // parameter: debtor ID/reference. 
        // return list of closed receivables for individual debtor. 
        [HttpGet("GetClosedReceivables")]
        public List<Receivable> GetClosedReceivables(Guid debtorReference)
        {
            return _context.Receivables.Where(r => r.DebtorId == debtorReference.ToString() && r.ClosedDate != null).ToList();
        }


        // define a function to provide statistics per debtor. 
        // statistics such as:
        // open invoices (count, total value, and imminent due date) 
        // closed invoices (count, total value and percentage of cancelled invoices vs closed due to complete payment)
        // parameter: debtor ID/reference. 
        // need to define a separate model. 
        // a model called ReceivableStatistics. 
        // field 0: Guid DebtorId. 
        // field 1: int? OpenInvoiceCount. 
        // field 2: double? OpenInvoiceTotalValue. 
        // field 3: List<Receivable>? OverdueReceivables. 
        // field 4: List<Receivable>? ImminentReceivables. --> due within 1 month. 
        // field 5: int? ClosedInvoiceCount. 
        // field 6: double? ClosedInvoiceTotalValue. 
        // field 7: double? CancelledInvoicePercentage. 
        // field 8: double? CompletedInvoicePercentage. 
        [HttpGet("GetReceivableStatistics")]
        public async Task<ActionResult<ReceivableStatistics>> GetReceivableStatistics(Guid debtorReference)
        {
            try
            {
                var result = new ReceivableStatistics(); 
                // check for debtorReference if it exists. 
                var debtor = _context.Debtors.FirstOrDefault(d => d.Id == debtorReference);
                if (debtor == null)
                {
                    throw new Exception("No debtor found with specified ID"); 
                }

                result.DebtorId = debtor.Id; 
                // get all receivables. 
                var openReceivables = _context.Receivables.Where(r => r.DebtorId == debtor.Id.ToString() && r.ClosedDate == null).ToList();
                var closedReceivables = _context.Receivables.Where(r => r.DebtorId == debtor.Id.ToString() && r.ClosedDate != null).ToList();

                // process open receivables. 
                result.OpenInvoiceCount = openReceivables.Count;
                result.ImminentReceivables = new List<Receivable>();
                result.OverdueReceivables = new List<Receivable>(); 
                foreach(Receivable r in openReceivables)
                {
                    result.OpenInvoiceTotalValue += Convert.ToDouble(r.OpeningValue); 

                    if (DateTime.Parse(r.DueDate) < DateTime.Now)
                    {
                        result.OverdueReceivables.Add(r);
                    }
                    
                    if (Math.Abs((DateTime.Parse(r.DueDate) - DateTime.UtcNow).TotalDays) <= 30)
                    {
                        result.ImminentReceivables.Add(r); 
                    }
                }

                // process closed receivables. 
                result.ClosedInvoiceCount = closedReceivables.Count();
                result.CancelledInvoicePercentage = closedReceivables.Where(cr => cr.IsCancelled == true).Count() / result.ClosedInvoiceCount;
                result.CompletedInvoicePercentage = closedReceivables.Where(cr => cr.IsCancelled == false).Count() / result.ClosedInvoiceCount;
                foreach (Receivable r in closedReceivables)
                {
                    result.ClosedInvoiceTotalValue += Convert.ToDouble(r.PaidValue);
                }

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex); 
            }
            
        }

    }
}