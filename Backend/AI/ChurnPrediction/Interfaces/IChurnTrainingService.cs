using System.Threading.Tasks;

namespace ProductManager.API.AI.ChurnPrediction.Interfaces
{
    public interface IChurnTrainingService
    {
        Task TrainModelAsync();
    }
}
