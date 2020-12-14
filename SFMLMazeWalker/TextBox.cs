using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFMLMazeWalker
{
    class TextBox : Drawable 
    {
        public Vector2f Position { get; set; }
        public Vector2f Size { get; set; }
        public Text Text { get; set; }
        public Color BackgroundColor { get; set; }
        public Color OutlineColor { get; set; }
        public string TextString { get; set; }


        public bool HasFocus = false;
        private FloatRect Colider { get; set; }

        public TextBox(Vector2f position, Vector2f size, string str, Font font)
        {
            Position = position;
            Size = size;
            TextString = str;


            Colider = new FloatRect(Position, Size);
            Text = new Text("", font);
            Text.DisplayedString = str;
            Text.Position = new Vector2f(Position.X + 5, Colider.Top + (Colider.Height/2) - Text.GetLocalBounds().Height/2 - 8);
            Text.LineSpacing = 0;
            Text.FillColor = Color.Red;
        }

        public void TextEntered(TextEventArgs e)
        {
            if (HasFocus)
            {
                switch (e.Unicode[0])
                {
                    case '\b':
                        if (TextString.Length > 0)
                        TextString = TextString.Remove(TextString.Length - 1, 1);
                        return;
                    default:
                        if (CheckLength(e.Unicode))
                        TextString += e.Unicode;
                        break;
                }
            }
        }

        private bool CheckLength(string e)
        {
            Text.DisplayedString += e;
            if (Text.GetGlobalBounds().Width < Colider.Width - 10)
            {
                Text.DisplayedString = TextString;
                return true;
            }
            Text.DisplayedString = TextString;
            return false;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (!HasFocus && Colider.Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y) && Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                HasFocus = true;
            }
            else if (HasFocus && !Colider.Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y) && Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                HasFocus = false;
            }

            Text.DisplayedString = TextString;

            var back = new RectangleShape()
            {
                Position = Position,
                Size = Size,
                FillColor = BackgroundColor,
                OutlineColor = OutlineColor,
                OutlineThickness = HasFocus ? 2 : 0
            };
            

            back.Draw(target, states);
            Text.Draw(target, states);
            //test.Draw(target, states);
        }
    }
}
