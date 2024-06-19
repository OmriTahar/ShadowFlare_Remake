using System;
using UnityEngine;

namespace ShadowFlareRemake {
    public abstract class View : MonoBehaviour {

        protected IModel Model { get; private set; }

        private bool _initialized = false;

        public void SetModel(IModel model) {

            if(model == Model) { 
                return;
            }

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

        protected virtual void Initialize() { }

        protected abstract void ModelChanged();
       
        protected virtual void ModelReplaced() { }
      
        protected virtual void Clean() { }
    }

    public abstract class View<T> : View where T : class, IModel {

        new protected T Model { get { return base.Model as T; } }

        public void SetModel(T model) {

            base.SetModel(model);
        }
    }
}

