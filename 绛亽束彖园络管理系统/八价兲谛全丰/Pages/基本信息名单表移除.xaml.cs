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
    public partial class 基本信息名单表移除 : Page
    {
        readonly List<string> 元氏表 = PublicDataClass.元氏表;
        readonly Server Ins = PublicDataClass.Instance;

        public 基本信息名单表移除()
        {
            InitializeComponent();
            tb1.ItemsSource = 元氏表;
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(基本信息名单表移除_Resize);
            if (PublicDataClass.Instance != null)
            {
                cb_db.ItemsSource = PublicFunctions.GetCustomDatabaseNameList(ref Ins);
                cb_db.IsEditable = false;
            }
        }

        private void cb_db_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_db.Text != null)
            {
                List<string> mylist = new();
                Database db = Ins.Databases[$"{cb_db.SelectedValue}"];
                mylist = PublicFunctions.GetCustomTableNameList(ref db).FindAll(delegate (string s) { return s.Contains(tb_表名.Text.Trim()); });
                tb_表名.ItemsSource = mylist;
                tb_表名.IsDropDownOpen = true;
            }
        }

        private void 基本信息名单表移除_Resize(object sender, System.EventArgs e)
        {
            topgrid.Width = this.ActualWidth;
            topgrid.Height = this.ActualHeight;
            var w = this.ActualWidth - 35;
            if (w > topgrid.MaxWidth) w = topgrid.MaxWidth - 35;
            tb1.Width = w < 0 ? 0 : w;
            tb2.Width = w < 0 ? 0 : w;
            cb_db.Width = w < 0 ? 0 : w;
            tb_表名.Width = w < 0 ? 0 : w;
        }



        private void tb_表名_KeyUp(object sender, KeyEventArgs e)
        {
            List<string> mylist = new();
            Database db = Ins.Databases[$"{cb_db.Text}"];
            mylist = PublicFunctions.GetCustomTableNameList(ref db).FindAll(delegate (string s) { return s.Contains(tb_表名.Text.Trim()); });
            tb_表名.ItemsSource = mylist;
            tb_表名.IsDropDownOpen = true;
        }

        private void Submit_移除(string c1, string c2)
        {
            Ins.Databases[$"{cb_db.Text}"].ExecuteNonQuery($"delete from [{tb_表名.Text}]  where 元氏='{c1}' and 名字='{c2}';");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "" || tb2.Text == "") { throw new Exception("不能为空"); }
                if (元氏表.Contains(tb1.Text) == false) { throw new Exception("不允许的元氏"); }
                Submit_移除(tb1.Text, tb2.Text);
                tb3.Text = tb1.Text + '⁒' + tb2.Text + " 已移除";
                tb1.Text = "";
                tb2.Text = "";
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }

        }

        private void Button_Click1(object sender, RoutedEventArgs e) // 从剪贴板批量提交
        {
            tb3.Text = "";
            string str = Clipboard.GetText(TextDataFormat.UnicodeText);
            string[] sArray = str.Split(new char[3] { '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            int j = 0;
            foreach (string i in sArray)
            {
                string ii = i.Replace("\n", "").Trim();
                string left = ii.Split('⁒')[0];
                string right = "";
                if (ii.Contains('⁜'))
                {
                    right = ii.Split('⁒')[1].Replace("⁜", "").Trim();
                }
                else if (ii.Contains('⁒'))
                {
                    right = ii.Split('⁒')[1];
                }
                else { MessageBox.Show(ii); continue; }
                try
                {
                    if (left == "" || right == "") { throw new Exception("不能为空"); }
                    Regex rx = new Regex("^[\u4e00-\u9fa5]$");
                    if (rx.IsMatch(ii.Substring(0, 1)))
                    {
                        if (元氏表.Contains(left) == false) { throw new Exception($"不允许的元氏: {left}"); }
                        Submit_移除(left, right);
                    }
                    else
                    {
                        if (元氏表.Contains(right) == false) { throw new Exception($"不允许的元氏: {right}"); }
                        Submit_移除(right, left);
                    }
                    j += 1;
                    tb1.Text = "";
                    tb2.Text = "";
                }
                catch (Exception ex)
                { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }

            }
            MessageBox.Show($"完成！移除{j}条记录。");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb_表名.Text == "") { throw new Exception("未指定表"); }
                Microsoft.SqlServer.Management.Smo.Table t = Ins.Databases[$"{cb_db.Text}"].Tables[$"{tb_表名.SelectedValue}"];
                PublicFunctions.快速清空表记录(ref t);
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        private void tb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tb1.SelectedValue == null) return;
            tb1.IsEnabled = false;
            try
            {
                string? value = tb1.SelectedValue.ToString(); // tb.Text 还没更新。。。
                Database db = Ins.Databases[$"{cb_db.SelectedValue}"];
                if (value != null && tb_表名.Text != null && db != null && db.Tables.Contains(tb_表名.Text))
                {
                    tb2.ItemsSource = PublicFunctions.查询当前表当前元氏的格(ref db, tb_表名.Text, value);
                    tb2.IsDropDownOpen = true;
                }
            }
            catch { }
            finally { tb1.IsEnabled = true; }
        }


        private void tb1_KeyUp(object sender, KeyEventArgs e)
        {
            tb1.ItemsSource = 元氏表.FindAll(delegate (string s) { return s.ToLower().Contains(tb1.Text.Trim().ToLower()); });
            tb1.IsDropDownOpen = true;
        }

        private void tb2_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void tb2_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb2.IsDropDownOpen = true;
        }

        private void tb1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb1.IsDropDownOpen = true;
        }

        private void tb_表名_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb_表名.IsDropDownOpen = true;
        }

        private void cb_db_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            cb_db.IsDropDownOpen = true;
        }

        private void tb2_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            tb2.IsDropDownOpen = false;
        }

        private void tb2_KeyDown(object sender, KeyEventArgs e)
        {
            Database db = Ins.Databases[$"{cb_db.SelectedValue}"];
            List<string> j = PublicFunctions.查询当前表当前元氏的格(ref db, tb_表名.Text, tb1.Text);
            if (tb2.Text.Length == 0) { tb2.ItemsSource = j; }
            else if (tb_表名.Text != null && db.Tables.Contains(tb_表名.Text))
            {
                Regex rx = new("^[\u4e00-\u9fa5]$");
                if (!rx.IsMatch(tb1.Text[..1]) || rx.IsMatch(tb2.Text[..1]))
                {
                    tb2.ItemsSource = j.FindAll(delegate (string s) { return s.ToLower().Contains(tb2.Text.Trim().ToLower()); });
                }
                else { tb2.ItemsSource = PublicFunctions.匹配拼音2汉字列表(j, tb2.Text); }
                tb2.IsDropDownOpen = true;
            }
        }
    }
}
