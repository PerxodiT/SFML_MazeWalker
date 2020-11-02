using MazeWalker;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SFMLMazeWalker
{
    class Menu
    {
        Texture background;
        public Button start, newgame, settings, exit;
        public bool isWin = false, isFirst = true;
        private bool isSettings = false;

        private Button VSynh, ExitSettings;

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

            VSynh = new Button("Вертикальная синхронизация: ", Program.button, new Vector2f(-4, 0));
            VSynh.Enabled = false;
            ExitSettings = new Button("Назад", Program.button, new Vector2f(-2, 0));
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

        private void SettingsMenu()
        {
            string VSynhText = Settings.VSync ? "Вкл" : "Выкл";
            VSynh = new Button($"Вертикальная синхронизация: {VSynhText}", Program.button, new Vector2f(-4, 0));
            VSynh.SetAction(VSynhOnClick);
            VSynh.SetPosition(new Vector2f(Settings.sWidth + 200, Settings.sHeight - 100 - 25));
            VSynh.SetDestinationPoint(new Vector2f(Settings.sWidth - 50 - VSynh.GetGlobalBounds().Width, Settings.sHeight - 100 - 25));
            VSynh.StartAnimation();

            ExitSettings = new Button("Назад", Program.button, new Vector2f(-2, 0));
            ExitSettings.SetAction(ExitSettingsOnClick);
            ExitSettings.SetAnimationEndAction(ExitSettingsOnAnimEnd);
            ExitSettings.SetPosition(new Vector2f(Settings.sWidth + 200, Settings.sHeight - 50 - 25));
            ExitSettings.SetDestinationPoint(new Vector2f(Settings.sWidth - 50 - ExitSettings.GetGlobalBounds().Width, Settings.sHeight - 50 - 25));
            ExitSettings.StartAnimation();

            VSynh.Enabled = true;
            ExitSettings.Enabled = true;
        }

        public void Draw(RenderWindow render)
        {
            var bg = new RectangleShape(new Vector2f(Settings.sWidth, Settings.sHeight))
            {
                Texture = background,
                Position = new Vector2f(0,0)
            };


            render.Draw(bg);

            if (isWin || isFirst) start.ChangeButtonState(ButtonState.Inactive);
            else start.ChangeButtonState(ButtonState.Active);

            render.Draw(start);
            render.Draw(newgame);
            render.Draw(settings);
            render.Draw(exit);
            render.Draw(VSynh);
            render.Draw(ExitSettings);

        }
    }
}
