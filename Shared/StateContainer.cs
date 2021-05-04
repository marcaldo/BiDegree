using System;

public class StateContainer
{
    private float currentTemp;

    public float CurrentTemp
    {
        get => currentTemp;
        set
        {
            currentTemp = value;
            NotifyStateChanged();
        }
    }

    public event Action OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}