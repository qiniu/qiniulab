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
        }

        private void UrlSafeBase64Encode_Click(object sender, RoutedEventArgs e)
        {
            string toEncodeString = this.ToEncodeStringTextBox.Text;
            if (!string.IsNullOrEmpty(toEncodeString))
            {
                this.EncodedStringTextBox.Text = Qiniu.Util.StringUtils.urlSafeBase64Encode(toEncodeString);
            }
        }

        private void UrlSafeBase64Decode_Click(object sender, RoutedEventArgs e)
        {
            string toDecodeString = this.ToDecodeStringTextBox.Text;
            if (!string.IsNullOrEmpty(toDecodeString))
            {
                this.DecodedStringTextBox.Text = Qiniu.Util.StringUtils.urlsafeBase64Decode(toDecodeString);
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
                    string qetag = QETag.hash(fileName);
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
                    string crc32 = CRC32.CheckSumFile(fileName).ToString();
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
    }
}
