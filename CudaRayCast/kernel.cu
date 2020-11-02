
#include "cuda_runtime.h"
#include "device_launch_parameters.h"
#include <math.h>
#include <stdio.h>

using namespace std;
cudaError_t RayCastWithCuda(float* px, float* py, double* angles, float* offsets, double* dists, bool* Walls, int* mapHeight);


extern "C" __global__ void Ray(float* px, float* py, double* angles, float* offsets, double* dists, bool* Walls, int* mapHeight)
{
    int tid = blockDim.x * blockIdx.x + threadIdx.x;
    if (tid < 1920)
    {
        int x, y;
        float sin_ang = sin(angles[tid]);
        float cos_ang = cos((float)angles[tid]);
        sin_ang = sin_ang == 0 ? 0.0001F : sin_ang;
        cos_ang = cos_ang == 0 ? 0.0001F : cos_ang;

        float DiagLen = mapHeight[0] * 2;

        int dirX;
        int dirY;

        // Dist fo x's
        // Selecting direction
        dirX = cos_ang >= 0 ? 1 : -1;
        dirY = sin_ang >= 0 ? 1 : -1;
        int mx = dirX == 1 ? (int)px[0] + 1 : (int)px[0];
        int my = dirY == 1 ? (int)py[0] + 1 : (int)py[0];



        float sideDistX = (mx - px[0]) / cos_ang;
        sideDistX = abs(sideDistX);

        float deltaDistX = 1 / cos_ang;
        deltaDistX = abs(deltaDistX);

        float distX;
        x = (int)(px[0] + sideDistX * cos_ang + (dirX * 0.001F));
        y = (int)(py[0] + sideDistX * sin_ang + (dirY * 0.001F));

        if (Walls[x + y * mapHeight[0]])
        {
            distX = sideDistX;
        }
        else if (deltaDistX > DiagLen)
            distX = (float)DiagLen; //When intersects outside the map dist = MaxDist
        else
        {
            distX = sideDistX;
            for (int x2 = x; x2 > 0 && x2 < mapHeight[0]; x2 += dirX)
            {
                distX += deltaDistX;
                x = (int)(px[0] + distX * cos_ang + (dirX * 0.01F));
                y = (int)(py[0] + distX * sin_ang + (dirY * 0.01F));
                if (Walls[x + y * mapHeight[0]])
                {
                    break;
                }

            }
        }


        // Dist fo y's
        float sideDistY = (my - py[0]) / sin_ang;
        sideDistY = abs(sideDistY);

        float deltaDistY = 1 / sin_ang;
        deltaDistY = abs(deltaDistY);

        float distY;
        x = (int)((px[0] + sideDistY * cos_ang) + (dirX * 0.001F));
        y = (int)((py[0] + sideDistY * sin_ang) + (dirY * 0.001F));

        if (Walls[x + y * mapHeight[0]])
        {
            distY = sideDistY;
        }
        else if (deltaDistY > DiagLen)
            distY = (float)DiagLen; //When intersects outside the map dist = MaxDist
        else
        {
            distY = sideDistY;
            for (int y2 = y; y2 > 0 && y2 < mapHeight[0]; y2 += dirY)
            {
                distY += deltaDistY;
                x = (int)((px[0] + distY * cos_ang) + (dirX * 0.01F));
                y = (int)((py[0] + distY * sin_ang) + (dirY * 0.01F));
                if (Walls[x + y * mapHeight[0]])
                {
                    break;
                }
            }
        }
        if (distX < distY)
        {
            offsets[tid] = ((py[0] + distX * sin_ang) - (int)(py[0] + distX * sin_ang));
            dists[tid] = distX;
        }
        else
        {
            offsets[tid] = ((px[0] + distY * cos_ang) - (int)(px[0] + distY * cos_ang));
            dists[tid] = distY;
        }
    }
}

main(void) {

}

cudaError_t RayCastWithCuda(float* px, float* py, double* angles, float* offsets, double* dists, bool* Walls, int* mapHeight)
{
    return cudaError_t();
}
