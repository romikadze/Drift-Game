using System.IO;
using UnityEngine;

namespace Source.Scripts.Data
{
    public class JsonDataSaver : IDataSaver
    {
        public void Save<T>(T data, string path)
        {
            var savePath = Path.Combine(Application.persistentDataPath, path);
            
            var json = JsonUtility.ToJson(data);
            
            File.WriteAllText(savePath, json);
        }

        public T Load<T>(string path)
        {
            var loadPath = Path.Combine(Application.persistentDataPath, path);
            
            if (File.Exists(loadPath))
            {
                var json = File.ReadAllText(loadPath);
                return JsonUtility.FromJson<T>(json);
            }

            return default;
        }
    }
}