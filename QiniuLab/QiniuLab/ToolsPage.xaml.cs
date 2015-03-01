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
    }
}
