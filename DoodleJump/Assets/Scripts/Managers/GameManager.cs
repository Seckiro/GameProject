using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public FloorSystem floorSystem;

    private List<ISystemBase> _listSystem = new List<ISystemBase>();


    private void Awake()
    {
        SystemRegister();
    }
    private void Start()
    {
        SystemInit();
    }

    private void SystemRegister()
    {
        _listSystem.Add(new FloorSystem());
    }


    private void SystemInit()
    {
        foreach (var item in _listSystem)
        {
            item.SystemInit();
        }
    }

}
