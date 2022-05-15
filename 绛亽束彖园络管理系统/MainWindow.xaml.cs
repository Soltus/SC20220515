using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        WindowsManager2_SingleStringArgsDC dc = new();
        public MainWindow(bool newin = true)
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            #region 登录界面加载及验证
            //显示登陆界面，验证后返回。
            if (newin)
            {
                连接的实例.Text = "";
                登录 loginWindow = new();
                loginWindow.ShowDialog();
                if (loginWindow.DialogResult != Convert.ToBoolean(1))
                {
                    this.Hide();
                }
            }
            //显示登陆界面 结束
            #endregion
            this.Activate();
            App.Current.MainWindow = this;
        }

        bool _AllowClose = false;
        bool _ShowingDialog = false;
        bool _reLogin = false;
        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.Activate();
            //If the user has elected to allow the close, simply let the closing event happen.
            if (_AllowClose || _reLogin) return;

            //NB: Because we are making an async call we need to cancel the closing event
            e.Cancel = true;

            //we are already showing the dialog, ignore
            if (_ShowingDialog) return;

            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(24);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "确认要关闭主窗口吗？关闭后并不会退出程序，可在系统托盘重新打开主窗口~";

            Button btn1 = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn1.Style = style;
            btn1.Width = 115;
            btn1.Height = 30;
            btn1.Margin = new Thickness(2);
            btn1.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn1.CommandParameter = true;
            btn1.Content = "是";

            Button btn2 = new Button();
            Style style2 = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn2.Style = style2;
            btn2.Width = 115;
            btn2.Height = 30;
            btn2.Margin = new Thickness(2);
            btn2.Command = MaterialDesignThemes.Wpf.DialogHost.CloseDialogCommand;
            btn2.CommandParameter = false;
            btn2.Content = "否";


            DockPanel dck = new DockPanel();
            dck.Children.Add(btn1);
            dck.Children.Add(btn2);

            StackPanel stk = new StackPanel();
            stk.Width = 230;
            stk.Height = 180;
            stk.Children.Add(txt1);
            stk.Children.Add(dck);

            //Set flag indicating that the dialog is being shown
            _ShowingDialog = true;
            object result = await MaterialDesignThemes.Wpf.DialogHost.Show(stk);
            _ShowingDialog = false;
            //The result returned will come form the button's CommandParameter.
            //If the user clicked "Yes" set the _AllowClose flag, and re-trigger the window Close.
            if (result is bool boolResult && boolResult)
            {
                _AllowClose = true;
                右下角通知 x = new("别忘了在系统托盘召唤我~");
                x.Show();
                Close();
            }
        }

        private void 八价兲谛全丰专区_Click(object sender, RoutedEventArgs e)
        {
            // new 八价兲谛全丰专区().Show();
            WindowsManager<八价兲谛全丰专区>.Show(new object()); // 单实例
            this.Hide();
        }

        // 注销登录
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _reLogin = true;
            if (PublicDataClass.Instance != null) { PublicDataClass.Instance = null; }
            //foreach (Window w in Application.Current.Windows) w.Close();
            PublicFunctions.隐藏非指定窗口(string.Empty);
            登录 dl = new();
            dl.ShowDialog();
            _reLogin = false;
            this.Activate();
            App.Current.MainWindow = this;
        }
    }
}
