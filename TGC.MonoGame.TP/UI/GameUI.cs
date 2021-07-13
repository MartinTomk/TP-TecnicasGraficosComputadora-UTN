using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TGC.MonoGame.TP.UI
{
    public class GameUI
    {
        public SpriteBatch _spriteBatch;
        private TGCGame _game;
        private SpriteFont _font;
        private Texture2D hudBG;
        private Texture2D barritaVida;
        private Texture2D bote;
        private Texture2D mira;
        private Rectangle spriteRectangle;
        private Vector2 spriteOrigin;
        private Vector2 textPos;


        public GameUI(TGCGame game)
        {
            _game = game;
            hudBG = _game.Content.Load<Texture2D>("Textures/Menu/hud");
            barritaVida = _game.Content.Load<Texture2D>("Textures/Menu/barrita");
            bote = _game.Content.Load<Texture2D>("Textures/Menu/bote");
            mira = _game.Content.Load<Texture2D>("Textures/Menu/mira");
            _font = _game.Content.Load<SpriteFont>("Fonts/bebas");
        }

        public void Draw()
        {
            var bullets = $"{_game.availableBullets}";
            var textSpeed = $"Speed: {_game.PlayerControlledShip.BoatVelocity}";
            //var rotation = $"Angle rotation: {_game.PlayerControlledShip.RotationRadians}";
            //var textLife = $"Health:  {_game.PlayerControlledShip._currentLife}";

            //var angle1 = $"AngleToPlayer: {_game.Ships[2].AngleToPlayer}";
            //var angle2 = $"FrontDirAngle: {_game.Ships[2].FrontDirectionAngle}";

            var viewportHeight = _game.GraphicsDevice.Viewport.Height;


            _game.spriteBatch.Begin();

            //_game.spriteBatch.Draw(hudBG, new Vector2(10, 10), Color.White);
            _game.spriteBatch.Draw(hudBG, new Vector2(10, 10), null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);

            for (int i = 0; i < _game.PlayerControlledShip._currentLife; i++)
            {
                _game.spriteBatch.Draw(barritaVida, new Vector2(i * 34 + 160, 54), null, Color.White, 0f, Vector2.Zero, new Vector2(0.85f, 0.9f), SpriteEffects.None, 0f);
            }


            spriteRectangle = new Rectangle(10, 380, mira.Width, mira.Height);
            spriteOrigin = new Vector2(mira.Width / 2, mira.Height / 2);

            _game.spriteBatch.Draw(mira, new Vector2(178, 545), null, Color.White, _game.PlayerControlledShip.RotationRadians, spriteOrigin, 0.8f, SpriteEffects.None, 0f);
            _game.spriteBatch.Draw(bote, new Vector2(122, 430), null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);

            
            _game.spriteBatch.DrawString(_font, textSpeed, new Vector2(30, 650), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);

            textPos = new Vector2(50, 30);

            if (_game.availableBullets < 10)
                textPos.X = 65;
            
            _game.spriteBatch.DrawString(_font, bullets, textPos, Color.LightBlue);

            _game.spriteBatch.End();
        }
    }
}