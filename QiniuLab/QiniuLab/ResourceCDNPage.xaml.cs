using System.Windows;
using System.Windows.Controls;
using Qiniu.Common;
using Qiniu.CDN;
using Qiniu.CDN.Model;

namespace QiniuLab
{
    /// <summary>
    /// ResourceCDNPage.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceCDNPage : Page
    {
        private FusionManager fusionMgr;

        public ResourceCDNPage()
        {
            InitializeComponent();

            TryInit();        
        }

        private bool TryInit()
        {
            if (fusionMgr != null)
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
                fusionMgr = new FusionManager(mac);
                return true;
            }
        }

        private void Button_FusionRefresh_Click(object sender, RoutedEventArgs e)
        {
            if(!TryInit())
            {
                return;
            }

            string rURLs = TextBox_FusionRefreshURLs.Text.Trim();
            string rDIRs = TextBox_FusionRefreshDIRs.Text.Trim();

            RefreshRequest request = new RefreshRequest();
            request.AddUrls(rURLs.Split(';'));
            request.AddDirs(rDIRs.Split(';'));
            var result = fusionMgr.refresh(request);

            TextBox_FusionRefreshResponse.Text = result.ToString();
        }

        private void Button_FusionPrefetch_Click(object sender, RoutedEventArgs e)
        { 
            if (!TryInit())
            {
                return;
            }

            string rURLs = TextBox_FusionPrefetchURLs.Text.Trim();

            PrefetchRequest request = new PrefetchRequest();
            request.AddUrls(rURLs.Split(';'));
            var result = fusionMgr.prefetch(request);

            TextBox_FusionPrefetchResponse.Text = result.ToString();
        }

        private void Button_FusionBandwidth_Click(object sender, RoutedEventArgs e)
        {           
            if (!TryInit())
            {
                return;
            }

            string domains = TextBox_FusionBandwidthDomains.Text.Trim();
            string startDate = TextBox_FusionBandwidthStartDate.Text.Trim();
            string endDate = TextBox_FusionBandwidthEndDate.Text.Trim();
            string granularity = TextBox_FusionBandwidthGranularity.Text.Trim();

            BandwidthRequest request = new BandwidthRequest(startDate,endDate,granularity,domains);
            var result = fusionMgr.bandwidth(request);

            TextBox_FusionBandwidthResponse.Text = result.ToString();
        }

        private void Button_FusionFlux_Click(object sender, RoutedEventArgs e)
        {
            if (!TryInit())
            {
                return;
            }

            string domains = TextBox_FusionBandwidthDomains.Text.Trim();
            string startDate = TextBox_FusionBandwidthStartDate.Text.Trim();
            string endDate = TextBox_FusionBandwidthEndDate.Text.Trim();
            string granularity = TextBox_FusionBandwidthGranularity.Text.Trim();

            FluxRequest request = new FluxRequest(startDate, endDate, granularity, domains);
            var result = fusionMgr.flux(request);

            TextBox_FusionFluxResponse.Text = result.ToString();
        }

        private void Button_FusionLoglist_Click(object sender, RoutedEventArgs e)
        {
            if (!TryInit())
            {
                return;
            }

            string domains = TextBox_FusionLoglistDomains.Text.Trim();
            string day = TextBox_FusionLoglistDay.Text.Trim();

            LogListRequest request = new LogListRequest(day, domains);
            var result = fusionMgr.logList(request);

            TextBox_FusionLogListResponse.Text = result.ToString();
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
            string timestamp = TextBox_FusionHotlinkTimestamp.Text.Trim();
            string key = TextBox_FusionHotlinkQiniuKey.Text.Trim();

            HotLinkRequest request = new HotLinkRequest();
            request.Host = host;
            request.Path = path;
            request.File = file;
            request.Query = query;
            request.Timestamp = timestamp;
            request.Key=key;

            var result = fusionMgr.hotLink(request);

            TextBox_FusionHotLinkResponse.Text = result.ToString();
        }

    }
}
