using MazeWalker;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;

namespace SFMLMazeWalker
{
    class Program
    {
        public static bool isMenu = true;
        public static bool Exit = false;
        public static ContextSettings settings = new ContextSettings()
        {
            AntialiasingLevel = 16,
            AttributeFlags = ContextSettings.Attribute.Debug
        };
        public static RenderWindow window = new RenderWindow(
            new VideoMode((uint)Settings.sWidth, (uint)Settings.sHeight),
            "Maze Walker - The game",
            Styles.Fullscreen,
            settings
        );
        static double frametime = 1;
        public static Player player;
        public static Map map;
        public static Menu menu;

        public static Music ingame;
        public static Music inmenu;
        public static SoundBuffer stepsbuf;
        public static Sound steps;
        public static SoundBuffer buttonbuf;
        public static Sound button;
        public static int KeysPressed;
        private static bool isEnding = false;
        private static bool isMatrix = false;

        public static void Init()
        {
            window.Closed += Window_Closed;
            window.KeyPressed += Window_KeyPressed;
            window.KeyReleased += Window_KeyReleased;
            window.MouseMoved += Window_MouseMoved;
        }

        public static string SettingsFileName = "Settings.bin";
        static void Main(string[] args)
        {
            Settings.Load();

            Init();

            float FPS = 0;
            int fcount = 0;
            map = new Map();
            player = new Player(map);

            Text fps = new Text(FPS.ToString(), new Font(Resources.FPS_Font));
            fps.FillColor = Color.Red;
            fps.Position = new Vector2f(Settings.sWidth - 50, 10);


            ingame = new Music(Resources.ingame_music);
            inmenu = new Music(Resources.menu_music);
            stepsbuf = new SoundBuffer(Resources.steps);
            steps = new Sound(stepsbuf);

            buttonbuf = new SoundBuffer(Resources.button);
            button = new Sound(buttonbuf);

            steps.Loop = true;
            ingame.Loop = true;
            inmenu.Loop = true;

            menu = new Menu();
            menu.OnOpen();


            while (window.IsOpen)
            {
                window.SetVerticalSyncEnabled(Settings.VSync);
                window.SetFramerateLimit(Settings.FPS_Limit);
                fps.Position = new Vector2f(Settings.sWidth - 50, 10);

                if (Exit) window.Close();
                fcount++;
                DateTime start = DateTime.Now;
                fps.DisplayedString = ((int)FPS).ToString();
                window.DispatchEvents();

                KeysPressed = 0;
                if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                {
                    if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.W))
                        steps.Play();
                    KeysPressed++;
                    player.Walk(Keyboard.Key.W, frametime);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                {
                    if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.A))
                        steps.Play();
                    KeysPressed++;
                    player.Walk(Keyboard.Key.A, frametime);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                {
                    if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.S))
                        steps.Play();
                    KeysPressed++;
                    player.Walk(Keyboard.Key.S, frametime);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                {
                    if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.D))
                        steps.Play();
                    KeysPressed++;
                    player.Walk(Keyboard.Key.D, frametime);
                }


                window.Clear(Color.White);
                window.SetMouseCursorVisible(isMenu);
                //Drawing
                if (menu.isWin) EndScene();
                if (isMenu)
                {
                    menu.Draw(window);
                }
                else
                {
                    inmenu.Stop();
                    player.Draw(window);
                    map.Draw(window);
                    player.DrawOnMap(window);
                }
                //====================
                window.Draw(fps);
                window.Display();


                frametime = (DateTime.Now - start).TotalSeconds;
                if (fcount == 20)
                {
                    FPS = (float)(1 / frametime);
                    fcount = 0;
                }
            }
        }

        private static void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.W || e.Code == Keyboard.Key.A || e.Code == Keyboard.Key.S || e.Code == Keyboard.Key.D) steps.Pause();
        }

        private static void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (isMenu) return;
            int rotate = Mouse.GetPosition(window).X - (Settings.sWidth / 2);
            player.Turn(rotate * 0.001F);
            Mouse.SetPosition(new Vector2i(Settings.sWidth / 2, Settings.sHalfHeight), window);

        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (!isEnding)
            {
                if (e.Code == Keyboard.Key.V)
                {
                    Program.menu.isWin = true; Program.menu.OnOpen();
                    return;
                }
                if (e.Code == Keyboard.Key.Escape && (!menu.isFirst))
                {
                    isMenu = !isMenu;
                    if (isMenu) menu.OnOpen();
                    return;
                }
                if (e.Code == Keyboard.Key.M)
                {
                    map.isVisible = !map.isVisible;
                    return;
                }
                return;
            }
            if (e.Code != Keyboard.Key.Unknown)
                isMatrix = false;
            return;
        }


        static public void EndScene()
        {
            isEnding = true;
            window.SetMouseCursorVisible(false);
            Font font = new Font(Resources.MenuFont);
            for (byte i = 0; i <= 250; i++)
            {
                window.Clear(Color.Black);

                player.Draw(window);
                map.Draw(window);
                player.DrawOnMap(window);

                var bg = new RectangleShape(new Vector2f(Settings.sWidth, Settings.sHeight))
                {
                    FillColor = new Color(0, 0, 0, i),
                    Position = new Vector2f(0, 0)
                };
                window.Draw(bg);
                window.Display();
            }

            Text win = new Text("Код 36: Матрица взломана. Выход найден.", font);
            FloatRect winbox = win.GetGlobalBounds();
            var text_pos = new Vector2f(Settings.sWidth / 2 - winbox.Width / 2, Settings.sHalfHeight - winbox.Height / 2);
            win.Position = text_pos;

            for (byte i = 250; i >= 10; i--)
            {
                window.Clear(Color.Black);
                win.FillColor = new Color(0, 255, 0, (byte)(250 - i));
                window.Draw(win);
                window.Display();
            }
            MatrixEffect matrix = new MatrixEffect();
            isEnding = false;
            isMatrix = true;
            //window.KeyPressed += Window_KeyPressed;
            while (isMatrix && window.IsOpen)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) isMatrix = false;
                window.DispatchEvents();
                window.Clear(Color.Black);
                window.Draw(matrix);
                window.Draw(win);
                window.Display();
            }
            menu.isWin = false;
            menu.isFirst = true;
            isMenu = true;
            window.SetMouseCursorVisible(true);
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
