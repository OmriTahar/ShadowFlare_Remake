using ShadowFlareRemake.Tools;
using UnityEngine;

namespace ShadowFlareRemake.Cameras
{
    public class CamerasView : View<CamerasModel>
    {
        [Header("Cameras")]
        [SerializeField] private MultiStateView _camerasMSV;

        protected override void ModelChanged()
        {
            SetActiveCamera();
        }

        private void SetActiveCamera()
        {
            _camerasMSV.ChangeState((int)Model.CurrentActiveCamera);
        }
    }
}

