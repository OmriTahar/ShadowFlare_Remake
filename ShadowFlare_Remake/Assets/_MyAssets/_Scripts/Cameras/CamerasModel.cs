using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Cameras {
    public class CamerasModel : Model {

        public UIScreenCover CurrentScreenCover { get; private set; }

        public CamerasModel() {
            CurrentScreenCover = UIScreenCover.None;
        }

        public void SetCurrentScreenCover(UIScreenCover currentScreenCover) {
            CurrentScreenCover = currentScreenCover;
            Changed();
        }
    }
}


