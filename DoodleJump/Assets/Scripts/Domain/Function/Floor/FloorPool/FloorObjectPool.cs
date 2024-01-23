using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorObjectPool
{
    private Transform _parent;
    private GameObject _template;
    private int InitCount = 10;
    private Stack<GameObject> _gameObjectPool = new Stack<GameObject>();
    private Stack<GameObject> _gameObjectActive = new Stack<GameObject>();

    public static FloorObjectPool Create(GameObject template, Transform parent)
    {
        FloorObjectPool floorObjectPool = new FloorObjectPool();
        floorObjectPool._template = template;
        floorObjectPool._parent = parent;
        floorObjectPool.InitPool();
        return floorObjectPool;
    }

    public GameObject GetFloor()
    {
        if (_gameObjectPool.TryPop(out GameObject result))
        {
            if (result != null)
            {
                result.SetActive(true);
                _gameObjectActive.Push(result);
                return result;
            }
            else
            {
                return GetFloor();
            }
        }
        InitPool();
        return GetFloor();
    }

    public void Recovery(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _gameObjectPool.Push(gameObject);
    }

    public void RecoveryAll()
    {
        foreach (var item in _gameObjectActive)
        {
            Recovery(item);
        }
    }

    private void InitPool()
    {
        if (_template != null)
        {
            for (int i = 0; i < InitCount; i++)
            {
                GameObject gameObject = GameObject.Instantiate(_template);
                gameObject.transform.SetParent(_parent);
                gameObject.SetActive(false);
                _gameObjectPool.Push(gameObject);
            }
        }
    }

}
