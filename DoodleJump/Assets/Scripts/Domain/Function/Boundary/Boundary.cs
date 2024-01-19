using System;
using UniRx;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private Vector3 _startPos = Vector3.zero;

    private IDisposable _boundaryTask;

    private Camera _followCamera;
    private Transform _followCameraObj;
    private BoxCollider2D _boundaryBox;
    private SpriteRenderer _spriteRenderer;

    public bool BoundaryActive => _boundaryBox.enabled;

    private void Awake()
    {
        _boundaryBox = this.GetComponent<BoxCollider2D>();
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _startPos = this.transform.position;
    }

    public void SetBackGround(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }

    public void SetFollowCamera(Camera followCamera)
    {
        _followCamera = followCamera;
        _followCameraObj = _followCamera.transform;
    }

    public void SetBoundaryStart()
    {
        SetStartPos();
        _boundaryBox.enabled = true;
        _boundaryTask?.Dispose();
        _boundaryTask = Observable.EveryUpdate().Subscribe(_ =>
        {
            Vector3 cameraPosition = _followCameraObj.position;
            this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0);
        });
    }

    public void SetBoundaryEnd()
    {
        _boundaryBox.enabled = false;
        _boundaryTask?.Dispose();
    }

    private void SetStartPos()
    {
        this.transform.localPosition = _startPos;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ×óÓÒÔ¼Êø
        GameObject player = collision.gameObject;
        if (GameUitility.PlayerTagDetermine(player))
        {
            Vector3 vector3 = player.transform.position;
            player.transform.position = new Vector3(-vector3.x * 0.9f, vector3.y, vector3.z);
        }
        if ((this.transform.localPosition.y - (_boundaryBox.size.y / 2)) > player.transform.localPosition.y)
        {
            GameManager.Instance.GameEnd();
        }
    }
}
