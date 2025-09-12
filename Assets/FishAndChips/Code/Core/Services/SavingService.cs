using System;
using System.IO;
using UnityEngine;

namespace FishAndChips
{
    /// <summary>
    /// Service handling reading and writing from files.
    /// </summary>
    public class SavingService : Singleton<SavingService>
    {
        #region -- Private Methods --
        private void Save(string fullPath, string json)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(json);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }
        }

        private string Load(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                try
                {
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.LogException(e);
                }
            }
            return string.Empty;
        }

        private void Delete(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
		#endregion

		#region -- Public Methods --
		public void SaveJson(string filePath, string fileName, string json)
        {
            string path = filePath;
            string fullPath = Path.Combine(path, fileName);
            Save(fullPath, json);
        }

        public void SaveJson(string fileName, string json)
        {
            string path = Application.persistentDataPath;
            string fullPath = Path.Combine(path, fileName);
            Save(fullPath, json);
        }

        public string LoadJson(string filePath, string fileName)
        {
			string path = filePath;
			string fullPath = Path.Combine(path, fileName);
            return Load(fullPath);
        }

        public string LoadJson(string fileName)
        {
            string path = Application.persistentDataPath;
			string fullPath = Path.Combine(path, fileName);
            return Load(fullPath);
        }

        public void DeleteFile(string filePath, string fileName)
        {
			string path = filePath;
			string fullPath = Path.Combine(path, fileName);
            Delete(fullPath);
		}

        public void DeleteFile(string fileName)
        {
            string path = Application.persistentDataPath;
			string fullPath = Path.Combine(path, fileName);
            Delete(fullPath);
		}
        #endregion
    }
}
