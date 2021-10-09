using System;
using System.Collections.Generic;
using System.Text;

namespace Nielsen.TechTest.Q1.RemoteRobotController
{
    public class RobotMoveFailedEventArgs : EventArgs
    {
        private readonly Exception _error;
        private string _message = null;

        public RobotMoveFailedEventArgs(Exception error) : base()
        {
            this._error = error;
        }

        public RobotMoveFailedEventArgs(string message) : base()
        {
            this._message = message;
        }

        public bool IsError
        {
            get { return this._error != null; }
        }

        public Exception Error
        {
            get { return this._error; }
        }
        public string WarningOrInfo
        {
            get { return this._message; }
        }
    }

    public delegate void RobotMoveFailedEventHandler(object sender, RobotMoveFailedEventArgs evnt);
}
