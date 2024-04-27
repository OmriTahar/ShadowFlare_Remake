using System;

namespace ShadowFlareRemake {
    public abstract class Model : IModel {

        public event Action OnChange;

        protected void Changed() {

            OnChange?.Invoke();
        }
    }
}

