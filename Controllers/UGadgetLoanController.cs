using Microsoft.AspNetCore.Mvc;
using LoanManagementSystem.Models;
using LoanManagementSystem.ViewModel;
using LoanManagementSystem.Data;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LoanManagementSystem.Controllers
{
    [Authorize]
    public class UGadgetLoanController : Controller
    {
        private readonly ILogger<UGadgetLoanController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public UGadgetLoanController(ApplicationDbContext dbContext, ILogger<UGadgetLoanController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Purchase()
        {
            return View();
        }


        [Route("UGadgetLoan/ConfirmPurchase")]
        public ActionResult ConfirmPurchase([FromQuery] int gadgetId)
        {
            var gadgetLoan = _dbContext.gadgetloans.FirstOrDefault(gl => gl.Id == gadgetId);
            var imp = _dbContext.imps.FirstOrDefault(imp => imp.Id == gadgetId);
            var paymentTerms = _dbContext.imps.Select(imp => imp.PaymentTerm).Distinct().ToList();

            if (gadgetLoan == null)
            {
                return NotFound();
            }
            var model = new PurchaseViewModel
            {
                GadgetLoanId = gadgetId,
                GadgetName = gadgetLoan.GadgetName,
                Description = gadgetLoan.Description,
                Price = gadgetLoan.Price,
                Interest = imp.Interest,
                PaymentTerm = imp.PaymentTerm,
                Payment = CalculatePayment(gadgetLoan.Price, (decimal)imp.Interest, imp.PaymentTerm),
                AvailablePaymentTerms = paymentTerms
            };

            return View(model);
        }

        private decimal CalculatePayment(decimal price, decimal interest, int paymentTerm)
        {
            decimal monthlyInterest = interest / 100 / paymentTerm;
            decimal payment = (price * monthlyInterest) / (1 - (decimal)Math.Pow(1 + (double)monthlyInterest, -paymentTerm));

            return Math.Round(payment, 2);
        }

    }
}
