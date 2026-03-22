using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp;

namespace QuantityMeasurementApi.Controllers
{
    [ApiController]
    [Route("api/v1/quantities")]
    public class QuantityApiController : ControllerBase
    {
        private readonly IQuantityMeasurementService quantityService;
        private readonly IQuantityOperationRepository operationRepository;

        public QuantityApiController(
            IQuantityMeasurementService quantityService,
            IQuantityOperationRepository operationRepository)
        {
            this.quantityService = quantityService ?? throw new ArgumentNullException(nameof(quantityService));
            this.operationRepository = operationRepository ?? throw new ArgumentNullException(nameof(operationRepository));
        }

        // ===== Request/Response DTOs =====

        public class CompareRequest
        {
            public QuantityDto First { get; set; } = new QuantityDto();
            public QuantityDto Second { get; set; } = new QuantityDto();
        }

        public class CompareResponse
        {
            public bool Equal { get; set; }
        }

        public class ConvertRequest
        {
            public QuantityDto Quantity { get; set; } = new QuantityDto();
            public string TargetUnit { get; set; } = string.Empty;
        }

        public class ConvertResponse
        {
            public QuantityDto Result { get; set; } = new QuantityDto();
        }

        public class AddRequest
        {
            public QuantityDto First { get; set; } = new QuantityDto();
            public QuantityDto Second { get; set; } = new QuantityDto();
            public string ResultUnit { get; set; } = string.Empty;
        }

        public class AddResponse
        {
            public QuantityDto Result { get; set; } = new QuantityDto();
        }

        public class SubtractRequest
        {
            public QuantityDto First { get; set; } = new QuantityDto();
            public QuantityDto Second { get; set; } = new QuantityDto();
            public string ResultUnit { get; set; } = string.Empty;
        }

        public class SubtractResponse
        {
            public QuantityDto Result { get; set; } = new QuantityDto();
        }

        public class DivideRequest
        {
            public QuantityDto First { get; set; } = new QuantityDto();
            public QuantityDto Second { get; set; } = new QuantityDto();
        }

        public class DivideResponse
        {
            public double Ratio { get; set; }
        }

        // ===== Endpoints =====

    
        [HttpPost("compare")]
        public async Task<ActionResult<CompareResponse>> Compare([FromBody] CompareRequest request)
        {
            if (request == null || request.First == null || request.Second == null)
            {
                return BadRequest("Invalid request payload.");
            }

            bool equal;
            try
            {
                equal = quantityService.CompareQuantities(request.First, request.Second);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }

            // Map bool to numeric: 1.0 = true, 0.0 = false
            double resultNumeric = equal ? 1.0 : 0.0;

            var op = new QuantityOperation
            {
                Category = request.First.Category,  // assume same category
                OperationType = "COMPARE",
                FirstValue = request.First.Value,
                FirstUnit = request.First.Unit,
                SecondValue = request.Second.Value,
                SecondUnit = request.Second.Unit,
                ResultValue = resultNumeric,
                ResultUnit = null  // no unit for a boolean
            };

            await operationRepository.SaveAsync(op);

            return Ok(new CompareResponse { Equal = equal });
        }

        [HttpPost("convert")]
        public async Task<ActionResult<ConvertResponse>> Convert([FromBody] ConvertRequest request)
        {
            if (request == null || request.Quantity == null || string.IsNullOrWhiteSpace(request.TargetUnit))
            {
                return BadRequest("Invalid request payload.");
            }

            QuantityDto resultDto;
            try
            {
                resultDto = quantityService.ConvertQuantity(request.Quantity, request.TargetUnit);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }

            var op = new QuantityOperation
            {
                Category = request.Quantity.Category,
                OperationType = "CONVERT",
                FirstValue = request.Quantity.Value,
                FirstUnit = request.Quantity.Unit,
                SecondValue = null,
                SecondUnit = null,
                ResultValue = resultDto.Value,
                ResultUnit = resultDto.Unit
            };

            await operationRepository.SaveAsync(op);

            return Ok(new ConvertResponse
            {
                Result = resultDto
            });
        }

        [HttpPost("add")]
        public async Task<ActionResult<AddResponse>> Add([FromBody] AddRequest request)
        {
            if (request == null || request.First == null || request.Second == null || string.IsNullOrWhiteSpace(request.ResultUnit))
            {
                return BadRequest("Invalid request payload.");
            }

            QuantityDto resultDto;
            try
            {
                resultDto = quantityService.AddQuantities(request.First, request.Second, request.ResultUnit);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }

            var op = new QuantityOperation
            {
                Category = request.First.Category,
                OperationType = "ADD",
                FirstValue = request.First.Value,
                FirstUnit = request.First.Unit,
                SecondValue = request.Second.Value,
                SecondUnit = request.Second.Unit,
                ResultValue = resultDto.Value,
                ResultUnit = resultDto.Unit
            };

            await operationRepository.SaveAsync(op);

            return Ok(new AddResponse
            {
                Result = resultDto
            });
        }

        [HttpPost("subtract")]
        public async Task<ActionResult<SubtractResponse>> Subtract([FromBody] SubtractRequest request)
        {
            if (request == null || request.First == null || request.Second == null || string.IsNullOrWhiteSpace(request.ResultUnit))
            {
                return BadRequest("Invalid request payload.");
            }

            QuantityDto resultDto;
            try
            {
                resultDto = quantityService.SubtractQuantities(request.First, request.Second, request.ResultUnit);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }

            var op = new QuantityOperation
            {
                Category = request.First.Category,
                OperationType = "SUBTRACT",
                FirstValue = request.First.Value,
                FirstUnit = request.First.Unit,
                SecondValue = request.Second.Value,
                SecondUnit = request.Second.Unit,
                ResultValue = resultDto.Value,
                ResultUnit = resultDto.Unit
            };

            await operationRepository.SaveAsync(op);

            return Ok(new SubtractResponse
            {
                Result = resultDto
            });
        }

        [HttpPost("divide")]
        public async Task<ActionResult<DivideResponse>> Divide([FromBody] DivideRequest request)
        {
            if (request == null || request.First == null || request.Second == null)
            {
                return BadRequest("Invalid request payload.");
            }

            double ratio;
            try
            {
                ratio = quantityService.DivideQuantities(request.First, request.Second);
            }
            catch (QuantityMeasurementException ex)
            {
                return BadRequest(ex.Message);
            }

            var op = new QuantityOperation
            {
                Category = request.First.Category,
                OperationType = "DIVIDE",
                FirstValue = request.First.Value,
                FirstUnit = request.First.Unit,
                SecondValue = request.Second.Value,
                SecondUnit = request.Second.Unit,
                ResultValue = ratio,
                ResultUnit = null // ratio has no unit
            };

            await operationRepository.SaveAsync(op);

            return Ok(new DivideResponse
            {
                Ratio = ratio
            });
        }

        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            return Ok("QuantityMeasurementApi is running.");
        }
    }
}