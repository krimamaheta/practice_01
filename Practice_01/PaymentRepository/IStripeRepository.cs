using practice_01.Resources;

namespace Practice_01.PaymentRepository
{
    public interface IStripeRepository
    {

        Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken);
        Task<ChargeResource> Createcharge(CreateChargeResource resource, CancellationToken cancellationToken);
        Task<List<CustomerResource>> GetAll(CancellationToken cancellationToken);

        Task<CustomerResource> FindCustomerByEmail(string email, CancellationToken cancellationToken);


        //one api
        Task<(ChargeResource, CustomerResource)> CreateCustomerAndCharge(CreateCustomerResource customerResource, CreateChargeResource chargeResource, CancellationToken cancellationToken);


        Task<string> CreateCheckoutSession();
    
    }
}
