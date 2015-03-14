using Microsoft.Win32;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ResourceUploadPage.xaml
    /// </summary>
    public partial class ResourceUploadPage : Page
    {
        public ResourceUploadPage()
        {
            InitializeComponent();
            this.UploadKeyTextBox.IsEnabled = false;
        }

        private void CreateTokenButton_Click(object sender, RoutedEventArgs e)
        {
            string scope = this.ScopeTextBox.Text;
            int expires =3600;
            try
            {
                expires = Convert.ToInt32(this.ExpireTextBox.Text.Trim());
            }
            catch (Exception) { }
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
                ComboBoxItem item=(ComboBoxItem)this.CallbackBodyTypeComboBox.SelectedItem;
                callbackBodyType = item.Content.ToString();
            }
            string callbackBody = this.CallbackBodyTextBox.Text;
            int callbackFetchKey = this.CallbackFetchKeyCheckBox.IsChecked.Value ? 1 : 0;
            string persistentOps = this.PersistentOpsTextBox.Text;
            string persistentPipeline = this.PersistentPipelineTextBox.Text;
            string persistentNotifyUrl = this.PersistentNotifyUrlTextBox.Text;
            int fsizeLimit =-1;
            try
            {
                fsizeLimit = Convert.ToInt32(this.FsizeLimitTextBox.Text.Trim());
            }
            catch (Exception) { }
            string mimeLimit = this.MimeLimitTextBox.Text;
            int detectMime = this.DetectMimeCheckBox.IsChecked.Value ? 1 : 0;

            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = scope;
            putPolicy.SetExpires(expires);
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
            string accessKey = QiniuLab.AppSettings.Default.ACCESS_KEY;
            string secretKey = QiniuLab.AppSettings.Default.SECRET_KEY;
            Mac mac = new Mac(accessKey, secretKey);
            string uploadToken = Auth.createUploadToken(putPolicy, mac);
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

            //upload file
            UploadManager uploadManager = new UploadManager();
        //   uploadManager.uploadFile(filePath,key,uploadToken,
        }

        private void BrowseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowReadOnly = true;
            bool? dr=dlg.ShowDialog();
            if (dr.GetValueOrDefault(false))
            {
                this.UploadFileTextBox.Text = dlg.FileName;
            }
        }

        private void EnableKeyCheckBox_Change(object sender, RoutedEventArgs e)
        {
            this.UploadKeyTextBox.IsEnabled = this.EnableKeyCheckBox.IsChecked.Value;
        }

        
    }
}
