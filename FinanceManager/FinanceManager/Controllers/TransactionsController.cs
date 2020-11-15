using FinanceManager.Data;
using FinanceManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TransactionsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            var transactionsList = _context.Transactions.Include(c => c.Category);

            return await transactionsList.ToListAsync();
        }

        // GET: api/Transactions/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.Include(c => c.Category).FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // GET: api/Transactions/report/Date=mm-dd-yyy
        [HttpGet("report/Date={startdate}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetDailyReport(DateTime startdate)
        {
            var transactionsList = _context.Transactions.Include(c => c.Category);

            var output = transactionsList.Where(d => d.ExpenseDate == startdate);

            return await output.ToListAsync();
        }

        // GET: api/Transactions/report/StartDate=mm-dd-yyy&EndDate=mm-dd-yyy
        [HttpGet("report/StartDate={startdate}&EndDate={enddate}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetMonthReport(DateTime startdate, DateTime enddate)
        {
            var transactionsList = _context.Transactions.Include(c => c.Category);

            var output = transactionsList.Where(d => d.ExpenseDate >= startdate && d.ExpenseDate <= enddate);

            return await output.ToListAsync();
        }

        // PUT: api/Transactions/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return transaction;
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
