using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFMLMazeWalker;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace MazeWalker
{
    class Player
    {
        float x { get; set; }
        float y { get; set; }
        double a { get; set; }
        float sin_a, cos_a;
        private Map Map;
        RayCast RayCast;
        Texture wall_texture;
        System.Drawing.Point mapActivator;

        public Player(Map map)
        {
            this.x = map.In.x + 0.5F;
            this.y = map.In.y + 0.5F;
            this.a = 0;
            this.Map = map;

            Random rnd = new Random(DateTime.Now.Millisecond);
            mapActivator = new System.Drawing.Point(rnd.Next(Settings.MapSize) * 2 + 1, rnd.Next(Settings.MapSize) * 2 + 1);

            sin_a = (float)Math.Sin(a);
            cos_a = (float)Math.Cos(a);
            RayCast = new RayCast(Map);
            wall_texture = new Texture(Resources.wall);
            wall_texture.Smooth = true;

        }

        public void Turn(double rot)
        {
            a += rot;
            a = a % (Math.PI * 2);
            sin_a = (float)Math.Sin(a);
            cos_a = (float)Math.Cos(a);
        }

        public void Walk(Keyboard.Key Dir, double frame_time)
        {
            float d = (float)(Settings.PlayerSpeed * frame_time);
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

        public void Draw(RenderWindow render)
        {
            DrawBG(render);
            double[] dists = new double[Settings.RAY_COUNT];
            double[] offsets = new double[Settings.RAY_COUNT];
            double[] angles = new double[Settings.RAY_COUNT];


            int i = 0;
            double angle = a - Settings.Half_FOV;
            while (angle < a + Settings.Half_FOV && i < Settings.RAY_COUNT) {
                angles[i] = angle;
                angle += Settings.deltaFOV;
                i++;
            }

            for (int tid = 0; tid < dists.Length; tid++)
            {
                dists[tid] = RayCast.Ray((float)x, (float)y, angles[tid], out offsets[tid]) * Math.Cos(a - angles[tid]);
                //RayCast.Ray(x, y, angles[tid], out offsets[tid], out dists[tid], a);
            }


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
        }
        
        public void DrawOnMap(RenderWindow render)
        {
            int c = Map.Tile;
            RectangleShape mapact = new RectangleShape(new Vector2f(c, c))
            {
                Position = new Vector2f(mapActivator.X * c, mapActivator.Y * c),
                FillColor = Color.Magenta
            };

            CircleShape circle = new CircleShape(c / 2) {
                FillColor = Color.Red,
                Origin = new Vector2f(5, 5),
                Position = new Vector2f(x * c + c/4, y * c + c/4),
            };
            render.Draw(mapact);
            render.Draw(circle);
        }
    }
}
