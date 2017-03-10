using Microsoft.Win32;
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
    /// Interaction logic for ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : Page
    {
        public ToolsPage()
        {
            InitializeComponent();

            var now = DateTime.Now;

            DatePicker_TargetDate.DisplayDate = now;
            DatePicker_TargetDate.SelectedDate = now;

            for (int h = 0; h < 24; ++h)
            {
                ComboBox_TimeHour.Items.Add(h);
            }
            for (int n = 0; n < 60; ++n)
            {
                ComboBox_TimeMin.Items.Add(n);
                ComboBox_TimeSec.Items.Add(n);
            }

            ComboBox_TimeHour.SelectedIndex = now.Hour - 1;
            ComboBox_TimeMin.SelectedIndex = now.Minute - 1;
            ComboBox_TimeSec.SelectedIndex = now.Second - 1;
        }

        private void UrlSafeBase64Encode_Click(object sender, RoutedEventArgs e)
        {
            string toEncodeString = this.ToEncodeStringTextBox.Text;
            if (!string.IsNullOrEmpty(toEncodeString))
            {
                this.EncodedStringTextBox.Text = Base64.UrlSafeBase64Encode(toEncodeString);
            }
        }

        private void UrlSafeBase64Decode_Click(object sender, RoutedEventArgs e)
        {
            string toDecodeString = this.ToDecodeStringTextBox.Text;
            if (!string.IsNullOrEmpty(toDecodeString))
            {
                try
                {
                    this.DecodedStringTextBox.Text =
                            Encoding.UTF8.GetString(Base64.UrlsafeBase64Decode(toDecodeString));
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "解码错误", MessageBoxButton.OK);
                }
            }
        }

        private void BrowseQETagSourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                this.QETagSourceFileTextBox.Text = dlg.FileName;
            }
        }

        private void CalcQETagButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = this.QETagSourceFileTextBox.Text;
            if (File.Exists(fileName))
            {
                try
                {
                    string qetag = ETag.CalcHash(fileName);
                    this.QETagResultTextBox.Text = qetag;
                }
                catch (Exception) { }
            }
        }

        private void BrowseCRC32SourceFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                this.CRC32SourceFileTextBox.Text = dlg.FileName;
            }
        }

        private void CalcCRC32Button_Click(object sender, RoutedEventArgs e)
        {
            string fileName = this.CRC32SourceFileTextBox.Text;
            if (File.Exists(fileName))
            {
                try
                {
                    string crc32 = CRC32.checkSumFile(fileName).ToString();
                    this.CRC32ResultTextBox.Text = crc32;
                }
                catch (Exception) { }
            }
        }

        private void CRC32SourceFileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.CRC32ResultTextBox.Text = "";
        }

        private void QETagSourceFileTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.QETagResultTextBox.Text = "";
        }

        private void ClearUrlSafeBase64Encode_Click(object sender, RoutedEventArgs e)
        {
            this.EncodedStringTextBox.Text = "";
            this.ToEncodeStringTextBox.Text = "";
        }

        private void ClearUrlSafeBase64Decode_Click(object sender, RoutedEventArgs e)
        {
            this.DecodedStringTextBox.Text = "";
            this.ToDecodeStringTextBox.Text = "";
        }

        private void TextBox_MD5SrcFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.TextBox_FileMD5Result.Text = "";
        }

        private void Button_OpenMD5SrcFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                this.TextBox_MD5SrcFileName.Text = dlg.FileName;
            }
        }

        private void Button_CalcFileMD5_Click(object sender, RoutedEventArgs e)
        {
            string fileName = this.TextBox_MD5SrcFileName.Text.Trim();
            if (File.Exists(fileName))
            {
                try
                {
                    var md5 = System.Security.Cryptography.MD5.Create();
                    byte[] data = File.ReadAllBytes(fileName);
                    byte[] hashData = md5.ComputeHash(data);
                    StringBuilder sb = new StringBuilder(hashData.Length * 2);
                    foreach (byte b in hashData)
                    {
                        sb.AppendFormat("{0:X2}", b);
                    }
                    this.TextBox_FileMD5Result.Text = sb.ToString();
                }
                catch (Exception) { }
            }
        }

        private void Button_DateTimeToTimestamp_Click(object sender, RoutedEventArgs e)
        {
            if (!DatePicker_TargetDate.SelectedDate.HasValue)
            {
                DatePicker_TargetDate.SelectedDate = DateTime.Now;
            }

            if(ComboBox_TimeHour.SelectedIndex<0)
            {
                ComboBox_TimeHour.SelectedIndex = 0;
            }

            if(ComboBox_TimeMin.SelectedIndex<0)
            {
                ComboBox_TimeMin.SelectedIndex = 0;
            }

            if(ComboBox_TimeSec.SelectedIndex<0)
            {
                ComboBox_TimeSec.SelectedIndex = 0;
            }

            var date = DatePicker_TargetDate.SelectedDate ?? DateTime.Now;

            int h = ComboBox_TimeHour.SelectedIndex;
            int m = ComboBox_TimeMin.SelectedIndex;
            int s = ComboBox_TimeSec.SelectedIndex;

            DateTime dt = new DateTime(date.Year, date.Month, date.Day, h, m, s);

            TextBox_TargetTimestamp.Text = UnixTimestamp.ConvertToTimestamp(dt).ToString();
        }

        private void Button_TimestampToDateTime_Click(object sender, RoutedEventArgs e)
        {
            string ts = TextBox_Timestamp.Text.Trim();

            long timestamp = 0;
            if (long.TryParse(ts, out timestamp))
            {
                TextBox_DateTime.Text = UnixTimestamp.ConvertToDateTime(timestamp).ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }
}
