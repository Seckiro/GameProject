using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ReplacementPanelLogic : IReplacementPanel
{
    SpriteLibraryAsset _backGroundAsset;

    List<string> _listBackGround = new List<string>();
    public int ListBackGroundCount => _listBackGround.Count;
    public void Init()
    {
        BackGroundSpritInit();
    }

    public void CharacterCallBack(GameObject gameObject, int index)
    {

    }

    public void BackGroundSpritInit()
    {
        _backGroundAsset = GameManager.Instance.GetSystem<BoundarySystem>().SpriteLibraryAsset;
        foreach (var item in _backGroundAsset.GetCategoryLabelNames(BoundarySystem.SpriteLibraryAssetName))
        {
            _listBackGround.Add(item);
        }
    }

    public void BackGroundCallBack(GameObject gameObject, int index)
    {
        var sparite = _backGroundAsset.GetSprite(BoundarySystem.SpriteLibraryAssetName, _listBackGround[index - 1]);

        Text text = gameObject.transform.Find("Text1").GetComponent<Text>();
        text.text = sparite.name;

        Image image = gameObject.transform.Find("Image").GetComponent<Image>();
        image.sprite = sparite;

        Button button = gameObject.GetComponent<Button>();
        button.AddListener(() =>
        {
            GameManager.Instance.GetSystem<BoundarySystem>().SetBoundarySprite(text.text);
        });

    }

    public string RegisterInfo()
    {
        Init();
        return this.GetType().ToString();
    }
}
