using UnityEngine;

public class BasePanel
{
    private bool isDebug = false;
    private bool _isPause = false;

    public float panelSpeed = 1;
    public UIManager uiManager;
    public CanvasGroup canvasGroup;
    public GameObject panelObj;
    public Transform panelRoot;
    public virtual void init()
    {
        canvasGroup = panelObj.GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.0f;
        panelObj.SetActive(false);
    }

    /// <summary>
    /// 面板被打开
    /// </summary>
    /// <param name="Params"></param>
    public virtual void OnEnter(params object[] Params)
    {
        panelObj.SetActive(true);
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// 面板点击事件暂停
    /// </summary>
    public virtual void OnPause()
    {
        _isPause = true;
    }
    /// <summary>
    /// 面板点击事件回复恢复
    /// </summary>
    public virtual void OnResume()
    {
        _isPause = false;
    }
    /// <summary>
    /// 面板退出
    /// </summary>
    public virtual void OnEixt()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        panelObj.SetActive(false);
    }

    public virtual void Update()
    {
        if (!_isPause)
        {
            Tick();
        }
    }

    /// <summary>
    /// 等价UpDate,用于每一帧的更新
    /// </summary>
    public virtual void Tick()
    {

    }

    public virtual void RegisterInterfae(IUIBase iUIBase)
    {
        if (isDebug)
        {
            Debug.Log($"--{iUIBase.GetType().Name} --");
        }
    }
}
