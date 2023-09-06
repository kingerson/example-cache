namespace MS.IConstruye.Application.Command
{
    public class CreateOrderCommand : CommandBase<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
