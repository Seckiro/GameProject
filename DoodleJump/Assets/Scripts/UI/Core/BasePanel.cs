using UnityEngine;

public class BasePanel
{
    public float panelSpeed = 1;
    public UIManager uiManager;
    public CanvasGroup canvasGroup;
    public GameObject gameObjectPanel;

    public virtual void init()
    {
        canvasGroup = gameObjectPanel.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.0f;
    }

    /// <summary>
    /// 面板被打开
    /// </summary>
    /// <param name="Params"></param>
    public virtual void OnEnter(params object[] Params)
    {
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 面板点击事件暂停
    /// </summary>
    public virtual void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 面板点击事件回复恢复
    /// </summary>
    public virtual void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 面板退出
    /// </summary>
    public virtual void OnEixt()
    {
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// 等价UpDate,用于每一帧的更新
    /// </summary>
    public virtual void Tick()
    {

    }

    public virtual void RegisterInterfae(IUIBase iUIBase)
    {
        Debug.Log("------------" + iUIBase.RegisterInfo() + "------------");
    }
}
