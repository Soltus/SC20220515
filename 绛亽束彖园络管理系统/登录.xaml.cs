using MaterialDesignThemes.Wpf;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 登录.xaml 的交互逻辑
    /// </summary>
    public partial class 登录 : Window
    {
        public string UserName;
        public string UserPassword;

        WindowsManager2_SingleStringArgsDC dc = new();

        // 强依赖
        public void ShowToast(string text, TimeSpan? time = null)
        {
            Toast toast = new Toast();
            toast.Content = text;
            toast.Margin = new Thickness(0, 10, 0, 0);
            ToastPanel.Children.Add(toast);
            if (time == null)
            {
                toast.SetTimeClose(TimeSpan.FromSeconds(3));
            }
            else
            {
                toast.SetTimeClose(time.Value);
            }
        }

        public 登录()
        {
            InitializeComponent();
            try
            {
                ManagedComputer mc = new();
                WmiConnectionInfo wci = mc.ConnectionSettings;
                WebView2Controlers.logger.Info("MachineName = " + wci.MachineName);
                List<string> list = new();
                foreach (ServerInstance si in mc.ServerInstances) list.Add(si.Name);
                Instance.ItemsSource = list;
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message);
                List<string> _x = new();
                _x.Add("未检测到可用的 SQL Server 实例");
                Instance.ItemsSource = _x;
                Instance.SelectedIndex = 0;
                //Instance.IsEnabled = false;
            }
            this.Activate();
            Account.Text = App.config.AppSettings.Settings["DefaultUser"].Value;
        }

        private void MoveWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            App.Current.Shutdown();
            Process.GetCurrentProcess().Kill();
        }


        async private void Login_Button(object sender, RoutedEventArgs e)
        {
            if (Account.Text.ToString() == "") { ShowToast("不可为空"); return; }
            if (Password.Password.ToString() == "") { ShowToast("不可为空"); return; }
            root.IsEnabled = false;
            // Connecting to a named instance of SQL Server with SQL Server Authentication using ServerConnection  
            ServerConnection srvConn = new();
            srvConn.ServerInstance = @".\" + Instance.SelectedItem;   // connects to named instance  
            srvConn.LoginSecure = false;   // set to true for Windows Authentication  
            srvConn.Login = Account.Text.ToString();
            srvConn.Password = Password.Password.ToString();
            dc.Name = $"Try to connect to server {srvConn.ServerInstance}.";
            WindowsManager2<右下角累加通知>.Show(dc);
            string eee = "";
            Task ppp = Task.Run(() => Tototo(ref srvConn, ref eee));
            ppp.Wait();
            if (PublicDataClass.Instance != null)
            {
                PublicDataClass.Instance.Refresh();
                PublicDataClass.Instance.Databases.Refresh();
                this.DialogResult = Convert.ToBoolean(1);
                dc.Name = $"Successfull connection to server {srvConn.ServerInstance}. for v{PublicDataClass.Instance.Information.Version.Major}";
                WindowsManager2<右下角累加通知>.Show(dc);
                if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                App.Current.MainWindow.Show();
                this.Close();
                ppp.Dispose();
            }
            else if (eee != "")
            {
                dc.Name = eee;
                WindowsManager2<右下角累加通知>.Show(dc);
                eee = "";
            }
            else
            {
                ShowToast("连接超时,请检查配置");
            }
            root.IsEnabled = true;
            WindowsManager2<右下角累加通知>.Close();
        }

        private void 使用Windows身份验证_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            root.IsEnabled = false;
            // Connecting to a named instance of SQL Server with SQL Server Authentication using ServerConnection  
            ServerConnection srvConn = new();
            srvConn.ServerInstance = @".\" + Instance.SelectedItem;   // connects to named instance  
            srvConn.LoginSecure = true;   // set to true for Windows Authentication
            dc.Name = $"Try to connect to server {srvConn.ServerInstance}.";
            WindowsManager2<右下角累加通知>.Show(dc);
            string eee = "";
            Task ppp = Task.Run(() => Tototo(ref srvConn, ref eee));
            //bool IsComplate = ppp.Wait(15000);
            ppp.Wait();
            if (PublicDataClass.Instance != null)
            {
                PublicDataClass.Instance.Refresh();
                PublicDataClass.Instance.Databases.Refresh();
                this.DialogResult = Convert.ToBoolean(1);
                dc.Name = $"Successfull connection to server {srvConn.ServerInstance}. for v{PublicDataClass.Instance.Information.Version.Major}";
                WindowsManager2<右下角累加通知>.Show(dc);
                if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                App.Current.MainWindow.Show();
                this.Close();
                ppp.Dispose();
            }
            else if (eee != "") { 
                dc.Name = eee;
                eee = "";
                WindowsManager2<右下角累加通知>.Show(dc);
            }
            else { ShowToast("连接超时,请检查配置"); }
            root.IsEnabled = true;
            WindowsManager2<右下角累加通知>.Close();
        }


        void Tototo(ref ServerConnection srvConn, ref string eee)
        {
            try
            {
                PublicDataClass.Instance = new Server(srvConn);
                Console.WriteLine(PublicDataClass.Instance.Information.Version);   // connection is established  
            }
            catch (Exception ex)
            {
                eee = ex.Message;
                PublicDataClass.Instance = null;
            }

        }


        //“注册账户”TextBlock触发事件
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //RegisterWindow register1 = new RegisterWindow();  //Login为窗口名，把要跳转的新窗口实例化
            //this.Close();  //关闭当前窗口
            //register1.ShowDialog();   //打开新窗口
            dc.Name = "不支持在此注册账号\n请在第三方软件使用管理员账号登录 SQL Server 并手动创建账号 ~";
            WindowsManager2<右下角累加通知>.Show(dc);
        }

        private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            //右下角累加通知 x = new("注意：SQL Server 安装完成后先重启计算机 ~");x.Show();
            string str = System.Environment.CurrentDirectory;
            dc.Name = "注意：SQL Server 安装完成后先重启计算机 ~";
            WindowsManager2<右下角累加通知>.Show(dc);
            try
            {
                Process.Start($"{str}\\exe\\SQL2019-SSEI-Expr.exe");

                this.WindowState = WindowState.Minimized;
            }
            catch
            {
                try
                { // 没有效果
                    Process pr = new Process();
                    pr.StartInfo.WorkingDirectory = $"{str}\\exe\\";
                    pr.StartInfo.FileName = $"{str}\\exe\\SQL2019-SSEI-Expr.exe";
                    pr.StartInfo.UseShellExecute = false;
                    pr.StartInfo.CreateNoWindow = false;
                    pr.Start();

                    this.WindowState = WindowState.Minimized;
                }
                catch (Exception ex)
                {
                    Clipboard.SetDataObject($"{str}\\exe\\");

                    dc.Name = $"{ex.Message}";
                    WindowsManager2<右下角累加通知>.Show(dc);
                    dc.Name = $"无法启动   {str}\\exe\\SQL2019-SSEI-Expr.exe \n请尝试手动运行（路径已复制到剪切板）";
                    WindowsManager2<右下角累加通知>.Show(dc);
                }
            }
        }
    }
}
