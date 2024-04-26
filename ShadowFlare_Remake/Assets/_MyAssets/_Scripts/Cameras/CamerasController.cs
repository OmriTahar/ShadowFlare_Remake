using UnityEngine;
using ShadowFlareRemake.Events;

namespace ShadowFlareRemake.Cameras {
    public class CamerasController : Controller {

        [Header("References")]
        [SerializeField] private CamerasView _view;

        private CamerasModel _model;

        protected override void Awake() {

            base.Awake();
            InitModel();
            Accept<UIScreenCoverEvent>(SetCurrentScreenCover);
        }
   
        private void InitModel() {

            _model = new CamerasModel();
            _view.SetModel(_model);
        }

        private void SetCurrentScreenCover(UIScreenCoverEvent e) {

            _model.SetCurrentScreenCover(e.CurrentScreenCover);
        }
    }
}
