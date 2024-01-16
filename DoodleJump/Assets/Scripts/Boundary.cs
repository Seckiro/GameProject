using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ×óÓÒÔ¼Êø
        GameObject gameObject = collision.gameObject;
        if (gameObject.CompareTag("Player") && gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            Vector3 vector3 = gameObject.transform.position;
            gameObject.transform.position = new Vector3(-vector3.x * 0.9f, vector3.y, vector3.z);
        }
    }

}
