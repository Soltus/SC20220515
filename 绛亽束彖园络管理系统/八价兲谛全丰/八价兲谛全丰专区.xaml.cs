using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 八价兲谛全丰专区.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class 八价兲谛全丰专区 : Window
    {

        public 八价兲谛全丰专区()
        {
            InitializeComponent();
            // 防止缩放导致显示过大
            var r = SystemParameters.WorkArea;
            this.Width = r.Right - 200;
            this.Height = r.Bottom - 100;
            标题容器.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };
            副标题容器.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                    this.DragMove();
            };
            DispatcherTimer dispatcherTimer = new();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            WebView2Controlers.WebFrame = frame1;
            WebView2Controlers.URI = new Uri("pack://application:,,,/绛亽束彖园络管理系统;component/八价兲谛全丰/Pages/webv_page1.xaml");
            WebView2Controlers.Current = "https://cn.bing.com";
            frame1.NavigationService.Navigating += (s, e) =>
            {
                WinTitle.UriString = Uri.UnescapeDataString(frame1.NavigationService.Source.ToString()); // 正常显示中文
                int index = frame1.Source.ToString().LastIndexOf("/");
                WinTitle.窗口标题 = frame1.Source.ToString()[(index + 1)..].Replace(".xaml", "");
                WinTitle.Bicon = BitmapFrame.Create(new Uri("pack://application:,,,/images/绛亽.jpg"), BitmapCreateOptions.None, BitmapCacheOption.Default);
                if (frame1.Source.OriginalString.Split("/webv_page").Length > 1) { drawer_OpenMode.SelectedIndex = 1; } // WebView 使用 Standar 模式弹出抽屉防止被遮挡
                else { drawer_OpenMode.SelectedIndex = 0; }
            };
            RB_八价兲谛全丰专区.Checked += (s, e) =>
            { frame1.Navigate(new Uri("pack://application:,,,/绛亽束彖园络管理系统;component/八价兲谛全丰/menu_page1.xaml")); };
            帮助文档.Checked += (s, e) =>
            //{ frame1.Source = new Uri("pack://application:,,,/绛亽束彖园络管理系统;component/八价兲谛全丰/Pages/webv_page1.xaml"); };
            { frame1.Navigate(WebView2Controlers.URI); };
            this.IsVisibleChanged += (s, e) => { if (this.Visibility == Visibility.Visible && frame1.NavigationService.CurrentSource != null) frame1.NavigationService.Refresh(); }; // 刷新缓存
        }


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            副标题就是我.Text = DateTime.Now.ToString().Split()[1];
        }

        public void HeadMin_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        public void HeadMax_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }
        public void HeadClose_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public bool c_changeState = true;
        GridLength g0 = new(0, GridUnitType.Pixel);
        GridLength g1 = new(158, GridUnitType.Pixel);


        private void ShowHiddenMenu(object sender, MouseButtonEventArgs e)
        {

            if (c_changeState)
            {

                c_changeState = false;
                menu_c1.Width = g0;
                menu_grid.Width = menu_c0.Width.Value;
                Directory_BT.Text = "\xe648";
                via.Visibility = Visibility.Collapsed;

            }
            else
            {
                c_changeState = true;
                menu_c1.Width = g1;
                menu_grid.Width = menu_c0.Width.Value + menu_c1.Width.Value;
                Directory_BT.Text = "\xe6ad";
                via.Visibility = Visibility.Visible;

            }
        }



        private void min_MouseEnter(object sender, MouseEventArgs e)
        {
            tb_head_min.Text = "\xe618";
        }
        private void min_MouseLeave(object sender, MouseEventArgs e)
        {
            tb_head_min.Text = "\xe608";
        }

        private void max_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized) { tb_head_max.Text = "\xe722"; }
            else { tb_head_max.Text = "\xe61a"; }
        }
        private void max_MouseLeave(object sender, MouseEventArgs e)
        {
            tb_head_max.Text = "\xe608";
        }

        private void close_MouseEnter(object sender, MouseEventArgs e)
        {
            tb_head_close.Text = "\xe60f";
        }
        private void close_MouseLeave(object sender, MouseEventArgs e)
        {
            tb_head_close.Text = "\xe608";
        }

        private void drawer_OpenMode_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (frame1.Source.OriginalString.Split("/webv_page").Length > 1) { drawer_OpenMode.SelectedIndex = 1; }
            else { drawer_OpenMode.SelectedIndex = 0; }
        }
    }

}
