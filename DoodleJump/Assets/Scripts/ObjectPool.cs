using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorObjectPool
{
    private Transform _parent;
    private GameObject _template;
    private int InstantiateCount = 10;
    private Stack<GameObject> _gameObjects = new Stack<GameObject>();

    public FloorObjectPool Create(GameObject template, Transform parent)
    {
        _template = template;
        InitPool();
        return new FloorObjectPool();
    }

    public GameObject GetFloorObj()
    {
        if (_gameObjects.TryPop(out GameObject result))
        {
            if (result != null)
            {
                result.SetActive(true);
                return result;
            }
            else
            {
                return GetFloorObj();
            }
        }
        InitPool();
        return GetFloorObj();
    }

    public void Recovery(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _gameObjects.Push(gameObject);
    }

    private void InitPool()
    {
        if (_template != null)
        {
            for (int i = 0; i < InstantiateCount; i++)
            {
                GameObject gameObject = GameObject.Instantiate(_template);
                gameObject.transform.SetParent(_parent);
                gameObject.SetActive(false);
                _gameObjects.Push(GameObject.Instantiate(_template));
            }
        }
    }

}
