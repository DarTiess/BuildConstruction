using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Card;
using CodeBase.Infrastructure.SaveService;
using Infrastructure.Services;
using Infrastructure.Services.Messeges;
using Newtonsoft.Json;
using UnityEngine;
using Formatting = System.Xml.Formatting;


namespace CodeBase.Infrastructure
{
    public class PersistentData : ISaveHandler, ILoadHandler, IDisposable
    {
        private GameSave _gameSave;
        private readonly Messenger _messenger;
        private readonly string _savePath;

        public PersistentData(Messenger messenger)
        {
            _messenger = messenger;
            _messenger.Sub<BuildInstalled>(BuildPlacedSave);
            _messenger.Sub<DeleteBuild>(BuildDelete);
            _savePath = Path.Combine(Application.persistentDataPath, "GameSave.json");
        }

        private void BuildDelete(DeleteBuild build)
        {
            if (_gameSave == null || _gameSave.BuildMap == null)
            {
                Debug.LogWarning("Попытка удалить здание, но GameSave пуст.");
                return;
            }
            
            if (_gameSave.BuildMap.ContainsKey(build.Position))
            {
                _gameSave.BuildMap.Remove(build.Position);
                Save();
                Debug.Log($" Здание на {build.Position} удалено из GameSave.");
            }
            else
            {
                Debug.LogWarning($" Нет здания для удаления на {build.Position}. Возможно, ошибка сохранения/загрузки.");
            }
        }

        private void BuildPlacedSave(BuildInstalled build)
        {
            if (_gameSave == null) 
                _gameSave = new GameSave(); 

            _gameSave.BuildMap[build.GridPosition] = build.Type;
            Save();
        }

        public void Initialize()
        {
            if (!File.Exists(_savePath))
            {
                Debug.Log("Файл сохранения отсутствует. Создаём новый.");
                _gameSave = new GameSave();
                return;
            }

            try
            {
                string json = File.ReadAllText(_savePath);
                var loadedData = JsonConvert.DeserializeObject<Dictionary<string, BuildType>>(json);

                _gameSave = new GameSave
                {
                    BuildMap = loadedData.ToDictionary(
                        kvp => StringToVector2Int(kvp.Key), 
                        kvp => kvp.Value
                    )
                };
            }
            catch (Exception ex)
            {
                Debug.LogError($" Ошибка загрузки сохранения: {ex.Message}");
                _gameSave = new GameSave();
            }
        }
        private Vector2Int StringToVector2Int(string str)
        {
            try
            {
                string[] values = str.Split(',');
                return new Vector2Int(int.Parse(values[0]), int.Parse(values[1]));
            }
            catch (Exception ex)
            {
                Debug.LogError($" Ошибка преобразования строки '{str}' в Vector2Int: {ex.Message}");
                return Vector2Int.zero; // Возвращаем (0,0) в случае ошибки
            }
        }
        private void Save()
        {
            try
            {
                var serializableBuildMap = _gameSave.BuildMap.ToDictionary(
                    kvp => $"{kvp.Key.x},{kvp.Key.y}", 
                    kvp => kvp.Value
                );

                string json = JsonConvert.SerializeObject(serializableBuildMap, (Newtonsoft.Json.Formatting)Formatting.Indented);
                File.WriteAllText(_savePath, json);
                Debug.Log($"Данные сохранены. {serializableBuildMap.Count} зданий.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка при сохранении: {ex.Message}");
            }
        }

        public void LoadToObject(ILoader loader)
        {
            loader.Load(_gameSave);
        }

        public void SaveFromObject(ISaver saver)
        {
            saver.Save(_gameSave);
            Save();
        }

        public void Dispose()
        {
            _messenger.Unsub<BuildInstalled>(BuildPlacedSave);
        }
    }
}