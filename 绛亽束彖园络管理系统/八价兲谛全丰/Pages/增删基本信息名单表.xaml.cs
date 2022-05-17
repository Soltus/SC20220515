using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// 增删基本信息名单表.xaml 的交互逻辑
    /// </summary>
    public partial class 增删基本信息名单表 : Page
    {

        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private UpdateProgressBarDelegate updatePbDelegate;
        public int ProgressBar1Value = 0;

        Server Ins = PublicDataClass.Instance;

        public 增删基本信息名单表()
        {
            InitializeComponent();
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(增删基本信息名单表_Resize);
            tb1.ItemsSource = PublicFunctions.GetCustomDatabaseNameList(ref Ins);
            获取结果.IsExpanded = false;
        }

        private void 增删基本信息名单表_Resize(object sender, System.EventArgs e)
        {
            topgrid.Width = this.ActualWidth;
            topgrid.Height = this.ActualHeight;
            var w = this.ActualWidth - 35;
            if (w > topgrid.MaxWidth) w = topgrid.MaxWidth - 35;
            tb1.Width = w < 0 ? 0 : w;
            tb2.Width = w < 0 ? 0 : w;
            progressBar1.Width = tb1.Width - 35;
        }

        private void Submit_添加(string c1)
        {
            Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
            Microsoft.SqlServer.Management.Smo.Table table = new(db1, c1);
            Column 格叚 = new(table, "格叚", DataType.Bit);
            Column 元氏 = new(table, "元氏", DataType.NVarChar(10));
            Column 名字 = new(table, "名字", DataType.NVarChar(10));
            格叚.Nullable = false;
            Microsoft.SqlServer.Management.Smo.Index index = new(table, $"PK_{c1}");
            index.IndexKeyType = IndexKeyType.DriPrimaryKey;
            index.IndexedColumns.Add(new IndexedColumn(index, "元氏"));
            index.IndexedColumns.Add(new IndexedColumn(index, "名字"));
            table.Indexes.Add(index);
            table.Columns.Add(元氏);
            table.Columns.Add(名字);
            table.Columns.Add(格叚);
            table.Create();
            db1.Defaults.Refresh();
            if (db1.Defaults.Contains("def_格叚"))
            {
                db1.Defaults["def_格叚"].Drop();
            }
            Microsoft.SqlServer.Management.Smo.Default def = new(db1, "def_格叚");
            def.TextHeader = "create default [def_格叚] as";
            def.TextBody = "0";
            def.Create();
            def.BindToColumn(c1.ToString(), 格叚.Name);
            table.Alter();
            def.UnbindFromColumn(c1.ToString(), 格叚.Name);
            def.Drop();

            //db1.ExecuteNonQuery($"create table {c1}(元氏 nvarchar(10),名字 nvarchar(10),格叚 bit not null default 0,primary key(元氏, 名字));");
        }

        private void Submit_删除(string c1)
        {
            Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
            db1.Tables[$"{c1}"].DropIfExists();
            db1.Tables.Refresh();

            //db1.ExecuteNonQuery($"drop table {c1};");
        }

        private void tb1_KeyUp(object sender, KeyEventArgs e)
        {

            List<string> mylist = new List<string>();
            mylist = PublicFunctions.GetCustomDatabaseNameList(ref Ins).ToList().FindAll(delegate (string s) { return s.Contains(tb1.Text.Trim()); });
            tb1.ItemsSource = mylist;
            tb1.IsDropDownOpen = true;
        }

        private void change_tb3(string obj, string nn, ref Database db1)
        {
            if (nn != "")
                tb3.Text = tb1.Text + '.' + obj + nn;
            tb2.Text = "";
            List<string> list = GetTableRowsList(db1);
            lv1.ItemsSource = list;
            获取结果.IsExpanded = true;
        }

        // 添加表
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                if (tb2.Text == "") { throw new Exception("不能为空"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                List<string> 已有表 = PublicFunctions.GetCustomTableNameList(ref db1);
                if (已有表.Contains(tb2.Text)) { throw new Exception("不能添加已存在的表"); }
                Submit_添加(tb2.Text.Trim());
                PublicDataClass.Instance.Databases.Refresh();;
                if (!已有表.Contains(tb2.Text))
                {
                    change_tb3(tb2.Text, " 已添加", ref db1);
                }
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox); }
            finally
            {
            }

        }

        public void 删除表_Click(object sender, RoutedEventArgs e)
        {
            string? strText = lv1.SelectedValue.ToString();
            if (strText != null) 删除表(strText.Split("══")[0].TrimEnd(' '));
        }

        public void 修改表_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("敬请期待");
        }

        public void 重命名表_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("敬请期待");
        }


        public List<string> GetTableRowsList(Database db)
        {
            db.Tables.Refresh();
            List<string> tableList = new();
            progressBar1.Maximum = db.Tables.Count;
            foreach (Microsoft.SqlServer.Management.Smo.Table table in db.Tables)
            {
                ProgressBar1Value++;

                updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
                Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(ProgressBar1Value) });

                tableList.Add(table.Name + $" {string.Empty.PadLeft(32 - PublicFunctions.GetSingleLength(table.Name), '═')}（{PublicFunctions.获取表记录数(table)} 行记录）");
            }
            return tableList;
        }

        public static List<string> GetStoredProcedureNameList(Database db)
        {
            List<string> storedProcedureNameList = new();
            foreach (StoredProcedure storedProcedure in db.StoredProcedures) storedProcedureNameList.Add(storedProcedure.Name); return storedProcedureNameList;
        }
        public List<string> GetCustomStoredProcedureNameList(Database db)
        {
            //List<string> storedProcedureNameList = new();
            List<string> customStoredProcedureNameList = new();
            ObservableCollection<string> storedProcedureNameList = new();
            lv1.ItemsSource = storedProcedureNameList;
            progressBar1.Maximum = db.StoredProcedures.Count;
            获取结果.IsExpanded = true;
            foreach (StoredProcedure storedProcedure in db.StoredProcedures)
            {
                ProgressBar1Value++;

                updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
                Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { System.Windows.Controls.ProgressBar.ValueProperty, Convert.ToDouble(ProgressBar1Value) });

                if (storedProcedure.IsSystemObject)
                {
                    storedProcedureNameList.Add($"exclude {storedProcedure.Name}");
                    if (storedProcedureNameList.Count > 5)
                        storedProcedureNameList.RemoveAt(0);
                }
                else
                { customStoredProcedureNameList.Add(storedProcedure.Name); };
            }
            return customStoredProcedureNameList;
            //return customStoredProcedureNameList.Union(storedProcedureNameList).ToList<string>();
        }
        public List<string> GetUserNameList(Database db) { List<string> userNameList = new List<string>(); foreach (User user in db.Users) userNameList.Add(user.Name); return userNameList; }






        // 获取表
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                bt1.IsEnabled = false;
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                List<string> list = GetTableRowsList(db1);
                lv1.ItemsSource = list;
                获取结果.IsExpanded = true;
                change_tb3(tb2.Text, "", ref db1);
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
            finally
            {
                bt1.IsEnabled = true;
                progressBar1.Value = 0;
                ProgressBar1Value = 0;
            }
        }

        // 获取存储过程
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                bt1.IsEnabled = false;
                if (tb1.Text == "") { throw new Exception("未指定数据库"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                List<string> list = GetCustomStoredProcedureNameList(db1);
                lv1.ItemsSource = list;
                获取结果.IsExpanded = true;
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
            finally
            {
                bt1.IsEnabled = true;
                progressBar1.Value = 0;
                ProgressBar1Value = 0;
            }
        }

        // 删除表
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            删除表(tb2.Text);
        }

        private void 删除表(string name)
        {
            try
            {
                bt1.IsEnabled = false;
                if (name == "") { throw new Exception("不能为空"); }
                Database db1 = PublicDataClass.Instance.Databases[$"{tb1.Text}"];
                List<string> 已有表 = PublicFunctions.GetCustomTableNameList(ref db1);
                if (已有表.Contains(name) == false) { throw new Exception("不能删除不存在的表"); }
                Submit_删除(name);
                change_tb3(name, " 已删除", ref db1);
            }
            catch (Exception ex)
            { WebView2Controlers.logger.Error(ex.Message); App.DCbox.Name = ex.Message; WindowsManager2<右下角累加通知>.Show(App.DCbox);; }
            finally
            {
                bt1.IsEnabled = true;
            }
        }

        private void lv1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListView listview = (ListView)sender;
            string? strText = listview.SelectedValue.ToString();
            if (strText != null) Clipboard.SetDataObject(strText.Split("══")[0].TrimEnd(' '));
        }

        private void tb1_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            tb1.IsDropDownOpen = true;
        }
    }
}
