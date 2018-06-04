using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OceanOneLauncher
{
    /// <summary>
    /// Interaktionslogik für Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            LabelArmaPath.Content = Properties.Settings.Default.ArmaPath;
            LabelMissionFile.Content = Properties.Settings.Default.MissionFilePath;

        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            this.Close();
        }
        
        private void SetPath_ARMA(object sender, RoutedEventArgs e)
        {
            Launcher.Settings_PathARMA();
            LabelArmaPath.Content = Properties.Settings.Default.ArmaPath;
        }
        private void SetPath_MissionFile(object sender, RoutedEventArgs e)
        {
            Launcher.Settings_PathMissionFile();
            LabelMissionFile.Content = Properties.Settings.Default.MissionFilePath;
        }
    }
}
