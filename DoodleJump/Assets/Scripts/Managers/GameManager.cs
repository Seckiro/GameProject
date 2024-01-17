using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMono<GameManager>
{

    IDisposable _UpdataTask;
    private List<ISystemBase> _listSystem = new List<ISystemBase>();


    private void Awake()
    {
        SystemRegister();
    }

    private void Start()
    {
        SystemInit();
    }

    public void GameStart()
    {
        ForeachListSystem(item =>
        {
            item.SystemStart();
        });
        _UpdataTask?.Dispose();
        _UpdataTask = Observable.EveryUpdate().Subscribe(_ => GameUpdate());
    }

    public void GameUpdate()
    {
        ForeachListSystem(item =>
        {
            item.SystemUpdata();
        });
    }

    private void SystemRegister()
    {
        _listSystem.Add(new CharacterSystem());
        _listSystem.Add(new BoundarySystem());
        _listSystem.Add(new FloorSystem());

    }

    private void SystemInit()
    {
        ForeachListSystem(item =>
        {
            item.SystemInit();
        });
    }

    private void ForeachListSystem(Action<ISystemBase> action)
    {
        foreach (ISystemBase item in _listSystem)
        {
            action?.Invoke(item);
        }
    }



}
