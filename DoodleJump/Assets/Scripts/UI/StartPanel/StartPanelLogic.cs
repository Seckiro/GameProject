using UnityEngine;

public class StartPanelLogic : IStartPanel
{
    public void init()
    {

    }

    public string RegisterInfo()
    {
        init();
        return "StartPanelLogic";
    }

    public void Start()
    {
        Debug.Log("button");
    }
}