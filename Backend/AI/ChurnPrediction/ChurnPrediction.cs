using Microsoft.ML.Data;

namespace ProductManager.API.AI.ChurnPrediction
{
    public class ChurnPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool WillChurn { get; set; }

        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
