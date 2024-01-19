using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIPanelBase
{
    /// <summary>
    /// µÈ¼ÛÓÚStartPanelLogic
    /// </summary>
    private IStartPanel _iStartPanel;


    private Button _btnStart;





    public override void init()
    {
        base.init();
        RegisterInterfae(new StartPanelLogic());

        _btnStart = panelRoot.Find<Button>("BtnStart").AddListener(_iStartPanel.Start);

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
        _iStartPanel = (StartPanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
