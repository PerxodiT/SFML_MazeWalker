
// MazeWalker.RayCast
extern "C" __global__  void Ray( float* px, int pxLen0,  float* py, int pyLen0,  double* angles, int anglesLen0,  float* offsets, int offsetsLen0,  double* dists, int distsLen0,  bool* Walls, int WallsLen0,  int* mapHeight, int mapHeightLen0);

// MazeWalker.RayCast
extern "C" __global__  void Ray( float* px, int pxLen0,  float* py, int pyLen0,  double* angles, int anglesLen0,  float* offsets, int offsetsLen0,  double* dists, int distsLen0,  bool* Walls, int WallsLen0,  int* mapHeight, int mapHeightLen0)
{
	int x = threadIdx.x;
	bool flag = x < 1920;
	if (flag)
	{
		float num = sinf((float)angles[(x)]);
		float num2 = cosf((float)angles[(x)]);
		num = ((num == 0.0f) ? 0.0001f : num);
		num2 = ((num2 == 0.0f) ? 0.0001f : num2);
		float num3 = (float)mapHeight[(0)] * sqrtf(2.0f);
		int num4 = (num2 >= 0.0f) ? 1 : -1;
		int num5 = (num >= 0.0f) ? 1 : -1;
		int num6 = (num4 == 1) ? ((int)px[(0)] + 1) : ((int)px[(0)]);
		int num7 = (num5 == 1) ? ((int)py[(0)] + 1) : ((int)py[(0)]);
		float num8 = ((float)num6 - px[(0)]) / num2;
		num8 = fabsf(num8);
		float num9 = 1.0f / num2;
		num9 = fabsf(num9);
		int num10 = (int)(px[(0)] + num8 * num2 + (float)num4 * 0.001f);
		int num11 = (int)(py[(0)] + num8 * num + (float)num5 * 0.001f);
		bool flag2 = Walls[(num10 + num11 * mapHeight[(0)])];
		float num12;
		if (flag2)
		{
			num12 = num8;
		}
		else
		{
			bool flag3 = num9 > num3;
			if (flag3)
			{
				num12 = num3;
			}
			else
			{
				num12 = num8;
				int num13 = num10;
				while (num13 > 0 && num13 < mapHeight[(0)])
				{
					num12 += num9;
					num10 = (int)(px[(0)] + num12 * num2 + (float)num4 * 0.01f);
					num11 = (int)(py[(0)] + num12 * num + (float)num5 * 0.01f);
					bool flag4 = Walls[(num10 + num11 * mapHeight[(0)])];
					if (flag4)
					{
						break;
					}
					num13 += num4;
				}
			}
		}
		float num14 = ((float)num7 - py[(0)]) / num;
		num14 = fabsf(num14);
		float num15 = 1.0f / num;
		num15 = fabsf(num15);
		num10 = (int)(px[(0)] + num14 * num2 + (float)num4 * 0.001f);
		num11 = (int)(py[(0)] + num14 * num + (float)num5 * 0.001f);
		bool flag5 = Walls[(num10 + num11 * mapHeight[(0)])];
		float num16;
		if (flag5)
		{
			num16 = num14;
		}
		else
		{
			bool flag6 = num15 > num3;
			if (flag6)
			{
				num16 = num3;
			}
			else
			{
				num16 = num14;
				int num17 = num11;
				while (num17 > 0 && num17 < mapHeight[(0)])
				{
					num16 += num15;
					num10 = (int)(px[(0)] + num16 * num2 + (float)num4 * 0.01f);
					num11 = (int)(py[(0)] + num16 * num + (float)num5 * 0.01f);
					bool flag7 = Walls[(num10 + num11 * mapHeight[(0)])];
					if (flag7)
					{
						break;
					}
					num17 += num5;
				}
			}
		}
		bool flag8 = num12 < num16;
		if (flag8)
		{
			offsets[(x)] = (py[(0)] + num12 * num) % 1.0f;
			dists[(x)] = (double)num12;
		}
		else
		{
			offsets[(x)] = (px[(0)] + num16 * num2) % 1.0f;
			dists[(x)] = (double)num16;
		}
	}
}
