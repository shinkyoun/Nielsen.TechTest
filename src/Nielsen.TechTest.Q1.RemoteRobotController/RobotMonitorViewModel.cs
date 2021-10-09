using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Mvvm.Input;

using Nielsen.TechTest.Q1.Common;
using System.Collections.ObjectModel;

namespace Nielsen.TechTest.Q1.RemoteRobotController
{
    public class RobotMonitorViewModel : BaseObject
    {
        private readonly ILogger _logger;
        private readonly ISimpleAsyncRobot<Location, MoveInstruction> _robot;
        private readonly Location _robotLocation = new Location();
        private readonly Location _robotScreenLocation = new Location();
        private readonly RectangleSize _robotSize = new RectangleSize()
        {
            Width = 10,
            Height = 10
        };
        private readonly RectangleSize _screenSize = new RectangleSize()
        {
            Width = 10,
            Height = 10
        };

        public RobotMonitorViewModel(ILogger logger,
            ISimpleAsyncRobot<Location, MoveInstruction> robot) : base()
        {
            this._logger = logger;
            this._robot = robot;
            this.ListOfMovingSteps = new ObservableCollection<int>()
            {
                0, 1, 2, 3
            };
            this.NoOfStepToMove = 2;

            this._robotSize.PropertyChanged += Size_PropertyChanged;
            this._screenSize.PropertyChanged += Size_PropertyChanged;

            this.LocationRefreshCommand = new AsyncRelayCommand(ExecuteToRefreshLocation);
            this.LocationRefreshCommand.PropertyChanged += MoveCommand_PropertyChanged;

            this.MoveRightCommand = new AsyncRelayCommand(ExecuteMoveRight);
            this.MoveRightCommand.PropertyChanged += MoveCommand_PropertyChanged;

            this.MoveUpCommand = new AsyncRelayCommand(ExecuteMoveUp);
            this.MoveUpCommand.PropertyChanged += MoveCommand_PropertyChanged;

            this.MoveLeftCommand = new AsyncRelayCommand(ExecuteMoveLeft);
            this.MoveLeftCommand.PropertyChanged += MoveCommand_PropertyChanged;

            this.MoveDownCommand = new AsyncRelayCommand(ExecuteMoveDown);
            this.MoveDownCommand.PropertyChanged += MoveCommand_PropertyChanged;
        }

        private void Size_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.RepositionForScreenSizeChange();
        }

        private void MoveCommand_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (String.Equals(e.PropertyName, "IsRunning", StringComparison.OrdinalIgnoreCase))
            {
                this.NotifyPropertyChanged(() => CanMoveNow);
                this.NotifyPropertyChanged(() => AnyCommandRunning);
            }
        }

        public event RobotMoveFailedEventHandler RobotMoveFailed;

        public Location RobotLocation
        {
            get { return this._robotLocation; }
        }

        public Location RobotScreenLocation
        {
            get { return this._robotScreenLocation; }
        }

        public RectangleSize RobotSize
        {
            get { return this._robotSize; }
        }

        public RectangleSize ScreenSize
        {
            get { return this._screenSize; }
        }

        public ObservableCollection<int> ListOfMovingSteps
        {
            get;
            private set;
        }

        public void ChangeByResizedDisplay(double width, double height, bool refreshLocation)
        {
            this.ScreenSize.Width = (int)width;
            this.ScreenSize.Height = (int)height;

            this.RepositionForScreenSizeChange();
            if (refreshLocation == true)
            {
                this.LocationRefreshCommand.ExecuteAsync(null);
            }
        }

        private void RepositionForScreenSizeChange()
        {
            var centerX = this._screenSize.Width / 2;
            var centerY = this._screenSize.Height / 2;

            var curLoc = this._robotLocation;

            var deltaX = curLoc.PositionX * this._robotSize.Width;
            var deltaY = curLoc.PositionY * this._robotSize.Height;

            var changedXScr = centerX + deltaX - (this._robotSize.Width / 2);
            var changedYScr = centerY - deltaY - (this._robotSize.Height / 2);

            if (changedXScr != this._robotScreenLocation.PositionX ||
                changedYScr != this._robotScreenLocation.PositionY)
            {
                this._robotScreenLocation.PositionX = changedXScr;
                this._robotScreenLocation.PositionY = changedYScr;

                this._logger.LogInformation($"\"Robot\" location changed to ({this._robotLocation.PositionX}, {this._robotLocation.PositionY})");
                this._logger.LogInformation($"\"Robot\" location (in screen) changed to ({this._robotScreenLocation.PositionX}, {this._robotScreenLocation.PositionY})");
            }
        }

        public bool AnyCommandRunning
        {
            get
            {
                return !this.CanMoveNow;
            }
        }

        public bool CanMoveNow
        {
            get
            {
                return (this.LocationRefreshCommand.IsRunning == false &&
                    this.MoveRightCommand.IsRunning == false &&
                    this.MoveUpCommand.IsRunning == false &&
                    this.MoveLeftCommand.IsRunning == false &&
                    this.MoveDownCommand.IsRunning == false);
            }
        }

        public MoveDirection MoveDirection
        {
            get;
            private set;
        }

        public int NoOfStepToMove
        {
            get;
            set;
        }

        protected async Task ExecuteMove(MoveDirection direction)
        {
            this.MoveDirection = direction;
            MoveInstruction inst = new MoveInstruction()
            {
                Direction = direction,
                NoOfStep = this.NoOfStepToMove
            };

            this._logger.LogInformation($"Requesting robot to move by {inst.NoOfStep}, {inst.Direction}");
            try
            {
                // to check if current location is changed or not by others
                Location curLoc = await this._robot.GetCurrentLocationAsync();
                if (curLoc.PositionX != this._robotLocation.PositionX ||
                    curLoc.PositionY != this._robotLocation.PositionY)
                {
                    var warningMsg = $"Location of \"Robot\" changed by others to ({curLoc.PositionX}, {curLoc.PositionY})";
                    this._logger.LogWarning(warningMsg);
                    this.RobotLocation.PositionX = curLoc.PositionX;
                    this.RobotLocation.PositionY = curLoc.PositionY;
                    this.RepositionForScreenSizeChange();
                    this.RobotMoveFailed?.Invoke(this, new RobotMoveFailedEventArgs(warningMsg + Environment.NewLine + "Try again!"));
                    return;
                }

                Location updated = await this._robot.MoveAsync(inst);
                this._logger.LogInformation($"\"Robot\" moved to ({updated.PositionX}, {updated.PositionY})");
                this.RobotLocation.PositionX = updated.PositionX;
                this.RobotLocation.PositionY = updated.PositionY;
                await Task.Delay(100); // just to show progress bar
                this.RepositionForScreenSizeChange();
            }
            catch (Exception err)
            {
                this._logger.LogError($"Failed \"Robot\" to move", err);
                this.RobotMoveFailed?.Invoke(this, new RobotMoveFailedEventArgs(err));
            }
        }

        public IAsyncRelayCommand LocationRefreshCommand
        {
            get;
            private set;
        }

        private async Task ExecuteToRefreshLocation()
        {
            this._logger.LogInformation($"Requesting current location of robot");
            try
            {
                Location updated = await this._robot.GetCurrentLocationAsync();
                this._logger.LogInformation($"Current location of \"Robot\" is ({updated.PositionX}, {updated.PositionY})");
                this.RobotLocation.PositionX = updated.PositionX;
                this.RobotLocation.PositionY = updated.PositionY;
                await Task.Delay(50); // just to show progress bar
                this.RepositionForScreenSizeChange();
            }
            catch (Exception err)
            {
                this._logger.LogError($"Failed to get the location of \"Robot\"", err);
                this.RobotMoveFailed?.Invoke(this, new RobotMoveFailedEventArgs(err));
            }
        }

        #region Move Right
        public IAsyncRelayCommand MoveRightCommand
        {
            get;
            private set;
        }

        private async Task ExecuteMoveRight()
        {
            await this.ExecuteMove(MoveDirection.Right);
        }
        #endregion

        #region Move Up
        public IAsyncRelayCommand MoveUpCommand
        {
            get;
            private set;
        }

        private async Task ExecuteMoveUp()
        {
            await this.ExecuteMove(MoveDirection.Up);
        }
        #endregion

        #region Move Left
        public IAsyncRelayCommand MoveLeftCommand
        {
            get;
            private set;
        }

        private async Task ExecuteMoveLeft()
        {
            await this.ExecuteMove(MoveDirection.Left);
        }
        #endregion

        #region Move Down
        public IAsyncRelayCommand MoveDownCommand
        {
            get;
            private set;
        }

        private async Task ExecuteMoveDown()
        {
            await this.ExecuteMove(MoveDirection.Down);
        }
        #endregion
    }
}
