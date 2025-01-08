using System;
using Source.Scripts.Data;
using Source.Scripts.Paths;
using Source.Scripts.Ui.Menu;
using Zenject;

namespace Source.Scripts.Menu
{
    public class SettingsManager : IInitializable, IDisposable
    {
        
        private Settings _settings;
        
        public void Initialize()
        {
            LoadSettings();
        }
        
        public Settings GetSettings()
        {
            return _settings;
        }

        private void LoadSettings()
        {
            JsonDataSaver dataSaver = new JsonDataSaver();
            _settings = dataSaver.Load<Settings>(SavePath.SETTINGS_DATA_PATH) ?? Settings.GetDefaultSettings();
        }
        
        private void SaveSettings()
        {
            JsonDataSaver dataSaver = new JsonDataSaver();
            dataSaver.Save(_settings, SavePath.SETTINGS_DATA_PATH);
        }

        public void Dispose()
        {
            SaveSettings();
        }
    }
}