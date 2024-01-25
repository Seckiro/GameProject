using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    private GameObject _curCharacter;

    public GameObject CurCharacter { get => _curCharacter; set => _curCharacter = value; }

    /// <summary>
    /// 玩家数据初始化
    /// </summary>
    public void OnInit()
    {
        // 读取配置数据
        _curCharacter = Resources.Load<GameObject>("Prefabs/Character");
    }

    /// <summary>
    /// 角色创建
    /// </summary>
    public void OnCreate()
    {
        _curCharacter = GameObject.Instantiate(_curCharacter);
        _curCharacter.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 角色销毁
    /// </summary>
    public void OnDestroy()
    {
        GameObject.Destroy(_curCharacter);
    }
}
