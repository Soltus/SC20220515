using System;
using System.Collections.Generic;
using System.Text;
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
    /// 右下角累加通知.xaml 的交互逻辑
    /// </summary>
    public partial class 右下角累加通知 : Window
    {
        public 右下角累加通知()
        {
            InitializeComponent();
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

    }
}
