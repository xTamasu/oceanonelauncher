using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace OceanOneLauncher
{
    class Launcher
    {
        #region HelperMethods
        // HELPER METHODEN BEGINN
        
        public static string GetPathMissionFile() // Gibt den Path zur Mission File PBO
        {
            return (Properties.Settings.Default.MissionFilePath + @"\OceanOne.Altis.pbo");
        }
        
        public static string GetPathMissionFileVersion() // Gibt den Path zur Version txt
        {
            return (Properties.Settings.Default.MissionFilePath + @"\OceanOne.Altis.Version.txt");
        }

        public static string GetPathMissionFileVersionTemp()
        {
            return (Properties.Settings.Default.MissionFilePath + @"\OceanOne.Altis.VersionServer.txt");
        } // Temporäre Datei zum Abgleichen der Versionen

        public static string GetPathARMA() // Gibt den Path zur ARMA.exe
        {
            return (Properties.Settings.Default.ArmaPath + @"\arma3launcher.exe");
        }
        public static void Error(string exception)
        {
            MessageBox.Show("Ein Fehler ist aufgetreten.. Bitte überprüfe deine Konfiguration oder kontaktiere den Ocean-One Support.\n\n" + exception);
        }

        public static string ReadFile(string filePath) // Liest eine Textdatei aus und gibt den Inhalt zurück
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string readTxt = sr.ReadToEnd();
                    return (readTxt);
                }
            }
            catch (System.Exception)
            {
                MessageBox.Show("Fehler: Eine Versionsdatei konnte nicht gefunden werden.\nBitte überprüfen Sie ihre Installation oder kontaktieren Sie den Ocean-One Support", "Ocean-One Updater");
                return (null);
            }
        }

        public static string ReadVersion() // gibt die Version zurück
        {
            return (ReadFile(GetPathMissionFileVersion()));
        }

        public static async Task DownloadFileAsync(string link, string path) // Downloaded eine Datei
        {
            using (var client = new HttpClient())
            {
                var httpResponse = await client.GetAsync(link).ConfigureAwait(false);
                httpResponse.EnsureSuccessStatusCode();

                using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    await httpResponse.Content.CopyToAsync(fileStream).ConfigureAwait(false);
                }
            }
        }

        public static async Task<bool> CheckVersion() // Überprüft ob eine neue Version vorhanden ist
        {
            await DownloadFileAsync("https://ocean-one.me/missionfile/OceanOne.Altis.VersionServer.txt", GetPathMissionFileVersionTemp());
            string newVersion = ReadFile(GetPathMissionFileVersionTemp());
            string localVersion = ReadFile(GetPathMissionFileVersion());
            if ((newVersion == localVersion) || newVersion == null)
                return (false);
            else
                return (true);
        }

        public static string ChoosePath()
        {
            try
            {
                var dialog = new CommonOpenFileDialog();
                dialog.IsFolderPicker = true;
                CommonFileDialogResult result = dialog.ShowDialog();
                return (dialog.FileName);
            }
            catch (System.Exception e)
            {
                Error(e.Message);
                return ("Ein Fehler ist aufgetreten.. Bitte versuche es erneut.");
            }
        }

        // HELPER METHODEN ENDE
        #endregion

        public static void PlayGame() // Startet ARMA3-Launcher und beendet den Ocean-One-Launcher
        {
            try
            {
                Process.Start(GetPathARMA());
                System.Environment.Exit(1);
            }
            catch (System.Exception e)
            {
                Error(e.Message);
            }
        }

        public static void LaunchWebsite() // Öffnet die Website im Standardbrowser
        {
            Process.Start("https://ocean-one.me/");
        }

        public static async Task Update()
        {
            try
            {
                if (!(File.Exists(GetPathMissionFileVersion())))
                    File.WriteAllText(GetPathMissionFileVersion(), "0.0");

                string versionLocal = ReadVersion();
                bool versionDiffernce = await CheckVersion();
                bool update = false;

                if (versionDiffernce) // Abfrage an den Benutzer zum aktualisieren
                {
                    MessageBoxResult versionResult = MessageBox.Show(
                        "Neue Version wurde gefunden. Aktualisieren?",
                        "Ocean-One Updater",
                        MessageBoxButton.YesNo
                        );

                    if (versionResult == MessageBoxResult.Yes)
                    {
                        update = true;
                    }
                    else
                    {
                        File.Delete(GetPathMissionFileVersionTemp());
                    }

                }
                else
                {
                    File.Delete(GetPathMissionFileVersionTemp());
                    MessageBox.Show("Aktuellste Version bereits vorhanden", "Ocean-One Updater");
                }

                if (update)
                {
                    await DownloadFileAsync("http://ocean-one.me/missionfile/OceanOne.Altis.pbo", GetPathMissionFile()).ConfigureAwait(false);
                    versionLocal = ReadFile(GetPathMissionFileVersionTemp());
                    File.WriteAllText(GetPathMissionFileVersion(), ReadFile(GetPathMissionFileVersionTemp()));
                    File.Delete(GetPathMissionFileVersionTemp());
                    update = false;
                    MessageBox.Show("Download abgeschlossen!", "Ocean-One Updater");
                }
            }
            catch (System.Exception e)
            {
                Error(e.Message);
            }
        }

        public static void Settings_PathARMA()
        {
            string path = ChoosePath();
            Properties.Settings.Default["ArmaPath"] = path;
            Properties.Settings.Default.Save();
        }

        public static void Settings_PathMissionFile()
        {
            string path = ChoosePath();
            Properties.Settings.Default["MissionFilePath"] = path;
            Properties.Settings.Default.Save();
        }
    }
}
