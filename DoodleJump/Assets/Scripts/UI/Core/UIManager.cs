using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMono<UIManager>
{
    [SerializeField]
    public UIConfig uIConfig;
    private MonoBehaviour monoBehaviour;
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();
    private Dictionary<PanelType, BasePanel> panelDict = new Dictionary<PanelType, BasePanel>();
    private IDisposable _timerTask;

    private void Awake()
    {
        Init();
        RegisterPanel(PanelType.LoadPanel);
        RegisterPanel(PanelType.SettingPanel);
        RegisterPanel(PanelType.StartPanel);
        RegisterPanel(PanelType.PausePanel);
        RegisterPanel(PanelType.EndPanel);
    }

    private void Start()
    {
        PushPanel(PanelType.LoadPanel);
    }

    public void Init()
    {
        monoBehaviour = this;
    }

    public BasePanel GetPanel(PanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<PanelType, BasePanel>();
        }
        BasePanel panel = null;
        if (!panelDict.TryGetValue(panelType, out panel) || panel == null)
        {
            GameObject insPanel = uIConfig.uiCanvasTransform.Find(panelType.ToString()).gameObject;

            if (insPanel != null)
            {
                panel = Activator.CreateInstance(Type.GetType(panelType.ToString())) as BasePanel;
                if (panel == null)
                {
                    return null;
                }
                else
                {
                    panel.panelObj = insPanel;
                    panel.panelRoot = insPanel.transform;
                    panel.uiManager = this;
                    panel.init();

                    panelDict.Add(panelType, panel);
                    return panel;
                }
            }
            else
            {
                Debug.LogError("在场景中未找到gamePanel!");
                return null;
            }
        }
        else
        {
            return panel;
        }
    }

    public void PushPanel(PanelType panelType, bool isTop = true, params object[] Params)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }

        BasePanel panel = null;

        if (panelStack.Count > 0)
        {
            panel = panelStack.Peek();
            panel.OnPause();
        }

        panel = GetPanel(panelType);

        if (isTop)
        {
            panel.panelObj.transform.SetAsLastSibling();
        }
        panel.OnEnter(Params);
        panelStack.Push(panel);
    }

    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        BasePanel panel;
        panel = panelStack.Pop();
        panel.panelObj.transform.SetAsFirstSibling();
        panel.OnEixt();

        if (panelStack.Count <= 0) return;

        panel = panelStack.Peek();
        panel.OnResume();
    }

    public void PopAllPanel()
    {
        for (int i = 0; i < panelStack.Count; i++)
        {
            PopPanel();
        }
    }

    public void RegisterPanel(PanelType panelType)
    {
        GetPanel(panelType);
    }

    private void Update()
    {
        foreach (var item in panelStack)
        {
            item.Update();
        }
    }
}

