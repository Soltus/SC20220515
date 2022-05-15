using System;
using System.Threading;
using System.Windows.Controls;

namespace 绛亽束彖园络管理系统
{
    public class Toast : Label
    {
        public Toast()
        {

        }

        public void SetTimeClose(TimeSpan time)
        {
            new Thread(() =>
            {
                Thread.Sleep(time);
                if (this.Parent is Panel)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        (this.Parent as Panel).Children.Remove(this);
                    }));
                }
            })
            { IsBackground = true }.Start();
        }
    }
}
