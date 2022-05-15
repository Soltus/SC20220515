using System.Windows;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 微信公众号.xaml 的交互逻辑
    /// </summary>
    public partial class 微信公众号 : Window
    {
        public 微信公众号()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (s, e) => { this.Close(); };
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
