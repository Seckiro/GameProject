using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : UIPanelBase
{
    /// <summary>
    /// �ȼ���PausePanelLogic
    /// </summary>
    IPausePanel iPausePanel;

    public override void Init()
    {
        base.Init();
        RegisterInterfae(new PausePanelLogic());
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
        iPausePanel = (PausePanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
