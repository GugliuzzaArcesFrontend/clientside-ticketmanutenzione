namespace clientside_ticketmanutenzione.Services;

public class ComponentsHandlingService
{
    public Theme SelectedTheme { get; private set; } = Theme.Auto;
    public event Action? OnToggleSidebar;
    public event Action? OnSetTheme;
    public void ToggleSidebar()
    {
        OnToggleSidebar?.Invoke();
    }
    public void SetTheme(Theme theme)
    {
        if (SelectedTheme == theme) return;
        SelectedTheme = theme;
        OnSetTheme?.Invoke();
    }
    public enum Theme{Light,Dark,Auto}
}
