using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementPanel : UIPanelBase
{
    IReplacementPanel iReplacementPanel;
    public override void init()
    {
        base.init();
        RegisterInterfae(new ReplacementPanelLogic());
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
