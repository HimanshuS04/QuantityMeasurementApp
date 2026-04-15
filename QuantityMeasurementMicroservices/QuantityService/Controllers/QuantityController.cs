using System.Net.Http.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityService.BusinessLogic;
using QuantityService.Data;
using QuantityService.Exceptions;
using QuantityService.Models;

namespace QuantityService.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    public class QuantityController : ControllerBase
    {
        private readonly IQuantityMeasurementService quantityService;
        private readonly IQuantityOperationRepository operationRepository;
        private readonly IHttpClientFactory httpClientFactory;

        public QuantityController(
            IQuantityMeasurementService quantityService,
            IQuantityOperationRepository operationRepository,
            IHttpClientFactory httpClientFactory)
        {
            this.quantityService = quantityService;
            this.operationRepository = operationRepository;
            this.httpClientFactory = httpClientFactory;
        }

        public class CompareRequest
        {
            public QuantityDto First { get; set; } = new();
            public QuantityDto Second { get; set; } = new();
        }

        public class ConvertRequest
        {
            public QuantityDto Quantity { get; set; } = new();
            public string TargetUnit { get; set; } = string.Empty;
        }

        public class ArithmeticRequest
        {
            public QuantityDto First { get; set; } = new();
            public QuantityDto Second { get; set; } = new();
            public string ResultUnit { get; set; } = string.Empty;
        }

        public class DivideRequest
        {
            public QuantityDto First { get; set; } = new();
            public QuantityDto Second { get; set; } = new();
        }

        // DTO sent to History Service
        public class HistoryOperationDto
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

        [AllowAnonymous]
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("QuantityService is running.");

        [AllowAnonymous]
        [HttpPost("compare")]
        public async Task<IActionResult> Compare([FromBody] CompareRequest request)
        {
            try
            {
                bool equal = quantityService.CompareQuantities(request.First, request.Second);
                await SaveOperation("COMPARE", request.First, request.Second, equal ? 1.0 : 0.0, null);
                return Ok(new { equal });
            }
            catch (QuantityMeasurementException ex) { return BadRequest(ex.Message); }
        }

        [AllowAnonymous]
        [HttpPost("convert")]
        public async Task<IActionResult> Convert([FromBody] ConvertRequest request)
        {
            try
            {
                var result = quantityService.ConvertQuantity(request.Quantity, request.TargetUnit);
                await SaveOperation("CONVERT", request.Quantity, null, result.Value, result.Unit);
                return Ok(new { result });
            }
            catch (QuantityMeasurementException ex) { return BadRequest(ex.Message); }
        }

        [AllowAnonymous]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = quantityService.AddQuantities(request.First, request.Second, request.ResultUnit);
                await SaveOperation("ADD", request.First, request.Second, result.Value, result.Unit);
                return Ok(new { result });
            }
            catch (QuantityMeasurementException ex) { return BadRequest(ex.Message); }
        }

        [AllowAnonymous]
        [HttpPost("subtract")]
        public async Task<IActionResult> Subtract([FromBody] ArithmeticRequest request)
        {
            try
            {
                var result = quantityService.SubtractQuantities(request.First, request.Second, request.ResultUnit);
                await SaveOperation("SUBTRACT", request.First, request.Second, result.Value, result.Unit);
                return Ok(new { result });
            }
            catch (QuantityMeasurementException ex) { return BadRequest(ex.Message); }
        }

        [AllowAnonymous]
        [HttpPost("divide")]
        public async Task<IActionResult> Divide([FromBody] DivideRequest request)
        {
            try
            {
                double ratio = quantityService.DivideQuantities(request.First, request.Second);
                await SaveOperation("DIVIDE", request.First, request.Second, ratio, null);
                return Ok(new { ratio });
            }
            catch (QuantityMeasurementException ex) { return BadRequest(ex.Message); }
        }

        private int? GetUserId()
        {
            var sub = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User?.FindFirst("sub")?.Value;
            return int.TryParse(sub, out int id) ? id : null;
        }

        private async Task SaveOperation(
            string opType,
            QuantityDto first,
            QuantityDto? second,
            double? resultValue,
            string? resultUnit)
        {
            var op = new QuantityOperation
            {
                UserId = GetUserId(),
                Category = first.Category,
                OperationType = opType,
                FirstValue = first.Value,
                FirstUnit = first.Unit,
                SecondValue = second?.Value,
                SecondUnit = second?.Unit,
                ResultValue = resultValue,
                ResultUnit = resultUnit
            };

            await operationRepository.SaveAsync(op);

             // Send to History Service
try
{
    var historyDto = new HistoryOperationDto
    {
        UserId = op.UserId,
        Category = first.Category.ToString(),
        OperationType = opType,
        FirstValue = first.Value,
        FirstUnit = first.Unit,
        SecondValue = second?.Value,
        SecondUnit = second?.Unit,
        ResultValue = resultValue,
        ResultUnit = resultUnit
    };

    var client = httpClientFactory.CreateClient("HistoryService");
    
    Console.WriteLine($"Sending to History Service: {System.Text.Json.JsonSerializer.Serialize(historyDto)}");
    
    var response = await client.PostAsJsonAsync("/api/v1/history/save", historyDto);
    
    Console.WriteLine($"History Service response: {response.StatusCode}");
    
    string responseBody = await response.Content.ReadAsStringAsync();
    Console.WriteLine($"History Service response body: {responseBody}");
}
catch (Exception ex)
{
    Console.WriteLine($"Exception calling History Service: {ex.GetType().Name}: {ex.Message}");
}
        }
    }
}