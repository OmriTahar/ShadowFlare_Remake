using ShadowFlareRemake.Cameras;
using ShadowFlareRemake.UI;
using UnityEngine;

namespace ShadowFlareRemake.CamerasManagement
{
    public class CamerasManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CamerasView _camerasView;

        private CamerasModel _model;

        private void Awake()
        {
            _model = new CamerasModel();
            _camerasView.SetModel(_model);
        }

        public void SetActiveCamera(UIScreenCover uIScreenCover)
        {
            var activeCamera = ActiveCamera.CenteredCamera;

            switch(uIScreenCover)
            {
                case UIScreenCover.NoCover:
                    activeCamera = ActiveCamera.CenteredCamera;
                    break;

                case UIScreenCover.RightIsCovered:
                    activeCamera = ActiveCamera.LeftCamera;
                    break;

                case UIScreenCover.LeftIsCovered:
                    activeCamera = ActiveCamera.RightCamera;
                    break;

                case UIScreenCover.FullCover:
                    activeCamera = ActiveCamera.CenteredCamera;
                    break;
            }

            _model.SetCurrentScreenCover(activeCamera);
        }
    }
}
