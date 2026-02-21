using Microsoft.ML;
using ProductManager.API.AI.ChurnPrediction.Interfaces;

namespace ProductManager.API.AI.ChurnPrediction.Services
{
    public class ChurnService : IChurnService
        {
            private readonly ChurnFeatureBuilder _featureBuilder;
            private readonly MLContext _mlContext;
            private readonly IWebHostEnvironment _env;
            private ITransformer? _model;

            public ChurnService(
                ChurnFeatureBuilder featureBuilder,
                IWebHostEnvironment env)
            {
                _featureBuilder = featureBuilder;
                _env = env;
                _mlContext = new MLContext();
            }

            private void EnsureModelLoaded()
            {
                if (_model != null)
                    return;

                var modelPath = Path.Combine(
                    _env.ContentRootPath,
                    "AI",
                    "Churn",
                    "churnModel.zip"
                );

                if (!File.Exists(modelPath))
                    throw new InvalidOperationException(
                        "Le modèle n’est pas encore entraîné. Appelez /api/ai/churn/train d’abord."
                    );

                using var stream = File.OpenRead(modelPath);
                _model = _mlContext.Model.Load(stream, out _);
            }

            public Task<ChurnPrediction> PredictAsync(int clientId)
            {
                EnsureModelLoaded();

                var features = _featureBuilder.Build(clientId);

                var engine = _mlContext.Model
                    .CreatePredictionEngine<ChurnInputModel, ChurnPrediction>(_model!);

                return Task.FromResult(engine.Predict(features));
            }
        }
    
  }
