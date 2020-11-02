using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeWalker
{
    static class Settings
    {
        public const int sWidth = 1920;//1280;
        public const int sHeight = 1080;//720;

        public const double toDeg = (180 / Math.PI * 2);

        public const int mWidth = 400;
        public const int mHeight = 300;
        public const int sHalfHeight = sHeight / 2;
        public const int WallHeight = sHeight / 4;

        public const int TextureScale = 1024;

        public const int RAY_COUNT = 1920;//1280;
        public const double FOV = Math.PI / 3;
        public const double Half_FOV = FOV / 2;
        public const double deltaFOV = FOV / RAY_COUNT;
        public static int MapSize = 25;

        public const double PlayerSpeed = 1.5;
        public const float Scale = sWidth / RAY_COUNT;
        public static float Camera_Dist = (float)(sWidth / (2 * Math.Tan(Half_FOV)));

        public static uint FPS_Limit = 60;
        public static bool VSync = false;
    }
}
