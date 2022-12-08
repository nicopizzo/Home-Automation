using Garage.Persitance;
using Garage.Persitance.Interfaces;
using Home.Core.Gpio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Garage.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GarageController : ControllerBase
    {
        private readonly IGarageRepo _GarageRepo;
        private readonly ILogger<GarageController> _Logger;

        public GarageController(IGarageRepo repo, ILogger<GarageController> logger)
        {
            _GarageRepo = repo;
            _Logger = logger;
        }

        [HttpPut("toggleGarage")]
        public async Task<IActionResult> toggleGarage()
        {
            _Logger.Log(LogLevel.Information, "Toggle Start...");
            await _GarageRepo.ToggleGarage();
            _Logger.Log(LogLevel.Information, "Toggle Finished...");
            return Ok();
        }

        [HttpGet("getGarageStatus")]
        public GarageStatus getGarageStatus()
        {
            _Logger.Log(LogLevel.Information, "getGarageStatus Start...");
            return _GarageRepo.GetGarageStatus();
        }

        [HttpGet("getPinDetails/{pin}")]
        public PinDetails getPinDetails(int pin)
        {
            _Logger.Log(LogLevel.Information, "GetPinDetails Start...");
            return _GarageRepo.GetPinDetails(pin);
        }

        [HttpPost("setPinDetails")]
        public ActionResult setPinDetails(PinDetails details)
        {
            _Logger.Log(LogLevel.Information, "SetPinDetails Start...");
            _GarageRepo.SetPinDetails(details);
            return Ok();
        }
    }
}