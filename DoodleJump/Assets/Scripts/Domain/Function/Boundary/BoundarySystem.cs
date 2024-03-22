using UnityEngine;
using UnityEngine.U2D.Animation;


public class BoundarySystem : SystemBase
{
    public static string SpriteLibraryAssetCategoryName = "BackGround";
    public static string SpriteLibraryAssetPath = "Assets/Res/SpriteLibs/BackGroundMain.spriteLib";

    private Boundary _boundary;
    private SpriteLibraryAsset _spriteLibraryAsset;

    public override bool SystemActive { get; set; }

    public SpriteLibraryAsset SpriteLibraryAsset { get => _spriteLibraryAsset; }

    public override void SystemInit()
    {
        _spriteLibraryAsset = ResManager.Instance.Load<SpriteLibraryAsset>(SpriteLibraryAssetPath);

        _boundary = Object.FindObjectOfType<Boundary>();
        _boundary.SetSpriteLibraryAsset(_spriteLibraryAsset);
        _boundary.Init();
    }

    public override void SystemReady()
    {
        _boundary.SetFollowCamera(Camera.current);
    }

    public override void SystemStart()
    {
        _boundary.SetBoundaryStart();
    }

    public override void SystemEnd()
    {
        _boundary.SetBoundaryEnd();
    }

    public void SetBoundarySprite(string name)
    {
        _boundary.SetSprite(name);
    }
}
