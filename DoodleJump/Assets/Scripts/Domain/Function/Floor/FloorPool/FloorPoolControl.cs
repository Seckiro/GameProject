using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 生成类型 生成概率
/// </summary>
public class FloorPoolControl
{
    /// <summary>
    /// 概率累计总和
    /// </summary>
    private int _tatal = 0;
    private int _currentGrade = 0;

    private float _generateRange = 8f;
    private float _currentHeight = 0.0f;

    private Vector3 _floorLsatPos = Vector3.zero;

    private GameObject _poolRootPraent = null;
    private Dictionary<string, GameObject> _poolPraent = new Dictionary<string, GameObject>();

    private GameGradeDataTable _gameGradeDataTable = null;
    private GameGradeDataAsset _curGradeDataAsset = null;

    private List<FloorBase> _listLifeFloor = new List<FloorBase>();

    private Dictionary<FloorType, FloorObjectPool> _dictFloorPoolOfType = new Dictionary<FloorType, FloorObjectPool>();
    private Dictionary<Vector2, FloorType> _dictFloorTypeCreateProbability = new Dictionary<Vector2, FloorType>();

    public int CurrentGrade { get => _currentGrade; set => _currentGrade = value; }

    public void LoadFloorData()
    {
        _gameGradeDataTable = ResManager.Instance.Load<GameGradeDataTable>("Assets/Res/Configs/GameGradeDataTable.asset");
    }

    public void StartGame()
    {
        SetGradeData(_currentGrade);
    }

    public void UpdateCurrentFloor(int currentGrade)
    {
        CreateFloor();
        UpdateCurrentGrade(currentGrade);
        RecoveryFloor();
    }

    public void GameEnd()
    {
        ClearFloorData();
        _gameGradeDataTable = null;
    }

    private void SetGradeData(int grade)
    {
        ClearFloorData();
        _curGradeDataAsset = _gameGradeDataTable.gameGradeAsset[grade];
        var curFloorTypeProbabilityData = _curGradeDataAsset._dictFloorTypeProbability;
        SetFloorObjPool(curFloorTypeProbabilityData);
        SetFloorProbability(curFloorTypeProbabilityData);

    }

    private void SetFloorObjPool(GameGradeDataAsset.DictFloorTypeProbability dictFloorTypeProbability)
    {
        if (_poolRootPraent == null)
        {
            _poolRootPraent = new GameObject("FloorPoolPraent");
            _poolRootPraent.transform.localPosition = Vector3.zero;
            _poolRootPraent.transform.localScale = Vector3.one;
        }

        foreach (var item in dictFloorTypeProbability)
        {
            _tatal += item.Value;
            if (!_dictFloorPoolOfType.ContainsKey(item.Key))
            {
                FloorType type = item.Key;
                string poolParentName = $"{item.Key}FloorPraent";
                if (!_poolPraent.ContainsKey(poolParentName))
                {
                    GameObject nodeParent = new GameObject(poolParentName);
                    nodeParent.transform.SetParent(_poolRootPraent.transform);
                    _poolRootPraent.transform.localPosition = Vector3.zero;
                    _poolRootPraent.transform.localScale = Vector3.one;
                    _poolPraent.Add(poolParentName, nodeParent);
                }
                GameObject floorNode = ResManager.Instance.Load<GameObject>($"Assets/Res/Prefabs/{type}Floor.prefab");
                FloorObjectPool floorObjectPool = FloorObjectPool.Create(floorNode, _poolPraent[poolParentName].transform);
                _dictFloorPoolOfType.Add(item.Key, floorObjectPool);
            }
        }
    }

    private void SetFloorProbability(GameGradeDataAsset.DictFloorTypeProbability dictFloorTypeProbability)
    {
        float last = 0;
        foreach (var item in dictFloorTypeProbability)
        {
            var range = new Vector2(last, last + (item.Value / _tatal));
            last = range.y;
            _dictFloorTypeCreateProbability.Add(range, item.Key);
        }
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

    private GameObject GenerateFloorObj()
    {
        FloorObjectPool _floorObjectPool = _dictFloorPoolOfType[GenerateFloorType(Random.Range(0, _tatal))];

        GameObject gameObject = _floorObjectPool.GetFloor();

        if (gameObject.GetComponent<FloorBase>().FloorObjectPool == null)
        {
            gameObject.GetComponent<FloorBase>().FloorObjectPool = _floorObjectPool;
        }

        return gameObject;
    }

    private void CreateFloor()
    {
        if (_currentHeight < Camera.main.transform.position.y + _generateRange)
        {
            GameObject floorObj = GenerateFloorObj();
            SetFloorPos(floorObj);
            _listLifeFloor.Add(floorObj.GetComponent<FloorBase>());
        }
    }

    private void SetFloorPos(GameObject floorObj)
    {
        Vector3 targetPos = GetTargetPos(_currentHeight);

        while (MathF.Abs(Vector3.Distance(_floorLsatPos, targetPos)) < _curGradeDataAsset.floorMinIntervalRangPosX)
        {
            targetPos = GetTargetPos(_currentHeight);
        }

        _floorLsatPos = targetPos;
        _currentHeight = _floorLsatPos.y;
        floorObj.transform.position = _floorLsatPos;
    }

    private Vector3 GetTargetPos(float currentHeight)
    {
        return new Vector3(Random.Range(-_curGradeDataAsset.floorMaxIntervalRangPosX, _curGradeDataAsset.floorMaxIntervalRangPosX), currentHeight + Random.Range(_curGradeDataAsset.floorHightMin, _curGradeDataAsset.floorHightMax), 0);
    }

    private void UpdateCurrentGrade(int currentGrade)
    {
        if (currentGrade != _currentGrade)
        {
            _currentGrade = currentGrade;
            SetGradeData(_currentGrade);
        }
    }

    private void RecoveryFloor()
    {
        for (int i = 0; i < _listLifeFloor.Count; i++)
        {
            FloorBase item = _listLifeFloor[i];
            if (_currentHeight - (2 * _generateRange) > item.transform.position.y)
            {
                item.Recovery();
                _listLifeFloor.Remove(item);
                i--;
            }
        }
    }

    private void ClearFloorData()
    {
        _tatal = 0;
        _curGradeDataAsset = null;
        _dictFloorPoolOfType.Clear();
        _dictFloorTypeCreateProbability.Clear();
    }

}
