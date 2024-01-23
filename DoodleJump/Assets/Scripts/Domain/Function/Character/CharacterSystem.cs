using UnityEngine;

public class CharacterSystem : SystemBase
{
    private Character _currentCharacter;

    public Character CurrentCharacter => _currentCharacter;

    public override bool SystemActive { get; set; }

    private void CreateCharacter()
    {
        GameObject characterObj = new GameObject();
        _currentCharacter = characterObj.AddComponent<Character>();
    }

    public override void SystemInit()
    {
        CreateCharacter();
    }

    public override void SystemStart()
    {
        _currentCharacter.CharacterStart();
    }

    public override void SystemUpdate()
    {
        _currentCharacter.CharacterUpdate();
    }

    public override void SystemEnd()
    {
        _currentCharacter.CharacterEnd();
    }

    public override void SystemDestroy()
    {
        _currentCharacter.CharacterDestroy();
    }
}
