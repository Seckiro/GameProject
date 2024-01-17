using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(menuName = "Assets/CreateGameGradeDataTable", fileName = "GameGradeDataTable")]
public class GameGradeDataTable : ScriptableObject
{
    public List<GameGradeDataAsset> gameGradeAsset = new List<GameGradeDataAsset>();
}

[Serializable]
public class GameGradeDataAsset
{
    [Serializable]
    public class DictFloorTypeProbability : SerializableDictionary<FloorType, int> { }

    // ¥’∫œ”√
    public float floorMaxIntervalRangPosX = 3f;
    public float floorMinIntervalRangPosX = 1f;

    public float floorHightMax = 1f;
    public float floorHightMin = 0.5f;
    public DictFloorTypeProbability _dictFloorTypeProbability = new DictFloorTypeProbability();
}