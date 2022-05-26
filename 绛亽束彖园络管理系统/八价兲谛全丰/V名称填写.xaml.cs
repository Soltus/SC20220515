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
using System.Windows.Navigation;
using System.Windows.Shapes;
using 绛亽束彖园络管理系统;

namespace 八价兲谛全丰
{
    /// <summary>
    /// V名称填写.xaml 的交互逻辑
    /// </summary>
    public partial class V名称填写 : Page
    {
        readonly List<string> 元氏表 = PublicDataClass.元氏表;
        public V名称填写()
        {
            InitializeComponent();
            tb1.ItemsSource = 元氏表;
        }
    }
}
