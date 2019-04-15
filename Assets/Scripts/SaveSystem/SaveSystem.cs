using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveAllData(GameManager manager, string pathName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
         
        string path = Application.persistentDataPath + pathName;
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(manager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadSavedData(string pathName)
    {
        string path = Application.persistentDataPath + pathName;

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }

        else
        {
            GameManager.Instance.NewGame = true;
            return null;
        }
    }
}
