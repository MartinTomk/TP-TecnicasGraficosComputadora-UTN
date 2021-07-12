using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TGC.MonoGame.TP.Cameras;

namespace TGC.MonoGame.TP.Bullet
{
    public class Bullet
    {
        private float _bulletSpeed = 10f;
        private float _timeElapsed;
        private Vector3 _destinationPosition;
        private float _lifeTime = 10;

        private TGCGame _game;
        private Camera _camera;
        private bool _searchImpact;
        public bool _active;
        public bool _available;

        public Bullet()
        {
            _available = true;
        }

        public void Init(TGCGame tgcGame, Vector3 origin)
        {
            _game = tgcGame;
            _camera = _game.CurrentCamera;
            _destinationPosition = origin;
            _timeElapsed = 0;
            _searchImpact = false;
            _active = true;
        }

        public void Update()
        {

            if (_timeElapsed > _lifeTime || Impact(_destinationPosition) || _destinationPosition.Y < 0.01f)
            {
                _searchImpact = false;
                _active = false;
                _available = false;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _destinationPosition.Y += _bulletSpeed - (_timeElapsed * 3f);
            _destinationPosition.Z = (Math.Abs(_destinationPosition.Z) * -1) - _bulletSpeed;

            _game.BulletModel.Draw(_game.World * Matrix.CreateTranslation(_destinationPosition), _camera.View, _camera.Projection);
            _timeElapsed += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            _searchImpact = true;

        }

        private bool Impact(Vector3 destination)
        {
            bool willCollide = false;
            if (_searchImpact)
            {
                BoundingSphere FuturePosition = new BoundingSphere(destination, 1f);

                for (var index = 0; index < _game.Ships.Length && !willCollide; index++)
                {
                    if (FuturePosition.Intersects(_game.Ships[index].BoatSphere))
                    {
                        willCollide = true;
                    }
                }
            }

            return willCollide;
        }
    }
}
