using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 基本信息表集合运算.xaml 的交互逻辑
    /// </summary>
    public partial class 基本信息表集合运算 : Page
    {
        Server Ins = PublicDataClass.Instance;
        public 基本信息表集合运算()
        {
            InitializeComponent();
            tb1.ItemsSource = PublicFunctions.GetCustomDatabaseNameList(ref Ins);
            dg1.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            dg2.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            dg3.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            dg4.LoadingRow += new EventHandler<DataGridRowEventArgs>(dg_LoadingRow);
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(Resize);
        }
        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        private void dg_set_all()
        {
            int c = 4;
            if (dg1.Columns.Count > 2) { dg4.Visibility = Visibility.Collapsed; dg3.Visibility = Visibility.Collapsed; c -=2; } else { dg4.Visibility = Visibility.Visible; dg3.Visibility = Visibility.Visible; }
            dg_set(ref dg1, c);
            dg_set(ref dg2, c);
            dg_set(ref dg3, c);
            dg_set(ref dg4, c);
        }
        private void Resize(object sender, System.EventArgs e)
        {
            dg_set_all();
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
        private void tb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb2_1.ItemsSource = tb2.ItemsSource;
            tb2_1.IsDropDownOpen = true;
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

        // 补集查询
        private void bt1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults($"select * from [{tb2.Text}] except select * from [{tb2_1.Text}];");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                ds = db1.ExecuteWithResults($"select * from [{tb2_1.Text}] except select * from [{tb2.Text}];");
                dg2.ItemsSource = ds.Tables[0].DefaultView;
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        // 交集查询
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults($"select * from [{tb2.Text}] intersect select * from [{tb2_1.Text}];");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                dg2.ItemsSource = ds.Tables[0].DefaultView;
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        // 并集查询
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults($"select * from [{tb2.Text}] union select * from [{tb2_1.Text}];");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                dg2.ItemsSource = ds.Tables[0].DefaultView;
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        // 补集查询（排序）
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults
                    ($"declare @tempTable TABLE(元氏 nvarchar(10),名字 nvarchar(10),格叚 bit);insert into @tempTable select * from [{tb2.Text}] except select * from [{tb2_1.Text}];select a.元氏,a.名字,a.格叚 from @tempTable as a join (select 元氏,COUNT(*) as 元数 from @tempTable group by 元氏) as b on a.元氏=b.元氏 order by b.元数 desc;");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                string dataSetContent = $"--- {tb2.Text}新增（{ds.Tables[0].Rows.Count}） ---\n";
                dataSetContent += ds.元氏DSToString();
                ds = db1.ExecuteWithResults
                    ($"declare @tempTable TABLE(元氏 nvarchar(10),名字 nvarchar(10),格叚 bit);insert into @tempTable select * from [{tb2_1.Text}] except select * from [{tb2.Text}];select a.元氏,a.名字,a.格叚 from @tempTable as a join (select 元氏,COUNT(*) as 元数 from @tempTable group by 元氏) as b on a.元氏=b.元氏 order by b.元数 desc;");
                dg2.ItemsSource = ds.Tables[0].DefaultView;
                dataSetContent += $"\n--- {tb2.Text}移除（{ds.Tables[0].Rows.Count}） ---\n";
                dataSetContent += ds.元氏DSToString();
                Clipboard.SetDataObject(dataSetContent.Trim());
                App.DCbox.Name = $" 补集查询（排序）已复制到剪贴板";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        // 交集查询（排序）
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults
                    ($"declare @tempTable TABLE(元氏 nvarchar(10),名字 nvarchar(10),格叚 bit);insert into @tempTable select * from [{tb2.Text}] intersect select * from [{tb2_1.Text}];select a.元氏,a.名字,a.格叚 from @tempTable as a join (select 元氏,COUNT(*) as 元数 from @tempTable group by 元氏) as b on a.元氏=b.元氏 order by b.元数 desc;");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                dg2.ItemsSource = ds.Tables[0].DefaultView;
                string dataSetContent = $"--- {tb2.Text}继承（{ds.Tables[0].Rows.Count}） ---\n";
                dataSetContent += ds.元氏DSToString();
                Clipboard.SetDataObject(dataSetContent.Trim());
                App.DCbox.Name = $" 交集查询（排序）已复制到剪贴板";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        // 并集查询（排序）
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = db1.ExecuteWithResults
                    ($"declare @tempTable TABLE(元氏 nvarchar(10),名字 nvarchar(10),格叚 bit);insert into @tempTable select * from [{tb2.Text}] union select * from [{tb2_1.Text}];select a.元氏,a.名字,a.格叚 from @tempTable as a join (select 元氏,COUNT(*) as 元数 from @tempTable group by 元氏) as b on a.元氏=b.元氏 order by b.元数 desc;");
                dg1.ItemsSource = ds.Tables[0].DefaultView;
                dg2.ItemsSource = ds.Tables[0].DefaultView;
                string dataSetContent = $"--- {tb2.Text} + {tb2_1.Text}（{ds.Tables[0].Rows.Count}） ---\n";
                dataSetContent += ds.元氏DSToString();
                Clipboard.SetDataObject(dataSetContent.Trim());
                App.DCbox.Name = $" 并集查询（排序）已复制到剪贴板";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
                dg_set_all();
                e查询结果.IsExpanded = true;
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }

        // 跨表迁移记录
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }

        // 组成情况
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "" || tb2_1.Text == "") { throw new Exception("未指定数据表"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                if (db1.Tables[tb2.Text].Columns.Contains("元氏") == false) { throw new Exception("该表未包含元氏列"); }
                DataSet ds = new();
                ds = db1.ExecuteWithResults($"select COUNT(*) as 卩 from [{tb2.Text}] where 格叚 = 0;");
                string y1 = ds.Tables[0].Rows[0][0].ToString();
                ds = db1.ExecuteWithResults($"select COUNT(*) as 卩 from [{tb2_1.Text}] where 格叚 = 0;");
                string y2 = ds.Tables[0].Rows[0][0].ToString();
                ds = db1.ExecuteWithResults($"select COUNT(*) as 卪 from [{tb2.Text}] where 格叚 = 1;");
                string x1 = ds.Tables[0].Rows[0][0].ToString();
                ds = db1.ExecuteWithResults($"select COUNT(*) as 卪 from [{tb2_1.Text}] where 格叚 = 1;");
                string x2 = ds.Tables[0].Rows[0][0].ToString();
                int xx =  int.Parse(x1) - int.Parse(x2);
                int yy =  int.Parse(y1) - int.Parse(y2);
                string repo1 = "   1  ";
                string repo2 = "   2  ";
                if (xx == 0) repo1 += $"萝丽丝共 {x1} 格，同上价持平。\n"; else if (xx < 0) repo1 += $"萝丽丝共 {x1} 格，同比减少 {xx} 格。\n" ; else repo1 += $"萝丽丝共 {x1} 格，同比增加 {xx} 格。\n";
                if (yy == 0) repo2 += $"布鲁斯共 {y1} 格，同上价持平。\n"; else if (yy < 0) repo2 += $"布鲁斯共 {y1} 格，同比减少 {yy} 格。\n" ; else repo2 += $"布鲁斯共 {y1} 格，同比增加 {yy} 格。\n";
                var results = new StringBuilder("");
                List<元氏计数> list = new();
                string si = string.Empty;
                foreach (string i in PublicDataClass.元氏表)
                {
                    string _tt = db1.ExecuteWithResults($"select COUNT(*) from [{tb2.Text}] where 元氏 = '{i}';").Tables[0].Rows[0][0].ToString();
                    string _tt2 = db1.ExecuteWithResults($"select COUNT(*) from [{tb2_1.Text}] where 元氏 = '{i}';").Tables[0].Rows[0][0].ToString();
                    int _ff = int.Parse(_tt) - int.Parse(_tt2);
                    if (_ff == 0) si = $"同上价持平"; else if (_ff < 0) si = $"同比减少 {Math.Abs(_ff)} 格"; else si = $"同比增加 {_ff} 格";
                    list.Add(new 元氏计数(int.Parse(_tt), i, si));
                }
                list = list.OrderByDescending(o => o.数量).ToList();//降序
                int j = 2;
                foreach (var i in list)
                {
                    //results += i;
                    j++;
                    results.Append($" {j.ToString().PadLeft(3)}  ");
                    results.Append(i.ToAnaString());
                }
                Clipboard.SetDataObject(repo1+repo2+results);
                App.DCbox.Name = $" 组成情况已复制到剪贴板";
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
            catch (Exception ex) { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
        }
    }
}
