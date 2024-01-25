using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private CharacterInput _characterInput;
    private CharacterDisplay _characterDisplay;

    public CharacterInput CharacterInput { get => _characterInput; set => _characterInput = value; }
    public CharacterDisplay CharacterDisplay { get => _characterDisplay; set => _characterDisplay = value; }

    private void Awake()
    {
        _characterInput = new CharacterInput(this);
        _characterDisplay = new CharacterDisplay(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void CharacterStart()
    {
    }

    public void CharacterUpdate()
    {
    }
    public void CharacterEnd()
    {
    }

    public void CharacterDestroy()
    {
    }



}
