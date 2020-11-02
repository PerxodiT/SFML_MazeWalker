using MazeWalker;
using SFML.Graphics;
using SFML.System;
using System;
using System.Numerics;
using System.Threading;
using System.Threading.Channels;

namespace SFMLMazeWalker
{
    class MatrixEffectLine : Drawable
    {
        Vector2f Speed;
        int Length = 0;
        Random random = new Random(DateTime.Now.Millisecond);
        Text[] Symbols;
        uint SymbolHeight = 10;
        Font Font = new Font(Resources.katakana);
        Color FirstColor = new Color(100, 255, 100, 170);
        Color NextColor = new Color(0, 170, 0, 170);
        int x = 0;
        bool hasEnded = true;
        int timer = 0;
        char randomchar = (char)0;
        int changeTime;

        public MatrixEffectLine(int x, uint FontHeight, Random Random, int ChangeTime)
        {
            changeTime = ChangeTime;
            random = Random;
            SymbolHeight = FontHeight;
            this.x = x;
            Init();
        }

        public void Init()
        {
            Length = (int)random.Next((int)(Settings.sHeight*0.3F) / (int)SymbolHeight, (int)(Settings.sHeight * 0.8F) / (int)SymbolHeight);
            Symbols = new Text[Length];
            Speed = new Vector2f(0, random.Next(5, 11));


            char randomchar = (char)random.Next(33, 109);
            Symbols[Length - 1] = new Text($"{randomchar}", Font)
            {
                FillColor = FirstColor,
                CharacterSize = SymbolHeight
            };
            if (hasEnded) Symbols[Length - 1].Position = new Vector2f(x, -(int)SymbolHeight - 4);
            for (int i = Length - 2, j = 2; i >= 0; i--, j++)
            {
                randomchar = (char)random.Next(33, 109);
                Symbols[i] = new Text($"{randomchar}", Font)
                {
                    FillColor = NextColor,
                    CharacterSize = SymbolHeight
                };
                if (hasEnded) Symbols[i].Position = new Vector2f(x, (-(int)SymbolHeight - 4) * j);
            }
        }
        private void Update()
        {

            if (timer == 0)
            {
                randomchar = (char)random.Next(33, 109);
                Symbols[Length - 1].DisplayedString = $"{randomchar}";
            }
            if (hasEnded) Symbols[Length - 1].Position = new Vector2f(x, -(int)SymbolHeight - 4);
            Symbols[Length - 1].Position = Symbols[Length - 1].Position + Speed;

            for (int i = Length - 2, j = 2; i >= 0; i--, j++)
            {
                if (timer == 0)
                {
                    randomchar = (char)random.Next(33, 109);
                    Symbols[i].DisplayedString = $"{randomchar}";
                }
                if (hasEnded) Symbols[i].Position = new Vector2f(x, (-(int)SymbolHeight - 4) * j);
                Symbols[i].Position = Symbols[i].Position + Speed;
            }
            hasEnded = true;
            if (Symbols[0].Position.Y < Settings.sHeight) hasEnded = false;
            timer = (timer + 1) % changeTime;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            Update();
            foreach (Text t in Symbols)
            {
                t.Draw(target, states);
            }
        }
    }
}
