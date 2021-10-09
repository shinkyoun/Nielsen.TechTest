using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Extensions.Logging;
using Nielsen.TechTest.Q1.Common;

namespace Nielsen.TechTest.Q1.RemoteRobotController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger<MainWindow> _logger;
        private readonly ISimpleAsyncRobot<Location, MoveInstruction> _robot;

        public MainWindow(ILogger<MainWindow> logger,
            ISimpleAsyncRobot<Location, MoveInstruction> robot)
        {
            this._logger = logger;
            this._robot = robot;
            this.RobotScreenViewModel = new RobotMonitorViewModel(this._logger, this._robot);
            this.RobotScreenViewModel.RobotMoveFailed += RobotScreenViewModel_RobotMoveFailed;
            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += MainWindow_SizeChanged;
            this.DataContext = this.RobotScreenViewModel;

            InitializeComponent();
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.RobotScreenViewModel.ChangeByResizedDisplay(this.ctlLocationDisplay.ActualWidth,
                this.ctlLocationDisplay.ActualHeight,
                false);
        }

        private void MainWindow_Loaded(object sender, EventArgs e)
        {
            this.RobotScreenViewModel.ChangeByResizedDisplay(this.ctlLocationDisplay.ActualWidth,
                this.ctlLocationDisplay.ActualHeight,
                true);
        }

        private void RobotScreenViewModel_RobotMoveFailed(object sender, RobotMoveFailedEventArgs evnt)
        {
            if (evnt.IsError == true)
            {
                MessageBox.Show($"{evnt.Error.Message}{Environment.NewLine}{Environment.NewLine}{evnt.Error.StackTrace}",
                    "Robot Move Failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(evnt.WarningOrInfo,
                    "Stopped to request to move Robot",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        public RobotMonitorViewModel RobotScreenViewModel
        {
            get;
            private set;
        }

    }
}
