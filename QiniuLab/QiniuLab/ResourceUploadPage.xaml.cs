using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Qiniu.Common;
using Qiniu.Util;
using Qiniu.IO;
using Qiniu.IO.Model;

namespace QiniuLab
{
    /// <summary>
    /// Interaction logic for ResourceUploadPage.xaml
    /// </summary>
    public partial class ResourceUploadPage : Page
    {
        //private string jsonResultTemplate;
        private bool cancelUpload;
        private string mimeType; // 添加 @fengyh

        public ResourceUploadPage()
        {
            InitializeComponent();
            this.init();
        }

        private void init()
        {
            this.ExpireTextBox.Text = "3600";
            this.UploadKeyTextBox.IsEnabled = false;
            this.cancelUpload = false;
            this.UploadProgressBar.Visibility = Visibility.Hidden;
        }

        private void CreateTokenButton_Click(object sender, RoutedEventArgs e)
        {
            string scope = this.ScopeTextBox.Text.Trim();
            int expire = 3600;
           
            if(string.IsNullOrEmpty(scope))
            {
                TextBox_UploadResultText.Text = "配置出错，请输入正确的Scope\r\n";
                return;
            }

            bool ok = int.TryParse(this.ExpireTextBox.Text.Trim(), out expire);
            if(!ok || expire<0)
            {
                TextBox_UploadResultText.Text = "配置出错，请输入正确的expire\r\n";
                return;
            }

            int insertOnly = this.InsertOnlyCheckBox.IsChecked.Value ? 1 : 0;
            string saveKey = this.SaveKeyTextBox.Text;
            string endUser = this.EndUserTextBox.Text;
            string returnBody = this.ReturnBodyTextBox.Text;
            string callbackUrl = this.CallbackUrlTextBox.Text;
            string callbackHost = this.CallbackHostTextBox.Text;
            string callbackBodyType = "";
            int callbackBodyTypeIndex = this.CallbackBodyTypeComboBox.SelectedIndex;
            if (callbackBodyTypeIndex != -1)
            {
                ComboBoxItem item = (ComboBoxItem)this.CallbackBodyTypeComboBox.SelectedItem;
                callbackBodyType = item.Content.ToString();
            }
            string callbackBody = this.CallbackBodyTextBox.Text;
            int callbackFetchKey = this.CallbackFetchKeyCheckBox.IsChecked.Value ? 1 : 0;
            string persistentOps = this.PersistentOpsTextBox.Text;
            string persistentPipeline = this.PersistentPipelineTextBox.Text;
            string persistentNotifyUrl = this.PersistentNotifyUrlTextBox.Text;
            int fsizeLimit = -1;
            try
            {
                fsizeLimit = Convert.ToInt32(this.FsizeLimitTextBox.Text.Trim());
            }
            catch (Exception) { }
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            //
            // 如果用户设置了mimeType   
            //     mimeType = MimeTypeTextBox.Text
            //     PutPolicy.detectMime = 0
            //
            // 如果用户未设置mimeType   
            //     mimeType = application/octect-stream
            //     PutPolicy.detectMime = DetectMimeCheckBox.IsChecked
            //
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            int detectMime = 0;
            mimeType = this.MimeTypeTextBox.Text.Trim();
            if (mimeType.Length == 0)
            {
                mimeType = "application/octet-stream";
                detectMime = this.DetectMimeCheckBox.IsChecked.Value ? 1 : 0;
            }

            string mimeLimit = this.MimeLimitTextBox.Text;

            int deleteAfterDays = -1;
            bool deleteAfterDaysOK = int.TryParse(this.DeleteAfterDaysTextBox.Text.Trim(), out deleteAfterDays);

            if(deleteAfterDaysOK && deleteAfterDays<=0)
            {
                TextBox_UploadResultText.Text = "配置出错，请输入正确的deleteAfterDays\r\n";
                return;
            }

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = scope;
            putPolicy.SetExpires(expire);
            if (insertOnly != 0)
            {
                putPolicy.InsertOnly = insertOnly;
            }
            if (!string.IsNullOrEmpty(saveKey))
            {
                putPolicy.SaveKey = saveKey;
            }
            if (!string.IsNullOrEmpty(endUser))
            {
                putPolicy.EndUser = endUser;
            }
            if (!string.IsNullOrEmpty(returnBody))
            {
                putPolicy.ReturnBody = returnBody;
            }
            if (!string.IsNullOrEmpty(callbackUrl))
            {
                putPolicy.CallbackUrl = callbackUrl;
            }
            if (!string.IsNullOrEmpty(callbackHost))
            {
                putPolicy.CallbackHost = callbackHost;
            }
            if (!string.IsNullOrEmpty(callbackBody))
            {
                putPolicy.CallbackBody = callbackBody;
            }
            if (!string.IsNullOrEmpty(callbackBodyType))
            {
                putPolicy.CallbackBodyType = callbackBodyType;
            }
            if (callbackFetchKey != 0)
            {
                putPolicy.CallbackFetchKey = callbackFetchKey;
            }
            if (!string.IsNullOrEmpty(persistentOps))
            {
                putPolicy.PersistentOps = persistentOps;
            }
            if (!string.IsNullOrEmpty(persistentPipeline))
            {
                putPolicy.PersistentPipeline = persistentPipeline;
            }
            if (!string.IsNullOrEmpty(persistentNotifyUrl))
            {
                putPolicy.PersistentNotifyUrl = persistentNotifyUrl;
            }
            if (fsizeLimit > 0)
            {
                putPolicy.FsizeLimit = fsizeLimit;
            }
            if (!string.IsNullOrEmpty(mimeLimit))
            {
                putPolicy.MimeLimit = mimeLimit;
            }
            if (detectMime != 0)
            {
                putPolicy.DetectMime = detectMime;
            }
            if (deleteAfterDays > 0)
            {
                putPolicy.DeleteAfterDays = deleteAfterDays;
            }
            string accessKey = QiniuLab.AppSettings.Default.ACCESS_KEY;
            string secretKey = QiniuLab.AppSettings.Default.SECRET_KEY;
            #region FIX_ADD_ZONE_CONFIG
            try
            {
                string bucket = scope;
                int pos = scope.IndexOf(':');
                if (pos > 0)
                {
                    bucket = scope.Remove(pos);
                }

                Config.AutoZone(AppSettings.Default.ACCESS_KEY, bucket, false);
                TextBox_UploadResultText.Clear();
            }
            catch (Exception ex)
            {
                TextBox_UploadResultText.Text = "配置出错，请检查您的输入(如密钥/scope/bucket等)\r\n" + ex.Message;
                return;
            }
            #endregion FIX_ADD_ZONE_CONFIG
            Mac mac = new Mac(accessKey, secretKey);
            string uploadToken = Qiniu.Util.Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            this.UploadTokenTextBox.Text = uploadToken;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            string uploadToken = this.UploadTokenTextBox.Text.Trim();
            string filePath = this.UploadFileTextBox.Text.Trim();
            string key = this.UploadKeyTextBox.Text;

            if (string.IsNullOrEmpty(uploadToken) || string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!this.EnableKeyCheckBox.IsChecked.Value)
            {
                key = null;
            }

            if (!File.Exists(filePath))
            {
                return;
            }

            // 移动代码(UploadButton_Click-->CreateTokenButton_Click) @fengyh, 2016-08-17-11:29
            //string mimeType = this.MimeTypeTextBox.Text.Trim(); 

            bool checkCrc32 = false;
            checkCrc32 = this.CheckCrc32CheckBox.IsChecked.Value;
            // 移动代码(UploadButton_Click-->CreateTokenButton_Click) @fengyh, 2016-08-17-11:29
            //if (mimeType.Length == 0)
            //{
            //    //mimeType = null;
            //    mimeType = "application/octet-stream"; 
            //} 

            string extraParamKey1 = this.ExtraParamKeyTextBox1.Text.Trim();
            string extraParamValue1 = this.ExtraParamValueTextBox1.Text.Trim();
            string extraParamKey2 = this.ExtraParamKeyTextBox2.Text.Trim();
            string extraParamValue2 = this.ExtraParamValueTextBox2.Text.Trim();
            string extraParamKey3 = this.ExtraParamKeyTextBox3.Text.Trim();
            string extraParamValue3 = this.ExtraParamValueTextBox3.Text.Trim();

            string recordKey = null;

            //update status
            this.cancelUpload = false;
            this.UploadProgressBar.Visibility = Visibility.Visible;

            //start upload
            Task.Factory.StartNew(() =>
            {
                string qetag = ETag.CalcHash(filePath);
                if (key == null)
                {
                    recordKey = Base64.UrlSafeBase64Encode(qetag);
                }
                else
                {
                    recordKey = Base64.UrlSafeBase64Encode(key + qetag);
                }

                Dictionary<string, string> extraParams = new Dictionary<string, string>();
                if (!extraParams.ContainsKey(extraParamKey1))
                {
                    extraParams.Add(extraParamKey1, extraParamValue1);
                }
                if (!extraParams.ContainsKey(extraParamKey2))
                {
                    extraParams.Add(extraParamKey2, extraParamValue2);
                }
                if (!extraParams.ContainsKey(extraParamKey3))
                {
                    extraParams.Add(extraParamKey3, extraParamValue3);
                }

                System.IO.FileInfo fi = new FileInfo(filePath);

                Qiniu.Http.HttpResult result = new Qiniu.Http.HttpResult();

                if (fi.Length > 4 * 1024 * 1024)
                {
                    UploadProgressHandler upph = delegate (long u, long t)
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            this.UploadProgressBar.Value = (int)(100.0 * u / t);
                        }));
                    };

                    UploadController upc = new UploadController(UploadControl);

                    ResumableUploader uploader = new ResumableUploader();

                    result = uploader.UploadFile(filePath, key, uploadToken, recordKey, 1, upph, upc, extraParams);
                }

                else
                {
                    FormUploader fu = new FormUploader();
                    result = fu.UploadFile(filePath, key, uploadToken, extraParams);
                }

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.TextBox_UploadResultText.Text = result.Text;
                    this.TextBox_UploadResultString.Text = result.ToString();
                }));               

            });
        }

        private void BrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowReadOnly = true;
            bool? dr = dlg.ShowDialog();
            if (dr.GetValueOrDefault(false))
            {
                this.UploadFileTextBox.Text = dlg.FileName;
            }
        }

        private void EnableKeyCheckBox_Change(object sender, RoutedEventArgs e)
        {
            this.UploadKeyTextBox.IsEnabled = this.EnableKeyCheckBox.IsChecked.Value;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.cancelUpload = true;
        }

        private UPTS UploadControl()
        {
            if (this.cancelUpload)
            {
                return UPTS.Aborted;
            }
            else
            {
                return UPTS.Activated;
            }
        }

    }
}
