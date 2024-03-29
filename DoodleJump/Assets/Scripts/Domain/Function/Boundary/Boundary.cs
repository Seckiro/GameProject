using System;
using UniRx;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Boundary : MonoBehaviour
{
    private Vector3 _startPos = Vector3.zero;

    private IDisposable _boundaryTask;

    private Camera _followCamera;
    private Transform _followCameraObj;
    private BoxCollider2D _boundaryBox;
    private SpriteLibrary _spriteLibrary;
    private SpriteResolver _spriteResolver;

    public bool BoundaryActive => _boundaryBox.enabled;

    private void Awake()
    {
        _boundaryBox = this.GetComponent<BoxCollider2D>();
        _spriteLibrary = this.GetComponent<SpriteLibrary>();
        _spriteResolver = this.GetComponent<SpriteResolver>();
        _startPos = this.transform.position;
    }

    public void Init()
    {
        LoadSprite();
    }

    public void LoadSprite()
    {
        string key = this.GetType().ToString();
        if (PlayerPrefs.HasKey(key))
        {
            SetSprite(PlayerPrefs.GetString(key));
        }
    }

    public void SaveSprite()
    {
        PlayerPrefs.SetString(this.GetType().ToString(), _spriteResolver.GetLabel());
    }


    public void SetSprite(string name)
    {
        if (_spriteResolver.SetCategoryAndLabel(BoundarySystem.SpriteLibraryAssetCategoryName, name))
        {
            PlayerPrefs.SetString(this.GetType().ToString(), name);
        }
        else
        {
            Debug.LogError("_spriteResolver SetSprite Error");
        }
    }

    public void SetSpriteLibraryAsset(SpriteLibraryAsset spriteLibraryAsset)
    {
        _spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
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
        // ����Լ��
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
