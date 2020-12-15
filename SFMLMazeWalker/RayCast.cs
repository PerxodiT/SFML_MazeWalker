using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenTK;

namespace MazeWalker
{
    struct RayCast
    {
        Map Map;
        const string kernel = @"__kernel void RayCast(
global read_only  float* pxarg,
global read_only  float* pyarg,
global read_only  float* angle,
global read_only   bool* map,
global read_only    int* WidthArg,
global write_only float* rDist,
global write_only float* rOffs
)
{
    int tid = get_global_id(0);
    float px = *pxarg, py = *pyarg;
    int Width = *WidthArg;
    int x, y;
    float sin_ang = native_sin(angle[tid]);
    float cos_ang = native_cos(angle[tid]);
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
    float distX = fabs((mx - px) / cos_ang);
    float deltaDistX = fabs(1 / cos_ang);
    for (; distX < Width;)
    {
        x = (int)((px + distX * cos_ang) + (dirX * 0.00001F));
        y = (int)((py + distX * sin_ang) + (dirY * 0.00001F));
        if (map[y + x * Width])
            break;
        distX += deltaDistX;
    }


    // Dist fo y's
    float distY = fabs((my - py) / sin_ang);
    float deltaDistY = fabs(1 / sin_ang);
    for (; distY < Width;)
    {
        x = (int)((px + distY * cos_ang) + (dirX * 0.00001F));
        y = (int)((py + distY * sin_ang) + (dirY * 0.00001F));
        if (map[y + x * Width])
            break;
        distY += deltaDistY;
    }
    if (distX < distY)
    {
        float* temp;
        rDist[tid] = distX;
        rOffs[tid] = modf(py + distX * sin_ang, temp);
        return;
    }
    else
    {
        float* temp;
        rDist[tid] = distY;
        rOffs[tid] = modf(px + distY * cos_ang, temp);
        return;
    }
}";
        int[] NumberMap;
        OpenCLTemplate.CLCalc.Program.Kernel RayCastKernel;

        public RayCast(Map map)
        {
            Map = map;

            OpenCLTemplate.CLCalc.InitCL();

            List<Cloo.ComputeDevice> L = OpenCLTemplate.CLCalc.CLDevices;
            OpenCLTemplate.CLCalc.Program.DefaultCQ = 0;

            OpenCLTemplate.CLCalc.Program.Compile(new string[] { kernel });
            RayCastKernel = new OpenCLTemplate.CLCalc.Program.Kernel("RayCast");


            NumberMap = new int[map.Width * map.Width];
            for (int x = 0; x < map.Width; x++)
                for (int y = 0; y < map.Width; y++)
                {
                    NumberMap[y + x * map.Width] = Map.map[x, y] ? 1 : 0;
                }
        }



        public unsafe void GpGPU_Ray(float px, float py, float[] angle, out float[] dists, out float[] offsets)
        {
            OpenCLTemplate.CLCalc.Program.Variable varPx = new OpenCLTemplate.CLCalc.Program.Variable(new float[1] { px });
            OpenCLTemplate.CLCalc.Program.Variable varPy = new OpenCLTemplate.CLCalc.Program.Variable(new float[1] { py });
            
            OpenCLTemplate.CLCalc.Program.Variable varAngle = new OpenCLTemplate.CLCalc.Program.Variable(angle);
            OpenCLTemplate.CLCalc.Program.Variable varMap = new OpenCLTemplate.CLCalc.Program.Variable(NumberMap);
            OpenCLTemplate.CLCalc.Program.Variable varWidth = new OpenCLTemplate.CLCalc.Program.Variable(new float[1] { Map.Width });
            OpenCLTemplate.CLCalc.Program.Variable varDists = new OpenCLTemplate.CLCalc.Program.Variable(new float[angle.Length]);
            OpenCLTemplate.CLCalc.Program.Variable varOffsets = new OpenCLTemplate.CLCalc.Program.Variable(new float[angle.Length]);

            OpenCLTemplate.CLCalc.Program.Variable[] args = new OpenCLTemplate.CLCalc.Program.Variable[] { varPx, varPy, varAngle, varMap, varWidth, varDists, varOffsets};
            var MaxWGSZ = OpenCLTemplate.CLCalc.CLDevices[0].MaxWorkGroupSize;


            int[] workers = new int[1] { angle.Length };

            /*
             varPx.WriteToDevice(new float[1] { px });
            varPy.WriteToDevice(new float[1] { py });
            varAngle.WriteToDevice(angle);
            varMap.WriteToDevice(NumberMap);
            varWidth.WriteToDevice(new float[1] { Map.Width });
            varDists.WriteToDevice(new float[angle.Length]);
            varOffsets.WriteToDevice(new float[angle.Length]);
             */


            RayCastKernel.Execute(args, workers);
            //OpenCLTemplate.CLCalc.Program.Sync();

            varPx.Dispose();
            varPy.Dispose();
            varAngle.Dispose();
            varMap.Dispose();
            varWidth.Dispose();


            dists = new float[angle.Length];
            offsets = new float[angle.Length];

            varDists.ReadFromDeviceTo(dists);
            varOffsets.ReadFromDeviceTo(offsets);

            varDists.Dispose();
            varOffsets.Dispose();

            RayCastKernel.Dispose();
        }


        [DllImport(@"RayCastDLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern void Ray(float px, float py, double angle, bool* map, int Width, [MarshalAs(UnmanagedType.LPArray, SizeConst = 2), Out()] float[] result);
        
        /*
__kernel void RayCast(
global read_only  float* pxarg,
global read_only  float* pyarg,
global read_only  float* angle,
global read_only   bool* map,
global read_only    int* Width,
global write_only float* result
)
{
    float px = *pxarg, py = *pyarg;
    int x, y;
    float sin_ang = native_sin(*angle);
    float cos_ang = native_cos(*angle);
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
    float distX = fabs((mx - px) / cos_ang);
    float deltaDistX = fabs(1 / cos_ang);
    for (; distX < *Width;)
    {
        x = (int)((px + distX * cos_ang) + (dirX * 0.00001F));
        y = (int)((py + distX * sin_ang) + (dirY * 0.00001F));
        if (map[y + x * *Width])
            break;
        distX += deltaDistX;
    }


    // Dist fo y's
    float distY = fabs((my - py) / sin_ang);
    float deltaDistY = fabs(1 / sin_ang);
    for (; distY < *Width;)
    {
        x = (int)((px + distY * cos_ang) + (dirX * 0.00001F));
        y = (int)((py + distY * sin_ang) + (dirY * 0.00001F));
        if (map[y + x * *Width])
            break;
        distY += deltaDistY;
    }
    if (distX < distY)
    {
        float* temp;
        result[0] = distX;
        result[1] = modf(py + distX * sin_ang, temp);
        return;
    }
    else
    {
        float* temp;
        result[0] = distY;
        result[1] = modf(px + distY * cos_ang, temp);
        return;
    }
}
         */


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
