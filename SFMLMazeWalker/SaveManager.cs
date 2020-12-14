using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MazeWalker;

namespace SFMLMazeWalker
{
    static class SaveManager
    {
        public static void Save()
        {
            Coord Start = (Coord)Program.map.In.Clone();
            Coord End = (Coord)Program.map.Out.Clone();

            MapSave mapSave = new MapSave()
            {
                Map = (bool[,])Program.map.map.Clone(),
                StartX = Start.x,
                StartY = Start.y,
                EndX = End.x,
                EndY = End.y,
                MapHeight = Program.map.Height,
                MapWidth = Program.map.Width,
                isVisible = Program.map.isVisible
            };

            BinaryFormatter formatter = new BinaryFormatter();
            Directory.CreateDirectory("Saves");
            using (FileStream fs = new FileStream($"Saves\\Profile{Settings.Profile}.map", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, mapSave);
                Console.WriteLine("Карта сериализована");
            }

            PlayerSave playerSave = new PlayerSave()
            {
                X = Program.player.x,
                Y = Program.player.y,
                A = Program.player.a,
                MapAX = Program.player.mapActivator.X,
                MapAY = Program.player.mapActivator.Y
            };

            using (FileStream fs = new FileStream($"Saves\\Profile{Settings.Profile}.plr", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, playerSave);
                Console.WriteLine("Игрок сериализован");
            }
        }

        public static void Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream($"Saves\\Profile{Settings.Profile}.map", FileMode.OpenOrCreate))
            {
                MapSave ms = (MapSave)formatter.Deserialize(fs);
                ms.Load();
                Console.WriteLine("Карта десериализована");
            }

            PlayerSave playerSave = new PlayerSave()
            {
                X = Program.player.x,
                Y = Program.player.y,
                A = Program.player.a,
                MapAX = Program.player.mapActivator.X,
                MapAY = Program.player.mapActivator.Y
            };

            using (FileStream fs = new FileStream($"Saves\\Profile{Settings.Profile}.plr", FileMode.OpenOrCreate))
            {
                PlayerSave ps = (PlayerSave)formatter.Deserialize(fs);
                ps.Load();
                Console.WriteLine("Игрок десериализован");
            }
        }

    }
    [Serializable]
    class MapSave
    {
        public bool[,] Map;
        public uint StartX;
        public uint StartY;
        public uint EndX;
        public uint EndY;
        public int MapWidth;
        public int MapHeight;
        public bool isVisible;

        public void Load()
        {
            Program.map = new Map(Map, MapWidth, MapHeight, new Coord(StartX, StartY), new Coord(EndX, EndY), isVisible);
        }
    }

    [Serializable]
    class PlayerSave
    {
        public float X;
        public float Y;
        public double A;
        public int MapAX;
        public int MapAY;
        public void Load()
        {
            Player player = new Player(Program.map);
            player.x = X;
            player.y = Y;
            player.a = A;
            player.mapActivator = new System.Drawing.Point(MapAX, MapAY);

            Program.player = player;
        }
    }
}
