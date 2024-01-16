using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    public GameObject[] _gameObjects;
    private Transform _cameraTrans;

    private FloorObjectPool _floorObjectPool = new FloorObjectPool();

    private float _currentHightY;
    private float _hight = 5.5f;

    private float _floorHightMax = 1f;
    private float _floorHightMin = 0.5f;

    private float _floorRangPosX = 2.8f;

    void Start()
    {
        _cameraTrans = Camera.main.transform;
        _currentHightY = _cameraTrans.position.y;
        _floorObjectPool.Create(_gameObjects[0], this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHightY < _cameraTrans.position.y + _hight)
        {
            CreatFloor();
        }
    }

    private void CreatFloor()
    {
        _currentHightY += Random.Range(_floorHightMin, _floorHightMax);
        float floorPosX = Random.Range(-_floorRangPosX, _floorRangPosX);

        GameObject gameObject = _floorObjectPool.GetFloorObj();
        gameObject.transform.position = new Vector3(floorPosX, _currentHightY, 0);
        gameObject.GetComponent<Floor>().floorObjectPool = _floorObjectPool;
        //对象池取对象
    }
}
