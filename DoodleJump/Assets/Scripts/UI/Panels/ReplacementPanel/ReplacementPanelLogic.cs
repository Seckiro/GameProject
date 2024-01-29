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

    public void CharacterClick(GameObject gameObject, int index)
    {

    }

    public void CharacterButtonClick(int index, bool active, GameObject gameObject)
    {

    }


    public void BackGroundSpritInit()
    {
        _backGroundAsset = ResManager.Instance.Load<SpriteLibraryAsset>("Assets/Res/BackGroundMain.spriteLib");
        Debug.Log(_backGroundAsset.name + _backGroundAsset == null);
        foreach (var item in _backGroundAsset.GetCategoryLabelNames("BackGround"))
        {
            _listBackGround.Add(item);
        }
    }

    public void BackGroundCallBack(GameObject gameObject, int index)
    {
        var image = gameObject.transform.Find("Image").GetComponent<Image>();

        gameObject.transform.Find("Text1").GetComponent<Text>().text = index.ToString();

        image.sprite = _backGroundAsset.GetSprite("BackGround", _listBackGround[index - 1]);
    }

    public void BackGroundClick(GameObject gameObject, int index)
    {
        Debug.Log($"BackGroundClick:{index}BackGroundClick+{gameObject.name}");
    }


    public void BackGroundButtonClick(int index, bool active, GameObject gameObject)
    {
        Debug.Log($"BackGroundClick:{index}BackGroundClick+{gameObject.name}");
    }

    public string RegisterInfo()
    {
        Init();
        Debug.Log("ReplacementPanelLogic");
        return this.GetType().ToString();
    }
}
