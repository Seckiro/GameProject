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
    /// ��屻��
    /// </summary>
    /// <param name="Params"></param>
    public virtual void OnEnter(params object[] Params)
    {
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// ������¼���ͣ
    /// </summary>
    public virtual void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// ������¼��ظ��ָ�
    /// </summary>
    public virtual void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    /// <summary>
    /// ����˳�
    /// </summary>
    public virtual void OnEixt()
    {
        canvasGroup.blocksRaycasts = false;
    }
    /// <summary>
    /// �ȼ�UpDate,����ÿһ֡�ĸ���
    /// </summary>
    public virtual void Tick()
    {

    }

    public virtual void RegisterInterfae(IUIBase iUIBase)
    {
        Debug.Log("------------" + iUIBase.RegisterInfo() + "------------");
    }
}
