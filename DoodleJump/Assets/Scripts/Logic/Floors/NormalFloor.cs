using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalFloor : FloorBase
{
    public override Vector2 Vector2Force => new Vector2(0, 100);

    public override void OnDepress(GameObject gameObject, Rigidbody2D rigidbody2D)
    {

    }

    public override void OnJumpUp(GameObject gameObject, Rigidbody2D rigidbody2D)
    {

    }
}
