namespace practice_01.Resources
{
    public record CreateChargeResource(
    string Currency,
    long Amount,
    string CustomerId,
    //string? ReceiptEmail,
    string Description
    );
}
