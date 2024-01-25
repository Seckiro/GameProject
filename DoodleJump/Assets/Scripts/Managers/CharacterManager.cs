using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    private GameObject _curCharacter;

    public GameObject CurCharacter { get => _curCharacter; set => _curCharacter = value; }

    /// <summary>
    /// ������ݳ�ʼ��
    /// </summary>
    public void OnInit()
    {
        // ��ȡ��������
        _curCharacter = Resources.Load<GameObject>("Prefabs/Character");
    }

    /// <summary>
    /// ��ɫ����
    /// </summary>
    public void OnCreate()
    {
        _curCharacter = GameObject.Instantiate(_curCharacter);
        _curCharacter.transform.position = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// ��ɫ����
    /// </summary>
    public void OnDestroy()
    {
        GameObject.Destroy(_curCharacter);
    }
}
