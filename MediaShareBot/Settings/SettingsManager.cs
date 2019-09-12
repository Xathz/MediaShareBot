using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MediaShareBot.Settings {

    public static class SettingsManager {

        /// <summary>
        /// Configuration settings.
        /// </summary>
        public static Configuration Configuration = new Configuration();

        /// <summary>
        /// Load settings from the disk at <see cref="Constants.SettingsFile" />.
        /// </summary>
        public static void Load() {
            if (!File.Exists(Constants.SettingsFile)) {
                LoggingManager.Log.Warn($"Settings file was not found at '{Constants.SettingsFile}', creating default one");
                SaveDefault();
            }

            try {
                LoadJSON(Constants.SettingsFile);

                LoggingManager.Log.Info("Settings loaded.");
                return;
            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }

            LoggingManager.Log.Fatal($"Can not load settings file '{Constants.SettingsFile}', please check it or delete it so a new one can be created");
            LoggingManager.Flush();
            Environment.Exit(2);
        }

        private static void LoadJSON(string settingsFile) {
            Configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(settingsFile));

            if (Configuration == null) {
                throw new ArgumentNullException("The configuration was null after deserialization");
            }
        }

        /// <summary>
        /// Save settings to the disk at <see cref="Constants.SettingsFile" />.
        /// </summary>
        public static void Save() {
            string tempFile = $"{Constants.SettingsFile}.temp";

            try {
                string json = JsonConvert.SerializeObject(Configuration, new JsonSerializerSettings() {
                    ContractResolver = new DefaultContractResolver {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented
                });

                File.WriteAllText(tempFile, json);

                if (File.Exists(Constants.SettingsFile)) {
                    File.Copy(Constants.SettingsFile, Path.ChangeExtension(tempFile, "previous"), true);
                }

                File.Copy(tempFile, Constants.SettingsFile, true);
                File.Delete(tempFile);

                LoggingManager.Log.Info("Settings saved");
            } catch (Exception ex) {
                LoggingManager.Log.Error(ex);
            }
        }

        /// <summary>
        /// Force default settings and save to the disk at <see cref="Constants.SettingsFile" />.
        /// </summary>
        public static void SaveDefault() {
            Configuration = new Configuration();
            Save();
        }

    }

}
