using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// 生成类型 生成概率
/// </summary>
public class FloorPoolSystem : SystemBase
{
    private int _tatal = 0;

    private GameObject _praent;

    private Dictionary<FloorType, FloorObjectPool> _dictFloorPoolOfType = new Dictionary<FloorType, FloorObjectPool>();
    private Dictionary<Vector2, FloorType> _dictFloorTypeCreateProbability = new Dictionary<Vector2, FloorType>();

    private FloorPoolSystem() { }

    public FloorPoolSystem(Dictionary<FloorType, int> dictFloorPoolOfType)
    {
        SetGenerateGradeProbability(dictFloorPoolOfType);
    }

    public void Dispose()
    {
        _tatal = 0;
        _dictFloorPoolOfType.Clear();
        _dictFloorTypeCreateProbability.Clear();
    }

    public GameObject GenerateFloor()
    {
        FloorObjectPool _floorObjectPool = _dictFloorPoolOfType[GenerateFloorType(Random.Range(0, _tatal))];

        GameObject gameObject = _floorObjectPool.GetFloor();

        gameObject.GetComponent<FloorBase>().FloorObjectPool = _floorObjectPool;

        return gameObject;
    }

    public void SetGenerateGradeProbability(Dictionary<FloorType, int> dictFloorPoolOfType)
    {
        Dispose();

        foreach (var item in dictFloorPoolOfType)
        {
            _tatal += item.Value;
            if (!_dictFloorPoolOfType.ContainsKey(item.Key))
            {
                if (_praent == null)
                {
                    _praent = new GameObject("FloorPraent");
                }
                GameObject floorAsset = LoadFloorAsset(item.Key);
                FloorObjectPool floorObjectPool = FloorObjectPool.Create(floorAsset, _praent.transform);
                _dictFloorPoolOfType.Add(item.Key, floorObjectPool);
            }
        }

        float last = 0;
        foreach (var item in dictFloorPoolOfType)
        {
            var range = new Vector2(last, last + (item.Value / _tatal));
            last = range.y;
            _dictFloorTypeCreateProbability.Add(range, item.Key);
        }
    }

    private GameObject LoadFloorAsset(FloorType floorType)
    {
        return ResManager.Instance.Load<GameObject>($"Assets/Res/Prefabs/{floorType}Floor.prefab");
    }

    private FloorType GenerateFloorType(float random)
    {
        foreach (var item in _dictFloorTypeCreateProbability)
        {
            Vector2 vector2 = item.Key;
            if (vector2.x < random && random <= vector2.y)
            {
                return item.Value;
            }
        }
        return FloorType.Normal;
    }
}
