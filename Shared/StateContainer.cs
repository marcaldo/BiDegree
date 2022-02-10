using System;

namespace BiDegree.Shared
{
    public sealed class StateContainer
    {
        private static StateContainer instance;
        public static StateContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StateContainer();
                }

                return instance;
            }
        }

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
}