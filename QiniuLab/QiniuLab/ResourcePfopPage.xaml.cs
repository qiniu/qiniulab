using Qiniu.Processing;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace QiniuLab
{
    /// <summary>
    /// Interaction logic for ResourcePfopPage.xaml
    /// </summary>
    public partial class ResourcePfopPage : Page
    {
        private string prefopResultTemplate;
        public ResourcePfopPage()
        {
            InitializeComponent();
            this.init();
        }

        private void init()
        {
            using (StreamReader sr = new StreamReader("Template/JsonFormat.html"))
            {
                prefopResultTemplate=sr.ReadToEnd();
            }
        }

        private void PfopButton_Click(object sender, RoutedEventArgs e)
        {
            string bucket = this.BucketTextBox.Text.Trim();
            string key = this.KeyTextBox.Text.Trim();
            string fops = this.FopsTextBox.Text.Trim();
            if (bucket.Length == 0 || key.Length == 0 || fops.Length == 0)
            {
                return;
            }
            string pipeline = this.PipelineTextBox.Text.Trim();
            bool force = this.ForceCheckBox.IsChecked.Value;
            string notifyURL = this.NotifyURLTextBox.Text.Trim();

            Mac mac = new Mac(QiniuLab.AppSettings.Default.ACCESS_KEY,
                QiniuLab.AppSettings.Default.SECRET_KEY);
            Pfop pfop = new Pfop(mac, bucket, key, fops);
            pfop.Pipeline = pipeline;
            pfop.Force = force;
            pfop.NotifyURL = notifyURL;
            PfopResult pfopResult = pfop.pfop();
            if (pfopResult.PersistentId != null)
            {
                this.PersistentIdTextBox.Text = pfopResult.PersistentId;
            }
            this.PfopResponseTextBox.Text = pfopResult.Response;
            this.PfopResponseInfoTextBox.Text = pfopResult.ResponseInfo.ToString();
        }

        private void PrefopButton_Click(object sender, RoutedEventArgs e)
        {
            string persistentId = this.PersistentIdTextBox.Text.Trim();
            if (persistentId.Length == 0) {
                return;
            }
            Prefop prefop = new Prefop(persistentId);
            PrefopResult result = prefop.prefop();
            this.PrefopResponseTextBox.Text = result.Response;
            this.PrefopResponseInfoTextBox.Text = result.ResponseInfo.ToString();
            string formattedResponse = this.prefopResultTemplate.Replace("#{RESPONSE}#", result.Response);
            this.PrefopFormatResponseWebBrowser.NavigateToString(formattedResponse);
        }

        private void OnPersistentId_Changed(object sender, TextChangedEventArgs e)
        {
            this.PrefopResponseInfoTextBox.Text = "";
            this.PrefopResponseTextBox.Text = "";
        }
    }
}
