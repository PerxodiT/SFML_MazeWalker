using System.Runtime.InteropServices;

//using System.ComponentModel.Composition;
//using System.Xaml;

namespace MazeWalker
{
    struct RayCast
    {
        Map Map;
        //const int THREADS = Settings.RAY_COUNT;

        public RayCast(Map map)
        {
            Map = map;
        }

        [DllImport(@"RayCastDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void Ray(float px, float py, double angle, bool* map, int Width, [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out()] float[] result);

        /*
        public double Ray(float px, float py, double angle, out double offset)
        {
            int x, y;
            float sin_ang = (float)GMath.Sin((float)angle);
            float cos_ang = (float)GMath.Cos((float)angle);
            sin_ang = sin_ang == 0 ? 0.0001F : sin_ang;
            cos_ang = cos_ang == 0 ? 0.0001F : cos_ang;

            int dirX;
            int dirY;

            // Selecting direction
            dirX = cos_ang >= 0 ? 1 : -1;
            dirY = sin_ang >= 0 ? 1 : -1;
            int mx = dirX == 1 ? (int)px + 1 : (int)px;
            int my = dirY == 1 ? (int)py + 1 : (int)py;



            // Dist fo x's
            float distX = GMath.Abs((mx - px) / cos_ang);
            float deltaDistX = GMath.Abs(1 / cos_ang);
            for (; distX < Map.Width;)
            {
                x = (int)((px + distX * cos_ang) + (dirX * 0.00001F));
                y = (int)((py + distX * sin_ang) + (dirY * 0.00001F));
                if (Map.isWall(x, y))
                    break;
                distX += deltaDistX;
            }


            // Dist fo y's
            float distY = GMath.Abs((my - py) / sin_ang);
            float deltaDistY = GMath.Abs(1 / sin_ang);
            for (; distY < Map.Width;)
            {
                x = (int)((px + distY * cos_ang) + (dirX * 0.00001F));
                y = (int)((py + distY * sin_ang) + (dirY * 0.00001F));
                if (Map.isWall(x, y))
                    break;
                distY += deltaDistY;
            }

            if (distX < distY)
            {
                offset = ((py + distX * sin_ang) % 1);
                return distX;
            }
            else
            {
                offset = ((px + distY * cos_ang) % 1);
                return distY;
            }
        }
        */
    }
}
