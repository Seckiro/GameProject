public enum UIPanelType
{
    None = 0,
    LoadPanel,
    StartPanel,
    EndPanel,
    SettingPanel,
    PausePanel,
    ReplacementPanel,
}

public static class UIPanelTypeRegister
{
    public static void RegisterPanel()
    {
        UIManager.Instance.RegisterPanel(UIPanelType.LoadPanel);
        UIManager.Instance.RegisterPanel(UIPanelType.SettingPanel);
        UIManager.Instance.RegisterPanel(UIPanelType.StartPanel);
        UIManager.Instance.RegisterPanel(UIPanelType.PausePanel);
        UIManager.Instance.RegisterPanel(UIPanelType.EndPanel);
        UIManager.Instance.RegisterPanel(UIPanelType.ReplacementPanel);
    }
}