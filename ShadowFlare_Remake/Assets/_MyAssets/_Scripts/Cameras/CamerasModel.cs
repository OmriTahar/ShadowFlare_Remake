
namespace ShadowFlareRemake.Cameras
{
    public class CamerasModel : Model
    {
        public ActiveCamera CurrentActiveCamera { get; private set; }

        public CamerasModel()
        {
            CurrentActiveCamera = ActiveCamera.CenteredCamera;
        }

        public void SetCurrentScreenCover(ActiveCamera currentActiveCamera)
        {
            CurrentActiveCamera = currentActiveCamera;
            Changed();
        }
    }
}


