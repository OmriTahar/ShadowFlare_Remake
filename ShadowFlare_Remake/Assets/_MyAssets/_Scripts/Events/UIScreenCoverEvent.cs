using ShadowFlareRemake.Enums;

namespace ShadowFlareRemake.Events {
    public class UIScreenCoverEvent : IGameEvent {

        public UIScreenCover CurrentScreenCover;

        public UIScreenCoverEvent(UIScreenCover currentScreenCover) {
            CurrentScreenCover = currentScreenCover;
        }
    }
}
