using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 基本信息表查询.xaml 的交互逻辑
    /// </summary>
    public partial class 基本信息表查询 : Page
    {

        // public Server? Server_基本信息表查询 { get; set; }
        Server Ins = PublicDataClass.Instance;

        public static DataView? Dgdg { get; set; }

        public 基本信息表查询()
        {
            InitializeComponent();
            tb1.ItemsSource = PublicFunctions.GetCustomDatabaseNameList(ref Ins);
            dg1.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            dg2.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            dg3.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            dg4.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(基本信息表查询_Resize);
        }

        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dg_set_all()
        {
            int c = 4;
            if (dg1.Columns.Count > 3) { dg4.Visibility = Visibility.Collapsed; c--; } else { dg4.Visibility = Visibility.Visible; }
            if (dg1.Columns.Count > 5) { dg3.Visibility = Visibility.Collapsed; c--; } else { dg3.Visibility = Visibility.Visible; }
            if (dg1.Columns.Count > 7) { dg2.Visibility = Visibility.Collapsed; c--; } else { dg2.Visibility = Visibility.Visible; }
            dg_set(ref dg1, c);
            dg_set(ref dg2, c);
            dg_set(ref dg3, c);
            dg_set(ref dg4, c);
        }

        private void 基本信息表查询_Resize(object sender, System.EventArgs e)
        {
            dg_set_all();
            tb4.MaxWidth = this.ActualWidth / 2;
        }

        private void dg_set(ref DataGrid dg, int c = 1)
        {
            dg.BorderBrush = new SolidColorBrush(Colors.HotPink);
            dg.BorderThickness = new Thickness(1, 8, 1, 8);
            dg.CanUserReorderColumns = true;
            dg.CanUserResizeColumns = true;
            dg.CanUserSortColumns = true;
            dg.CanUserAddRows = false; // 去除最后一行空白
            dg.CanUserDeleteRows = true;
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            dg.ClipToBounds = true;
            dg.FontSize = 12;
            dg.Height = 440;
            int dg1_c = dg.Columns.Count * c;
            dg.Width = this.ActualWidth / c - 80;
        }




        // 获取指定表全部记录
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                DataSet ds = db1.ExecuteWithResults($"select * from [{tb2.Text}];");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        // 格叚比例查询
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults($"select COUNT(*) as 卩 from [{tb2.Text}] where 格叚 = 0;");
                string 卩 = ds.Tables[0].Rows[0][0].ToString();
                ds = db1.ExecuteWithResults($"select COUNT(*) as 卪 from [{tb2.Text}] where 格叚 = 1;");
                string 卪 = ds.Tables[0].Rows[0][0].ToString();
                MessageBox.Show($"卩:{卩}  卪:{卪}");
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        // 元氏比例查询
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                //string results = "";
                //foreach (string i in PublicDataClass.元氏表)
                //{
                //    DataSet ds = db1.ExecuteWithResults($"select COUNT(*) as 卩 from [{tb2.Text}] where 元氏 = '{i}';");
                //    results = results + $"{i}: " + ds.Tables[0].Rows[0][0].ToString() + "\n";
                //}
                DataSet ds = db1.ExecuteWithResults($"select 元氏,COUNT(*) as 元数 from [{tb2.Text}] group by 元氏 order by 元数 desc;");
                App.DCbox.Name = $"\n{ds.元氏DSToString().Replace("⁒"," ")}"; WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        // 元氏比例查询（排序）
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                var results = new StringBuilder("");
                List<元氏计数> list = new List<元氏计数>();
                foreach (string i in PublicDataClass.元氏表)
                {
                    DataSet ds = db1.ExecuteWithResults($"select COUNT(*) from [{tb2.Text}] where 元氏 = '{i}';");
                    list.Add(new 元氏计数(int.Parse(ds.Tables[0].Rows[0][0].ToString()), i));
                }
                list = list.OrderByDescending(o => o.数量).ToList();//降序
                int j = 0;
                foreach (var i in list)
                {
                    //results += i;
                    j++;
                    results.Append($" {j.ToString().PadLeft(3)}  ");
                    results.Append(i);
                }
                App.DCbox.Name = $"\n{results}"; WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        private void tb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tb1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tb1.Text != null)
            {
                List<string> mylist = new List<string>();
                Database db = PublicDataClass.Instance.Databases[$"{tb1.SelectedValue}"];
                mylist = PublicFunctions.GetCustomTableNameList(ref db).FindAll(delegate (string s) { return s.Contains(tb2.Text.Trim()); });
                tb2.ItemsSource = mylist;
                tb2.IsDropDownOpen = true;
            }
        }

        // 自定义SQL
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                DataSet ds = PublicDataClass.Instance.Databases[$"{tb1.SelectedValue}"].ExecuteWithResults($"{tb4.Text}");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
        }

        private void e查询结果_Expanded(object sender, RoutedEventArgs e)
        {
            canhide.Visibility = Visibility.Collapsed;
        }

        private void e查询结果_Collapsed(object sender, RoutedEventArgs e)
        {
            canhide.Visibility = Visibility.Visible;
        }

        private void tb1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb1.IsDropDownOpen = true;
        }

        private void tb2_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb2.IsDropDownOpen = true;
        }

        // 获取记录到剪贴板
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults($"select * from [{tb2.Text}];");
                string dataSetContent = ds.元氏DSToString();
                Clipboard.SetDataObject(dataSetContent);
                App.DCbox.Name = $"{tb2.Text}全部记录已复制到剪贴板";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); ; }
        }

        // 获取记录到剪贴板（排序）
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults($"select a.元氏,a.名字,a.格叚 from [{tb2.Text}] as a join (select 元氏,COUNT(*) as 元数 from [{tb2.Text}] group by 元氏) as b on a.元氏=b.元氏 order by b.元数 desc;");
                string dataSetContent = ds.元氏DSToString();
                Clipboard.SetDataObject(dataSetContent);
                App.DCbox.Name = $"{tb2.Text}全部记录已复制到剪贴板";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); ; }
        }
    }
}
