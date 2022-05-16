using MaterialDesignThemes.Wpf;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
                App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);;
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
            App.DCbox.Name = $"Try to connect to server {srvConn.ServerInstance}.";
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
            string eee = "";
            Task ppp = Task.Run(() => Tototo(ref srvConn, ref eee));
            ppp.Wait();
            if (PublicDataClass.Instance != null)
            {
                PublicDataClass.Instance.Refresh();
                PublicDataClass.Instance.Databases.Refresh();
                this.DialogResult = Convert.ToBoolean(1);
                App.DCbox.Name = $"Successfull connection to server {srvConn.ServerInstance}. for v{PublicDataClass.Instance.Information.Version.Major}";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                App.Current.MainWindow.Show();
                this.Close();
                ppp.Dispose();
            }
            else if (eee != "")
            {
                App.DCbox.Name = eee;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                eee = "";
            }
            else
            {
                ShowToast("连接超时,请检查配置");
            }
            root.IsEnabled = true;
        }

        private void 使用Windows身份验证_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            root.IsEnabled = false;
            // Connecting to a named instance of SQL Server with SQL Server Authentication using ServerConnection  
            ServerConnection srvConn = new();
            srvConn.ServerInstance = @".\" + Instance.SelectedItem;   // connects to named instance  
            srvConn.LoginSecure = true;   // set to true for Windows Authentication
            App.DCbox.Name = $"Try to connect to server {srvConn.ServerInstance}.";
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
            string eee = "";
            Task ppp = Task.Run(() => Tototo(ref srvConn, ref eee));
            //bool IsComplate = ppp.Wait(15000);
            ppp.Wait();
            if (PublicDataClass.Instance != null)
            {
                PublicDataClass.Instance.Refresh();
                PublicDataClass.Instance.Databases.Refresh();
                this.DialogResult = Convert.ToBoolean(1);
                App.DCbox.Name = $"Successfull connection to server {srvConn.ServerInstance}. for v{PublicDataClass.Instance.Information.Version.Major}";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                if (App.Current.MainWindow == null) App.Current.MainWindow = new MainWindow(false);
                App.Current.MainWindow.Show();
                this.Close();
                ppp.Dispose();
            }
            else if (eee != "") { 
                App.DCbox.Name = eee;
                eee = "";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            else { ShowToast("连接超时,请检查配置"); }
            root.IsEnabled = true;
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
            App.DCbox.Name = "不支持在此注册账号\n请在第三方软件使用管理员账号登录 SQL Server 并手动创建账号 ~";
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
        }

        // 安装SQL Server
        async private void TextBlock_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            install.IsEnabled = false;
            string str = System.Environment.CurrentDirectory;
            FileInfo fi = new FileInfo($"{str}\\exe\\SQL2019-SSEI-Expr.exe");    
            if (!File.Exists($"{str}\\exe\\SQL2019-SSEI-Expr.exe"))
            {
                try
                {
                    App.DCbox.Name = "无法找到安装包，尝试从网络下载...";
                    WindowsManager2<右下角累加通知>.Show(App.DCbox);
                    bool yes = HttpClientHelper.IsAvailableNetworkActive();
                    if (!yes)
                    {
                        App.DCbox.Name = "无法访问，请检查网络连接";
                        WindowsManager2<右下角累加通知>.Show(App.DCbox); install.IsEnabled = true; return;
                    }
                    await HttpClientHelper.DownloadFile("https://go.microsoft.com/fwlink/?linkid=866658", fi);
                }
                catch (Exception ex)
                {
                    install.IsEnabled = false;
                    App.DCbox.Name = $"{ex.Message}";
                    WindowsManager2<右下角累加通知>.Show(App.DCbox); install.IsEnabled = true; return;
                }
            }
            App.DCbox.Name = "注意：SQL Server 安装完成后先重启计算机 ~";
            WindowsManager2<右下角累加通知>.Show(App.DCbox);
            try
            {
                this.WindowState = WindowState.Minimized;
                Process.Start($"{str}\\exe\\SQL2019-SSEI-Expr.exe").WaitForExit();
                install.IsEnabled = true;
                this.WindowState = WindowState.Normal;
                this.Activate();
            }
            catch
            {
                try
                {  // 以管理员身份运行
                    this.WindowState = WindowState.Minimized;
                    Process pr = new Process();
                    pr.StartInfo.FileName = $"{str}\\exe\\SQL2019-SSEI-Expr.exe";
                    pr.StartInfo.UseShellExecute = false;
                    pr.StartInfo.UseShellExecute = true;
                    pr.StartInfo.WorkingDirectory = Environment.CurrentDirectory;
                    pr.StartInfo.Verb = "RunAs";
                    pr.StartInfo.CreateNoWindow = false;
                    pr.Start();
                    pr.WaitForExit();
                    install.IsEnabled = true;
                    this.WindowState = WindowState.Normal;
                    this.Activate();
                }
                catch (Exception ex)
                {
                    Clipboard.SetDataObject($"{str}\\exe\\");
                    App.DCbox.Name = $"{ex.Message}";
                    WindowsManager2<右下角累加通知>.Show(App.DCbox);
                    App.DCbox.Name = $"无法启动 {str}\\exe\\SQL2019-SSEI-Expr.exe \n请尝试手动运行（路径已复制到剪切板）";
                    WindowsManager2<右下角累加通知>.Show(App.DCbox);
                    install.IsEnabled = true; return;
                }
            }
        }

    }
}
