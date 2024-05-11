using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practice_01.PaymentRepository;
using practice_01.Resources;
using Stripe;
using Stripe.Checkout;
using Practice_01.Data;
using Microsoft.EntityFrameworkCore;
using Practice_01.ViewModel;
using Practice_01.Repository;
using Practice_01.Models;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly IStripeRepository _stripeService;
        private readonly ApplicationDbContext _context;
        private readonly IBookingRepository _Bookingrepository;
        public StripeController(IStripeRepository stripeService, ApplicationDbContext context, IBookingRepository Bookingrepository)
        {
            _stripeService = stripeService;
            _stripeService = stripeService ?? throw new ArgumentNullException(nameof(stripeService));
            _context= context;
            _Bookingrepository= Bookingrepository;
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


        //    [HttpPost("session")]
        //    public async Task<IActionResult> CreateCheckoutSession(string UserId,Guid eventId,DateTime eventDate)
        //    {
        //        try
        //        {
        //            //var booking=await _context.Bookings.FirstOrDefaultAsync(b=>b.UserId == UserId && b.EventId==eventId);
        //            //var payment=await _context.Bookings.Where(x=>x.UserId==UserId && x.EventId==eventId && x.EventDate==eventDate).
        //            //    Select(x=>x.Payment).FirstOrDefaultAsync();
        //            //if (payment == null)
        //            //{
        //            //    return NotFound("Booking Not Found");
        //            //}
        //            var options = new Stripe.Checkout.SessionCreateOptions
        //            {
        //                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        //{
        //    new Stripe.Checkout.SessionLineItemOptions
        //    {
        //        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
        //        {
        //            //UnitAmount = (10*100),
        //            //UnitAmount=(46000*100),
        //            UnitAmount=(long)(payment * 100),
        //            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
        //            {
        //                Name = "Event Booking Payment",
        //            },
        //            Currency = "inr",
        //        },
        //        Quantity = 1,
        //    },
        //},
        //                TaxIdCollection = new Stripe.Checkout.SessionTaxIdCollectionOptions
        //                {
        //                    Enabled = true,
        //                },
        //                Mode = "payment",
        //                SuccessUrl = "https://example.com/success",
        //                CancelUrl = "https://example.com/cancel",
        //            };
        //            var service = new SessionService();

        //            var session = service.Create(options);

        //            return Ok(new { checkoutUrl = session.Url });

        //        }
        //        catch (StripeException e)
        //        {
        //            return BadRequest(e.Message);
        //        }
        //    }


            [HttpPost("session")]
            public async Task<IActionResult> CreateCheckoutSession(BookingModel bookingModel)
            {
            try
            {
                // Add booking to the database
                var isBookingAdded = await _Bookingrepository.AddBookingAsync(bookingModel);

                // Check if booking was added successfully
                if (isBookingAdded)
                {
                    // Retrieve the booking object from the repository
                    var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.UserId == bookingModel.UserId && b.EventId == bookingModel.EventId && b.EventDate == bookingModel.EventDate);

                    if (booking != null)
                    {
                        // Create the checkout session options using the booking details
                        var options = new Stripe.Checkout.SessionCreateOptions
                        {
                            LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
                    {
                        new Stripe.Checkout.SessionLineItemOptions
                        {
                            PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(booking.Payment * 100), // Convert payment to smallest currency unit
                                ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Event Booking Payment",
                                },
                                Currency = "inr",
                            },
                            Quantity = 1,
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

                        // Create the checkout session
                        var service = new SessionService();
                        var session = service.Create(options);

                        // Return the checkout URL
                        return Ok(new { checkoutUrl = session.Url });
                    }
                    else
                    {
                        // Booking not found in the database
                        return NotFound("Booking not found");
                    }
                }
                else
                {
                    // Failed to add booking to the database
                    return BadRequest("Failed to add booking");
                }
            }
            catch (StripeException e)
            {
                // Handle Stripe exceptions
                return BadRequest(e.Message);
            }
        }


        const string endpointSecret = "whsec_55a8af68ceddd8b1d85b90ef13df513bf7a8c22e4b3dd5637c1712daafb4ac53";
        [HttpPost("webhook")]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    // Retrieve booking ID from session metadata or other relevant information
                    var bookingId = session.Metadata["bookingId"]; // Adjust this according to your metadata structure

                    // Retrieve booking based on booking ID
                    var booking = await _context.Bookings.FindAsync(bookingId);

                    if (booking != null)
                    {
                        // Update booking payment status to 'approved'
                        booking.PaymentStatus = "approved";
                        await _context.SaveChangesAsync();

                        // Return a success response
                        return Ok();
                    }
                    else
                    {
                        // Booking not found in the database
                        return NotFound("Booking not found");
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                // Handle Stripe exceptions
                return BadRequest(e.Message);
            }
        }
    }
}
