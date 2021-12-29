using BiDegree.Models;
using System;

namespace BiDegree.Shared
{
    public class StateContainer
    {
        private float currentTemp;
        private DisplayItemType displayItemType;

        public float CurrentTemp
        {
            get => currentTemp;
            set
            {
                currentTemp = value;
                NotifyStateChanged();
            }
        }

        public DisplayItemType DisplayItemType
        {
            get => displayItemType;
            set
            {
                displayItemType = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}