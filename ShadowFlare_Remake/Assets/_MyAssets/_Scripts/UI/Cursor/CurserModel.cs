
namespace ShadowFlareRemake.UI.Cursor
{
    public class CurserModel : Model
    {
        public enum CursorIconState { Move, Attack, PickUp, UI, Other }
        public CursorIconState CurrentCursorIconState { get; private set; }

        public CurserModel(CursorIconState cursorState = CursorIconState.Move)
        {
            UpdateCurser(cursorState);
        }

        public void UpdateCurser(CursorIconState newCursorState)
        {
            if(newCursorState == CurrentCursorIconState)
                return;

            CurrentCursorIconState = newCursorState;
            Changed();
        }

    }
}

