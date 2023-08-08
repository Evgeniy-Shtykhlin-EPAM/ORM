namespace TaskLibrary.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int StatusId { get; set; }
        public int ProductId { get; set; }
    }
}
