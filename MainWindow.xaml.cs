using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OceanOneLauncher
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool updating;

        public MainWindow()
        {
            InitializeComponent();
            if((File.Exists(Launcher.GetPathMissionFileVersion())))
            VersionMissionFile.Content = "aktuelle Version: " + Launcher.ReadFile(Launcher.GetPathMissionFileVersion());
            else
            {
                VersionMissionFile.Content = "keine Version gefunden";
            }
        }

        private void LaunchGameButton_Click(object sender, RoutedEventArgs e)
        {
            Launcher.PlayGame();
        }

        private void LaunchWebsiteButton_Click(object sender, RoutedEventArgs e)
        {
            Launcher.LaunchWebsite();
        }

        private async void UpdateButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (updating)
            {
                return;
            }

            updating = true;
            UpdateButton.Content = "Update läuft";
            OpenFileDialog
            await Launcher.Update();
            if(!((Launcher.ReadFile(Launcher.GetPathMissionFileVersion())) == "0.0"))
            VersionMissionFile.Content = "aktuelle Version: " + Launcher.ReadFile(Launcher.GetPathMissionFileVersion());

            UpdateButton.Content = "Update";
            updating = false;
        }

        private void Gear_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.MissionFilePath == "No Path defined")
            {
                Properties.Settings.Default.MissionFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Arma 3\MPMissionsCache";
            }
            Settings winSettings = new Settings();
            winSettings.Show();
        }

        /*private void DownloadBar(object sender, RoutedEventArgs e)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri("http://ocean-one.me/missionfile/OceanOne.Altis.pbo"), Launcher.GetPathMissionFile());
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Aktualisierung abgeschlossen!", "Ocean-One Updater");
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ProgressBar progressBar = new ProgressBar();
            progressBar.Maximum = (int)e.TotalBytesToReceive;
            progressBar.Value = (int)e.BytesReceived / 100;
        }
        */

        /*private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e) // Wird das benötigt?
        {

        }
        */
    }
}
