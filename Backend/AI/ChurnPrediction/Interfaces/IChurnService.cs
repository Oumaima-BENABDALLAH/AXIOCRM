namespace ProductManager.API.AI.ChurnPrediction.Interfaces
{
    public interface IChurnService
    {
        Task<ChurnPrediction> PredictAsync(int clientId);
    }
}
