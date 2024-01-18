using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色组件管理
/// 角色表现
/// 初始化数据
/// </summary>
public class Character : MonoBehaviour
{
    CharacterInput _characterInput;
    SpriteRenderer _spriteRenderer;
    BoxCollider2D _boxCollider2D;
    Rigidbody2D _rigidbody2D;

    internal void CharacterDestroy()
    {
        throw new NotImplementedException();
    }

    internal void CharacterEnd()
    {
        throw new NotImplementedException();
    }

    internal void CharacterStart()
    {
        throw new NotImplementedException();
    }

    internal void CharacterUpdate()
    {
        throw new NotImplementedException();
    }

    void Awake()
    {
        _rigidbody2D = gameObject.AddComponent<Rigidbody2D>();
        _boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        _spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        _characterInput = gameObject.AddComponent<CharacterInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterInput.speed = 5;

        _boxCollider2D.offset = new Vector2(0, 0);

        _boxCollider2D.size = _spriteRenderer.size;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
