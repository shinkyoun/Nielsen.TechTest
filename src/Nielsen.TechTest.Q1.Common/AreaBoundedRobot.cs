using System;
using System.Collections.Generic;
using System.Text;

namespace Nielsen.TechTest.Q1.Common
{
    public class AreaBoundedRobot<TLocationType, TMoveInstructionType> : DefaultRobot<TLocationType, TMoveInstructionType>
        where TLocationType : Location, new()
        where TMoveInstructionType : MoveInstruction
    {
        private readonly BoundingArea _area = new BoundingArea()
        {
            MinPositionX = 0,
            MaxPositionX = 25,
            MinPositionY = 0,
            MaxPositionY = 25
        };

        public AreaBoundedRobot(BoundingArea area) : base()
        {
            if (area != null)
            {
                this._area.MinPositionX = area.MinPositionX;
                this._area.MaxPositionX = area.MaxPositionX;
                this._area.MinPositionY = area.MinPositionY;
                this._area.MaxPositionY = area.MaxPositionY;
            }
        }

        protected override bool CanMove(TMoveInstructionType moveIntruction)
        {
            var fromBase = base.CanMove(moveIntruction);
            if (fromBase == false)
            {
                return false;
            }

            bool canMoveByLocation = false;
            int currentLoc = 0;
            string reason = String.Empty;

            lock (this.SyncLock)
            {
                var curLoc = this.CurrentLocation;

                switch (moveIntruction.Direction)
                {
                    case MoveDirection.Right:
                        currentLoc = curLoc.PositionX;
                        currentLoc += moveIntruction.NoOfStep;
                        canMoveByLocation = (currentLoc >= this._area.MinPositionX && currentLoc <= this._area.MaxPositionX);
                        reason = $"Cannot move horizontally. Moving point, {currentLoc} is out of boundary between {this._area.MinPositionX} and {this._area.MaxPositionX}";
                        break;
                    case MoveDirection.Left:
                        currentLoc = curLoc.PositionX;
                        currentLoc -= moveIntruction.NoOfStep;
                        canMoveByLocation = (currentLoc >= this._area.MinPositionX && currentLoc <= this._area.MaxPositionX);
                        reason = $"Cannot move horizontally. Moving point, {currentLoc} is out of boundary between {this._area.MinPositionX} and {this._area.MaxPositionX}";
                        break;
                    case MoveDirection.Up:
                        currentLoc = curLoc.PositionY;
                        currentLoc += moveIntruction.NoOfStep;
                        canMoveByLocation = (currentLoc >= this._area.MinPositionY && currentLoc <= this._area.MaxPositionY);
                        reason = $"Cannot move vertically. Moving point, {currentLoc} is out of boundary between {this._area.MinPositionY} and {this._area.MaxPositionY}";
                        break;
                    case MoveDirection.Down:
                        currentLoc = curLoc.PositionY;
                        currentLoc -= moveIntruction.NoOfStep;
                        canMoveByLocation = (currentLoc >= this._area.MinPositionY && currentLoc <= this._area.MaxPositionY);
                        reason = $"Cannot move vertically. Moving point, {currentLoc} is out of boundary between {this._area.MinPositionY} and {this._area.MaxPositionY}";
                        break;
                }
            }

            if (canMoveByLocation == false)
            {
                throw new CannotMoveException(reason);
            }

            return true;
        }
    }
}
