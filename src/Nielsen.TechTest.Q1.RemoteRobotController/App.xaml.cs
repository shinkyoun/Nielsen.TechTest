using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;

using Nielsen.TechTest.Q1.Common;

namespace Nielsen.TechTest.Q1.RemoteRobotController
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _svcProvider = null;

        private IConfiguration AddConfiguration()
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
#if DEBUG
            configBuilder.AddJsonFile("appsettings.Development.json", true, true);
#else
            configBuilder.AddJsonFile("appsettings.Production.json", true, true);
#endif
            return configBuilder.Build();
        }

        private void ConfigureServices(ServiceCollection services, IConfiguration config)
        {
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
            services.AddLogging(logBuilder =>
            {
                logBuilder.AddNLog();
            });

            services.AddHttpClient();
            services.AddSingleton<ISimpleAsyncRobot<Location, MoveInstruction>, RemoteRobotController>();
            services.AddSingleton<MainWindow>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evnt"></param>
        private void OnStartup(object sender, StartupEventArgs evnt)
        {
            #region this part could be in ctor but intentionally here to avoid codes in ctor
            ServiceCollection services = new ServiceCollection();

            IConfiguration config = this.AddConfiguration();
            services.AddSingleton<IConfiguration>(config);

            this.ConfigureServices(services, config);
            this._svcProvider = services.BuildServiceProvider();
            #endregion

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Stanely Choi, Q1 TestApp is starting");
            var mainWindow = this._svcProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void OnExit(object sender, ExitEventArgs evnt)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info($"Stanely Choi, Q1 TestApp is exiting{Environment.NewLine}");
            LogManager.Shutdown();
        }
    }
}
