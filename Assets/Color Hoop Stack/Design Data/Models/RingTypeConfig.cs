using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(fileName = "SFXTypeConfig", menuName = "Game Configuration/Ring Type Config", order = 1)]
public class RingTypeConfig : ScriptableObject
{
    public KVPList<RingType, Material> configDic;
}
