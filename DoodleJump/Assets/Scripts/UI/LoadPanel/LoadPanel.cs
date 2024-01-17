using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : BasePanel
{
    /// <summary>
    /// µÈ¼ÛÓÚLoadPanelLogic
    /// </summary>
    ILoadPanel iLoadPanel = null;

    public override void init()
    {
        base.init();
        RegisterInterfae(new LoadPanelLogic());
    }

    public override void OnEnter(params object[] Params)
    {
        base.OnEnter(Params);
        panelObj.SetActive(true);





        uiManager.PushUIPanel(UIPanelType.StartPanel);
        OnEixt();
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
    public override void RegisterInterfae(IUIBase iUIBase)
    {
        iLoadPanel = (LoadPanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
