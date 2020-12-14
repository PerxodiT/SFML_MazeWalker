using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFMLMazeWalker
{
    class SliderInput : Drawable
    {
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public float Progress { get; set; }

        private float BarHeight;
        private Vector2f BarPosition;
        private float MaxBarLength;
        private RectangleShape background, Bar;
        private FloatRect BarColider;

        public VoidEvent onChange { get; set; }


        public bool Active { get; set; }

        private Text BarText;


        public SliderInput(float X, float Y, float Height, float Width, bool active = true)
        {
            Active = active;
            Position = new Vector2f(X, Y);
            Size = new Vector2f(Width, Height);
            MaxBarLength = Width - 8;
            BarHeight = Height - 8;
            BarPosition = new Vector2f(X + 4F, Y + 4F);
            Progress = 1;

            BarText = new Text("", new Font(Resources.MenuFont));

            background = new RectangleShape()
            {
                Position = Position,
                Size = Size,
                FillColor = new Color(128, 128, 128, 128)
            };
            Bar = new RectangleShape()
            {
                Position = BarPosition,
                Size = new Vector2f(MaxBarLength * Progress, BarHeight),
                FillColor = Color.White
            };
            BarColider = new FloatRect(Bar.Position, new Vector2f(MaxBarLength, Bar.Size.Y));
        }

        public void SetText(string text)
        {
            BarText.DisplayedString = text;
        }

        public void SetTextColor(Color color)
        {
            BarText.FillColor = color;
        }

        public FloatRect GetLocalBounds()
        {
            return background.GetLocalBounds();
        }

        public FloatRect GetGlobalBounds()
        {
            return background.GetGlobalBounds();
        }

        private void onClick()
        {
            Progress = (Mouse.GetPosition().X - Bar.GetGlobalBounds().Left) / MaxBarLength;
            onChange?.Invoke();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (!Active) return;
            BarText.Position = new Vector2f(Position.X, Position.Y - 4 - Size.Y);

            background.Position = Position;
            background.Size = Size;
            background.FillColor = new Color(128, 128, 128, 128);
            
            Bar.Position = BarPosition;
            Bar.Size = new Vector2f(MaxBarLength * Progress, BarHeight);
            Bar.FillColor = Color.White;

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && BarColider.Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y)) onClick();

            background.Draw(target, states);
            Bar.Draw(target, states);
            BarText.Draw(target, states);
        }
    }
}
