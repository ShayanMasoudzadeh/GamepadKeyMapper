using ControllerToKeyboardAPP;
using System;
using System.Windows;
using System.Windows.Threading;

namespace ControllerToKeyboardAPP
{
    public partial class MainWindow : Window
    {
        private ControllerHandler _controllerHandler;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            _controllerHandler = new ControllerHandler();

            // Set up a timer to call the Update method regularly
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _controllerHandler.Update();
        }
    }
}