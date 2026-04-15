using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HistoryService.Data;
using HistoryService.Models;

namespace HistoryService.Controllers
{
    [ApiController]
    [Route("api/v1/history")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryRepository historyRepository;

        public HistoryController(IHistoryRepository historyRepository)
        {
            this.historyRepository = historyRepository;
        }

        // DTO received from Quantity Service
        public class SaveOperationDto
        {
            public int? UserId { get; set; }
            public string Category { get; set; } = string.Empty;
            public string OperationType { get; set; } = string.Empty;
            public double FirstValue { get; set; }
            public string FirstUnit { get; set; } = string.Empty;
            public double? SecondValue { get; set; }
            public string? SecondUnit { get; set; }
            public double? ResultValue { get; set; }
            public string? ResultUnit { get; set; }
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("HistoryService is running.");

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyHistory()
        {
            int? userId = GetUserId();
            if (!userId.HasValue) return Unauthorized();

            var history = await historyRepository.GetByUserIdAsync(userId.Value);
            return Ok(history);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetUserHistory(int userId)
        {
            var history = await historyRepository.GetByUserIdAsync(userId);
            return Ok(history);
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveOperation([FromBody] SaveOperationDto dto)
        {
            if (dto == null)
                return BadRequest("Operation is required.");

            if (!Enum.TryParse<OperationCategory>(dto.Category, true, out var category))
                return BadRequest($"Invalid category: {dto.Category}");

            var operation = new OperationHistory
            {
                UserId = dto.UserId,
                Category = category,
                OperationType = dto.OperationType,
                FirstValue = dto.FirstValue,
                FirstUnit = dto.FirstUnit,
                SecondValue = dto.SecondValue,
                SecondUnit = dto.SecondUnit,
                ResultValue = dto.ResultValue,
                ResultUnit = dto.ResultUnit
            };

            await historyRepository.SaveAsync(operation);
            return Ok("Saved successfully.");
        }

        private int? GetUserId()
        {
            var sub = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User?.FindFirst("sub")?.Value;
            return int.TryParse(sub, out int id) ? id : null;
        }
    }
}