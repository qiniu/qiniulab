using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Qiniu.Common;
using Qiniu.RSF;
using Qiniu.RSF.Model;

namespace QiniuLab
{
    /// <summary>
    /// Interaction logic for ResourcePfopPage.xaml
    /// </summary>
    public partial class ResourcePfopPage : Page
    {
        //private string prefopResultTemplate;
        public ResourcePfopPage()
        {
            InitializeComponent();
            //this.init();
        }

        //private void init()
        //{
        //    using (StreamReader sr = new StreamReader("Template/JsonFormat.html"))
        //    {
        //        prefopResultTemplate = sr.ReadToEnd();
        //    }
        //}

        private void PfopButton_Click(object sender, RoutedEventArgs e)
        {
            string bucket = this.BucketTextBox.Text.Trim();
            string key = this.KeyTextBox.Text.Trim();
            string fops = this.FopsTextBox.Text.Trim();
            if (bucket.Length == 0 || key.Length == 0 || fops.Length == 0)
            {
                return;
            }
            #region FIX_PFOP_ZONE_CONFIG
            try
            {
                Config.autoZone(AppSettings.Default.ACCESS_KEY, bucket, false);
                this.PfopResponseTextBox.Clear();
            }
            catch (Exception ex)
            {
                this.PfopResponseTextBox.Text = "配置出错，请检查您的输入(如密钥/scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_PFOP_ZONE_CONFIG
            string pipeline = this.PipelineTextBox.Text.Trim();
            bool force = this.ForceCheckBox.IsChecked.Value;
            string notifyURL = this.NotifyURLTextBox.Text.Trim();

            Task.Factory.StartNew(() =>
            {
                Mac mac = new Mac(AppSettings.Default.ACCESS_KEY,AppSettings.Default.SECRET_KEY);
                OperationManager ox = new OperationManager(mac);
                PfopResult pfopResult = ox.pfop(bucket, key, fops.Split(';'), pipeline, notifyURL, force);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (pfopResult.PersistentId != null)
                    {
                        this.PersistentIdTextBox.Text = pfopResult.PersistentId;
                    }

                    this.PfopResponseTextBox.Text = pfopResult.Text;
                    this.PfopResponseInfoTextBox.Text = pfopResult.ToString();
                }));
            });
        }

        private void PrefopButton_Click(object sender, RoutedEventArgs e)
        {
            string persistentId = this.PersistentIdTextBox.Text.Trim();
            if (persistentId.Length == 0)
            {
                return;
            }
            Task.Factory.StartNew(() =>
            {
                Mac mac = new Mac(AppSettings.Default.ACCESS_KEY, AppSettings.Default.SECRET_KEY);
                OperationManager ox = new OperationManager(mac);
                var result = ox.prefop(persistentId);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.PrefopResponseTextBox.Text = result.Text;
                    this.PrefopResponseInfoTextBox.Text = result.ToString();
                }));
            });
        }

        private void OnPersistentId_Changed(object sender, TextChangedEventArgs e)
        {
            this.PrefopResponseInfoTextBox.Text = "";
            this.PrefopResponseTextBox.Text = "";
        }
    }
}
