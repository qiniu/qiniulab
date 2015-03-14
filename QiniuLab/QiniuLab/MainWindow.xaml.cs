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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ResourceUploadPage resourceUploadPage;
        private ResourceDownloadPage resourceDownloadPage;
        private ResourceManagePage resourceManagePage;
        private ResourcePfopPage resourcePfopPage;
        private PictureOperationPage pictureOperationPage;
        private AudioOperationPage audioOperationPage;
        private VideoOperationPage videoOperationPage;
        private OtherOperationPage otherOperationPage;
        private ToolsPage toolsPage;
        private SettingsPage settingsPage;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void Nav_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Background = Brushes.AliceBlue;
        }

        private void Nav_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid grid = (Grid)sender;
            grid.Background = Brushes.White;
        }

        private void TopLink_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = Brushes.White;
        }

        private void TopLink_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = Brushes.WhiteSmoke;
        }

        private void NavToUrl(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            string targetUrl = "";
            if (label.Name.Equals("Site_Qiniu"))
            {
                targetUrl = "http://www.qiniu.com";
            }
            else if (label.Name.Equals("Site_Doc"))
            {
                targetUrl = "http://developer.qiniu.com";
            }
            else if (label.Name.Equals("Site_QA"))
            {
                targetUrl = "http://segmentfault.com/qiniu";
            }
            else if (label.Name.Equals("Site_Backend"))
            {
                targetUrl = "https://portal.qiniu.com";
            }
            if (!string.IsNullOrEmpty(targetUrl))
            {
                System.Diagnostics.Process.Start(targetUrl);
            }
        }

        private void Nav_ResourceUpload_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.resourceUploadPage == null)
            {
                this.resourceUploadPage = new ResourceUploadPage();
            }
            this.MainHostFrame.Content = this.resourceUploadPage;
        }

        private void Nav_ResourceDownload_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.resourceDownloadPage == null)
            {
                this.resourceDownloadPage = new ResourceDownloadPage();
            }
            this.MainHostFrame.Content = this.resourceDownloadPage;
        }

        private void Nav_ResourceManage_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.resourceManagePage == null)
            {
                this.resourceManagePage = new ResourceManagePage();
            }
            this.MainHostFrame.Content = this.resourceManagePage;
        }

        private void Nav_ResourcePfop_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.resourcePfopPage == null)
            {
                this.resourcePfopPage = new ResourcePfopPage();
            }
            this.MainHostFrame.Content = this.resourcePfopPage;
        }

        private void Nav_PictureOperation_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if(this.pictureOperationPage==null)
            {
                this.pictureOperationPage = new PictureOperationPage();
            }
            this.MainHostFrame.Content = this.pictureOperationPage;
        }

        private void Nav_AudioOperation_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.audioOperationPage == null)
            {
                this.audioOperationPage = new AudioOperationPage();
            }
            this.MainHostFrame.Content = this.audioOperationPage;
        }

        private void Nav_VideoOperation_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.videoOperationPage == null)
            {
                this.videoOperationPage = new VideoOperationPage();
            }
            this.MainHostFrame.Content = this.videoOperationPage;
        }

        private void Nav_OtherOperation_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.otherOperationPage == null)
            {
                this.otherOperationPage = new OtherOperationPage();
            }
            this.MainHostFrame.Content = this.otherOperationPage;
        }

        private void Nav_Tools_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.toolsPage == null)
            {
                this.toolsPage = new ToolsPage();
            }
            this.MainHostFrame.Content = this.toolsPage;
        }

        private void Nav_Settings_MouseClick(object sender, MouseButtonEventArgs e)
        {
            if (this.settingsPage == null)
            {
                this.settingsPage = new SettingsPage();
            }
            this.MainHostFrame.Content = this.settingsPage;
        }


    }
}
