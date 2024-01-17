using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    FloorSystem floorSystem;
    private void Start()
    {
        floorSystem = new FloorSystem();
        floorSystem.Init();
        floorSystem.Start();
    }

}
