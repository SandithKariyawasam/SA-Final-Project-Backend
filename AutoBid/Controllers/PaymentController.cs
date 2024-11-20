using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoBid.Models;
using AutoBid.Services;
using System.Threading.Tasks;
using AutoBid.Data;

namespace AutoBid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints in this controller
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly DataContext _context; // Inject the DataContext

        public PaymentController(IPaymentService paymentService, DataContext context)
        {
            _paymentService = paymentService;
            _context = context; // Initialize DataContext
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("Invalid payment amount.");
            }

            try
            {
                var clientSecret = await _paymentService.CreatePaymentIntent(request.Amount, request.Currency);
                return Ok(new { clientSecret });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating payment intent: {ex.Message}");
            }
        }

        [HttpPost("save-payment")]
        public async Task<IActionResult> SavePayment([FromBody] payments payment)
        {
            if (payment == null || payment.Amount <= 0)
            {
                return BadRequest("Invalid payment details.");
            }

            try
            {
                // Set the payment as paid
                payment.IsPaid = true;

                // Add the payment record to the database
                await _context.Payments.AddAsync(payment); // Assuming Payments is a DbSet<payments> in DataContext
                await _context.SaveChangesAsync(); // Save changes to the database

                return Ok(new { message = "Payment saved successfully.", paymentId = payment.PaymentId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving payment: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}


