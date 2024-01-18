using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUitility
{
    public static bool PlayerTagDetermine(GameObject Player)
    {
        return Player.CompareTag("Player");
    }

    public static bool PlayeRigidbody2DDetermine(GameObject Player, out Rigidbody2D rigidbody2D)
    {
        return Player.TryGetComponent<Rigidbody2D>(out rigidbody2D);
    }

    public static bool PlayeCollisionDetermine(GameObject Player, out Rigidbody2D rigidbody2D)
    {
        return PlayeRigidbody2DDetermine(Player, out rigidbody2D) && PlayerTagDetermine(Player);
    }

    public static bool PlayerSpriteRendererDetermine(GameObject Player, out SpriteRenderer spriteRenderer)
    {
        return Player.TryGetComponent<SpriteRenderer>(out spriteRenderer);
    }
}
