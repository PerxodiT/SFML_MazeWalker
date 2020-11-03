using SFML.Graphics;
using SFML.System;

namespace SFMLMazeWalker
{
    class TextAnimation : Drawable
    {
        public Text Text { get; private set; }
        public Vector2f Speed { get; set; }
        private bool isStarted = false;
        public Vector2f Destination { get; set; }
        public VoidEvent onEndAnimation;

        public TextAnimation(Text text, Vector2f speed)
        {
            Text = text;
            Speed = speed;
        }

        public void Start()
        {
            isStarted = true;
        }

        public void NextStep()
        {
            if (isStarted)
            {
                Text.Position = Text.Position + Speed;
                if (isEnded())
                {
                    isStarted = false;
                    onEndAnimation?.Invoke();
                }
            }
        }

        private bool isEnded()
        {
            bool b1 = true;
            bool b2 = true;
            if (Speed.X > 0) b1 = Text.Position.X > Destination.X;
            else if (Speed.X < 0) b1 = Text.Position.X < Destination.X;

            if (Speed.Y > 0) b2 = Text.Position.Y > Destination.Y;
            else if (Speed.Y < 0) b2 = Text.Position.Y < Destination.Y;

            return b1 && b2;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            NextStep();
            ((Drawable)Text).Draw(target, states);
        }
    }
}
