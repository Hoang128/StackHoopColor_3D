using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

public class DataGenMenu : OdinEditorWindow
{
    private int stackNumberMax = 4;

    [MenuItem("Tools/Color Stack Hoop/DataGenerator")]
    private static void ShowWindow()
    {
        Init();
    }

    private static void Init()
    {
        DataGenMenu dataGenMenu = new DataGenMenu();
        GetWindow<DataGenMenu>(false, "Data Generator", true);
    }

    [Button]
    private void GenerateLevelsData()
    {
        string levelDataPath = "Assets/Color Hoop Stack/Scripts/Design Data/LevelListConfig.asset";
        bool assetExists = AssetDatabase.GetMainAssetTypeAtPath(levelDataPath) != null;
        if (!assetExists)
        {
            string inputText;
            string filePath = "Assets/Resources/Data/level.json";
            JsonTextReader reader;

            inputText = File.ReadAllText(filePath);
            reader = new JsonTextReader(new StringReader(inputText));

            LevelListConfig levelListConfig = ScriptableObject.CreateInstance<LevelListConfig>();

            int stackNumber = -1;
            bool canReadLevel = false;
            bool canReadRing = false;
            LevelConfig currentLevelConfig = new LevelConfig();
            RingStackList currentRingStackList = new RingStackList();

            while (reader.Read())
            {
                if (reader.Value!=null)
                {
                    if (reader.Value.ToString() == "data")
                    {
                        if (!canReadLevel)
                            canReadLevel = true;
                    }
                    else if (reader.TokenType == JsonToken.Integer)
                    {
                        int ringType;
                        System.Int32.TryParse(reader.Value.ToString(), out ringType);
#if UNITY_EDITOR
                        Utils.Common.Log("value = " + ((RingType)ringType).ToString());
#endif
                        if (canReadLevel && canReadRing)
                        {
                            if (stackNumber == 3)
                            {
                                stackNumber = 0;
                                currentLevelConfig.ringStackList.Add(currentRingStackList);
                                currentRingStackList = new RingStackList();
                            }
                            else
                            {
                                stackNumber++;
                            }

                            currentRingStackList.ringList.Add((RingType)ringType);
                        }
                    }
                }
                else
                {
                    if (reader.TokenType == JsonToken.StartArray)
                    {
                        if (canReadLevel)
                        {
                            canReadRing = true;
                            currentLevelConfig = new LevelConfig();
                        }
                    }
                    else if (reader.TokenType == JsonToken.EndArray)
                    {
                        if (canReadLevel)
                        {
                            canReadRing = false;
                            levelListConfig.levelList.Add(currentLevelConfig);
                        }
                    }
                }    
            }

            AssetDatabase.CreateAsset(levelListConfig, levelDataPath);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = levelListConfig;
        }
    }

    [Button]
    private void ClearLevelsData()
    {
        string levelDataPath = "Assets/Color Hoop Stack/Scripts/Design Data/LevelListConfig.asset";
        bool assetExists = AssetDatabase.GetMainAssetTypeAtPath(levelDataPath) != null;
        if (assetExists)
        {
            AssetDatabase.DeleteAsset(levelDataPath);
        }

    }
}
#endif