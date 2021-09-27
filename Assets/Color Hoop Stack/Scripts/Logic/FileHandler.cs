using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class FileHandler
{
    readonly string filePath = Application.persistentDataPath + "/saveData.json";

    public bool IsSaveDataExist()
    {
        if (File.Exists(filePath))
            return true;
        return false;
    }

    public void ReadData()
    {
        string inputText;
        inputText = File.ReadAllText(filePath);
        JsonTextReader reader = new JsonTextReader(new StringReader(inputText));
        bool readCurrentLevel = false;

        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (readCurrentLevel)
                {
                    GameplayMgr.Instance.currentLevel = int.Parse(reader.Value.ToString());
                    readCurrentLevel = false;
                }
                else
                {
                    if (reader.Value.ToString() == "currentLevel")
                    {
                        readCurrentLevel = true;
                    }
                }
            }
        }
    }

    public void SaveData()
    {
        JObject output = new JObject(
            new JProperty("currentLevel", GameplayMgr.Instance.currentLevel)
            );

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText(filePath))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            output.WriteTo(writer);
        }
    }

    public void SaveDefaultData()
    {
        JObject output = new JObject(
            new JProperty("currentLevel", 0)
            );

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText(filePath))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            output.WriteTo(writer);
        }
    }
}
