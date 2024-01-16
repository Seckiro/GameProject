using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed;
    public GameObject character;
    private Vector3 velocity;

    private void LateUpdate()
    {
        if (character.transform.position.y > this.transform.position.y)
        {
            Vector3 cameraPos = this.transform.position;
            Vector3 target = new Vector3(0, character.transform.position.y, 0);
            this.transform.position = Vector3.SmoothDamp(cameraPos, target, ref velocity, smoothSpeed * Time.deltaTime);
        }
    }
}
