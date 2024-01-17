using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    /// <summary>
    /// µÈ¼ÛÓÚPausePanelLogic
    /// </summary>
    IPausePanel iPausePanel;

    public override void init()
    {
        base.init();
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
    public override void RegisterInterfae(IUIBase iUIBase)
    {
        iPausePanel = (PausePanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
