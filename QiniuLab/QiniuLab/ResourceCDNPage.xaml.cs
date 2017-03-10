using System.Windows;
using System.Windows.Controls;
using Qiniu.Util;
using Qiniu.CDN;
using Qiniu.CDN.Model;
using System.Collections.Generic;

namespace QiniuLab
{
    /// <summary>
    /// ResourceCDNPage.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceCDNPage : Page
    {
        private CdnManager cdnManager;

        private List<string> refreshUrls = new List<string>();
        private List<string> refreshDirs = new List<string>();
        private List<string> prefetchUrls = new List<string>();

        public ResourceCDNPage()
        {
            InitializeComponent();

            TryInit();        
        }

        private bool TryInit()
        {
            if (cdnManager != null)
            {
                return true;
            }

            string ak = AppSettings.Default.ACCESS_KEY;
            string sk = AppSettings.Default.SECRET_KEY;

            if (string.IsNullOrEmpty(ak) || ak.Length < 40 ||
                string.IsNullOrEmpty(sk) || sk.Length < 40)
            {
                return false;
            }
            else
            {
                Mac mac = new Mac(ak, sk);
                cdnManager = new CdnManager(mac);
                return true;
            }
        }

        private void Button_FusionRefreshUrls_Click(object sender, RoutedEventArgs e)
        {
            if(refreshUrls.Count<1)
            {
                TextBox_FusionRefreshUrlsResultText.Text = "请添加URL";
                return;
            }

            if(!TryInit())
            {
                return;
            }

            var result = cdnManager.RefreshUrls(refreshUrls.ToArray());

            TextBox_FusionRefreshUrlsResultText.Text = result.Text;
            TextBox_FusionRefreshUrlsResultString.Text = result.ToString();
        }

        private void Button_FusionRefreshDirs_Click(object sender, RoutedEventArgs e)
        {
            if (refreshDirs.Count < 1)
            {
                TextBox_FusionRefreshDirsResultText.Text = "请添加URL";
                return;
            }

            if (!TryInit())
            {
                return;
            }

            var result = cdnManager.RefreshDirs(refreshDirs.ToArray());

            TextBox_FusionRefreshDirsResultText.Text = result.Text;
            TextBox_FusionRefreshDirsResultString.Text = result.ToString();
        }

        private void Button_FusionPrefetchUrls_Click(object sender, RoutedEventArgs e)
        {
            if (prefetchUrls.Count < 1)
            {
                TextBox_FusionPrefetchUrlsResultText.Text = "请添加URL";
                return;
            }

            if (!TryInit())
            {
                return;
            }

            string rURLs = TextBox_FusionPrefetchURLs.Text.Trim();

            PrefetchRequest request = new PrefetchRequest();
            request.AddUrls(rURLs.Split(';'));
            var result = cdnManager.PrefetchUrls(request);

            TextBox_FusionPrefetchUrlsResultText.Text = result.Text;
            TextBox_FusionPrefetchUrlsResultString.Text = result.ToString();
        }

        private void Button_FusionBandwidth_Click(object sender, RoutedEventArgs e)
        {           
            if (!TryInit())
            {
                return;
            }

            string domains = TextBox_FusionBandwidthDomains.Text.Trim();

            if(string.IsNullOrEmpty(domains))
            {
                TextBox_FusionBandwidthResultText.Text = "请填写domain";
                return;
            }

            string startDate = TextBox_FusionBandwidthStartDate.Text.Trim();

            if (string.IsNullOrEmpty(startDate))
            {
                TextBox_FusionBandwidthResultText.Text = "请填写startDate";
                return;
            }

            string endDate = TextBox_FusionBandwidthEndDate.Text.Trim();

            if (string.IsNullOrEmpty(endDate))
            {
                TextBox_FusionBandwidthResultText.Text = "请填写endDate";
                return;
            }

            string granularity = TextBox_FusionBandwidthGranularity.Text.Trim();

            if (string.IsNullOrEmpty(granularity))
            {
                TextBox_FusionBandwidthResultText.Text = "请填写granularity";
                return;
            }

            BandwidthRequest request = new BandwidthRequest(startDate,endDate,granularity,domains);
            var result = cdnManager.GetBandwidthData(request);

            TextBox_FusionBandwidthResultText.Text = result.Text;
            TextBox_FusionBandwidthResultString.Text = result.ToString();
        }

        private void Button_FusionFlux_Click(object sender, RoutedEventArgs e)
        {
            if (!TryInit())
            {
                return;
            }

            string domains = TextBox_FusionFluxDomains.Text.Trim();

            if (string.IsNullOrEmpty(domains))
            {
                TextBox_FusionFluxResultText.Text = "请填写domain";
                return;
            }

            string startDate = TextBox_FusionFluxStartDate.Text.Trim();

            if (string.IsNullOrEmpty(startDate))
            {
                TextBox_FusionFluxResultText.Text = "请填写startDate";
                return;
            }

            string endDate = TextBox_FusionFluxEndDate.Text.Trim();

            if (string.IsNullOrEmpty(endDate))
            {
                TextBox_FusionFluxResultText.Text = "请填写endDate";
                return;
            }

            string granularity = TextBox_FusionFluxGranularity.Text.Trim();

            if (string.IsNullOrEmpty(granularity))
            {
                TextBox_FusionFluxResultText.Text = "请填写granularity";
                return;
            }

            FluxRequest request = new FluxRequest(startDate, endDate, granularity, domains);
            var result = cdnManager.GetFluxData(request);

            TextBox_FusionFluxResultText.Text = result.Text;
            TextBox_FusionFluxResultString.Text = result.ToString();
        }

        private void Button_FusionLoglist_Click(object sender, RoutedEventArgs e)
        {
            if (!TryInit())
            {
                return;
            }

            string domains = TextBox_FusionLoglistDomains.Text.Trim();

            if (string.IsNullOrEmpty(domains))
            {
                TextBox_FusionLogListResultText.Text = "请填写domain";
                return;
            }

            string day = TextBox_FusionLoglistDay.Text.Trim();

            if (string.IsNullOrEmpty(day))
            {
                TextBox_FusionLogListResultText.Text = "请填写day";
                return;
            }

            LogListRequest request = new LogListRequest(day, domains);
            var result = cdnManager.GetCdnLogList(request);

            TextBox_FusionLogListResultText.Text = result.Text;
            TextBox_FusionLogListResultString.Text = result.ToString();
        }

        private void Button_Hotlink_Click(object sender, RoutedEventArgs e)
        {
            if (!TryInit())
            {
                return;
            }

            string host = TextBox_FusionHotlinkHost.Text.Trim();
            string path = TextBox_FusionHotlinkPath.Text.Trim();
            string file = TextBox_FusionHotlinkFile.Text.Trim();
            string query = TextBox_FusionHotlinkQuery.Text.Trim();
            string expire = TextBox_FusionHotlinkExpire.Text.Trim();
            string key = TextBox_FusionHotlinkQiniuKey.Text.Trim();

            if(string.IsNullOrEmpty(host))
            {
                TextBox_FusionHotLinkText.Text = "请填写host";
                return;
            }

            if(string.IsNullOrEmpty(expire))
            {                
                expire = "3600";
                TextBox_FusionHotlinkExpire.Text = expire;
            }

            var request = new TimestampAntiLeechUrlRequest();
            request.Host = host;
            request.Path = path;
            request.File = file;
            request.Query = query;
            request.Timestamp = UnixTimestamp.GetUnixTimestamp(long.Parse(expire)).ToString();
            request.Key=key;

            string rawUrl = host + path + file + query;
            if(!UrlHelper.IsValidUrl(rawUrl))
            {
                TextBox_FusionHotLinkText.Text = string.Format("不符合格式的URL： {0}", rawUrl);
                return;
            }

            if(string.IsNullOrEmpty(key))
            {
                TextBox_FusionHotLinkText.Text = string.Format("请填写key");
                return;
            }

            var result = cdnManager.CreateTimestampAntiLeechUrl(request);

            TextBox_FusionHotLinkText.Text = result;
        }

        private void Button_RemoveRefreshUrls_Click(object sender, RoutedEventArgs e)
        {
            string ss = "";

            var selected = ListBox_FusionRefreshURLs.SelectedItems;
            if(selected!=null && selected.Count>0)
            {
                foreach(var s in selected)
                {
                    refreshUrls.Remove(s.ToString());
                    ss += s.ToString() + "\n";
                }
            }

            ListBox_FusionRefreshURLs.Items.Clear();
            foreach(string url in refreshUrls)
            {
                ListBox_FusionRefreshURLs.Items.Add(url);
            }

            TextBox_FusionRefreshUrlsResultText.Text = ss;
        }

        private void Button_AddRefreshUrl_Click(object sender, RoutedEventArgs e)
        {
            string url = TextBox_FusionRefreshURL.Text.Trim();

            if(string.IsNullOrEmpty(url))
            {
                TextBox_FusionRefreshUrlsResultText.Text = "请填写URL";
                return;
            }

            if(!UrlHelper.IsValidUrl(url))
            {
                TextBox_FusionRefreshUrlsResultText.Text = "URL格式不正确: " + url;
                return;
            }

            if(refreshUrls.Contains(url))
            {
                TextBox_FusionRefreshUrlsResultText.Text = "已添加过的URL: " + url;
                return;
            }

            if (refreshUrls.Count >= 100)
            {
                TextBox_FusionRefreshUrlsResultText.Text = "添加的URL已达上限";
                return;
            }

            TextBox_FusionRefreshUrlsResultText.Clear();

            refreshUrls.Add(url);
            ListBox_FusionRefreshURLs.Items.Add(url);
        }

        private void Button_AddRefreshUrlsFromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string localFile = ofd.FileName;
                    TextBox_URLFileName.Text = localFile;

                    int i = 0;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(localFile))
                    {
                        int n = 100 - refreshUrls.Count;
                        while (i < n)
                        {
                            if (sr.EndOfStream)
                            {
                                break;
                            }
                            string line = sr.ReadLine();
                            if (UrlHelper.IsValidUrl(line))
                            {
                                refreshUrls.Add(line);
                                ++i;
                            }
                        }
                    }

                    TextBox_FusionRefreshUrlsResultText.Text = "文件读取完毕, 已找到URL数量: " + i;
                }   
            }
            catch(System.Exception ex)
            {
                TextBox_FusionRefreshUrlsResultText.Text = ex.Message;
                TextBox_FusionRefreshUrlsResultString.Text = ex.StackTrace;
            }
        }

        private void Button_AddFusionRefreshDir_Click(object sender, RoutedEventArgs e)
        {
            string dir = TextBox_FusionRefreshDIRs.Text.Trim();

            if (string.IsNullOrEmpty(dir))
            {
                TextBox_FusionRefreshUrlsResultText.Text = "请填写URL";
                return;
            }

            if (!UrlHelper.IsValidDir(dir))
            {
                TextBox_FusionRefreshUrlsResultText.Text = "URL格式不正确: " + dir;
                return;
            }

            if (refreshDirs.Contains(dir))
            {
                TextBox_FusionRefreshUrlsResultText.Text = "已添加过的URL: " + dir;
                return;
            }

            if (refreshDirs.Count >= 10)
            {
                TextBox_FusionRefreshDirsResultText.Text = "添加的URL已达上限";
                return;
            }

            TextBox_FusionRefreshDirsResultText.Clear();

            refreshDirs.Add(dir);
            ListBox_FusionRefreshDIRs.Items.Add(dir);
        }

        private void Button_AddDirsFromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string localFile = ofd.FileName;
                    TextBox_DirsFileName.Text = localFile;

                    int i = 0;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(localFile))
                    {
                        int n = 10 - refreshDirs.Count;
                        while (i < n)
                        {
                            if (sr.EndOfStream)
                            {
                                break;
                            }
                            string line = sr.ReadLine();
                            if (UrlHelper.IsValidDir(line))
                            {
                                refreshDirs.Add(line);
                                ++i;
                            }
                        }
                    }

                    TextBox_FusionRefreshDirsResultText.Text = "文件读取完毕, 已找到URL数量: " + i;
                }
            }
            catch (System.Exception ex)
            {
                TextBox_FusionRefreshDirsResultText.Text = ex.Message;
                TextBox_FusionRefreshDirsResultString.Text = ex.StackTrace;
            }
        }

        private void Button_AddPrefetchUrls_Click(object sender, RoutedEventArgs e)
        {
            string url = TextBox_FusionPrefetchURLs.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                TextBox_FusionPrefetchUrlsResultText.Text = "请填写URL";
                return;
            }

            if (!UrlHelper.IsValidUrl(url))
            {
                TextBox_FusionPrefetchUrlsResultText.Text = "URL格式不正确: " + url;
                return;
            }

            if (prefetchUrls.Contains(url))
            {
                TextBox_FusionPrefetchUrlsResultText.Text = "已添加过的URL: " + url;
                return;
            }

            if (prefetchUrls.Count >= 100)
            {
                TextBox_FusionPrefetchUrlsResultText.Text = "添加的URL已达上限";
                return;
            }

            TextBox_FusionPrefetchUrlsResultText.Clear();

            prefetchUrls.Add(url);
            ListBox_FusionPrefetchUrls.Items.Add(url);
        }

        private void Button_AddPrefetchUrlsFromFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string localFile = ofd.FileName;
                    TextBox_PrefetchUrlsFileName.Text = localFile;

                    int i = 0;
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(localFile))
                    {
                        int n = 100 - prefetchUrls.Count;
                        while (i < n)
                        {
                            if (sr.EndOfStream)
                            {
                                break;
                            }
                            string line = sr.ReadLine();
                            if (UrlHelper.IsValidDir(line))
                            {
                                prefetchUrls.Add(line);
                                ++i;
                            }
                        }
                    }

                    TextBox_FusionPrefetchUrlsResultText.Text = "文件读取完毕, 已找到URL数量: " + i;
                }
            }
            catch (System.Exception ex)
            {
                TextBox_FusionPrefetchUrlsResultText.Text = ex.Message;
                TextBox_FusionPrefetchUrlsResultString.Text = ex.StackTrace;
            }
        }

        private void Button_RemovePrefetchUrls_Click(object sender, RoutedEventArgs e)
        {
            string ss = "";

            var selected = ListBox_FusionRefreshURLs.SelectedItems;
            if (selected != null && selected.Count > 0)
            {
                foreach (var s in selected)
                {
                    refreshUrls.Remove(s.ToString());
                    ss += s.ToString() + "\n";
                }
            }

            ListBox_FusionRefreshURLs.Items.Clear();
            foreach (string url in refreshUrls)
            {
                ListBox_FusionRefreshURLs.Items.Add(url);
            }

            TextBox_FusionRefreshUrlsResultText.Text = ss;
        }

        private void Button_RemoveRefreshDirs_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
