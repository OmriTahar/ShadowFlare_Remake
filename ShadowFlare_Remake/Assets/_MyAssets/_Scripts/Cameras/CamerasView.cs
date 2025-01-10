using UnityEngine;

namespace ShadowFlareRemake.Cameras
{
    public class CamerasView : View<CamerasModel>
    {
        [Header("General References")]
        [SerializeField] private Transform _mainCameraTransform;
        [SerializeField] private Transform _followTarget;

        [Header("Cameras")]
        [SerializeField] private Transform _centerCamera;
        [SerializeField] private Transform _leftCamera;
        [SerializeField] private Transform _rightCamera;

        private void Update()
        {
            transform.position = _followTarget.position;
        }

        protected override void ModelChanged()
        {
            HandleSetActiveCamera();
        }

        private void HandleSetActiveCamera()
        {
            switch (Model.CurrentActiveCamera)
            {
                case ActiveCamera.CenteredCamera:
                    _mainCameraTransform.SetParent(_centerCamera);
                    break;

                case ActiveCamera.LeftCamera:
                    _mainCameraTransform.SetParent(_leftCamera);
                    break;

                case ActiveCamera.RightCamera:
                    _mainCameraTransform.SetParent(_rightCamera);
                    break;

                default:
                    break;
            }

            //_mainCameraTransform.localPosition = Vector3.zero;
            //_mainCameraTransform.localRotation = Quaternion.identity;
        }
    }
}

