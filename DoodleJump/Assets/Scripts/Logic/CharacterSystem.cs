using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSystem : SystemBase
{
    private Character _curCharacter;
    private void CreateCharacter()
    {
        GameObject characterObj = new GameObject();
        _curCharacter = characterObj.AddComponent<Character>();
    }


}
