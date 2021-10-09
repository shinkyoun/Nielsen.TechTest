using System;

namespace Nielsen.TechTest.Q1.Common
{
    public class Location : BaseObject
    {
        private int _posX = 0;
        private int _posY = 0;

        public Location()
        {
        }

        public virtual int PositionX
        {
            get {  return this._posX; }
            set 
            {
                if (this._posX != value)
                {
                    this._posX = value;
                    this.NotifyPropertyChanged(() => this.PositionX);
                }
            } 
        }

        public virtual int PositionY
        {
            get { return this._posY; }
            set
            {
                if (this._posY != value)
                {
                    this._posY = value;
                    this.NotifyPropertyChanged(() => this.PositionY);
                }
            }
        }
    }
}
