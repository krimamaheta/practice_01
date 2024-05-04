namespace practice_01.Resources
{
    public record CreateCustomerResource(
    string Email,
    string Name,
    CreateCardResource Card);
}
