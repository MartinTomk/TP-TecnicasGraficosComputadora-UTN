using TGC.MonoGame.TP.Menu.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TGC.MonoGame.TP.Menu.States
{
    public class MenuScreen : Screen
    {
        public const int ST_PRESENTACION = 0;
        public const int ST_MENU = 1;
        public const int ST_CONTROLS = 2;
        public int status = ST_PRESENTACION;

        private Texture2D background;
        private Texture2D BGInstructions;
        private Texture2D BGControls;

        private List<Button> _ButtonsMainMenu;

        private Texture2D buttonTexture;
        private SpriteFont buttonFont;

        Button menuButton;
        Button newGameButton;
        //Button godGameButton;
        Button controlsButton;
        Button quitGameButton;
        Button goBackButton;


        public MenuScreen(Game game, GraphicsDeviceManager graphics, ContentManager content): base(game, graphics, content)
        {

            background = _content.Load<Texture2D>("Textures/Menu/Presentacion_IM");
            //Title = _content.Load<Texture2D>("Background/Titulo");
            BGInstructions = _content.Load<Texture2D>("Textures/Menu/MenuBG");
            BGControls = _content.Load<Texture2D>("Textures/Menu/controlesBG");


            buttonTexture = _content.Load<Texture2D>("Textures/Menu/Boton");
            buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            menuButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(560, 600),
                Text = "Menu",
            };

            menuButton.Click += MenuButton_Click;

            newGameButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(120, 80),
                Text = "Jugar",
            };

            newGameButton.Click += NewGameButton_Click;
            newGameButton.visible = false;

            controlsButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(320, 80),
                Text = "Controles",
            };

            controlsButton.Click += controlsButton_Click;
            controlsButton.visible = false;

            goBackButton = new Button(buttonTexture, buttonFont, false)
            {
                Position = new Vector2(800, 600),
                Text = "Menu Principal",
            };
            
            goBackButton.Click += goBackButton_Click;
            goBackButton.visible = false;

            //godGameButton = new Button(buttonTexture, buttonFont, true)
            //{
            //   Position = new Vector2(300, 300),
            //    Text = "Modo Dios",
            //};

            //godGameButton.Click += GodGameButton_Click;



            quitGameButton = new Button(buttonTexture, buttonFont, true)
            {
                Position = new Vector2(1000, 600),
                Text = "Salir",
            };

            quitGameButton.Click += QuitGameButton_Click;
            quitGameButton.visible = false;


            _ButtonsMainMenu = new List<Button>()
            {
                newGameButton,
                //godGameButton,
                controlsButton,
                quitGameButton,
                goBackButton,
                menuButton,
            };
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            switch (status)
            {
                case ST_PRESENTACION:
                    spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
                    break;
                case ST_MENU:
                    spriteBatch.Draw(BGInstructions, Vector2.Zero, Color.White);
                    newGameButton.visible = true;
                    quitGameButton.visible = true;
                    controlsButton.visible = true;
                    break;
                case ST_CONTROLS:
                    spriteBatch.Draw(BGControls, Vector2.Zero, Color.White);
                    goBackButton.visible = true;
                    quitGameButton.visible = true;
                    
                    spriteBatch.DrawString(buttonFont, $"Movimiento del barco:\n" +
                                $"\n" +
                                $"      W: Mover hacia adelante\n" +
                                $"      A: Rotar hacia la izquierda\n" +
                                $"      S: Mover hacia atras\n" +
                                $"      D: Rotar hacia la derecha\n" +
                                $"\n", new Vector2(650, 150), Color.White);

                    spriteBatch.DrawString(buttonFont, $"Movilidad de la mira:\n" +
                                $"\n" +
                                $"      Apuntar: Mouse\n" +
                                $"      Disparar: Boton Izquierdo del Mouse\n" +
                                $"      Vista: Boton Derecho del Mouse\n" +
                                $"\n", new Vector2(900, 150), Color.White);

                    break;
            }

            foreach (Button button in _ButtonsMainMenu)
                button.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }


        private void MenuButton_Click(object sender, EventArgs e)
        {
            menuButton.visible = false;
            status = ST_MENU;
        }
        private void NewGameButton_Click(object sender, EventArgs e)
        {
            using (var game = new TGCGame())
                game.Run();
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            goBackButton.visible = false;
            status = ST_MENU;
        }
        
        private void controlsButton_Click(object sender, EventArgs e)
        {
            controlsButton.visible = false;
            newGameButton.visible = false;
            status = ST_CONTROLS;
        }

        //private void GodGameButton_Click(object sender, EventArgs e)
        //{
        //    using (var game = new ChinchuGame(true))
        //        game.Run();
        //}


        public override void Update(GameTime gameTime)
        {
            foreach (var button in _ButtonsMainMenu)
                button.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }
    }
}
