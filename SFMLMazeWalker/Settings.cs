using SFML.Graphics;
using SFML.Window;
using SFMLMazeWalker;
using System;

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
}
