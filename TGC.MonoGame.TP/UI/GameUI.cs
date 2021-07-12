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


        public GameUI(TGCGame game)
        {
            _game = game;
            
            _font = _game.Content.Load<SpriteFont>("Fonts/Font");
        }

        public void Draw()
        {
            var bullets = $"Bullets available: {_game.availableBullets}";
            var textSpeed = $"Current Speed: {_game.PlayerControlledShip.BoatVelocity}";
            var rotation = $"Angle rotation: {_game.PlayerControlledShip.RotationRadians}";
            var textLife = $"Health:  {_game.PlayerControlledShip._currentLife}";

            //var angle1 = $"AngleToPlayer: {_game.Ships[2].AngleToPlayer}";
            //var angle2 = $"FrontDirAngle: {_game.Ships[2].FrontDirectionAngle}";

            var viewportHeight = _game.GraphicsDevice.Viewport.Height;

            
            _game.spriteBatch.Begin();
            _game.spriteBatch.DrawString(_font, textSpeed, new Vector2(10, viewportHeight - 20), Color.White);
            _game.spriteBatch.DrawString(_font, rotation, new Vector2(10, viewportHeight - 50), Color.White);
            _game.spriteBatch.DrawString(_font, textLife, new Vector2(10, viewportHeight - 80), Color.White);
            _game.spriteBatch.DrawString(_font, bullets, new Vector2(10, viewportHeight - 110), Color.White);

            // pal debug :D
            //_game.spriteBatch.DrawString(_font, angle1, new Vector2(10, viewportHeight - 200), Color.White);
            //_game.spriteBatch.DrawString(_font, angle2, new Vector2(10, viewportHeight - 230), Color.White);

            _game.spriteBatch.End();
        }
    }
}
