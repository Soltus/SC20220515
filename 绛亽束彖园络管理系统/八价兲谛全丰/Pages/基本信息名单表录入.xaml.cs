using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 核芯束蒂愍天蒂_添加.xaml 的交互逻辑
    /// </summary>
    public partial class 基本信息名单表录入 : Page
    {
        List<string> 元氏表 = PublicDataClass.元氏表;
        Server Ins = PublicDataClass.Instance;


        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private UpdateProgressBarDelegate updatePbDelegate;
        public int ProgressBar1Value = 0;

        public 基本信息名单表录入()
        {
            InitializeComponent();
            tb1.ItemsSource = 元氏表;
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(基本信息名单表录入_Resize);
            if (PublicDataClass.Instance != null)
            {
                cb_db.ItemsSource = PublicFunctions.GetCustomDatabaseNameList(ref Ins);
            }


        }

        private void cb_db_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_db.Text != null)
            {
                List<string> mylist = new();
                Database db = PublicDataClass.Instance.Databases[$"{cb_db.SelectedValue}"];
                mylist = PublicFunctions.GetCustomTableNameList(ref db).FindAll(delegate (string s) { return s.Contains(tb_表名.Text.Trim()); });
                tb_表名.ItemsSource = mylist;
                tb_表名.IsDropDownOpen = true;
            }
        }

        private void 基本信息名单表录入_Resize(object sender, System.EventArgs e)
        {
            topgrid.Width = this.ActualWidth;
            topgrid.Height = this.ActualHeight;
            var w = this.ActualWidth - 35;
            if (w > topgrid.MaxWidth) w = topgrid.MaxWidth - 35;
            tb1.Width = w < 0 ? 0 : w;
            tb2.Width = w < 0 ? 0 : w;
            cb_db.Width = w < 0 ? 0 : w;
            tb_表名.Width = w < 0 ? 0 : w;
            progressBar1.Width = w < 0 ? 0 : w;
        }

        public static List<string> GetTableNameList(Database db)
        { List<string> tableList = new(); foreach (Microsoft.SqlServer.Management.Smo.Table table in db.Tables) tableList.Add(table.Name); return tableList; }



        private void tb1_KeyUp(object sender, KeyEventArgs e)
        {

            List<string> mylist = new();
            mylist = 元氏表.FindAll(delegate (string s) { return s.ToLower().Contains(tb1.Text.Trim().ToLower()); });
            tb1.ItemsSource = mylist;
            tb1.IsDropDownOpen = true;
        }

        private void tb_表名_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> mylist = new();
            Database db = PublicDataClass.Instance.Databases[$"{cb_db.Text}"];
            mylist = PublicFunctions.GetCustomTableNameList(ref db).FindAll(delegate (string s) { return s.Contains(tb1.Text.Trim()); });
            tb_表名.IsDropDownOpen = true;
        }

        private void Submit_添加(string c1, string c2, bool? c3, ref int count, ref List<string>? change)
        {
            string sql = $"insert into [{tb_表名.Text}](元氏,名字,格叚) values('{c1}','{c2}','{c3}');";
            try
            {
                PublicDataClass.Instance.Databases[$"{cb_db.Text}"].ExecuteNonQuery(sql);
                count++;
                if (change != null)
                    change.Add($"{c1}⁒{c2}");
            }
            catch (Exception ex)
            {
                WebView2Controlers.logger.Error(ex.Message);
                PublicDataClass.Instance.Databases[$"{cb_db.Text}"].ExecuteNonQuery
                    ($"if not exists(select 1 from [{tb_表名.Text}] where 元氏 = '{c1}' and 名字 = '{c2}') {sql};");
            }
            ProgressBar1Value++;

            updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
            Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(ProgressBar1Value) });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "" || tb2.Text == "") { throw new Exception("不能为空"); }
                if (元氏表.Contains(tb1.Text) == false) { throw new Exception("不允许的元氏"); }
                int count = 0;
                List<string>? change = null;
                Submit_添加(tb1.Text, tb2.Text, cb1.IsChecked, ref count, ref change);
                tb3.Text = tb1.Text + '⁒' + tb2.Text + " 已添加";
                tb1.Text = "";
                tb2.Text = "";
                cb1.IsChecked = false;
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        private void Button_Click1(object sender, RoutedEventArgs e) // 从剪贴板批量提交
        {
            progressBar1.Value = 0;
            ProgressBar1Value = 0;
            progressBar1.IsIndeterminate = true;
            try
            {
                if (tb_表名.Text == "") { throw new ArgumentNullException("表名"); }
            }
            catch (ArgumentNullException ex)
            {
                WebView2Controlers.logger.Error(ex.Message);
                App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);;
                return;
            }
            tb3.Text = "";
            string str = Clipboard.GetText(TextDataFormat.UnicodeText);
            string[] sArray = str.Split(new char[3] { '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string>? change = new List<string>();
            int len = Regex.Matches(str, "⁒").Count;
            if (len != sArray.Length) { MessageBox.Show($"非格式化数据，无法识别：\n{str}"); return; }
            int j = 0;
            progressBar1.Maximum = len;
            progressBar1.Minimum = 0;
            progressBar1.IsIndeterminate = false;
            foreach (string i in sArray)
            {
                string ii = i.Replace("\n", "").Trim();
                string left = ii.Split('⁒')[0];
                string right = "";
                bool sex = false;
                if (ii.Contains('⁜'))
                {
                    right = ii.Split('⁒')[1].Replace("⁜", "").Trim();
                    sex = true;
                }
                else if (ii.Contains('⁒'))
                {
                    right = ii.Split('⁒')[1];
                }
                else { MessageBox.Show($"无效数据{ii}"); continue; }
                if (left == "" || right == "") { throw new Exception("不能为空"); }
                Regex rx = new Regex("^[\u4e00-\u9fa5]$");
                if (rx.IsMatch(ii.Substring(0, 1)))
                {
                    if (元氏表.Contains(left) == false) { throw new Exception($"不允许的元氏: {left}"); }
                    Submit_添加(left, right, sex, ref j, ref change);
                }
                else
                {
                    if (元氏表.Contains(right) == false) { throw new Exception($"不允许的元氏: {right}"); }
                    Submit_添加(right, left, sex, ref j, ref change);
                }
                //j += 1;
                //tb1.Text = "";
                //tb2.Text = "";
            }
            //MessageBox.Show($"完成！新增{j}条记录。");
            MessageBoxResult result = MessageBox.Show($"新增{j}条记录。\n\n【确定】-> 完成录入 \n【取消】-> 回滚", "完成！", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                if (j > 0)
                {
                    foreach (string ii in change)
                    {
                        string left = ii.Split('⁒')[0];
                        string right = ii.Split('⁒')[1];
                        try
                        {
                            PublicDataClass.Instance.Databases[$"{cb_db.Text}"].ExecuteNonQuery
                                ($"delete from [{tb_表名.Text}] where 元氏 = '{left}' and 名字 = '{right}';");
                        }
                        catch { }
                        ProgressBar1Value--;

                        updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
                        Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(ProgressBar1Value) });

                    }
                }
            }
            progressBar1.Value = 0;
            ProgressBar1Value = 0;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_表名.Text == "") { throw new Exception("未指定表"); }
                Table t = PublicDataClass.Instance.Databases[$"{cb_db.Text}"].Tables[$"{tb_表名.SelectedValue}"];
                PublicFunctions.快速清空表记录(ref t);
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        private void cb_db_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            cb_db.IsDropDownOpen = true;
        }

        private void tb_表名_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb_表名.IsDropDownOpen = true;
        }

        private void tb1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb1.IsDropDownOpen = true;
        }
    }
}
