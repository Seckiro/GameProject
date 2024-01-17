using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    /// <summary>
    /// µÈ¼ÛÓÚSettingPanelLogic
    /// </summary>
    ISettingPanel iSettingPanel;

    public override void init()
    {
        base.init();
        RegisterInterfae(new SettingPanelLogic());
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
        iSettingPanel = (SettingPanelLogic)iUIBase;
        base.RegisterInterfae(iUIBase);
    }
}
