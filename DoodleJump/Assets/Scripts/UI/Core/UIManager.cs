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
        init();
        RegisterPanel(PanelType.LoadPanel);
    }

    private void Start()
    {
        PushPanel(PanelType.LoadPanel);
    }

    public void init()
    {
        monoBehaviour = this;
    }

    public BasePanel GetPanel(PanelType panelType)
    {
        if (panelDict == null)
            panelDict = new Dictionary<PanelType, BasePanel>();

        BasePanel panel;
        if (!panelDict.TryGetValue(panelType, out panel) || panel == null)
        {
            GameObject insPanel = uIConfig.transform.Find(panelType.ToString()).gameObject;

            if (insPanel != null)
            {
                panel = System.Activator.CreateInstance(Type.GetType(panelType.ToString())) as BasePanel;
                if (panel == null)
                {
                    return null;
                }
                else
                {
                    panel.gameObjectPanel = insPanel;
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
        BasePanel panel;

        if (panelStack.Count > 0)
        {
            panel = panelStack.Peek();
            panel.OnPause();
        }
        panel = GetPanel(panelType);

        if (isTop)
        {
            panel.gameObjectPanel.transform.SetAsLastSibling();
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
        panel.gameObjectPanel.transform.SetAsFirstSibling();
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
            item.Tick();
        }
    }
}

