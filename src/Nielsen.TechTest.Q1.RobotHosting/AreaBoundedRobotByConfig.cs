using System;
using Microsoft.Extensions.Options;
using Nielsen.TechTest.Q1.Common;

namespace Nielsen.TechTest.Q1.RobotHosting
{
    public class AreaBoundedRobotByConfig : AreaBoundedRobot<Location, MoveInstruction>
    {
        public AreaBoundedRobotByConfig(IOptions<BoundingArea> areaFromConfig) : base(areaFromConfig.Value)
        {
        }
    }
}
