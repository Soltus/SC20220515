using System.Windows;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// GetUserInput.xaml 的交互逻辑
    /// </summary>
    public partial class GetUserInput : Window
    {
        public string Url { get; set; }
        public GetUserInput()
        {
            InitializeComponent();
        }

        private void Commit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Url = this.id.Text;
            Close();
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
