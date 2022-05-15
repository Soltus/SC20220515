using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Taskbar = (TaskbarIcon)FindResource("Taskbar"); // 任务栏托盘
            base.OnStartup(e);
            //MainWindow mw = new();
            //mw.Show();
            App.Current.MainWindow = new MainWindow(true);
            //Current.MainWindow.Show();

        }

        public static TaskbarIcon Taskbar;



        readonly public static System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);


    }


    public class ShowMessageCommand : ICommand
    {
        public void Execute(object parameter)
        {
            MessageBox.Show(parameter.ToString());
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }

    public class DelegateCommand : ICommand
    {
        public Action? CommandAction { get; set; }
        public Func<bool>? CanExecuteFunc { get; set; }

        public void Execute(object? parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class NotifyIconViewModel
    {
        /// <summary>
        /// 不要声明为 static
        /// </summary>
        public ICommand ShowWindowCommand => new DelegateCommand
        {

            CanExecuteFunc = () => App.Current.MainWindow == null || App.Current.MainWindow.Visibility != Visibility.Visible,
            CommandAction = () =>
            {
                if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                App.Current.MainWindow.Show();
            }
        };

        /// <summary>
        /// 隐藏窗口
        /// </summary>
        public ICommand HideWindowCommand => new DelegateCommand
        {
            CommandAction = () =>
            {
                //Application.Current.MainWindow.Visibility = Visibility.Hidden;
                App.Current.MainWindow.Hide();
            },
            CanExecuteFunc = () => App.Current.MainWindow != null && App.Current.MainWindow.Visibility == Visibility.Visible
        };


        /// <summary>
        /// 关闭软件
        /// </summary>
        public ICommand ExitApplicationCommand => new DelegateCommand
        { CommandAction = () => {

            if (PublicDataClass.Instance != null)
                foreach (Microsoft.SqlServer.Management.Smo.Database db in PublicDataClass.Instance.Databases) { PublicDataClass.Instance.KillAllProcesses(db.Name); }
            Application.Current.Shutdown(); Process.GetCurrentProcess().Kill(); 
        } 
        };

        /// <summary>
        /// 展示微信公众号
        /// </summary>
        public ICommand ShowWXCommand => new DelegateCommand
        { CommandAction = () => { WindowsManager<微信公众号>.Show(new object()); } };


    }




}
