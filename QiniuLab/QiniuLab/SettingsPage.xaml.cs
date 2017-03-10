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
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.AccessKeyTextBox.Text = QiniuLab.AppSettings.Default.ACCESS_KEY;
            this.SecretKeyTextBox.Text = QiniuLab.AppSettings.Default.SECRET_KEY;

            this.SecretKeyTextBox.Visibility = Visibility.Hidden;
            this.SKVisButton.Content = "显示";
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            QiniuLab.AppSettings.Default.ACCESS_KEY = this.AccessKeyTextBox.Text.Trim();
            QiniuLab.AppSettings.Default.SECRET_KEY = this.SecretKeyTextBox.Text.Trim();
            QiniuLab.AppSettings.Default.Save();
        }

        /// <summary>
        /// 秘钥(SK)显示/隐藏功能
        /// 2016-08-17 16:41 @fengyh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SKVisButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SecretKeyTextBox.Visibility == Visibility.Hidden)
            {
                this.SecretKeyTextBox.Visibility = Visibility.Visible;
                this.SKVisButton.Content = "隐藏";
            }
            else
            {
                this.SecretKeyTextBox.Visibility = Visibility.Hidden;
                this.SKVisButton.Content = "显示";
            }
        }
    }
}
