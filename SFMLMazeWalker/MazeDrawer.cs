using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeWalker
{
    static class MazeDrawer
    {
        public static Image Draw(Maze maze)
        {
            //25x25
            int MapWidth = maze.Width * 2 + 1; 
            int MapHeight = maze.Height * 2 + 1;

            Image map = new Image((uint)MapWidth, (uint)MapHeight);
            //Graphics g = Graphics.FromImage(map);
            //map.Clear(Color.FromArgb(255, 255, 255, 255));

            for (int x = 0; x < maze.Width * 2 + 1; x++)
                for (int y = 0; y < maze.Height * 2 + 1; y++)
                    if (x % 2 == 0 && y % 2 == 0) map.SetPixel((uint)x, (uint)y, Color.Black);
                    else map.SetPixel((uint)x, (uint)y, Color.White);

            for (int x = 0; x < maze.Width; x++)
                for (int y = 0; y < maze.Height; y++)
                    DrawCell(maze.Board, map, x, y);
            return map;
        }
        
        static private void DrawCell(Cell[,] cells, Image map, int x, int y)
        {
            var cell = cells[x, y];
            var wall = Color.Black;

            if (cell.NorthWall)
                map.SetPixel(c(x) - 1, c(y), wall);
            if (cell.SouthWall)
                map.SetPixel(c(x) + 1, c(y), wall);
            if (cell.EastWall)
                map.SetPixel(c(x), c(y) + 1, wall);
            if (cell.WestWall)
                map.SetPixel(c(x), c(y) - 1, wall);
        }

        static private uint c(int coord)
        {
            return (uint)(2 * coord + 1);
        }
    }
}
