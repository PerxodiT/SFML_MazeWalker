using MazeWalker;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System.Threading;

namespace SFMLMazeWalker
{
    class Menu
    {
        Texture background;
        public Button start, newgame, settings, exit;
        public bool isWin = false, isFirst = true;
        private bool isSettings = false;

        private Button VSynh, ExitSettings, Resolution, FPSLimit;

        public Menu()
        {
            background = new Texture(Resources.menu_bg);
            background.Smooth = true;
        }

        private void StartOnClick() { Program.isMenu = false; Program.inmenu.Stop(); Program.ingame.Play(); isFirst = false; }
        private void NewGameOnClick()
        {
            isFirst = false;
            Program.map = new Map();
            Program.player = new Player(Program.map);
            Program.isMenu = false;
            Program.inmenu.Stop();
            Program.ingame.Play();
        }
        private void ExitOnClick() { Program.Exit = true; }
        private void SettingsOnClick()
        {
            start.SetSpeed(new Vector2f(-5, 0));
            start.SetDestinationPoint(new Vector2f(-200, 50));
            start.StartAnimation();

            newgame.SetSpeed(new Vector2f(-4, 0));
            newgame.SetDestinationPoint(new Vector2f(-200, 50));
            newgame.StartAnimation();

            settings.SetSpeed(new Vector2f(-3, 0));
            settings.SetDestinationPoint(new Vector2f(-200, 50));
            settings.StartAnimation();

            exit.SetSpeed(new Vector2f(-2, 0));
            exit.SetDestinationPoint(new Vector2f(-200, 50));
            exit.StartAnimation();

            FPSLimit.Enabled = true;
            VSynh.Enabled = true;
            ExitSettings.Enabled = true;
            Resolution.Enabled = true;
            SettingsMenu();
        }

        public void OnOpen()
        {
            start = new Button("Продолжить", Program.button, new Vector2f(5, 0));
            start.SetAction(StartOnClick);
            start.SetDestinationPoint(new Vector2f(50, 50));
            start.SetPosition(new Vector2f(-200, 50));
            start.StartAnimation();

            newgame = new Button("Новая игра", Program.button, new Vector2f(4, 0));
            newgame.SetAction(NewGameOnClick);
            newgame.SetDestinationPoint(new Vector2f(50, 100));
            newgame.SetPosition(new Vector2f(-200, 100));
            newgame.StartAnimation();

            settings = new Button("Настройки", Program.button, new Vector2f(3, 0));
            settings.SetAction(SettingsOnClick);
            settings.SetDestinationPoint(new Vector2f(50, 150));
            settings.SetPosition(new Vector2f(-200, 150));
            settings.StartAnimation();

            exit = new Button("Выход", Program.button, new Vector2f(2, 0));
            exit.SetAction(ExitOnClick);
            exit.SetDestinationPoint(new Vector2f(50, 150));
            exit.SetPosition(new Vector2f(-200, 200));
            exit.StartAnimation();

            FPSLimit = new Button($"Ограничение FPS: {Settings.FPS_Limit}", Program.button, new Vector2f(-6, 0));
            Resolution = new Button($"Разрешение {Settings.sWidth}X{Settings.sHeight}", Program.button, new Vector2f(-5, 0));
            string VSynhText = Settings.VSync ? "Вкл" : "Выкл";
            VSynh = new Button($"Вертикальная синхронизация: {VSynhText}", Program.button, new Vector2f(-4, 0));
            ExitSettings = new Button("Назад", Program.button, new Vector2f(-2, 0));

            FPSLimit.Enabled = false;
            Resolution.Enabled = false;
            VSynh.Enabled = false;
            ExitSettings.Enabled = false;

            Program.ingame.Pause();
            if (Program.inmenu.Status != SoundStatus.Playing)
                Program.inmenu.Play();
        }


        private void ExitSettingsOnClick()
        {
            OnOpen();
        }
        private void ExitSettingsOnAnimEnd()
        {
            isSettings = false;
        }
        private void VSynhOnClick()
        {
            Settings.VSync = !Settings.VSync;
            Thread.Sleep(200);
            string VSynhText = Settings.VSync ? "Вкл" : "Выкл";
            VSynh.SetText($"Вертикальная синхронизация: {VSynhText}");
        }
        private void ResolutionOnClick()
        {
            Thread.Sleep(300);
            Settings.ResMode = (Settings.ResMode + 1) % Settings.ResCount;
            Settings.Update(true);
        }

        private void FPSLimitOnClick()
        {
            Thread.Sleep(300);
            Settings.CurrentFPS_ID = (Settings.CurrentFPS_ID + 1) % Settings.FpsLimits.Length;
            Settings.FPS_Limit = Settings.FpsLimits[Settings.CurrentFPS_ID];
            FPSLimit.SetText($"Ограничение FPS: {Settings.FPS_Limit}");
        }

        public void SettingsMenu()
        {
            isSettings = true;

            FPSLimit.SetText($"Ограничение FPS: {Settings.FPS_Limit}");
            FPSLimit.SetAction(FPSLimitOnClick);
            FPSLimit.SetPosition(new Vector2f(Settings.sWidth + 200, Settings.sHeight - 200 - 25));
            FPSLimit.SetDestinationPoint(new Vector2f(Settings.sWidth - 50 - FPSLimit.GetGlobalBounds().Width, Settings.sHeight - 200 - 25));
            FPSLimit.StartAnimation();

            Resolution.SetText($"Разрешение {Settings.sWidth}X{Settings.sHeight}");
            Resolution.SetAction(ResolutionOnClick);
            Resolution.SetPosition(new Vector2f(Settings.sWidth + 200, Settings.sHeight - 150 - 25));
            Resolution.SetDestinationPoint(new Vector2f(Settings.sWidth - 50 - Resolution.GetGlobalBounds().Width, Settings.sHeight - 150 - 25));
            Resolution.StartAnimation();

            string VSynhText = Settings.VSync ? "Вкл" : "Выкл";
            VSynh.SetText($"Вертикальная синхронизация: {VSynhText}");
            VSynh.SetAction(VSynhOnClick);
            VSynh.SetPosition(new Vector2f(Settings.sWidth + 200, Settings.sHeight - 100 - 25));
            VSynh.SetDestinationPoint(new Vector2f(Settings.sWidth - 50 - VSynh.GetGlobalBounds().Width, Settings.sHeight - 100 - 25));
            VSynh.StartAnimation();

            ExitSettings.SetAction(ExitSettingsOnClick);
            ExitSettings.SetAnimationEndAction(ExitSettingsOnAnimEnd);
            ExitSettings.SetPosition(new Vector2f(Settings.sWidth + 200, Settings.sHeight - 50 - 25));
            ExitSettings.SetDestinationPoint(new Vector2f(Settings.sWidth - 50 - ExitSettings.GetGlobalBounds().Width, Settings.sHeight - 50 - 25));
            ExitSettings.StartAnimation();
        }

        public void Draw(RenderWindow render)
        {
            var bg = new RectangleShape(new Vector2f(Settings.sWidth, Settings.sHeight))
            {
                Texture = background,
                Position = new Vector2f(0, 0)
            };


            render.Draw(bg);

            if (isWin || isFirst) start.ChangeButtonState(ButtonState.Inactive);
            else start.ChangeButtonState(ButtonState.Active);

            render.Draw(start);
            render.Draw(newgame);
            render.Draw(settings);
            render.Draw(exit);

            render.Draw(FPSLimit);
            render.Draw(Resolution);
            render.Draw(VSynh);
            render.Draw(ExitSettings);


        }
    }
}
