using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 右下角通知.xaml 的交互逻辑
    /// </summary>
    public partial class 右下角通知 : Window
    {
        public 右下角通知()
        {
            InitializeComponent();
            this.Loaded += StartCloseTimer; // 定时关闭
            note.MouseLeftButtonDown += (s, e) => { this.Close(); };
            // 右下角显示
            var r = SystemParameters.WorkArea;
            this.Left = r.Right - ActualWidth;
            this.Top = r.Bottom - ActualHeight;
            this.SizeChanged += (o, e) =>
            {
                var r = SystemParameters.WorkArea;
                this.Left = r.Right - ActualWidth;
                this.Top = r.Bottom - ActualHeight;
            };
        }

        public 右下角通知(string n) : this()
        {
            note.Text = n;
        }

        private void StartCloseTimer(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new();
            timer.Interval = TimeSpan.FromSeconds(8); // 8秒
            timer.Tick += TimerTick; // 注册计时器到点后触发的回调
            timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick; // 取消注册
            this.Close();
        }
    }
}
