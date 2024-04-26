using ShadowFlareRemake.Tools;
using UnityEngine;

namespace ShadowFlareRemake.Cameras {
    public class CamerasView : View<CamerasModel> {

        [Header("Cameras")]
        [SerializeField] private MultiStateView _camerasMSV;

        private enum Camera { CenteredCamera, LeftCamera, RightCamera }

        protected override void ModelChanged() {
            HandleCameras();
        }

        private void HandleCameras() {

            switch(Model.CurrentScreenCover) {

                case Enums.UIScreenCover.None:
                    _camerasMSV.ChangeState((int)Camera.CenteredCamera);
                    break;

                case Enums.UIScreenCover.RightIsCovered:
                    _camerasMSV.ChangeState((int)Camera.LeftCamera);
                    break;

                case Enums.UIScreenCover.LeftIsCovered:
                    _camerasMSV.ChangeState((int)Camera.RightCamera);
                    break;

                case Enums.UIScreenCover.BothAreCovered:
                    _camerasMSV.ChangeState((int)Camera.CenteredCamera);
                    break;
            }
        }
    }
}

