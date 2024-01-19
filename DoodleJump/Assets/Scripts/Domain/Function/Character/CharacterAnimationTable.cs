using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Assets/CharacterAnimationTable", fileName = "CharacterAnimationTable")]
public class CharacterAnimationTable : ScriptableObject
{
    [Serializable]
    public class DictCharacterAnimation : SerializableDictionary<int, CharacterAnimationData> { }

    public DictCharacterAnimation _dictCharacterAnimationData = new DictCharacterAnimation();
}

[Serializable]
public class CharacterAnimationData
{
    public Texture2D Up;
    public Texture2D Down;
    public Texture2D Left;
    public Texture2D Right;
    public Texture2D Idle;
    public Texture2D IdleLeft;
    public Texture2D IdleRight;
    public Texture2D Attack;
    public Texture2D AttackLeft;
    public Texture2D AttackRight;
    public Texture2D Hurt;
}
