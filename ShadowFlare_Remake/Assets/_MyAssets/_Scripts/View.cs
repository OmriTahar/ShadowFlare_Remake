using System;
using UnityEngine;

namespace ShadowFlareRemake {
    public abstract class View : MonoBehaviour {

        protected IModel Model { get; private set; }

        private bool _initialized = false;

        public void SetModel(IModel model) {

            if(model == Model) { return; }

            if(Model != null) {
                Model.OnChange -= ModelChanged;
            }
            Model = model;

            if(!_initialized) {
                _initialized = true;
                Initialize();
            }
            ModelReplaced();

            if(Model != null) {
                Model.OnChange += ModelChanged;
                ModelChanged();
            }
        }
        private void OnDestroy() {

            if(Model != null) {
                Model.OnChange -= ModelChanged;
            }
            if(_initialized) {
                Clean();
            }
        }

        /// <summary>
        /// Will be called when a model is internally changed, only when a model is available, so inside, Model is never null
        /// </summary>
        protected abstract void ModelChanged();
        /// <summary>
        /// Called when a model is set, but not if the new model is the same as the old model, Model can be null when the method is called
        /// </summary>
        protected virtual void ModelReplaced() { }
        /// <summary>
        /// Called once, upon setting a model for the first time
        /// </summary>
        protected virtual void Initialize() { }
        /// <summary>
        /// Called when the view is destroyed if the view was initalized
        /// </summary>
        protected virtual void Clean() { }
        protected T CastModel<T>() where T : IModel {
            return (T)Model;
        }
        public bool Has(Model model) {
            return Model == model;
        }
    }
    public abstract class View<T> : View where T : class, IModel {
        new protected T Model { get { return base.Model as T; } }

        [Obsolete("Calling SetModel on an incorrect type", true)]
        new public void SetModel(IModel model) {
            base.SetModel(model);
        }
        public void SetModel(T model) {
            base.SetModel(model);
        }
    }
}

