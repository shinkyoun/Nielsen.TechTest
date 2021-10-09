using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nielsen.TechTest.Q1.Common
{
    public class DefaultRobot<TLocationType, TMoveInstructionType> : ISimpleRobot<TLocationType, TMoveInstructionType>,
        ISimpleAsyncRobot<TLocationType, TMoveInstructionType>
        where TLocationType : Location, new()
        where TMoveInstructionType : MoveInstruction
    {
        private readonly TLocationType _currentLocation = new TLocationType();
        private readonly object _syncLock = new object();

        public DefaultRobot()
        {
        }

        protected object SyncLock
        {
            get {  return this._syncLock; }
        }

        protected TLocationType CurrentLocation
        {
            get { return this._currentLocation; }
        }

        public TLocationType GetCurrentLocation()
        {
            lock (this.SyncLock)
            {
                TLocationType rtn = new TLocationType();
                rtn.PositionX = this._currentLocation.PositionX;
                rtn.PositionY = this._currentLocation.PositionY;
                return rtn;
            }
        }
        public Task<TLocationType> GetCurrentLocationAsync()
        {
            TLocationType rtn = this.GetCurrentLocation();
            return Task<TLocationType>.FromResult(rtn);
        }

        protected virtual bool CanMove(TMoveInstructionType moveIntruction)
        {
            return (moveIntruction != null &&
                moveIntruction.Direction != MoveDirection.Stop &&
                moveIntruction.NoOfStep != 0);
        }

        public virtual TLocationType Move(TMoveInstructionType moveIntruction)
        {
            var needMove = this.CanMove(moveIntruction);
            if (needMove == false)
            {
                return this._currentLocation;
            }

            lock (this.SyncLock)
            {
                switch (moveIntruction.Direction)
                {
                    case MoveDirection.Right:
                        this._currentLocation.PositionX += moveIntruction.NoOfStep;
                        break;
                    case MoveDirection.Left:
                        this._currentLocation.PositionX -= moveIntruction.NoOfStep;
                        break;
                    case MoveDirection.Up:
                        this._currentLocation.PositionY += moveIntruction.NoOfStep;
                        break;
                    case MoveDirection.Down:
                        this._currentLocation.PositionY -= moveIntruction.NoOfStep;
                        break;
                }
            }

            return this.GetCurrentLocation();
        }
        public Task<TLocationType> MoveAsync(TMoveInstructionType moveIntruction)
        {
            TLocationType rtn = this.Move(moveIntruction);
            return Task<TLocationType>.FromResult(rtn);
        }
    }
}
