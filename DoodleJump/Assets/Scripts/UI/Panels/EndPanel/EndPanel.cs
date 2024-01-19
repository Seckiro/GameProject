using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : UIPanelBase
{
    /// <summary>
    /// µÈ¼ÛÓÚEndPanelLogic
    /// </summary>
    IEndPanel iEndPanel;

    public override void init()
    {
        base.init();
        RegisterInterfae(new EndPanelLogic());
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
        iEndPanel = (EndPanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
