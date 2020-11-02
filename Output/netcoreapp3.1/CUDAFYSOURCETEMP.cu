
// MazeWalker.RayCast
extern "C" __global__  void Ray( float* px, int pxLen0,  float* py, int pyLen0,  double* angles, int anglesLen0,  float* offsets, int offsetsLen0,  double* dists, int distsLen0,  bool* Walls, int WallsLen0,  int* mapHeight, int mapHeightLen0);

// MazeWalker.RayCast
extern "C" __global__  void Ray( float* px, int pxLen0,  float* py, int pyLen0,  double* angles, int anglesLen0,  float* offsets, int offsetsLen0,  double* dists, int distsLen0,  bool* Walls, int WallsLen0,  int* mapHeight, int mapHeightLen0)
{
	int num = blockIdx.x * blockDim.x + threadIdx.x;
	bool flag = num < 1920;
	if (flag)
	{
		int num2 = mapHeight[(0)];
		int num3 = mapHeight[(0)] * 2;
		float num4 = sinf((float)angles[(num)]);
		float num5 = cosf((float)angles[(num)]);
		num4 = ((num4 == 0.0f) ? 0.0001f : num4);
		num5 = ((num5 == 0.0f) ? 0.0001f : num5);
		int num6 = (num5 >= 0.0f) ? 1 : -1;
		int num7 = (num4 >= 0.0f) ? 1 : -1;
		int num8 = (num6 == 1) ? ((int)px[(0)] + 1) : ((int)px[(0)]);
		int num9 = (num7 == 1) ? ((int)py[(0)] + 1) : ((int)py[(0)]);
		float num10 = abs(((float)num8 - px[(0)]) / num5);
		float num11 = abs(1.0f / num5);
		int num12 = (int)(px[(0)] + num10 * num5 + (float)num6 * 0.001f);
		int num13 = (int)(py[(0)] + num10 * num4 + (float)num7 * 0.001f);
		bool flag2 = Walls[(num12 + num13 * mapHeight[(0)])];
		float num14;
		if (flag2)
		{
			num14 = num10;
		}
		else
		{
			bool flag3 = num11 > (float)num3;
			if (flag3)
			{
				num14 = (float)num3;
			}
			else
			{
				num14 = num10;
				int num15 = num12;
				while (num15 > 0 && num15 < num2)
				{
					num14 += num11;
					num12 = (int)(px[(0)] + num14 * num5 + (float)num6 * 0.01f);
					num13 = (int)(py[(0)] + num14 * num4 + (float)num7 * 0.01f);
					bool flag4 = num12 + num13 * mapHeight[(0)] < WallsLen0;
					if (flag4)
					{
						bool flag5 = Walls[(num12 + num13 * mapHeight[(0)])];
						if (flag5)
						{
							break;
						}
					}
					num15 += num6;
				}
			}
		}
		float num16 = abs(((float)num9 - py[(0)]) / num4);
		float num17 = abs(1.0f / num4);
		num12 = (int)(px[(0)] + num16 * num5 + (float)num6 * 0.001f);
		num13 = (int)(py[(0)] + num16 * num4 + (float)num7 * 0.001f);
		bool flag6 = Walls[(num12 + num13 * mapHeight[(0)])];
		float num18;
		if (flag6)
		{
			num18 = num16;
		}
		else
		{
			bool flag7 = num17 > (float)num3;
			if (flag7)
			{
				num18 = (float)num3;
			}
			else
			{
				num18 = num16;
				int num19 = num13;
				while (num19 > 0 && num19 < num2)
				{
					num18 += num17;
					num12 = (int)(px[(0)] + num18 * num5 + (float)num6 * 0.01f);
					num13 = (int)(py[(0)] + num18 * num4 + (float)num7 * 0.01f);
					bool flag8 = num12 + num13 * mapHeight[(0)] < WallsLen0;
					if (flag8)
					{
						bool flag9 = Walls[(num12 + num13 * mapHeight[(0)])];
						if (flag9)
						{
							break;
						}
					}
					num19 += num7;
				}
			}
		}
		bool flag10 = num14 < num18;
		if (flag10)
		{
			offsets[(num)] = abs(py[(0)] + num14 * num4 - (float)((int)(py[(0)] + num14 * num4)));
			dists[(num)] = (double)num14;
		}
		else
		{
			offsets[(num)] = abs(px[(0)] + num18 * num5 - (float)((int)(px[(0)] + num18 * num5)));
			dists[(num)] = (double)num18;
		}
	}
}
