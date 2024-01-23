using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : SystemBase
{
    private int _currentGrade = 0;
    private int _currentScore = 0;

    private Camera _followCamera;
    private Transform _followCameraObj;

    private List<int> _listGrade = new List<int>();

    private event Action<int> _scoreUpdateEvent;

    private event Action<int> _gradeUpdateEvent;

    public override bool SystemActive { get; set; }

    public event Action<int> ScoreUpdateEvent { add => _scoreUpdateEvent += value; remove => _scoreUpdateEvent -= value; }

    public event Action<int> GradeUpdateEvent { add => _gradeUpdateEvent += value; remove => _gradeUpdateEvent -= value; }

    public override void SystemInit()
    {
        _scoreUpdateEvent = null;
        _gradeUpdateEvent = null;
        // 初始化等级数据
    }

    public override void SystemReady()
    {
        _followCamera = Camera.current;
        _followCameraObj = _followCamera.transform;
    }

    public override void SystemStart()
    {
        _scoreUpdateEvent?.Invoke(0);
        _gradeUpdateEvent?.Invoke(_currentGrade);
    }

    public override void SystemUpdate()
    {
        _currentScore = (int)_followCameraObj.position.y;
        _scoreUpdateEvent?.Invoke(_currentScore);
        if (_currentGrade < _listGrade.Count && _currentScore > _listGrade[_currentGrade])
        {
            _gradeUpdateEvent?.Invoke(++_currentGrade);
        }
    }

    public override void SystemDestroy()
    {
        _scoreUpdateEvent = null;
        _gradeUpdateEvent = null;
    }
}
