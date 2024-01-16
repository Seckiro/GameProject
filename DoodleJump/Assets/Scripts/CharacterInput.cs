using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public bool isAerial = true;
    public float speed = 5f;
    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody2D != null && isAerial)
        {
            float h = Input.GetAxisRaw("Horizontal");
            _rigidbody2D.velocity = new Vector2(speed * h, _rigidbody2D.velocity.y);

            if (h != 0)
            {
                transform.localScale = new Vector3(-h, 1, 1);
            }
        }
    }
}
