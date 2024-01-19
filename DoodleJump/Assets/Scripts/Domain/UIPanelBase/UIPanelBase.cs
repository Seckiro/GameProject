using UnityEngine;

public class UIPanelBase
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

    public virtual void OnEnter(params object[] Params)
    {
        panelObj.SetActive(true);
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnPause()
    {
        _isPause = true;
    }

    public virtual void OnResume()
    {
        _isPause = false;
    }

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

    public virtual void Tick()
    {

    }

    public virtual void RegisterInterfae(IUIPanelBase iUIBase)
    {
        if (isDebug)
        {
            Debug.Log($"--{iUIBase.GetType().Name} --");
        }
    }
}
