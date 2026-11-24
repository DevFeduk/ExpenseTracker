namespace ExpenseList
{
    public class Expense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; } = "";
        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
