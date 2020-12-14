
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
        public static float TEST;

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
            window.TextEntered += Window_TextEntered;
            window.JoystickConnected += Window_JoystickConnected;
            window.JoystickButtonPressed += Window_JoystickButtonPressed;
        }

        private static void Window_JoystickButtonPressed(object sender, JoystickButtonEventArgs e)
        {
            TEST = e.Button;
            //if (e.Button == 5)
            //player.Run();
        }

        private static void Window_JoystickConnected(object sender, JoystickConnectEventArgs e)
        {
            window.SetJoystickThreshold(0.05F);
        }

        private static void Window_TextEntered(object sender, TextEventArgs e)
        {
            menu.textBox.TextEntered(e);
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

            Text debug = new Text($"Время расчета: {player.CalcTime.TotalMilliseconds}\nВремя отрисовки: {player.DrawTime.TotalMilliseconds}", new Font(Resources.FPS_Font));
            debug.Position = new Vector2f(50, Settings.sHeight - 400);
            debug.FillColor = Color.Magenta;

            Settings.Update(true);

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

                if (player.Stamina > 1) player.Stamina = 1;
                if (player.Stamina < 0) player.Stamina = 0;
                Joystick.Update();
                if (!Joystick.IsConnected(0))
                {
                    if (Keyboard.IsKeyPressed(Keyboard.Key.RShift) || Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                        player.Run();
                    else player.Stamina += player.Stamina < 1 ? 0.0025F : 0;
                    if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                    {
                        if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.W))
                            steps.Play();
                        player.Walk(Keyboard.Key.W, frametime);
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                    {
                        if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.A))
                            steps.Play();
                        player.Walk(Keyboard.Key.A, frametime);
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                    {
                        if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.S))
                            steps.Play();
                        player.Walk(Keyboard.Key.S, frametime);
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    {
                        if (steps.Status != SoundStatus.Playing && Keyboard.IsKeyPressed(Keyboard.Key.D))
                            steps.Play();
                        player.Walk(Keyboard.Key.D, frametime);
                    }
                }
                else
                {
                    player.JoystickTurn();
                    player.JoystickWalk(frametime);

                    if (Joystick.IsButtonPressed(0,5))
                        player.Run();
                    else player.Stamina += player.Stamina < 1 ? 0.0025F : 0;
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.F1)) Help();


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



                if (fcount == 20)
                    debug.DisplayedString = $"" +
                            $"Joystick.Axis.Y = {MathF.Round(Joystick.GetAxisPosition(0, Joystick.Axis.Z) / 100F, 4)}\n" +
                            $"Joystick.Axis.X = {Joystick.IsButtonPressed(0, 6)}\n" +
                            $"Test = {TEST}\n";

                window.Draw(debug);
                window.Draw(fps);
                window.Display();


                frametime = (DateTime.Now - start).TotalSeconds;
                if (fcount == 20)
                {
                    FPS = (float)(1 / frametime);
                    fcount = 0;
                    window.Draw(debug);
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

        static bool isHelp = true;
        private static void HelpOFF()
        {
            isHelp = false;
        }
        public static void Help()
        {
            isHelp = true;
            string HelpString = "\t\tПРИВЕТСТВИЕ \n\n " +
                                "Вас приветствует разработчик игры MazeWalker, \n " +
                                "вы попали в справку о данной игре. \n\n\n " +
                                "\t\tУПРАВЛЕНИЕ \n\n " +
                                "Управление является стандартным для игр и производится клавишами \n " +
                                "\tW – Движение вперед \n " +
                                "\tA – Движение влево \n " +
                                "\tS – Движение назад \n " +
                                "\tD – Движение вправо \n " +
                                "Управление поворотом производится мышью \n " +
                                "Так же доступно ускорение  \n " +
                                "оно включается на клавишу Shift  \n " +
                                "(не важно левый или правый) \n " +
                                "Ускорение действует пока есть выносливость \n " +
                                "она показана голубой полосой в левом нижнем углу экрана \n\n\n " +
                                "\t\tЦЕЛЬ \n\n " +
                                "Целью игры является нахождение выхода из лабиринта, \n " +
                                "каждый раз лабиринт генерируется случайно но при \n " +
                                "выходе в меню игра сохраняется в текущий профиль, \n " +
                                "который выбирается в меню нажатием на надпись «Текущий профиль», \n " +
                                "затем для загрузки нажмите кнопку «Загрузить». \n " +
                                "Фиолетовой меткой на миникарте обозначается \n " +
                                "местоположение бонуса дающего доступ к карте \n " +
                                "со стенами на которой обозначен выход зеленой меткой.";

            var HelpText = new Button(HelpString, Program.button, new Vector2f(0, -0.5F));
            HelpText.ChangeButtonState(ButtonState.NotClickable);
            HelpText.SetPosition(new Vector2f((Settings.sWidth - HelpText.GetLocalBounds().Width) / 2, Settings.sHeight + 20));
            HelpText.SetDestinationPoint(new Vector2f((Settings.sWidth - HelpText.GetLocalBounds().Width) / 2, -HelpText.GetLocalBounds().Height - 20));
            HelpText.SetAnimationEndAction(HelpOFF);
            HelpText.StartAnimation();

            while (isHelp && window.IsOpen)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) isHelp = false;
                window.DispatchEvents();
                window.Clear(Color.Black);

                window.Draw(HelpText);
                window.Display();
            }
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (menu.textBox.HasFocus) return;
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
            double fading = 0;
            while (isMatrix && window.IsOpen)
            {
                fading += 0.02;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape)) isMatrix = false;
                window.DispatchEvents();
                window.Clear(new Color(0,0,0,100));
                window.Draw(matrix);

                win.FillColor = new Color(0, 255, 0, (byte)(Math.Sin(fading) * 128 + 127));
                fading = fading % (Math.PI * 2);

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
