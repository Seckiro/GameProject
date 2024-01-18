using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D _boundaryBox;

    private void Awake()
    {
        _boundaryBox = this.GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        this.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0);
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
            Debug.Log("����");
        }
    }
}

public class BoundarySystem : SystemBase
{
    private SpriteRenderer _spriteRenderer;

    private BoxCollider2D _boundaryBox;

    private BoxCollider2D _deathBox;

    private GameObject _boundaryObj;

    public override bool SystemActive { get; set; }

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
