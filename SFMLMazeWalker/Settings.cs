using SFML.Graphics;
using SFML.Window;
using SFMLMazeWalker;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;

namespace MazeWalker
{
    static class Settings
    {
        public static int ResMode = 0;
        public static VideoMode[] Resolutions = VideoMode.FullscreenModes;
        public static int ResCount = Resolutions.Length;

        public static int sWidth = (int)Resolutions[0].Width;//1280;
        public static int sHeight = (int)Resolutions[0].Height;//720;

        public const double toDeg = (180 / Math.PI * 2);
        public const double PlayerSpeed = 1.5;
        public const int TextureScale = 1024;

        public static int mWidth = 400;
        public static int mHeight = 300;


        public static double FOV = Math.PI / 3;
        public static int sHalfHeight = (int)sHeight / 2;
        public static int WallHeight = (int)sHeight / 4;


        public static int RAY_COUNT = sWidth;//1280;
        public static double Half_FOV = FOV / 2;
        public static double deltaFOV = FOV / RAY_COUNT;
        public static int MapSize = 25;

        public static float Scale = sWidth / RAY_COUNT;
        public static float Camera_Dist = (float)(sWidth / (2 * Math.Tan(Half_FOV)));



        public static void Save()
        {
            SettingsToSave save = new SettingsToSave();
            File.Delete("Settings.bin");
            Stream SaveFileStream = File.Create("Settings.bin");
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(SaveFileStream, save);
            SaveFileStream.Close();
        }

        public static void Load()
        {
            Console.WriteLine("Settings loading from file.");
            try
            {
                if (File.Exists("Settings.bin"))
                {
                    Console.WriteLine("Reading saved file");
                    Stream openFileStream = File.OpenRead("Settings.bin");
                    BinaryFormatter deserializer = new BinaryFormatter();
                    SettingsToSave load = (SettingsToSave)deserializer.Deserialize(openFileStream);
                    openFileStream.Close();
                    //openFileStream();
                    load.Load();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when loading settings: \n" + e.Message);
            }
            finally { Save(); }
        }

        public static void Update(bool isResolutionChanhed = false)
        {
            mWidth = 400;
            mHeight = 300;

            sWidth = (int)Resolutions[ResMode].Width;
            sHeight = (int)Resolutions[ResMode].Height;

            if (isResolutionChanhed)
            {
                Program.window.Close();
                Program.window = new RenderWindow(
                    Resolutions[ResMode],
                    "Maze Walker - The game",
                    Styles.Fullscreen,
                    Program.settings
                );
                Program.Init();
                Program.menu.SettingsMenu();
                sHalfHeight = (int)sHeight / 2;
                WallHeight = (int)sHeight / 4;
                RAY_COUNT = sWidth;
                Scale = sWidth / RAY_COUNT;
                Camera_Dist = (float)(sWidth / (2 * Math.Tan(Half_FOV)));
            }


            FOV = Math.PI / 3;

            Half_FOV = FOV / 2;
            deltaFOV = FOV / RAY_COUNT;
            MapSize = 25;

        }

        public static uint[] FpsLimits = new uint[] { 144, 120, 60, 30, 200, 1000 };
        public static int CurrentFPS_ID = 0;
        public static uint FPS_Limit = 144;
        public static bool VSync = false;
    }

    [Serializable]
    class SettingsToSave
    {
        int ResMode;
        int sWidth;
        int sHeight;
        int mWidth;
        int mHeight;
        double FOV;
        int sHalfHeight;
        int WallHeight;
        int RAY_COUNT;
        double Half_FOV;
        double deltaFOV;
        int MapSize;
        float Scale;
        float Camera_Dist;
        uint[] FpsLimits;
        int CurrentFPS_ID;
        uint FPS_Limit;
        bool VSync;

        public SettingsToSave()
        {
            ResMode = Settings.ResMode;
            sWidth = Settings.sWidth;
            sHeight = Settings.sHeight;
            mWidth = Settings.mWidth;
            mHeight = Settings.mHeight;
            FOV = Settings.FOV;
            sHalfHeight = Settings.sHalfHeight;
            WallHeight = Settings.WallHeight;
            RAY_COUNT = Settings.RAY_COUNT;
            Half_FOV = Settings.Half_FOV;
            deltaFOV = Settings.deltaFOV;
            MapSize = Settings.MapSize;
            Scale = Settings.Scale;
            Camera_Dist = Settings.Camera_Dist;
            FpsLimits = Settings.FpsLimits;
            CurrentFPS_ID = Settings.CurrentFPS_ID;
            FPS_Limit = Settings.FPS_Limit;
            VSync = Settings.VSync;
        }

        public void Load()
        {
            Settings.ResMode = ResMode;
            Settings.sWidth = sWidth;
            Settings.sHeight = sHeight;
            Settings.mWidth = mWidth;
            Settings.mHeight = mHeight;
            Settings.FOV = FOV;
            Settings.sHalfHeight = sHalfHeight;
            Settings.WallHeight = WallHeight;
            Settings.RAY_COUNT = RAY_COUNT;
            Settings.Half_FOV = Half_FOV;
            Settings.deltaFOV = deltaFOV;
            Settings.MapSize = MapSize;
            Settings.Scale = Scale;
            Settings.Camera_Dist = Camera_Dist;
            Settings.FpsLimits = FpsLimits;
            Settings.CurrentFPS_ID = CurrentFPS_ID;
            Settings.FPS_Limit = FPS_Limit;
            Settings.VSync = VSync;
        }
    }
}
