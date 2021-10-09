using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nielsen.TechTest.Q1.Common;

namespace Nielsen.TechTest.Q1.RobotHosting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RobotController : ControllerBase
    {
        private readonly ILogger<RobotController> _logger;
        private readonly ISimpleAsyncRobot<Location, MoveInstruction> _robot;

        public RobotController(ILogger<RobotController> logger,
            ISimpleAsyncRobot<Location, MoveInstruction> robot)
        {
            this._logger = logger;
            this._robot = robot;
        }

        [HttpGet]
        public async Task<Location> Get()
        {
            var curLoc = await this._robot.GetCurrentLocationAsync();
            this._logger.LogInformation($"Returning current location ({curLoc.PositionX}, {curLoc.PositionY})");
            return curLoc;
        }

        [HttpPut]
        public async Task<Location> Put([FromBody] MoveInstruction movingInstruction)
        {
            try
            {
                // actually if  null will never hit
                if (movingInstruction == null)
                {
                    throw new ArgumentNullException("Moving Instruction is empty");
                }
                var updatedLocation = await this._robot.MoveAsync(movingInstruction);
                this._logger.LogInformation($"Requesting to move by {movingInstruction.NoOfStep} ({movingInstruction.Direction})");
                this._logger.LogInformation($"Robot moved to location, ({updatedLocation.PositionX}, {updatedLocation.PositionY})");
                return updatedLocation;
            }
            catch (Exception err)
            {
                this._logger.LogError(err, "Failed to move robot", null);
                throw; // throw as it is. Startup will handle this
            }
        }
    }
}
