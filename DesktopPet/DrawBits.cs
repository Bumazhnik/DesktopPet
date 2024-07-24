using DesktopPet.Characters;

namespace DesktopPet
{
    internal class DrawBits : IDrawState
    {
        private Action<Bitmap> drawAction;
        private CharacterState? previousState = null;
        private Dictionary<CharacterState, Bitmap> stateBitmaps = new();

        public DrawBits(Action<Bitmap> drawAction)
        {
            this.drawAction = drawAction;
        }
        public void AddState(CharacterState state, Bitmap bmp)
        {
            stateBitmaps.Add(state, bmp);
        }

        public void DrawState(CharacterState state)
        {
            if (previousState != state)
            {
                drawAction(stateBitmaps[state]);
            }
            previousState = state;
        }
    }
}
