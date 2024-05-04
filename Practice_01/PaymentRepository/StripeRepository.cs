using practice_01.Resources;
using Practice_01.Data;
using Stripe;
using Stripe.Checkout;

namespace Practice_01.PaymentRepository
{
    public class StripeRepository : IStripeRepository
    {
        private readonly TokenService _tokenService;
        private readonly CustomerService _customerService;
        private readonly ChargeService _chargeService;
        private readonly ApplicationDbContext _context;
        //private readonly string _stripeApiKey;
        //private string? stripeApiKey;

        public StripeRepository(TokenService tokenService, CustomerService customerService, ChargeService chargeService, ApplicationDbContext context)
        {
            _tokenService = tokenService;
            _customerService = customerService;
            _chargeService = chargeService;
            _context = context;
            //this.stripeApiKey = stripeApiKey;
            //this.stripeApiKey = stripeApiKey;
        }

        //public StripeRepository(string stripeApiKey,TokenService tokenService, CustomerService customerService, ChargeService chargeService, ApplicationDbContext context)
        //{
        //    _stripeApiKey = stripeApiKey;
        //    StripeConfiguration.ApiKey = _stripeApiKey;
        //    _tokenService = tokenService;
        //    _customerService = customerService;
        //    _chargeService = chargeService;
        //    _context = context;

        //}

        //public StripeRepository(IStripeRepository stripeService, string? stripeApiKey)
        //{
        //    //this.stripeService = stripeService;
        //    this.stripeApiKey = stripeApiKey;
        //}

        public async Task<ChargeResource> Createcharge(CreateChargeResource resource, CancellationToken cancellationToken)
        {
            var ChargeOptions = new ChargeCreateOptions
            {
                Currency = resource.Currency,
                Amount = resource.Amount,
                //ReceiptEmail = resource.ReceiptEmail,
                Customer = resource.CustomerId,
                Description = resource.Description,
            };
            var charge = await _chargeService.CreateAsync(ChargeOptions, null, cancellationToken);

            Console.WriteLine(charge);

            return new ChargeResource(
            charge.Id,
            charge.Currency,
            charge.Amount,
            charge.CustomerId,
            charge.ReceiptEmail,
            charge.Description);
            //throw new NotImplementedException();
        }

        public async Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken)
        {
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = resource.Card.Name,
                    Number = resource.Card.Number,
                    ExpYear = resource.Card.ExpiryYear,
                    ExpMonth = resource.Card.ExpiryMonth,
                    Cvc = resource.Card.Cvc
                }
            };
            var token = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);

            var customerOptions = new CustomerCreateOptions
            {
                Email = resource.Email,
                Name = resource.Name,
                Source = token.Id
            };
            var customer = await _customerService.CreateAsync(customerOptions, null, cancellationToken);

            return new CustomerResource(customer.Id, customer.Email, customer.Name);
            //throw new NotImplementedException();
        }

        public async Task<CustomerResource> FindCustomerByEmail(string email, CancellationToken cancellationToken)
        {
            var customerService = new CustomerService();
            var options = new CustomerListOptions
            {
                Email = email,
                Limit = 1
            };
            var customers = await customerService.ListAsync(options, null, cancellationToken);
            if (customers.Data.Count == 0)
            {
                return null;
            }
            var stripeCustomer = customers.Data[0];
            return new CustomerResource(stripeCustomer.Id, stripeCustomer.Email, stripeCustomer.Name);
        }

        public async Task<List<CustomerResource>> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerService.ListAsync(null, null, cancellationToken);

            var customerResources = new List<CustomerResource>();

            foreach (var customer in customers)
            {
                customerResources.Add(new CustomerResource(customer.Id, customer.Email, customer.Name));
            }

            return customerResources;
        }


        //one api customer and payment
        public async Task<(ChargeResource, CustomerResource)> CreateCustomerAndCharge(CreateCustomerResource customerResource, CreateChargeResource chargeResource, CancellationToken cancellationToken)
        {
            // Step 1: Create Token
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = customerResource.Card.Name,
                    Number = customerResource.Card.Number,
                    ExpYear = customerResource.Card.ExpiryYear,
                    ExpMonth = customerResource.Card.ExpiryMonth,
                    Cvc = customerResource.Card.Cvc
                }
            };
            var token = await _tokenService.CreateAsync(tokenOptions, null, cancellationToken);

            // Step 2: Create Customer
            var customerOptions = new CustomerCreateOptions
            {
                Email = customerResource.Email,
                Name = customerResource.Name,
                Source = token.Id
            };
            var customer = await _customerService.CreateAsync(customerOptions, null, cancellationToken);

            // Step 3: Create Charge
            var chargeOptions = new ChargeCreateOptions
            {
                Currency = chargeResource.Currency,
                Amount = chargeResource.Amount,
                //ReceiptEmail = chargeResource.ReceiptEmail,
                Customer = customer.Id,
                //Description = chargeResource.Description,
            };
            var charge = await _chargeService.CreateAsync(chargeOptions, null, cancellationToken);

            // Step 4: Return both resources
            var chargeResourceResult = new ChargeResource(
                charge.Id,
                charge.Currency,
                charge.Amount,
                charge.CustomerId,
                charge.ReceiptEmail,
                charge.Description
                );

            var customerResourceResult = new CustomerResource(customer.Id, customer.Email, customer.Name);

            return (chargeResourceResult, customerResourceResult);
        }

        public async Task<string> CreateCheckoutSession()
        {
            var test = StripeConfiguration.ApiKey;
            var options = new SessionCreateOptions
            {
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel", // Optional
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                        Quantity = 2,
                    },
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Id;
            //throw new NotImplementedException();
        }
    }
}
