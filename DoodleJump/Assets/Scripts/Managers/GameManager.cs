using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private IDisposable _UpdataTask;

    private Dictionary<Type, ISystemBase> _dictSystem = new Dictionary<Type, ISystemBase>();

    private void Awake()
    {
        SystemRegister();

        GameInit();

        UIManager.Instance.Init();
    }

    private void Start()
    {
        Debug.Log("GameStart");
    }

    public T GetSystem<T>() where T : class, ISystemBase, new()
    {
        if (_dictSystem.TryGetValue(typeof(T), out ISystemBase systemBase))
        {
            return systemBase as T;
        }
        return null;
    }

    public void GameInit()
    {
        ForeachDictSystem(item =>
        {
            item.SystemInit();
        });
    }

    public void GameStart()
    {
        ForeachDictSystem(item =>
        {
            item.SystemStart();
        });
        _UpdataTask?.Dispose();
        _UpdataTask = Observable.EveryUpdate().Subscribe(_ => GameUpdate());
    }

    public void GameUpdate()
    {
        ForeachDictSystem(item =>
        {
            item.SystemUpdate();
        });
    }

    public void GameEnd()
    {
        ForeachDictSystem(item =>
        {
            item.SystemEnd();
        });
        _UpdataTask?.Dispose();
        _UpdataTask = null;
    }

    public void GameDestroy()
    {
        ForeachDictSystem(item =>
        {
            item.SystemDestroy();
        });
    }

    public void GameQuit()
    {
        ForeachDictSystem(item =>
        {
            item.SystemQuit();
        });
    }

    private void ForeachDictSystem(Action<ISystemBase> action)
    {
        foreach (var item in _dictSystem)
        {
            action?.Invoke(item.Value);
        }
    }

    private void ResgisterSystem(ISystemBase systemBase)
    {
        if (systemBase != null)
        {
            _dictSystem[systemBase.GetType()] = systemBase;
        }
    }

    private void SystemRegister()
    {
        //ResgisterSystem(new CharacterSystem());
        ResgisterSystem(new BoundarySystem());
        //ResgisterSystem(new FloorSystem());
        //ResgisterSystem(new CameraFollowSystem());
    }
}
