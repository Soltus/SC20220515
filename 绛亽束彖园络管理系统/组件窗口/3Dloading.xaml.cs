using System;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace 绛亽束彖园络管理系统.组件窗口
{
    /// <summary>
    /// _3Dloading.xaml 的交互逻辑
    /// </summary>
    public partial class _3Dloading : Window
    {
        private Timer timer = new Timer(2000);

        public _3Dloading()
        {
            InitializeComponent();
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate ()
            {
                this.Close();
            });
        }
    }
}
