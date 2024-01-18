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
        // 左右约束
        GameObject player = collision.gameObject;
        if (GameUitility.PlayerTagDetermine(player))
        {
            Vector3 vector3 = player.transform.position;
            player.transform.position = new Vector3(-vector3.x * 0.9f, vector3.y, vector3.z);
        }
        if ((this.transform.localPosition.y - (_boundaryBox.size.y / 2)) > player.transform.localPosition.y)
        {
            Debug.Log("死亡");
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
        //读取数据

        //加载 背景
        _spriteRenderer = _boundaryObj.AddComponent<SpriteRenderer>();

        // 添加边界 初始化
        _boundaryBox = _boundaryObj.AddComponent<BoxCollider2D>();

        // 添加死亡区域 初始化
        _deathBox = _boundaryObj.AddComponent<BoxCollider2D>();

    }

    public override void SystemStart()
    {
        // 激活所有区域
    }

    public override void SystemEnd()
    {
        base.SystemEnd();
    }

}
