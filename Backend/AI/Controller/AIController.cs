using Microsoft.AspNetCore.Mvc;
using ProductManager.API.AI.ChurnPrediction.Interfaces;

namespace ProductManager.API.AI.Controller
{
    [ApiController]
    [Route("api/ai")]
    public class AIController : ControllerBase
    {
        private readonly IChurnService _churnService;
        private readonly IChurnTrainingService _trainingService;

        public AIController(
            IChurnService churnService,
            IChurnTrainingService trainingService)
        {
            _churnService = churnService;
            _trainingService = trainingService;
        }

        [HttpPost("churn/train")]
        public async Task<IActionResult> Train()
        {
            await _trainingService.TrainModelAsync();
            return Ok("Model trained successfully");
        }

        [HttpGet("churn/{clientId}")]
        public async Task<IActionResult> GetChurn(int clientId)
        {
            var result = await _churnService.PredictAsync(clientId);
            return Ok(result);
        }
    }
}
