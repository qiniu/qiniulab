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
using Qiniu.Common;
using Qiniu.IO;

namespace QiniuLab
{
    /// <summary>
    /// Interaction logic for ResourceDownloadPage.xaml
    /// </summary>
    public partial class ResourceDownloadPage : Page
    {
        public ResourceDownloadPage()
        {
            InitializeComponent();
        }

        private void CreateAuthButton_Click(object sender, RoutedEventArgs e)
        {
            long deadline = 0;
            TimeSpan ts = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            if (this.DeadlineTextBox.IsEnabled)
            {
                try
                {
                    deadline = Convert.ToInt64(this.DeadlineTextBox.Text.Trim());
                }
                catch (Exception) { }
            }
            else if (this.ExpireTextBox.IsEnabled)
            {
                try
                {
                    long expires=Convert.ToInt64(this.ExpireTextBox.Text.Trim());
                    deadline =(long) ts.TotalSeconds + expires;
                }
                catch (Exception) { }
            }
            if (deadline == 0)
            {
                deadline = (long)+ts.TotalSeconds + 3600;
            }

            string srcUrl = this.SourceUrlTextBox.Text.Trim();

            try
            {
                Uri uri = new Uri(srcUrl);
                if (!string.IsNullOrEmpty(uri.Query))
                {
                    srcUrl += "&e=" + deadline;
                }
                else
                {
                    srcUrl += "?e=" + deadline;
                }
                Mac mac = new Mac(AppSettings.Default.ACCESS_KEY,AppSettings.Default.SECRET_KEY);
                DownloadManager dx = new DownloadManager(mac);
                string token = dx.createDownloadToken(srcUrl);
                srcUrl += "&token=" + token;
                this.AuthedUrlTextBox.Text = srcUrl;
            }
            catch (Exception) { }
        }

        private void DeadlineCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.DeadlineTextBox.IsEnabled = true;
        }

        private void DeadlineCheckBox_UnChanged(object sender, RoutedEventArgs e)
        {
            this.DeadlineTextBox.IsEnabled = false;
        }

        private void ExpireCheckBox_UnChanged(object sender, RoutedEventArgs e)
        {
            this.ExpireTextBox.IsEnabled = false;
        }

        private void ExpireCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            this.ExpireTextBox.IsEnabled = true;
        }
    }
}
