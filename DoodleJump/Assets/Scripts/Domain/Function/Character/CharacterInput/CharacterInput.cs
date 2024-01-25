using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInput
{
    public float _speed = 5f;

    private Character _character;
    private Transform _transform;
    private Rigidbody2D _rigidbody2D;
    private CharacterControls _characterControls;

    public CharacterInput(Character character)
    {
        _character = character;
        _transform = _character.transform;
        _characterControls = new CharacterControls();
        if (!_character.TryGetComponent<Rigidbody2D>(out _rigidbody2D))
        {
            _rigidbody2D = _character.gameObject.AddComponent<Rigidbody2D>();
        }
        _characterControls.Player.Fire.performed += Fire;
        _characterControls.Player.Move.performed += Move;
    }

    public void SetSpeed(float speed)
    {
        this._speed = speed;
    }

    public void OnEnable()
    {
        _characterControls.Enable();
    }

    public void OnDisable()
    {
        _characterControls.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (_rigidbody2D != null)
        {
            var vector = context.ReadValue<Vector2>();
            float h = vector.x;
            //float v = vector.y;
            _rigidbody2D.velocity = new Vector2(_speed * h, _rigidbody2D.velocity.y);

            if (h != 0)
            {
                _transform.localScale = new Vector3(-h, 1, 1);
            }
        }
    }

    private void Fire(InputAction.CallbackContext context)
    {
        if (context.ReadValue<bool>())
        {
            Debug.Log("Fire");
        }
    }
}
