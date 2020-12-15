using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLMazeWalker;
using System;
using System.Diagnostics;
using SFML.Audio;

namespace MazeWalker
{
    class Player
    {
        public TimeSpan DrawTime = new TimeSpan(), CalcTime = new TimeSpan();
        public float x { get; set; }
        public float y { get; set; }
        public double a { get; set; }
        float sin_a, cos_a;
        private Map Map;
        private ProgressBar StaminaBar;

        private double PlayerSpeed;
        public float Stamina;

        //public int Lives = 5;
        //public int profile = 0;
        //bool isSaving = false;
        RayCast RayCast;
        Texture wall_texture;
        public System.Drawing.Point mapActivator;
        public Player(Map map)
        {
            this.x = map.In.x + 0.5F;
            this.y = map.In.y + 0.5F;
            this.a = 0;
            this.Map = map;

            PlayerSpeed = Settings.PlayerSpeed;
            Stamina = 1F;

            Random rnd = new Random(DateTime.Now.Millisecond);
            mapActivator = new System.Drawing.Point(rnd.Next(Settings.MapSize) * 2 + 1, rnd.Next(Settings.MapSize) * 2 + 1);

            sin_a = (float)Math.Sin(a);
            cos_a = (float)Math.Cos(a);
            RayCast = new RayCast(Map);
            wall_texture = new Texture(Resources.wall);
            wall_texture.Smooth = true;

            StaminaBar = new ProgressBar(20F, Settings.sHeight - 50, 35, Settings.sWidth / 4F);
            StaminaBar.SetText("Выносливость");
            StaminaBar.SetTextColor(Color.Cyan);


        }

        public void ReInit()
        {
            StaminaBar = new ProgressBar(20F, Settings.sHeight - 50, 35, Settings.sWidth / 4F);
        }

        public void Run()
        {
            if (Stamina > 0)
            {
                PlayerSpeed = Settings.BasePlayerSpeed * 2;
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.A) ||
                    Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.D) ||
                    (Joystick.IsButtonPressed(0,5) && isMovingByJoystick()) ) Stamina -= 0.005F;
            }
            else
                PlayerSpeed = Settings.BasePlayerSpeed / 2;
        }

        public void Turn(double rot)
        {
            a += rot;
            a = a % (Math.PI * 2);
            sin_a = (float)Math.Sin(a);
            cos_a = (float)Math.Cos(a);
        }

        public void JoystickTurn()
        {
            a += Math.Round(Joystick.GetAxisPosition(0, Joystick.Axis.U), 4) * 0.0005F;
            a = a % (Math.PI * 2);
            sin_a = (float)Math.Sin(a);
            cos_a = (float)Math.Cos(a);
        }

        public void Walk(Keyboard.Key Dir, double frame_time)
        {
            float d = (float)(PlayerSpeed * frame_time);
            switch (Dir)
            {
                case Keyboard.Key.W:
                    if (!Map.isWall((int)(x + (d + 0.05F) * cos_a), (int)y))
                        x = x + d * cos_a;
                    if (!Map.isWall((int)x, (int)(y + (d + 0.05F) * sin_a)))
                        y = y + d * sin_a;
                    break;
                case Keyboard.Key.S:
                    if (!Map.isWall((int)(x - (d + 0.05F) * cos_a), (int)y))
                        x = x - d * cos_a;
                    if (!Map.isWall((int)x, (int)(y - (d + 0.05F) * sin_a)))
                        y = y - d * sin_a;
                    break;
                case Keyboard.Key.A:
                    if (!Map.isWall((int)(x + (d + 0.05F) * sin_a), (int)y))
                        x = x + d * sin_a;
                    if (!Map.isWall((int)x, (int)(y - (d + 0.05F) * cos_a)))
                        y = y - d * cos_a;
                    break;
                case Keyboard.Key.D:
                    if (!Map.isWall((int)(x - (d + 0.05F) * sin_a), (int)y))
                        x = x - d * sin_a;
                    if (!Map.isWall((int)x, (int)(y + (d + 0.05F) * cos_a)))
                        y = y + d * cos_a;
                    break;
            }
            PlayerSpeed = Settings.BasePlayerSpeed;
            if (new System.Drawing.Point((int)x, (int)y) == mapActivator) Map.isVisible = true;
            if ((int)x == Map.Out.x && (int)y == Map.Out.y) { Program.menu.isWin = true; Program.menu.OnOpen(); }
        }

        private bool isMovingByJoystick()
        {
            float Yaxis = MathF.Round(Joystick.GetAxisPosition(0, Joystick.Axis.X) / 100F, 4);
            float Xaxis = -MathF.Round(Joystick.GetAxisPosition(0, Joystick.Axis.Y) / 100F, 4);
            if (Yaxis == 0 && Xaxis == 0) return false;
            return true;
        }

        public void JoystickWalk(double frame_time)
        {
            float Yaxis = MathF.Round(Joystick.GetAxisPosition(0, Joystick.Axis.X) / 100F, 4);
            float Xaxis = -MathF.Round(Joystick.GetAxisPosition(0, Joystick.Axis.Y) / 100F, 4);

            var length = MathF.Sqrt(Xaxis*Xaxis + Yaxis*Yaxis);
            var speedlength = MathF.Sqrt(Xaxis * Xaxis + Yaxis * Yaxis) > 1 ? 1 : MathF.Sqrt(Xaxis * Xaxis + Yaxis * Yaxis);
            if (Program.steps.Status != SoundStatus.Playing && speedlength > 0)
                Program.steps.Play();
            float d = Xaxis==0 && Yaxis == 0 ? 0 : (float)(speedlength * PlayerSpeed * frame_time);
            
            float angle = MathF.Acos(Xaxis/length) * (Yaxis >= 0 ? 1 : -1) ;
            
            float lsin = MathF.Sin((float)a + angle), lcos = MathF.Cos((float)a + angle);

            if (!Map.isWall((int)(x + (d + 0.05F) * lcos), (int)y))
                x = x + d * lcos;
            if (!Map.isWall((int)x, (int)(y + (d + 0.05F) * lsin)))
                y = y + d * lsin;


            PlayerSpeed = Settings.BasePlayerSpeed;
            if (new System.Drawing.Point((int)x, (int)y) == mapActivator) Map.isVisible = true;
            if ((int)x == Map.Out.x && (int)y == Map.Out.y) { Program.menu.isWin = true; Program.menu.OnOpen(); }
        }

        private void DrawBG(RenderWindow render)
        {
            var sky = new RectangleShape(new Vector2f(Settings.sWidth, Settings.sHalfHeight))
            {
                FillColor = new Color(135, 206, 235),
                Position = new Vector2f(0, 0)
            };
            render.Draw(sky);
            var floor = new RectangleShape(new Vector2f(Settings.sWidth, Settings.sHalfHeight))
            {
                FillColor = new Color(104, 65, 50),
                Position = new Vector2f(0, Settings.sHalfHeight)
            };
            render.Draw(floor);
        }

        Stopwatch sw = new Stopwatch();
        public void Draw(RenderWindow render)
        {
            DrawBG(render);
            float[] dists = new float[Settings.RAY_COUNT];
            float[] offsets = new float[Settings.RAY_COUNT];
            float[] angles = new float[Settings.RAY_COUNT];

            sw.Start();
            int i = 0;
            double angle = a - Settings.Half_FOV;
            while (angle < a + Settings.Half_FOV && i < Settings.RAY_COUNT)
            {
                angles[i] = (float)angle;
                angle += Settings.deltaFOV;
                i++;
            }
            for (int tid = 0; tid < dists.Length; tid++)
            {
                unsafe
                {
                    fixed (bool* map = Map.map)
                    {
                        //float[] mas = new float[2];
                        RayCast.GpGPU_Ray(x,
                                y,
                                angles,
                                out dists,
                                out offsets);
                        //dists[tid] = mas[0] * (float)Math.Cos(a - angles[tid]);
                        //offsets[tid] = mas[1];
                    }
                }
            }
            sw.Stop();
            CalcTime = sw.Elapsed;
            sw.Reset();
            sw.Start();
            int j = 0;
            float Screen_x = 0;
            foreach (double dist in dists)
            {
                float wall_height = (float)(Settings.Camera_Dist * Settings.WallHeight / dist * .0036F);
                float sky = Settings.sHalfHeight - wall_height / 2;

                var line = new RectangleShape(new Vector2f(Settings.Scale, wall_height))
                {
                    Texture = wall_texture,
                    TextureRect = new IntRect((int)(offsets[j] * Settings.TextureScale), 0, 2, Settings.TextureScale),
                    Position = new Vector2f(Screen_x, sky)
                };
                render.Draw(line);
                j++;
                Screen_x += Settings.Scale;
            }
            sw.Stop();
            DrawTime = sw.Elapsed;

            StaminaBar.Progress = Stamina;
            render.Draw(StaminaBar);

        }

        public void DrawOnMap(RenderWindow render)
        {
            int c = Map.Tile;
            RectangleShape mapact = new RectangleShape(new Vector2f(c, c))
            {
                Position = new Vector2f(mapActivator.X * c, mapActivator.Y * c),
                FillColor = Color.Magenta
            };

            CircleShape circle = new CircleShape(c / 2)
            {
                FillColor = Color.Red,
                Origin = new Vector2f(c/2, c/2),
                Position = new Vector2f(x * c + c / 4, y * c + c / 4),
            };
            render.Draw(mapact);
            render.Draw(circle);
        }
    }
}
