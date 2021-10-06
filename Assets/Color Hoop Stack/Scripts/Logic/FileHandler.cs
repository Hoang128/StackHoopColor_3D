using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LevelData
{
    private bool blankData = true;
    private Stack<MapData> mapDataStack;
    private int currentLevel;
    private MapData mapDataCurrent;
    private int ringTypeNumber;

    public LevelData()
    {
        this.blankData = true;
        this.mapDataStack = new Stack<MapData>();
        this.currentLevel = 0;
        this.mapDataCurrent = new MapData();
        this.ringTypeNumber = 0;
    }

    public LevelData(bool blankData, Stack<MapData> mapDataStack, int currentLevel, MapData mapDataCurrent, int ringTypeNumber)
    {
        this.blankData = blankData;
        this.mapDataStack = new Stack<MapData>(mapDataStack);
        this.currentLevel = currentLevel;
        this.mapDataCurrent = mapDataCurrent;
        this.ringTypeNumber = ringTypeNumber;
    }

    public Stack<MapData> MapDataStack { get => mapDataStack; set => mapDataStack = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public MapData MapDataCurrent { get => mapDataCurrent; set => mapDataCurrent = value; }
    public int RingTypeNumber { get => ringTypeNumber; set => ringTypeNumber = value; }
    public bool BlankData { get => blankData; set => blankData = value; }
}

    public class FileHandler
{
    readonly public string settingFilePath = Application.persistentDataPath + "/settingData.json";
    readonly public string levelFilePath = Application.persistentDataPath + "/levelData.json";

    public bool IsFileExist(string path)
    {
        if (File.Exists(path))
            return true;
        return false;
    }

    public void ReadSettingData()
    {
        string inputText;
        inputText = File.ReadAllText(settingFilePath);
        JsonTextReader reader = new JsonTextReader(new StringReader(inputText));
        bool readSoundEnable = false;
        bool readVibrateEnable = false;

        while (reader.Read())
        {
            if (reader.Value != null)
            {
                if (readSoundEnable)
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
                    if (reader.Value.ToString() == "soundEnable")
                    {
                        readSoundEnable = true;
                        readVibrateEnable = false;
                    }
                    else if (reader.Value.ToString() == "vibrateEnable")
                    {
                        readSoundEnable = false;
                        readVibrateEnable = true;
                    }
                }
            }
        }
    }

    public void SaveSettingData()
    {
        JObject output = new JObject(
            new JProperty("currentLevel", GameplayMgr.Instance.currentLevel),
            new JProperty("soundEnable", System.Convert.ToInt32(GameManager.Instance.SoundEnable)),
            new JProperty("vibrateEnable", System.Convert.ToInt32(GameManager.Instance.VibrateEnable))
            );

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText(settingFilePath))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            output.WriteTo(writer);
        }
    }

    public void SaveSettingDataDefault()
    {
        JObject output = new JObject(
            new JProperty("soundEnable", 1),
            new JProperty("vibrateEnable", 1)
            );

        // write JSON directly to a file
        using (StreamWriter file = File.CreateText(settingFilePath))
        using (JsonTextWriter writer = new JsonTextWriter(file))
        {
            output.WriteTo(writer);
        }
    }

    public void SaveLevelData()
    {
        LevelData levelData = new LevelData(
            false,
            GameplayMgr.Instance.mapDataStack, 
            GameplayMgr.Instance.currentLevel, 
            new MapData(GameplayMgr.Instance.ringStackList, GameplayMgr.Instance.stackCompleteNumber, GameplayMgr.Instance.ringStackList.Count), 
            GameplayMgr.Instance.ringTypeNumber
            );

        string output = JsonConvert.SerializeObject(levelData);

        using (StreamWriter outputFile = new StreamWriter(levelFilePath))
        {
            outputFile.WriteLine(output);
        }
    }

    public void SaveLevelDataDefault()
    {
        LevelData levelData = new LevelData();
    }

    public void LoadLevelData()
    {
        if (IsFileExist(levelFilePath))
        {
            string input;

            using (StreamReader inputFile = new StreamReader(levelFilePath))
            {
                input = inputFile.ReadLine();
            }
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(input);

            GameplayMgr.Instance.currentLevel = levelData.CurrentLevel;
            GameplayMgr.Instance.mapDataStack = new Stack<MapData>(levelData.MapDataStack.Reverse());
            GameplayMgr.Instance.ringTypeNumber = levelData.RingTypeNumber;
            if ((!levelData.BlankData) && (levelData.MapDataStack.Count > 0))
                GameplayMgr.Instance.LoadLevelMapData(levelData.MapDataCurrent);
        }
    }
}
