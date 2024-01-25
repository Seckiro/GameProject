using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDisplay
{
    private Character _character;

    private SpriteRenderer _spriteRenderer;

    private CharacterDisplay() { }

    public CharacterDisplay(Character character)
    {
        _character = character;
        if (!_character.TryGetComponent<SpriteRenderer>(out _spriteRenderer))
        {
            _spriteRenderer = _character.gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}
