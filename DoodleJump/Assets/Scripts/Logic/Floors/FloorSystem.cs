using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// 等级判断  生成位置
/// </summary>
public class FloorSystem : SystemBase
{
    /// <summary>
    /// 上下边界
    /// </summary>
    private float _generateRange = 8f;
    private float _currentHeight = 0.0f;

    private Vector3 _floorLsatPos = Vector3.zero;

    private FloorPoolManager _floorObjectPool = null;

    private GameGradeDataTable _gameGradeDataTable = null;
    private GameGradeDataAsset _gameGradeDataAsset = null;

    private List<FloorBase> _listLifeFloor = new List<FloorBase>();

    /// <summary>
    /// 加载用到的地板类型和概率
    /// </summary>
    public override void SystemInit()
    {
        _gameGradeDataTable = ResManager.Instance.Load<GameGradeDataTable>("Assets/Res/Configs/GameGradeDataTable.asset");
        _gameGradeDataAsset = _gameGradeDataTable.gameGradeAsset[0];
        _floorObjectPool = new FloorPoolManager(_gameGradeDataAsset._dictFloorTypeProbability);
    }

    public override void SystemStart()
    {
        _currentHeight = Camera.main.transform.position.y - _generateRange;
    }

    public override void SystemUpdata()
    {
        CreateFloor();
        RecoveryFloor();
    }

    public override void SystemDestroy()
    {
        CreateFloor();
        RecoveryFloor();
    }

    private void CreateFloor()
    {
        if (_currentHeight < Camera.main.transform.position.y + _generateRange)
        {
            GameObject floorObj = _floorObjectPool.GenerateFloor();
            SetFloorPos(floorObj);
            _listLifeFloor.Add(floorObj.GetComponent<FloorBase>());
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

    private void SetFloorPos(GameObject floorObj)
    {
        Vector3 targetPos = GetTargetPos(_currentHeight);

        while (MathF.Abs(Vector3.Distance(_floorLsatPos, targetPos)) < _gameGradeDataAsset.floorMinIntervalRangPosX)
        {
            targetPos = GetTargetPos(_currentHeight);
        }

        _floorLsatPos = targetPos;
        _currentHeight = _floorLsatPos.y;
        floorObj.transform.position = _floorLsatPos;
    }

    private Vector3 GetTargetPos(float currentHeight)
    {
        return new Vector3(Random.Range(-_gameGradeDataAsset.floorMaxIntervalRangPosX, _gameGradeDataAsset.floorMaxIntervalRangPosX), currentHeight + Random.Range(_gameGradeDataAsset.floorHightMin, _gameGradeDataAsset.floorHightMax), 0);
    }
}
