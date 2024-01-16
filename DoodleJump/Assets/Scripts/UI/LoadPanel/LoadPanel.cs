using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : BasePanel
{
    /// <summary>
    /// µÈ¼ÛÓÚLoadPanelLogic
    /// </summary>
    ILoadPanel iLoadPanel;

    public override void init()
    {
        base.init();
        RegisterInterfae(new LoadPanelLogic());
    }

    public override void OnEnter(params object[] Params)
    {
        base.OnEnter(Params);
        gameObjectPanel.SetActive(true);
    }

    public override void OnEixt()
    {
        base.OnEixt();
    }

    public override void Tick()
    {
        if (gameObjectPanel.activeSelf)
        {

        }
    }
    public override void RegisterInterfae(IUIBase iUIBase)
    {
        iLoadPanel = (LoadPanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
