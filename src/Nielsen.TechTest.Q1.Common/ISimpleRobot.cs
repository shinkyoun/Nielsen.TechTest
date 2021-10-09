using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nielsen.TechTest.Q1.Common
{
    public interface ISimpleRobot<TLocationType,TMoveInstructionType>
        where TLocationType : Location, new()
        where TMoveInstructionType : MoveInstruction
    {
        TLocationType GetCurrentLocation();
        TLocationType Move(TMoveInstructionType moveIntruction);
    }

    public interface ISimpleAsyncRobot<TLocationType, TMoveInstructionType>
        where TLocationType : Location, new()
        where TMoveInstructionType : MoveInstruction
    {
        Task<TLocationType> GetCurrentLocationAsync();
        Task<TLocationType> MoveAsync(TMoveInstructionType moveIntruction);
    }
}
