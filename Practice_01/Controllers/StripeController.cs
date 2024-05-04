using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_01.PaymentRepository;
using practice_01.Resources;
using Stripe;
using Stripe.Checkout;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripeRepository _stripeService;
        public StripeController(IStripeRepository stripeService)
        {
            _stripeService = stripeService;
            _stripeService = stripeService ?? throw new ArgumentNullException(nameof(stripeService));
        }
        [HttpPost("customer")]
        public async Task<ActionResult<CustomerResource>> CreateCustomer([FromBody] CreateCustomerResource resource, CancellationToken cancellationToken)
        {
            var response = await _stripeService.CreateCustomer(resource, cancellationToken);
            return Ok(response);
        }
        [HttpPost("charge")]
        public async Task<ActionResult<ChargeResource>> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken)
        {
            var response = await _stripeService.Createcharge(resource, cancellationToken);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _stripeService.GetAll(HttpContext.RequestAborted);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> FindCustomerByEmail(string email)
        {
            try
            {
                var customer = await _stripeService.FindCustomerByEmail(email, HttpContext.RequestAborted);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }


        //new method
        //[HttpPost("create-customer-and-charge")]
        //public async Task<IActionResult> CreateCustomerAndCharge([FromBody] CreateCustomerAndChargeRequest request, CancellationToken cancellationToken)
        //{
        //    var (chargeResource, customerResource) = await _stripeService.CreateCustomerAndCharge(request.Customer, request.Charge, cancellationToken);

        //    return Ok(new
        //    {
        //        Charge = chargeResource,
        //        Customer = customerResource
        //    });
        //}
        [HttpPost("create-customer-and-charge")]
        public async Task<IActionResult> CreateCustomerAndCharge([FromBody] CreateCustomerAndChargeRequest request, CancellationToken cancellationToken)
        {
            var (chargeResource, customerResource) = await _stripeService.CreateCustomerAndCharge(request.Customer, request.Charge, cancellationToken);

            return Ok(new
            {
                Message = "Payment successfully done.",
                Charge = chargeResource,
                Customer = customerResource
            });
        }






        //[HttpPost]
        //public async Task<IActionResult> CreateCheckoutSession()
        //{
        //    try
        //    {
        //        var sessionId = await _stripeService.CreateCheckoutSession();
        //        return Ok(new { sessionId });
        //    }
        //    catch (StripeException e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
        //  StripeConfiguration.ApiKey = "sk_test_tR3PYbcVNZZ796tH88S4VQ2u";
        [HttpPost("session")]
        public IActionResult CreateCheckoutSession()
        {
            try
            {
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
    {
        new Stripe.Checkout.SessionLineItemOptions
        {
            PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
            {
                UnitAmount = (10*100),
                ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                {
                    Name = "T-shirt",
                },
                Currency = "inr",
            },
            Quantity = 2,
        },
    },
                    TaxIdCollection = new Stripe.Checkout.SessionTaxIdCollectionOptions
                    {
                        Enabled = true,
                    },
                    Mode = "payment",
                    SuccessUrl = "https://example.com/success",
                    CancelUrl = "https://example.com/cancel",
                };
                var service = new SessionService();

                var session = service.Create(options);

                return Ok(new { checkoutUrl = session.Url });
               
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
