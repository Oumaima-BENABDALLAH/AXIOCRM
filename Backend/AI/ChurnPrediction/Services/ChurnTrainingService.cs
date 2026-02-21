using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using ProductManager.API.AI.ChurnPrediction.Interfaces;
using ProductManager.API.Data;

namespace ProductManager.API.AI.ChurnPrediction.Services
{
    public class ChurnTrainingService : IChurnTrainingService
    {
        private readonly AppDbContext _context;
        private readonly ChurnFeatureBuilder _featureBuilder;
        private readonly MLContext _mlContext;

        public ChurnTrainingService(
            AppDbContext context,
            ChurnFeatureBuilder featureBuilder)
        {
            _context = context;
            _featureBuilder = featureBuilder;
            _mlContext = new MLContext();
        }

        public async Task TrainModelAsync()
        {
            var clients = await _context.Clients.ToListAsync();

            var data = new List<ChurnInputModel>();

            foreach (var client in clients)
            {
                var features = _featureBuilder.Build(client.Id);

                // Pour V1 fake label logique
                features.Label = features.DaysSinceLastOrder > 120;

                data.Add(features);
            }

            var trainingData = _mlContext.Data.LoadFromEnumerable(data);

            var pipeline = _mlContext.Transforms
                .Concatenate("Features",
                    nameof(ChurnInputModel.TotalOrders),
                    nameof(ChurnInputModel.TotalSpent),
                    nameof(ChurnInputModel.DaysSinceLastOrder),
                    nameof(ChurnInputModel.LatePayments))
                .Append(_mlContext.BinaryClassification
                    .Trainers.FastTree());

            var model = pipeline.Fit(trainingData);

            var modelPath = Path.Combine(Directory.GetCurrentDirectory(),"AI", "Churn","churnModel.zip");
            Directory.CreateDirectory(Path.GetDirectoryName(modelPath)!);
            _mlContext.Model.Save(model, trainingData.Schema, modelPath);
        }
    }
}
