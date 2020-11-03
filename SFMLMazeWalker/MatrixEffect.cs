using MazeWalker;
using SFML.Graphics;
using System;

namespace SFMLMazeWalker
{
    class MatrixEffect : Drawable
    {
        private MatrixEffectLine[] lines;
        private int TextHeight = 20;
        int Length = 0;
        Random Random = new Random(DateTime.Now.Millisecond);

        public MatrixEffect()
        {
            Length = Settings.sWidth / TextHeight;
            lines = new MatrixEffectLine[Length];
            for (int i = 0; i < Length; i++)
            {
                lines[i] = new MatrixEffectLine(i * TextHeight, (uint)TextHeight, Random, Random.Next(30, 60));
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            foreach (MatrixEffectLine l in lines) l.Draw(target, states);
        }

    }
}
