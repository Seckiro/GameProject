using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacementPanelLogic : IReplacementPanel
{
    public void init()
    {

    }

    public string RegisterInfo()
    {
        init();
        return this.GetType().ToString();
    }
}
