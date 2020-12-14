using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace SFMLMazeWalker
{
    class ProgressBar : Drawable
    {
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public float Progress { get; set; }

        private float BarHeight;
        private Vector2f BarPosition;
        private float MaxBarLength;
        private RectangleShape background;

        private Text BarText;


        public ProgressBar(float X, float Y, float Height, float Width)
        {
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
        }

        public void SetText(string text)
        {
            BarText.DisplayedString = text;
        }

        public void SetTextColor(Color color)
        {
            BarText.Color = color;
        }

        public FloatRect GetLocalBounds()
        {
            return background.GetLocalBounds();
        }

        public FloatRect GetGlobalBounds()
        {
            return background.GetGlobalBounds();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            BarText.Position = new Vector2f(Position.X, Position.Y - 4 - Size.Y);

            background = new RectangleShape()
            {
                Position = Position,
                Size = Size,
                FillColor = new Color(128,128,128,128)
            };

            var Bar = new RectangleShape()
            {
                Position = BarPosition,
                Size = new Vector2f(MaxBarLength * Progress, BarHeight),
                FillColor = new Color(4, 172, 255, (byte)(Progress < 0.5F ? (Math.Sin(Progress * Math.PI * 12) * 128) + 127 : 255))
            };

            background.Draw(target, states);
            Bar.Draw(target, states);
            BarText.Draw(target, states);
        }
    }
}
