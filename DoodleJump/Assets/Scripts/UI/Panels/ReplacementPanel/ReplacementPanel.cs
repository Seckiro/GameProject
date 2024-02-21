using System.Collections;
using System.Collections.Generic;
using CircularScrollView;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class ReplacementPanel : UIPanelBase
{
    private IReplacementPanel iReplacementPanel;

    private UICircularScrollView _characterScrollView;
    private UICircularScrollView _backGroundScrollView;

    public override void Init()
    {
        base.Init();
        RegisterInterfae(new ReplacementPanelLogic());

        //_characterScrollView = panelRoot.Find("Character/ScrollView").GetComponent<UICircularScrollView>();
        //_characterScrollView.Init(
        // iReplacementPanel.CharacterCallBack,
        // iReplacementPanel.CharacterClick,
        // iReplacementPanel.CharacterButtonClick);
        //_characterScrollView.ShowList(10);
        //_characterScrollView.gameObject.SetActive(false);


        _backGroundScrollView = panelRoot.Find("BackGround/ScrollView").GetComponent<UICircularScrollView>();
        _backGroundScrollView.Init(
            iReplacementPanel.BackGroundCallBack,
            iReplacementPanel.BackGroundClick,
            iReplacementPanel.BackGroundButtonClick);
        _backGroundScrollView.ShowList(iReplacementPanel.ListBackGroundCount);
        Debug.Log("ListBackGroundCount:" + iReplacementPanel.ListBackGroundCount);
        //_backGroundScrollView.gameObject.SetActive(false);
    }

    public override void OnEnter(params object[] Params)
    {
        base.OnEnter(Params);
        panelObj.SetActive(true);
    }

    public override void OnEixt()
    {
        base.OnEixt();
    }

    public override void Tick()
    {
        if (panelObj.activeSelf)
        {

        }
    }
    public override void RegisterInterfae(IUIPanelBase iUIBase)
    {
        iReplacementPanel = (ReplacementPanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
