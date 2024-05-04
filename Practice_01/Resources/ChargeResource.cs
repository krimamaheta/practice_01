namespace practice_01.Resources
{
    public record ChargeResource(
    string ChargeId,
    string Currency,
    long Amount,
   
    string? CustomerId,
    string ReceiptEmail,
    string Description
    );
}
