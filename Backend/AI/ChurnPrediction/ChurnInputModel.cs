namespace ProductManager.API.AI.ChurnPrediction
{
    public class ChurnInputModel
    {
        public float TotalOrders { get; set; }
        public float TotalSpent { get; set; }
        public float DaysSinceLastOrder { get; set; }
        public float LatePayments { get; set; }
        public bool Label { get; set; }
    }
}
