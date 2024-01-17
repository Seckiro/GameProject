using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    private void Update()
    {

        Vector3 cameraPosition = Camera.main.transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // ����Լ��
        GameObject gameObject = collision.gameObject;
        if (gameObject.CompareTag("Player") && gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            Vector3 vector3 = gameObject.transform.position;
            gameObject.transform.position = new Vector3(-vector3.x * 0.9f, vector3.y, vector3.z);
        }
    }
}

public class BoundarySystem : SystemBase
{
    private SpriteRenderer _spriteRenderer;

    private BoxCollider2D _boundaryBox;

    private BoxCollider2D _deathBox;

    private GameObject _boundaryObj;

    public override void SystemInit()
    {
        //��ȡ����

        //���� ����
        _spriteRenderer = _boundaryObj.AddComponent<SpriteRenderer>();

        // ��ӱ߽� ��ʼ��
        _boundaryBox = _boundaryObj.AddComponent<BoxCollider2D>();

        // ����������� ��ʼ��
        _deathBox = _boundaryObj.AddComponent<BoxCollider2D>();

    }

    public override void SystemStart()
    {
        // ������������
    }

    public override void SystemEnd()
    {
        base.SystemEnd();
    }

}
