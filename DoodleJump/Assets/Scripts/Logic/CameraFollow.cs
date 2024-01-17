using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Events;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed;
    public GameObject character;

    private Vector3 velocity;
    private IDisposable _updataTask = null;
    private IntReactiveProperty _cameraReactiveY;
    public event Action<int> UpdataHighEvent;

    private void Init(GameObject character, float smoothSpeed)
    {
        this.character = character;
        this.smoothSpeed = smoothSpeed;

        _cameraReactiveY = new IntReactiveProperty(0);
        _cameraReactiveY.Subscribe(UpdataHigh);
    }

    private void Start()
    {
        _updataTask?.Dispose();
        _updataTask = Observable.EveryLateUpdate().Subscribe(_ => OnUpdate());
    }

    private void End()
    {
        _cameraReactiveY.Dispose();
    }

    private void OnUpdate()
    {
        if (character.transform.position.y > this.transform.position.y)
        {
            Vector3 cameraPos = this.transform.position;
            Vector3 target = new Vector3(0, character.transform.position.y, -10);
            this.transform.position = Vector3.SmoothDamp(cameraPos, target, ref velocity, smoothSpeed * Time.deltaTime);
            _cameraReactiveY.Value = (int)this.transform.position.y;
        }
    }

    private void UpdataHigh(int hight)
    {
        UpdataHighEvent?.Invoke(hight);
    }
}
