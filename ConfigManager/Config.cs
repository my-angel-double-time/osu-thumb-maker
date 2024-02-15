using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Forms;
using System.Linq.Expressions;
//using WK.Libraries.BetterFolderBrowserNS;

namespace OsuThumbMaker.ConfigManager
{
    internal static class Config
    {
        // Return the current directory where the executable is running from
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;
        private static string OsuPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "osu!");

        public static Settings Settings = new ();

        private static string ConfigPath => BaseDirectory + "config.json";

        internal static void LoadConfig()
        {
            if (!File.Exists(ConfigPath)) 
            {
                Console.WriteLine("Config.json not found! Using default settings...");
                LoadDefault();
            }
            else
                LoadUserConfig();
        }
        
        private static void LoadDefault()
        {
            if (Settings.SongsDirectory == null)
            {
                var fb = new FolderBrowserDialog();
                fb.UseDescriptionForTitle = true;
                fb.Description = "Select your songs folder";
                fb.InitialDirectory = OsuPath;

                if (fb.ShowDialog() == DialogResult.OK)
                    Settings.SongsDirectory = fb.SelectedPath;
            }

            if (Settings.OsuDatabasePath == null)
            {
                var fd = new OpenFileDialog ();
                fd.InitialDirectory = OsuPath;
                fd.Title = "Select osu!.db";
                fd.Filter = "osu! database file (*.db)|*.db";
                fd.FilterIndex = 2;

                if (fd.ShowDialog() == DialogResult.OK)
                    Settings.OsuDatabasePath = fd.FileName;
            }

            var errorMessage = "";

            if (Settings.OsuDatabasePath == null)
                errorMessage += "osu!db";
            if (Settings.SongsDirectory == null)
                errorMessage += errorMessage != "" ? " and songs folder" : " songs folder";

            if (errorMessage != "")
                throw new Exception($"{errorMessage} not specified! exiting...");


            var json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(ConfigPath, json);
        }

        private static void LoadUserConfig()
        {
            using StreamReader sr = File.OpenText(ConfigPath);

            JsonSerializer jsonSr = new ();
            Settings = (Settings?)jsonSr.Deserialize(sr, typeof(Settings)) ?? throw new Exception("Failed to read user config!");

            if (Settings.OsuDatabasePath == null || Settings.SongsDirectory == null)
                LoadDefault();
        }

    }
}
