using UnityEngine;

public class StartPanelLogic : IStartPanel
{
    public void Init()
    {

    }

    public string RegisterInfo()
    {
        Init();
        return "StartPanelLogic";
    }

    public void Start()
    {
        GameManager.Instance.GameStart();
        UIManager.Instance.PopUIPanel();
    }
}