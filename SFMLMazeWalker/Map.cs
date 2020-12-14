using SFML.Graphics;
using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace MazeWalker
{
    [Serializable]
    class Coord : ICloneable
    {
        public Coord(uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }
        public uint x { get; set; }
        public uint y { get; set; }

        public object Clone()
        {
            return new Coord(this.x, this.y);
        }
    }

    [Serializable]
    class Map
    {

        public bool[,] map { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Tile { get; private set; }
        public double DiagLen { get; private set; }
        public Coord Out { get; private set; }
        public Coord In { get; private set; }

        public bool isVisible = false;

        [NonSerialized]
        public Maze maze;


        public Map()
        {
            maze = new Maze(Settings.MapSize, Settings.MapSize);
            maze.Generate();
            Dictionary<Coord, Color> walls = new Dictionary<Coord, Color> { };
            Image map_image = MazeDrawer.Draw(maze);
            Width = (int)map_image.Size.X;
            Height = (int)map_image.Size.Y;


            DiagLen = Math.Sqrt(Width * Width + Height * Height);
            Tile = Settings.mHeight / Height;
            map = new bool[Height, Width];

            for (uint y = 0; y < Height; y++)
                for (uint x = 0; x < Width; x++)
                {
                    var pixel = map_image.GetPixel(x, y);
                    if (pixel != new Color(255, 255, 255, 255))
                    {
                        walls.Add(Coord(x, y), pixel);
                        map[x, y] = true;
                    }
                }

            Out = new Coord((uint)maze.End.X * 2 + 1, (uint)maze.End.Y * 2 + 1);
            In = new Coord((uint)maze.Start.X * 2 + 1, (uint)maze.Start.Y * 2 + 1);

            Console.WriteLine("Map init complete!");
        }

        public Map(bool[,] Map, int width, int height, Coord start, Coord end, bool IsVisible)
        {
            maze = new Maze(Settings.MapSize, Settings.MapSize);

            Width = width;
            Height = height;

            isVisible = IsVisible;

            DiagLen = Math.Sqrt(Width * Width + Height * Height);
            Tile = Settings.mHeight / Height;
            map = new bool[Height, Width];

            for (uint y = 0; y < Height; y++)
                for (uint x = 0; x < Width; x++)
                    if (Map[x, y])
                        map[x, y] = true;

            Out = (Coord)end.Clone();
            In = (Coord)start.Clone();

            Console.WriteLine("Map init complete!");
        }

        public bool isWall(int x, int y)
        {
            if (x >= Width || y >= Height || x < 0 || y < 0) return true;
            return map[x, y];
        }

        public void Draw(RenderWindow render)
        {
            RectangleShape mapOutline = new RectangleShape(new Vector2f(Settings.MapSize * 2 * Tile + 5, Settings.MapSize * 2 * Tile + 5))
            {
                FillColor = Color.Transparent,
                OutlineColor = Color.Green,
                OutlineThickness = 5
            };
            render.Draw(mapOutline);
            if (isVisible)
            {
                for (int x = 0; x < Width; x++)
                    for (int y = 0; y < Height; y++)
                    {
                        if (map[x, y])
                        {
                            RectangleShape rect = new RectangleShape(new Vector2f(Tile, Tile))
                            {
                                FillColor = new Color(0, 0, 0, 200),
                                Position = new Vector2f(x * Tile, y * Tile)
                            };
                            render.Draw(rect);
                        }

                    }

                RectangleShape exit = new RectangleShape(new Vector2f(Tile, Tile))
                {
                    FillColor = new Color(0, 255, 0, 200),
                    Position = new Vector2f(Out.x * Tile, Out.y * Tile)
                };

                render.Draw(exit);
            }

        }

        static public Coord Coord(int x, int y)
        {
            return new Coord((uint)x, (uint)y);
        }

        static public Coord Coord(uint x, uint y)
        {
            return new Coord(x, y);
        }
    }
}
