using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D _rigidbody2D;

    private CharacterControls _characterControls;

    private void Awake()
    {
        //_rigidbody2D = GetComponent<Rigidbody2D>();
        _characterControls = new CharacterControls();
        _characterControls.Player.Fire.performed += Fire;
    }

    private void OnEnable()
    {
        _characterControls.Enable();
    }

    private void OnDisable()
    {
        _characterControls.Disable();
    }

    private void Move(Vector2 vector)
    {
        if (_rigidbody2D != null)
        {
            float h = vector.x;
            float v = vector.y;
            //_rigidbody2D.velocity = new Vector2(speed * h, _rigidbody2D.velocity.y);

            if (h != 0)
            {
                transform.localScale = new Vector3(-h, 1, 1);
            }

        }
    }


    /// <summary>
    /// ÊµÀý»¯×Óµ¯
    /// </summary>
    /// <param name="context"></param>
    private void Fire(InputAction.CallbackContext context)
    {

    }

    void Update()
    {
        Move(_characterControls.Player.Move.ReadValue<Vector2>());
    }
}
