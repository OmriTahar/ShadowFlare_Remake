using UnityEngine;

namespace ShadowFlareRemake.UI.Cursor {
    public class CurserView : View<CurserModel> {

        [Header("Curser Icons")]
        [SerializeField] private Texture2D Move;
        [SerializeField] private Texture2D Attack;
        [SerializeField] private Texture2D PickUp;
        [SerializeField] private Texture2D UI;
        [SerializeField] private Texture2D Other;

        protected override void ModelChanged() {

            UpdateCurser(Model.CurrentCursorIconState);
        }

        private void UpdateCurser(CurserModel.CursorIconState newCurserIconState) {

            var newIcon = GetCurserIcon(newCurserIconState);
            UnityEngine.Cursor.SetCursor(newIcon, Vector2.zero, CursorMode.Auto);
        }

        private Texture2D GetCurserIcon(CurserModel.CursorIconState cursorIconState) {

            switch(cursorIconState) {

                case CurserModel.CursorIconState.Move:
                    return Move;

                case CurserModel.CursorIconState.Attack:
                    return Attack;

                case CurserModel.CursorIconState.PickUp:
                    return PickUp;

                case CurserModel.CursorIconState.UI:
                    return UI;

                case CurserModel.CursorIconState.Other:
                    return Other;
            }
            return Other;
        }
    }
}

