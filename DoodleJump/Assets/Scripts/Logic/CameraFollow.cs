using System;
using UniRx;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private bool _followState = false;

    private float _smoothSpeed = 0.0f;

    private Vector3 _startPos = Vector3.zero;

    private Character _followCharacter;

    private Transform _followCharacterObj;

    private IDisposable _followTask = null;

    public bool FollowState => _followState;

    private void Awake()
    {
        _startPos = this.transform.position;
    }

    public void SetFollowInit(float smoothSpeed)
    {
        SetStartPos();
        this._smoothSpeed = smoothSpeed;
    }

    public void SetFollowObj(Character followCharacter)
    {
        _followCharacter = followCharacter;
        _followCharacterObj = _followCharacter.transform;
    }

    public void SetFollowStart()
    {
        SetStartPos();
        SetFollowState(true);
    }

    public void SetFollowEnd()
    {
        SetFollowState(false);
        this._followCharacter = null;
        this._smoothSpeed = 0.0f;
    }

    private void SetStartPos()
    {
        this.transform.position = _startPos;
    }

    private void SetFollowState(bool state)
    {
        _followState = state;
        if (_followState)
        {
            _followTask?.Dispose();
            _followTask = Observable.EveryLateUpdate().Subscribe(_ =>
            {
                if (_followCharacter.transform.position.y > this.transform.position.y)
                {
                    FollowMove();
                }
            });
        }
        else
        {
            float deathFollowTime = 2.0f;
            _followTask?.Dispose();
            _followTask = Observable.EveryLateUpdate().Subscribe(_ =>
            {
                deathFollowTime -= Time.deltaTime;
                FollowMove();
                if (deathFollowTime <= 0)
                {
                    _followTask?.Dispose();
                }
            });
        }
    }

    private void FollowMove()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 cameraPos = this.transform.position;
        this.transform.position = Vector3.SmoothDamp(cameraPos, new Vector3(0, _followCharacterObj.position.y, -10), ref velocity, _smoothSpeed * Time.deltaTime);
    }
}
