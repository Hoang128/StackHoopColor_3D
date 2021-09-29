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
        bool readSoundEnable = false;
        bool readVibrateEnable = false;

        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (readCurrentLevel)
                {
                    GameplayMgr.Instance.currentLevel = int.Parse(reader.Value.ToString());
                    readCurrentLevel = false;
                }
                else if (readSoundEnable)
                {
                    int intSoundEnable = int.Parse(reader.Value.ToString());
                    if (intSoundEnable == 0)
                    {
                        GameManager.Instance.SoundEnable = false;
                    }
                    else
                    {
                        GameManager.Instance.SoundEnable = true;
                    }
                    readSoundEnable = false;
                }
                else if (readVibrateEnable)
                {
                    int intVibrateEnable = int.Parse(reader.Value.ToString());
                    if (intVibrateEnable == 0)
                    {
                        GameManager.Instance.VibrateEnable = false;
                    }
                    else
                    {
                        GameManager.Instance.VibrateEnable = true;
                    }
                    readVibrateEnable = false;
                }
                else
                {
                    if (reader.Value.ToString() == "currentLevel")
                    {
                        readCurrentLevel = true;
                        readSoundEnable = false;
                        readVibrateEnable = false;
                    }
                    else if (reader.Value.ToString() == "soundEnable")
                    {
                        readCurrentLevel = false;
                        readSoundEnable = true;
                        readVibrateEnable = false;
                    }
                    else if (reader.Value.ToString() == "vibrateEnable")
                    {
                        readCurrentLevel = false;
                        readSoundEnable = false;
                        readVibrateEnable = true;
                    }
                }
            }
        }
    }

    public void SaveData()
    {
        JObject output = new JObject(
            new JProperty("currentLevel", GameplayMgr.Instance.currentLevel),
            new JProperty("soundEnable", System.Convert.ToInt32(GameManager.Instance.SoundEnable)),
            new JProperty("vibrateEnable", System.Convert.ToInt32(GameManager.Instance.VibrateEnable))
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
            new JProperty("currentLevel", 0),
            new JProperty("soundEnable", 1),
            new JProperty("vibrateEnable", 1)
            );

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText(filePath))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            output.WriteTo(writer);
        }
    }
}
