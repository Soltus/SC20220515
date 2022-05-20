using Microsoft.International.Converters.PinYinConverter;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Web.WebView2.Wpf;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace 绛亽束彖园络管理系统
{
    // 仅适用于不传参数的窗口
    public class WindowsManager<TWindow> where TWindow : Window, new()
    {
        static TWindow window;

        public static void Show(object vm)
        {
            if (window == null)
            {
                window = new TWindow();
                window.Closing += new CancelEventHandler(onClosing);
            }
            // 更新DataContext
            window.DataContext = vm;
            window.Show();
            // 再次打开窗口时需要激活
            window.Activate();
        }

        static void onClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            window = null;
        }

    }

    // 仅适用于使用DataContext通知累加更新的窗口
    public class WindowsManager2<TWindow> where TWindow : Window, new()
    {
        static TWindow window;

        public static void Show(object vm)
        {
            if (window == null)
            {
                window = new TWindow();
                window.Closing += new CancelEventHandler(onClosing);
            }
            // 更新DataContext
            window.DataContext = vm;
            window.Show();
            // 再次打开窗口时需要激活
            window.Activate();
        }

        static void onClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            window = null;
        }

    }
    public class WindowsManager2_SingleStringArgsDC : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        public string Name
        {
            get { return name; }
            set { name = $"\n[{DateTime.Now}]  {value}\n{name}"; Notify(); }
        }
        public void Clear()
        {
            name = string.Empty; Notify("Name");
        }

        private void Notify([CallerMemberName] string obj = "")
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(obj));
            }
        }
    }


    public class WebView2Controlers
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static bool IsOpenInNewWindow { get; set; }
        public static Frame WebFrame { get; set; }
        public static WebView2 WV2 { get; set; }
        public static Uri URI { get; set; }
        public static string Current { get; set; }

        private static BitmapFrame _b = BitmapFrame.Create(new Uri("pack://application:,,,/images/Edge.png"), BitmapCreateOptions.None, BitmapCacheOption.Default);
        public static BitmapFrame? Bicon { get => _b; set { _b = value; StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Bicon))); } }

        public static Logger logger = LogManager.GetCurrentClassLogger();
    }




    public class List_webv_page1_list<T>
    {
        private static T[] items = new T[3];
        private int index = 0;
        //向数组中添加项
        public void Add(T t)
        {
            if (index < 3)
            {
                items[index] = t;
                index++;
            }
            else
            {
                MessageBox.Show("数组已满！");
            }
        }
        //读取数组中的全部项
        public void Show()
        {
            foreach (T t in items)
            {
                MessageBox.Show(t.ToString());
            }
        }
        public static List<T> Items()
        {
            return items.ToList();
        }
    }


    public class WinTitle
    {
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        private static string _窗口标题 = "八价兲谛全丰专区";
        private static string _副窗口标题 = "";
        private static Uri _Uri = new("pack://application:,,,/绛亽束彖园络管理系统;component/八价兲谛全丰/menu_page1.xaml");
        private static string _UriString = "pack://application:,,,/绛亽束彖园络管理系统;component/八价兲谛全丰/menu_page1.xaml";

        public static string 窗口标题
        {
            get
            {
                return _窗口标题;
            }
            set
            {
                _窗口标题 = value;
                //通知
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(窗口标题)));
            }
        }

        public static string 副窗口标题
        {
            get
            {
                return _副窗口标题;
            }
            set
            {
                _副窗口标题 = value;
                //通知
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(副窗口标题)));
            }
        }

        public static Uri URI
        {
            get
            {
                return _Uri;
            }
            set
            {
                _Uri = value;
                //通知
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(URI)));
            }
        }

        public static string UriString
        {
            get
            {
                if (_UriString.StartsWith("http") || _UriString.StartsWith("magnet:?") || _UriString.StartsWith("tencent")) return _UriString;
                else return Uri.UnescapeDataString(_Uri.ToString()).Replace(";component/", "/");
            }
            set
            {
                _UriString = value;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(UriString)));
            }
        }

        private static BitmapFrame _b = BitmapFrame.Create(new Uri("pack://application:,,,/images/绛亽.jpg"), BitmapCreateOptions.None, BitmapCacheOption.Default);
        public static BitmapFrame? Bicon { get => _b; set { _b = value; StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(Bicon))); } }

    }

    public class UriConverter : IValueConverter
    {
        //当值从绑定源传播给绑定目标时，调用方法Convert
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            Uri uri = (Uri)value;
            return uri.ToString();
        }
        //当值从绑定目标传播给绑定源时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value as string;
            Uri uri;
            return new Uri(str, UriKind.RelativeOrAbsolute);
        }
    }

    public class XML_Serializer
    {
        /// <summary>
        /// serialize object to xml file.
        /// </summary>
        /// <param name="path">the path to save the xml file</param>
        /// <param name="obj">the object you want to serialize</param>
        public void serialize_to_xml(string path, object obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string content = string.Empty;
            //serialize
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, obj);
                content = writer.ToString();
            }
            //save to file
            using (StreamWriter stream_writer = new StreamWriter(path))
            {
                stream_writer.Write(content);
            }
        }

        /// <summary>
        /// deserialize xml file to object
        /// </summary>
        /// <param name="path">the path of the xml file</param>
        /// <param name="object_type">the object type you want to deserialize</param>
        public object deserialize_from_xml(string path, Type object_type)
        {
            XmlSerializer serializer = new XmlSerializer(object_type);
            using (StreamReader reader = new StreamReader(path))
            {
                return serializer.Deserialize(reader);
            }
        }
    }

    internal abstract class PublicDataClass // 抽象类，不支持实例化
    {
        public static List<string> 元氏表 { get; set; }

        public static Server? Instance { get; set; }
        public static string Connected_Ins { get; set; }


    }

    [Serializable]
    public class Serial_PublicDataClass
    {
        private List<string> _元氏表;
        public List<string> 元氏表
        {
            get
            {
                return _元氏表;
            }
            set
            {
                _元氏表 = value;
            }
        }
        /// <summary>
        /// 储存反序列化时候的溢出数据
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }


    class 元氏计数
    {
        private int _数量;
        private string _元氏;

        public 元氏计数(int 数量, string 元氏)
        {
            this._数量 = 数量;
            this._元氏 = 元氏;
        }

        public int 数量
        {
            get
            {
                return _数量;
            }

            set
            {
                _数量 = value;
            }
        }

        public string 元氏
        {
            get
            {
                return _元氏;
            }

            set
            {
                _元氏 = value;
            }
        }

        //重写ToString
        public override string ToString()
        {
            return "元氏: " + _元氏 + String.Empty.PadLeft(10 - PublicFunctions.GetSingleLength(_元氏), ' ') + "   元数: " + _数量 + "  \n";
        }
    }

    #region 公共函数
    internal class PublicFunctions
    {


        public static object? SerialReader_json(string file)
        {
            string jsonString = File.ReadAllText(file);
            var obj = JsonSerializer.Deserialize<Serial_PublicDataClass>(jsonString, App.jsonSerializerOptions);
            return obj;
        }
        async public static Task SerialWriter_json(string file, object sp)
        {
            using FileStream createStream = File.Create(file);
            await JsonSerializer.SerializeAsync(createStream, sp, App.jsonSerializerOptions);
            await createStream.DisposeAsync();
        }

        public static bool RemoteFileExists(string fileUrl)
        {
            bool result = false;//下载结果

            WebResponse response = null;
            try
            {
                WebRequest req = WebRequest.Create(fileUrl);

                response = req.GetResponse();

                result = response == null ? false : true;

            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
        public static void 关闭非指定窗口(List<string> wn)
        {
            // 遍历并关闭所有子窗口
            Window[] childArray = Application.Current.Windows.Cast<Window>().ToArray();
            for (int i = childArray.Length; i-- > 0;)
            {
                Window item = childArray[i];
                if (item.Title == "") continue; // 忽略无标题窗口
                if (wn.Contains(item.Title) == false) item.Close();
            }
        }

        public static void 隐藏非指定窗口(string wn)
        {
            // 遍历并关闭所有子窗口
            Window[] childArray = Application.Current.Windows.Cast<Window>().ToArray();
            for (int i = childArray.Length; i-- > 0;)
            {
                Window item = childArray[i];
                if (item.Title == "") continue; // 忽略无标题窗口
                if (item.Title != wn) item.Hide();
            }
        }

        public static int Text_Length(string Text)

        {

            int len = 0;

            for (int i = 0; i < Text.Length; i++)

            {

                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));

                if (byte_len.Length > 1)

                    len += 2; //如果长度大于1，是中文，占两个字节，+2

                else

                    len += 1;  //如果长度等于1，是英文，占一个字节，+1

            }

            return len;

        }

        public static int GetSingleLength(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException();
            }
            return Regex.Replace(input, @"[^\x00-\xff]", "aa").Length;//计算得到该字符串对应单字节字符串的长度
        }

        public static void 快速清空表记录(ref Microsoft.SqlServer.Management.Smo.Table table)
        {
            long rows = table.RowCount;
            table.TruncateData();
            table.Refresh();
            MessageBox.Show($" {rows} 行受影响", "done.");
        }

        public static void 重命名表(ref Microsoft.SqlServer.Management.Smo.Table table, string NewName)
        { table.Rename(NewName); }

        public static string 获取表记录数(Microsoft.SqlServer.Management.Smo.Table table)
        { string repo = table.RowCount.ToString(); return repo; }


        public static List<string> GetDatabaseNameList(Server server)
        {
            List<string> dbList = new(); foreach (Database db in server.Databases) dbList.Add(db.Name); return dbList;
        }

        public static ObservableCollection<string> GetCustomDatabaseNameList(ref Server server)
        {
            ObservableCollection<string> dbList = new();
            foreach (Database db in server.Databases)
            {
                if (db.IsSystemObject == false)
                    dbList.Add(db.Name);
            }
            return dbList;
        }

        public static List<string> GetTableNameList(Database db)
        {
            if (db != null) db.Tables.Refresh();
            List<string> tableList = new();
            foreach (Microsoft.SqlServer.Management.Smo.Table table in db.Tables) tableList.Add(table.Name);
            return tableList;
        }

        public static List<string>? GetCustomTableNameList(ref Database db)
        {
            if (db != null) db.Tables.Refresh(); else return null;
            List<string> tableList = new();
            foreach (Microsoft.SqlServer.Management.Smo.Table table in db.Tables)
            {
                if (table.IsSystemObject == false)
                    tableList.Add(table.Name);
            }
            return tableList;
        }

        public static List<string> 查询当前表当前元氏的格(ref Database db, string table, string 元氏)
        {
            List<string> tableList = new();
            try
            {
                DataSet ds = db.ExecuteWithResults($"select 名字 from [{table}] where 元氏 = '{元氏}';");
                int ro = ds.Tables[0].Rows.Count;
                for (int i = 0; i < ro; i++) tableList.Add(ds.Tables[0].Rows[i][0].ToString());
            }
            catch { }
            return tableList;
        }

        public static List<string> 匹配拼音2汉字列表(List<string> origin, string selwords)
        {

            List<string> pinyins = selwords.Split(" || ", StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> result = new();
            foreach (string pinyin in pinyins) { result.AddRange(拼音匹配纯汉字的列表(origin, pinyin)); }
            return result.Distinct<string>().ToList();
        }

        private static List<string> 拼音匹配纯汉字的列表(List<string> origin, string Rawselwords)
        {
            List<string> match = new();
            List<string> selwords = Rawselwords.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int j = 0; j < selwords.Count; j++)
            {
                string pattern = @"^[A-Za-z]+$"; //判断是汉字还是字母
                Regex regex = new(pattern);
                if (regex.IsMatch(selwords[j]))  //字母模糊搜索
                {
                    string _selwords = selwords[j].ToLower();
                    for (int i = 0; i < origin.Count; i++) //拼音模糊查询法
                    {
                        if (origin[i].Length < selwords.Count) continue;
                        string spells = "";
                        for (int ii = 0; ii < origin[i].Length; ii++) //遍历所有汉字
                        {
                            char c = origin[i][ii];
                            if (ChineseChar.IsValidChar(c)) //验证该汉字是否合法
                            {
                                ChineseChar CC = new(c);
                                ReadOnlyCollection<string> roc = CC.Pinyins;
                                spells = roc[0].ToLower();
                                if (ii == 0 && !spells.StartsWith(selwords[0])) { break; }
                                else if (ii == j && spells.StartsWith(_selwords)) { match.Add(origin[i]); continue; }
                                else if (ii == j && !spells.StartsWith(_selwords))
                                { match.Remove(origin[i]); break; }
                            }
                        }
                    }
                }
            }
            return match;
        }

    }

    public static class Extentions
    {
        public static string 元氏DSToString(this DataSet ds)
        {
            StringBuilder sb = new();

            foreach (var table in ds.Tables)
            {
                DataTable tbl = table as DataTable;
                //foreach (var column in tbl.Columns)
                //{
                //    DataColumn col = column as DataColumn;
                //    sb.Append(col.ColumnName + " ");
                //}
                sb.AppendLine();
                foreach (var row in tbl.Rows)
                {
                    DataRow dr = row as DataRow;
                    for (int i = 0; i < dr.ItemArray.Count(); i++)
                    {
                        sb.Append(dr.ItemArray[i].ToString() + "⁒");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine();
            }
            return sb.ToString().Replace("⁒False⁒", "").Replace("⁒True⁒", "⁜");
        }
    }


    #endregion

    public class HttpClientHelper
    {
        private static readonly object LockObj = new object();
        private static HttpClient client = null;
        public static bool IsAvailableNetworkActive() { try { Ping myPing = new(); string host = "cn.bing.com"; byte[] buffer = new byte[32]; int timeout = 1000; PingOptions pingOptions = new(); PingReply reply = myPing.Send(host, timeout, buffer, pingOptions); return (reply.Status == IPStatus.Success); } catch (Exception) { return false; } }

        public HttpClientHelper()
        {
            GetInstance();
        }
        public static HttpClient GetInstance()
        {

            if (client == null)
            {
                lock (LockObj)
                {
                    if (client == null)
                    {
                        client = new HttpClient();
                    }
                }
            }
            return client;
        }
        public async Task<string> PostAsync(string url, string strJson)//post异步请求方法
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //由HttpClient发出异步Post请求
                HttpResponseMessage res = await client.PostAsync(url, content);
                if (res.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string Post(string url, string strJson)//post同步请求方法
        {
            try
            {
                HttpContent content = new StringContent(strJson);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                //client.DefaultRequestHeaders.Connection.Add("keep-alive");
                //由HttpClient发出Post请求
                Task<HttpResponseMessage> res = client.PostAsync(url, content);
                if (res.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string str = res.Result.Content.ReadAsStringAsync().Result;
                    return str;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string Get(string url)
        {
            try
            {
                var responseString = client.GetStringAsync(url);
                return responseString.Result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task DownloadFile(string url, FileInfo file)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);

            try
            {
                var n = response.Content.Headers.ContentLength;
                var stream = await response.Content.ReadAsStreamAsync();
                using (var fileStream = file.Create())
                using (stream)
                {
                    byte[] buffer = new byte[1024];
                    var readLength = 0;
                    int length;
                    while ((length = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        readLength += length;

                        //Console.WriteLine("下载进度" + ((double)readLength) / n * 100);

                        // 写入到文件
                        fileStream.Write(buffer, 0, length);
                    }
                }

            }
            catch (Exception e)
            {
                App.DCbox.Name = e.Message;
                WindowsManager2<右下角累加通知>.Show(App.DCbox);
            }
        }


    }


}
