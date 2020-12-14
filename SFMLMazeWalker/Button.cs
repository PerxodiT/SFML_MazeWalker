using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SFMLMazeWalker
{
    enum ButtonState
    {
        Active,
        Inactive,
        NotClickable
    }
    delegate void VoidEvent();
    class Button : Drawable
    {
        TextAnimation button;
        Sound soundOnHilight;
        FloatRect colider;
        Color fillColor = Color.White;
        ButtonState State;
        bool soundPlayed = false;
        VoidEvent onClick;
        public bool Enabled { get; set; }



        public Button(string text, Sound hilight, Vector2f speed = new Vector2f(), bool Enabled = true, ButtonState state = ButtonState.Active)
        {
            this.Enabled = Enabled;
            button = new TextAnimation(new Text(text, new Font(Resources.MenuFont)), speed);
            button.Destination = new Vector2f(0, 0);
            soundOnHilight = hilight;
            button.Text.FillColor = fillColor;
            ChangeButtonState(state);
            colider = button.Text.GetGlobalBounds();
        }

        public FloatRect GetGlobalBounds()
        {
            return button.Text.GetGlobalBounds();
        }

        public FloatRect GetLocalBounds()
        {
            return button.Text.GetLocalBounds();
        }

        public void SetText(string text)
        {
            button.Text.DisplayedString = text;
        }
        public void SetSpeed(Vector2f speed)
        {
            button.Speed = speed;
        }

        public void SetDestinationPoint(Vector2f dest)
        {
            button.Destination = dest;
        }

        public void SetPosition(Vector2f position)
        {
            button.Text.Position = position;
        }

        public void SetAction(VoidEvent action)
        {
            onClick = action;
        }

        public void SetAnimationEndAction(VoidEvent action)
        {
            button.onEndAnimation = action;
        }

        public void ChangeButtonState(ButtonState state)
        {
            State = state;
            switch (state)
            {
                case ButtonState.Active: fillColor = Color.White; break;
                case ButtonState.NotClickable: fillColor = Color.White; break;
                case ButtonState.Inactive: fillColor = new Color(100, 100, 100, 150); break;
            }
        }

        public void StartAnimation()
        {
            button.Start();
        }

        public void Update()
        {

            button.Text.FillColor = fillColor;
            if (State == ButtonState.Active)
            {

                colider = button.Text.GetGlobalBounds();
                if (colider.Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                {
                    button.Text.FillColor = Color.Red;
                    if (!soundPlayed)
                    {
                        soundOnHilight.Play();
                        soundPlayed = true;
                    }
                }
                else
                {
                    soundPlayed = false;
                }

                if (Mouse.IsButtonPressed(Mouse.Button.Left) && colider.Contains(Mouse.GetPosition().X, Mouse.GetPosition().Y))
                    onClick?.Invoke();
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Enabled)
            {
                Update();
                ((Drawable)button).Draw(target, states);
            }
        }
    }
}
