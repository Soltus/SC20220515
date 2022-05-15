using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace 绛亽束彖园络管理系统
{
    /// <summary>
    /// webv_page1.xaml 的交互逻辑
    /// </summary>
    public partial class webv_page1 : Page
    {
        public webv_page1()
        {
            InitializeComponent();
            this.SizeChanged += new System.Windows.SizeChangedEventHandler(webv_page1_Resize);
            webview2.CoreWebView2InitializationCompleted += (s, e) => { webview2_Corewebview2InitializationCompleted(s, e); };
        }

        private void webv_page1_Resize(object sender, System.EventArgs e)
        {
            webview2.Width = this.ActualWidth - 1;
            webview2.Height = this.ActualHeight - 1;
        }

        // 右键菜单
        private void CoreWebView2_ContextMenuRequested(object? sender, CoreWebView2ContextMenuRequestedEventArgs args)
        {
            IList<CoreWebView2ContextMenuItem> allMenuList = args.MenuItems;

            CoreWebView2Deferral de = args.GetDeferral();
            args.Handled = true;
            ContextMenu cm = new();
            cm.Closed += (s, ex) => de.Complete();
            PopulateContextMenu(args, allMenuList, cm);
            cm.IsOpen = true;
        }

        // 以WPF形式展示菜单
        private void PopulateContextMenu(CoreWebView2ContextMenuRequestedEventArgs args, IList<CoreWebView2ContextMenuItem> menulist, ItemsControl cm)
        {
            var bc = new BrushConverter();
            cm.Opacity = 0.97;
            cm.FontSize = 16;
            cm.Foreground = (Brush)bc.ConvertFrom("#449");

            MenuItem newItem1 = new()
            {
                Header = "海文东提供技术支持",
                IsEnabled = false
            };
            cm.Items.Add(newItem1);

            MenuItem newItem3 = new()
            {
                Header = $"Microsoft Edge {webview2.CoreWebView2.Environment.BrowserVersionString}",
                IsEnabled = false
            };
            cm.Items.Add(newItem3);

            // 添加分割线
            Separator sep1 = new();
            cm.Items.Add(sep1);

            string pageUri = args.ContextMenuTarget.PageUri;
            MenuItem newItem2 = new()
            {
                Header = "默认浏览器打开当前网页",
                IsEnabled = true
            };
            newItem2.Click += (s, ex) => { System.Diagnostics.Process.Start("explorer.exe", pageUri); };
            cm.Items.Add(newItem2);

            MenuItem newItem4 = new()
            {
                Header = "返回首页",
                IsEnabled = true
            };
            newItem4.Click += (s, ex) => { webview2.CoreWebView2.Navigate("https://cn.bing.com"); };
            cm.Items.Add(newItem4);


            MenuItem newItem8 = new()
            {
                Header = "前进",
                IsEnabled = webview2.CoreWebView2.CanGoForward
            };
            newItem8.Click += (s, ex) => { webview2.CoreWebView2.GoForward(); };
            cm.Items.Add(newItem8);

            MenuItem newItem9 = new()
            {
                Header = "后退",
                IsEnabled = webview2.CoreWebView2.CanGoBack
            };
            newItem9.FontSize = 18;
            newItem9.Click += (s, ex) => { webview2.CoreWebView2.GoBack(); };
            cm.Items.Add(newItem9);

            MenuItem newItem10 = new()
            {
                Header = "导航到",
                IsEnabled = true
            };
            newItem10.Click += (s, ex) =>
            {
                GetUserInput wd = new();
                wd.id.Focus();
                wd.id.SelectAll();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    try
                    {
                        if (wd.Url.ToLower().StartsWith("http")) 
                        webview2.CoreWebView2.Navigate(wd.Url);
                        else webview2.CoreWebView2.Navigate($"http://{wd.Url}");
                    }
                    catch (Exception e) { webview2.CoreWebView2.ExecuteScriptAsync($"alert('{e.Message}')"); }
                }
            };
            cm.Items.Add(newItem10);

            MenuItem newItem11 = new()
            {
                Header = "脚本注入",
                IsEnabled = true
            };
            newItem11.Click += async (s, ex) =>
            {
                GetUserInput wd = new();
                wd.id.Text = "window.getComputedStyle(document.body).backgroundColor";
                wd.id.Focus();
                wd.id.SelectAll();
                wd.ShowDialog();
                while (wd.DialogResult == true)
                {
                    try
                    {
                        var repo = await webview2.CoreWebView2.ExecuteScriptAsync(wd.Url);
                        if (repo != null && repo != "null")
                            MessageBox.Show(repo);
                        break;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        wd.ShowDialog();
                    }
                }
            };
            cm.Items.Add(newItem11);


            MenuItem newItem_more = new()
            {
                Header = "更多选项",
                IsEnabled = true
            };
            newItem_more.MouseMove += (s, ex) => { newItem_more.IsSubmenuOpen = true; };
            cm.Items.Add(newItem_more);

            MenuItem newItem5 = new()
            {
                Header = "打开最近下载",
                IsEnabled = !webview2.CoreWebView2.IsDefaultDownloadDialogOpen
            };
            newItem5.Click += (s, ex) => { webview2.CoreWebView2.OpenDefaultDownloadDialog(); };
            newItem_more.Items.Add(newItem5);

            MenuItem newItem6 = new()
            {
                Header = "打开调试工具",
                IsEnabled = true
            };
            newItem6.Click += (s, ex) => { webview2.CoreWebView2.OpenDevToolsWindow(); };
            newItem_more.Items.Add(newItem6);

            MenuItem newItem7 = new()
            {
                Header = "打开任务管理",
                IsEnabled = true
            };
            newItem7.Click += (s, ex) => { webview2.CoreWebView2.OpenTaskManagerWindow(); };
            newItem_more.Items.Add(newItem7);

            MenuItem newItem101 = new()
            {
                Header = "清除所有 Cookie",
                IsEnabled = true
            };
            newItem101.Click += (s, ex) => { webview2.CoreWebView2.CookieManager.DeleteAllCookies(); };
            newItem_more.Items.Add(newItem101);

            // 添加分割线
            Separator sep2 = new();
            newItem_more.Items.Add(sep2);

            // 添加分割线
            //var newItem0 = webview2.CoreWebView2.Environment.CreateContextMenuItem(Label: "", iconStream: null, CoreWebView2ContextMenuItemKind.Separator);
            //menulist.Insert(0, newItem0);

            for (int i = 0; i < menulist.Count; i++)
            {
                CoreWebView2ContextMenuItem current = menulist[i];
                if (current.Kind == CoreWebView2ContextMenuItemKind.Separator)
                {
                    Separator sep = new Separator();
                    newItem_more.Items.Add(sep);
                    continue;
                }
                MenuItem newItem = new MenuItem();
                // The accessibility key is the key after the & in the label
                // Replace with '_' so it is underlined in the label
                newItem.Header = current.Label.Replace('&', '_');
                newItem.InputGestureText = current.ShortcutKeyDescription;
                newItem.IsEnabled = current.IsEnabled;
                if (current.Kind == CoreWebView2ContextMenuItemKind.Submenu)
                {
                    PopulateContextMenu(args, current.Children, newItem);
                }
                else
                {
                    if (current.Kind == CoreWebView2ContextMenuItemKind.CheckBox
                    || current.Kind == CoreWebView2ContextMenuItemKind.Radio)
                    {
                        newItem.IsCheckable = true;
                        newItem.IsChecked = current.IsChecked;
                    }

                    newItem.Click += (s, ex) =>
                    {
                        args.SelectedCommandId = current.CommandId;
                    };
                }
                newItem_more.Items.Add(newItem);
            }

        }

        private void webview2_Corewebview2InitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess == false)
            {
                MessageBox.Show("webview2_Corewebview2InitializationCompleted 事件，发生异常。");
                return;
            }
            WebView2Controlers.WV2 = webview2;
            webview2.SourceChanged += WebView2_SourceChanged;
            webview2.ContentLoading += WebView2_ContentLoading;
            webview2.CoreWebView2.HistoryChanged += CoreWebView2_HistoryChanged;
            webview2.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            //webview2.CoreWebView2.BasicAuthenticationRequested += CoreWebView2_BasicAuthenticationRequested;
            webview2.CoreWebView2.ProcessFailed += CoreWebView2_ProcessFailed;
            webview2.CoreWebView2.NewWindowRequested += abc;
            webview2.CoreWebView2.NavigationStarting += WebView2_NavigationStarting;
            webview2.CoreWebView2.NavigationCompleted += WebView2_NavigationCompleted;
            webview2.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
            webview2.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            webview2.CoreWebView2.DefaultDownloadDialogCornerAlignment = CoreWebView2DefaultDownloadDialogCornerAlignment.TopLeft; // 最近下载的打开位置
        }

        private void CoreWebView2_DocumentTitleChanged(object? sender, object e)
        {
            WinTitle.窗口标题 = webview2.CoreWebView2.DocumentTitle;
        }

        async private void abc(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show($"非必要，不新建。\n\n【确定】-> 新窗口打开 \n【取消】-> 当前页面打开", "是否打开新窗口", MessageBoxButton.OKCancel);
            //if (result == MessageBoxResult.Cancel)
            //{
            //    webview2.Source = new Uri(e.Uri.ToString());
            //    webview2.CoreWebView2.AddHostObjectToScript("bridge1", new Bridge1());
            //    e.Handled = true;//禁止弹窗
            //}

            //Update_frame1_NewWindow.Navigate(e); //还没实现好，无法后退

            webview2.Source = new Uri(e.Uri.ToString());
            e.Handled = true;//禁止弹窗
        }



        private async void WebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            webview2.Visibility = Visibility.Visible;
            //object obj = await webview2.CoreWebView2.ExecuteScriptAsync("document.title");
            //object obj2 = await webview2.CoreWebView2.ExecuteScriptAsync("document.charset");
            //WinTitle.窗口标题 = $"({obj2.ToString().Trim('"')}) {obj.ToString().Trim('"')}  [{e.NavigationId}]";
            WinTitle.窗口标题 = webview2.CoreWebView2.DocumentTitle;
            if (!webview2.Source.AbsoluteUri.StartsWith("http"))
            {
                WinTitle.UriString = webview2.Source.OriginalString; // 不是网页
            }
            else
            {
                string URL = webview2.Source.Scheme + "://" + webview2.Source.Host + webview2.Source.LocalPath; // 不用转换中文
                if (webview2.Source.Port != 80 && webview2.Source.Port != 443)
                {
                    URL = webview2.Source.Scheme + "://" + webview2.Source.Host + $":{webview2.Source.Port}" + webview2.Source.LocalPath;
                }
                WinTitle.UriString = URL;
                string url = $"{webview2.Source.Scheme + "://" + webview2.Source.Host}/favicon.ico";
                if (PublicFunctions.RemoteFileExists(url))
                    WinTitle.Bicon = BitmapFrame.Create(new Uri(url), BitmapCreateOptions.None, BitmapCacheOption.Default);
                else WinTitle.Bicon = null;
            }
            WebView2Controlers.logger.Info(WinTitle.UriString);
        }


        private void WebView2_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            webview2.Visibility = Visibility.Collapsed;
            webview2.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync
                ($"document.bgColor=\"#dee3f4\";");
            WinTitle.窗口标题 = $"{e.Uri}";
            string url = $"{webview2.Source.Scheme + "://" + webview2.Source.Host}/favicon.ico";
            if (PublicFunctions.RemoteFileExists(url))
                WinTitle.Bicon = BitmapFrame.Create(new Uri(url), BitmapCreateOptions.None, BitmapCacheOption.Default);
            else WinTitle.Bicon = null;
        }
        // SourceChanged 在 NavigationStarting 之后
        private void WebView2_SourceChanged(object? sender, CoreWebView2SourceChangedEventArgs e)
        {
            //webview2.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync
            //    ("window.onload=()=>{alert(\"？\");};");
        }

        async private void WebView2_ContentLoading(object? sender, CoreWebView2ContentLoadingEventArgs e)
        {
            WinTitle.窗口标题 = $"{e.NavigationId} : loading...";
            //MessageBox.Show("WebView2_ContentLoading 事件。"
            //              + Environment.NewLine + "e.IsErrorPage = " + e.IsErrorPage,
            //                "提示");
        }

        private void CoreWebView2_HistoryChanged(object? sender, object e)
        {
            
        }

        private void CoreWebView2_DOMContentLoaded(object? sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            webview2.Visibility = Visibility.Visible;
        }



        private void CoreWebView2_ProcessFailed(object? sender, CoreWebView2ProcessFailedEventArgs e)
        {
            webview2.Visibility = Visibility.Visible;
            MessageBox.Show("CoreWebView2_ProcessFailed 事件。"
                          + Environment.NewLine + "e.ExitCode = " + e.ExitCode
                          + Environment.NewLine + "e.FrameInfosForFailedProcess = " + e.FrameInfosForFailedProcess
                          + Environment.NewLine + "e.ProcessDescription = " + e.ProcessDescription
                          + Environment.NewLine + "e.ProcessFailedKind = " + e.ProcessFailedKind
                          + Environment.NewLine + "e.Reason = " + e.Reason,
                          "提示");
        }


        private void CoreWebView2_BasicAuthenticationRequested(object? sender, CoreWebView2BasicAuthenticationRequestedEventArgs e)
        {
            MessageBox.Show("CoreWebView2_BasicAuthenticationRequested 事件。"
                          + Environment.NewLine + "e.Uri = " + e.Uri
                          + Environment.NewLine + "e.Cancel = " + e.Cancel
                          + Environment.NewLine + "e.Challenge = " + e.Challenge
                          + Environment.NewLine + "e.Response = " + e.Response,
                            "提示");

        }


    }
}
