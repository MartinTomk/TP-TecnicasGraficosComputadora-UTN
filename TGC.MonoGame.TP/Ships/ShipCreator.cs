using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Cameras;

namespace TGC.MonoGame.TP.Ships
{
    public class Ship
    {
        private TGCGame Game;
        
        private float time;

        private Model ShipModel { get; set; }

        public string ModelName;
        private Effect ShipEffect { get; set; }

        public string EffectName;

        public Texture2D ShipTexture;
        public Texture2D ShipAoTexture;
        public Texture2D ShipNormalTexture;

        public string TextureName;
        public string TextureAoName;
        public string TextureNormalName;
        public Vector3 Position { get; set; }
        public Vector3 ModelRotation { get; set; }

        public Vector3 FrontDirection { get; set; }
        public Vector3 wavesRotation { get; set; }
        public Vector3 Scale { get; set; }
        private Ship[] OtherShips;
        public float Speed { get; set; }
        public float Length { get; set; }

        public float RotationRadians;

        public float MovementSpeed { get; set; }
        public float RotationSpeed { get; set; }


        public float BoatVelocity;
        public float BoatAcceleration;
        public bool bIsMoving;
        public Matrix BoatMatrix { get; set; }

        public bool playerMode = false;
        private Matrix waterMatrix { get; set; }

        //private Matrix[] BoneMatrix;

        public BoundingSphere BoatBox { get; set; }
        //public BoundingBox BoatBox { get; set; }
        
        public SpherePrimitive DebugSphere;
        public Vector3 ProaPos { get; set; }
        public Vector3 PopaPos { get; set; }
        public int _currentLife = 100;
        public Ship(TGCGame game, Vector3 pos, Vector3 rot, Vector3 scale, float speed, float length, string modelName, string effect, string textureName, string textureAoName, string textureNormalName)

        {
            Game = game;
            Position = pos;
            ModelRotation = rot;
            RotationRadians = rot.Y;
            Scale = scale;
            Speed = speed;
            Length = length;
            ModelName = modelName;
            EffectName = effect;
            TextureName = textureName;
            TextureAoName = textureAoName;
            TextureNormalName = textureNormalName;
            MovementSpeed = speed;
            RotationSpeed = 0.5f;
            BoatVelocity = 0.0f;
            BoatAcceleration = 0.5f;
            //BoatMatrix = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);
    }
        public void LoadContent()
        {
            ShipModel = Game.Content.Load<Model>(TGCGame.ContentFolder3D + ModelName);
            ShipEffect = Game.Content.Load<Effect>(TGCGame.ContentFolderEffects + EffectName);
            ShipTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + TextureName);
            ShipAoTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + TextureAoName);
            ShipNormalTexture = Game.Content.Load<Texture2D>(TGCGame.ContentFolderTextures + TextureNormalName);

            ShipEffect.Parameters["ambientColor"]?.SetValue(new Vector3(0.6f, 0.6f, 0.6f));
            //ShipEffect.Parameters["diffuseColor"]?.SetValue(new Vector3(0f, 0.25f, 0.48f));
            ShipEffect.Parameters["specularColor"]?.SetValue(new Vector3(0.98f, 0.98f, 0.98f));
            ShipEffect.Parameters["KAmbient"]?.SetValue(0.6f);
            ShipEffect.Parameters["KDiffuse"]?.SetValue(1f);
            ShipEffect.Parameters["KSpecular"]?.SetValue(.3f);
            ShipEffect.Parameters["shininess"]?.SetValue(5f);

            //BoatBox = new BoundingBox(Vector3.Transform(-Vector3.One * 0.5f, BoatMatrix), Vector3.Transform(Vector3.One * 0.5f, BoatMatrix));
            BoatBox = new BoundingSphere(Position, 50);

            DebugSphere = new SpherePrimitive(Game.GraphicsDevice, 10);
        }


        public void Draw(Camera cam)
        {
            ShipEffect.Parameters["baseTexture"]?.SetValue(ShipTexture);
            ShipEffect.Parameters["aoTexture"]?.SetValue(ShipAoTexture);
            ShipEffect.Parameters["normalTexture"]?.SetValue(ShipNormalTexture);
            DrawModel(ShipModel, BoatMatrix, ShipEffect, cam);
            DebugSphere.Draw(Matrix.Identity * Matrix.CreateTranslation(ProaPos), Game.CurrentCamera.View, Game.CurrentCamera.Projection);
            DebugSphere.Draw(Matrix.Identity * Matrix.CreateTranslation(PopaPos), Game.CurrentCamera.View, Game.CurrentCamera.Projection);
        }

        private void DrawModel(Model geometry, Matrix transform, Effect effect, Camera cam)
        {
            foreach (var mesh in geometry.Meshes)
            {
                effect.Parameters["World"]?.SetValue(transform);
                effect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(transform)));
                effect.Parameters["WorldViewProjection"]?.SetValue(transform * cam.View * cam.Projection);
                
                //effect.Parameters["View"].SetValue(cam.View);
                //effect.Parameters["Projection"].SetValue(cam.Projection);
                foreach (var meshPart in mesh.MeshParts)
                    meshPart.Effect = effect;
                mesh.Draw();
            }
        }
        public void Update(GameTime gameTime, Camera cam, Vector3 light)
        {
            // Esto es el tiempo que transcurre entre update y update (promedio 0.0166s)
            float elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            // Esto es el tiempo total transcurrido en el tiempo, siempre se incrementa
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            
            float InclinationRadians = GetBoatInclination();
            // ProaPos y PopaPos se settean en GetBoatInclination()
            float WavePosY = (ProaPos.Y + PopaPos.Y) / 2;
            //float WavePosY = GetWaterPositionY(time, Position.X, Position.Z);

            Vector3 InclinationAxis = Vector3.Cross(Vector3.Up, FrontDirection);
            Position = new Vector3(Position.X, WavePosY, Position.Z);
            BoatMatrix = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(ModelRotation.Y) * Matrix.CreateRotationY(RotationRadians) * Matrix.CreateFromAxisAngle(InclinationAxis, InclinationRadians) * Matrix.CreateTranslation(Position);
            
            FrontDirection = new Vector3((float)Math.Cos(-RotationRadians), 0.0f, (float)Math.Sin(-RotationRadians));
            //BoatBox = new BoundingBox(Vector3.Transform(-Vector3.One * 0.5f, BoatMatrix), Vector3.Transform(Vector3.One * 0.5f, BoatMatrix));
            BoatBox = new BoundingSphere(Position, 50);

            ShipEffect.Parameters["lightPosition"]?.SetValue(light);
            ShipEffect.Parameters["eyePosition"]?.SetValue(cam.Position);

            if (!bIsMoving)
                Decelerate(elapsedTime);

        }

        float frac(float val)
        {
            return val - MathF.Floor(val);
        }

        public Vector3 createWave(float time, float steepness, float numWaves, Vector2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, float xPos, float zPos)
        {
            Vector3 wave = new Vector3(0.0f, 0.0f, 0.0f);

            float spaceMult = (float)(2 * 3.14159265359 / waveLength);
            float timeMult = (float)(speed * 2 * 3.14159265359 / waveLength);

            Vector2 posXZ = new Vector2(xPos, zPos);
            wave.X = waveAmplitude * steepness * waveDir.X * MathF.Cos(Vector2.Dot(posXZ, waveDir) * spaceMult + time * timeMult);
            wave.Y = 2 * waveAmplitude * MathF.Pow((MathF.Sin(Vector2.Dot(posXZ, waveDir) * spaceMult + time * timeMult) + 1) / 2, peak);
            wave.Z = waveAmplitude * steepness * waveDir.Y * MathF.Cos(Vector2.Dot(posXZ, waveDir) * spaceMult + time * timeMult);
            return wave;
        }
        
        public float GetWaterPositionY(float time, float xPos, float zPos)
        {
            Vector3 worldPosition = Position;

            //createWave(float steepness, float numWaves, float2 waveDir, float waveAmplitude, float waveLength, float peak, float speed, float4 position) {

            Vector3 wave1 = createWave(time, 4, 5, new Vector2( 0.5f, 0.3f), 40, 160, 3, 10, xPos, zPos);
            Vector3 wave2 = createWave(time, 8, 5, new Vector2(0.8f, -0.4f), 12, 120, 1.2f, 20, xPos, zPos);
            Vector3 wave3 = createWave(time, 4, 5, new Vector2(0.3f, 0.2f), 2, 90, 5, 25, xPos, zPos);
            Vector3 wave4 = createWave(time, 2, 5, new Vector2(0.4f, 0.25f), 2, 60, 15, 15, xPos, zPos);
            Vector3 wave5 = createWave(time, 6, 5, new Vector2(0.1f, 0.8f), 20, 250, 2, 40, xPos, zPos);

            Vector3 wave6 = createWave(time, 4, 5, new Vector2(-0.5f, -0.3f), 0.5f, 8, 0.2f, 4, xPos, zPos);
            Vector3 wave7 = createWave(time, 8, 5, new Vector2(-0.8f, 0.4f), 0.3f, 5, 0.3f, 6, xPos, zPos);

            worldPosition = (wave1 + wave2 + wave3 + wave4 + wave5 + wave6  + wave7 ) / 6;
            return (float)worldPosition.Y - 2;
        }

        private float GetBoatInclination()
        {
            float xPosProa = Position.X + FrontDirection.X * Length / 2;
            float zPosProa = Position.Z + FrontDirection.Z * Length / 2;
            float xPosPopa = Position.X - FrontDirection.X * Length / 2;
            float zPosPopa = Position.Z - FrontDirection.Z * Length / 2;
            float WavePosYProa = GetWaterPositionY(time, xPosProa, zPosProa);
            float WavePosYPopa = GetWaterPositionY(time, xPosPopa, zPosPopa);
            ProaPos = new Vector3(xPosProa, WavePosYProa, zPosProa);
            PopaPos = new Vector3(xPosPopa, WavePosYPopa, zPosPopa);

            Vector3 Inclination = new Vector3(xPosProa, WavePosYProa, zPosProa) - new Vector3(xPosPopa, WavePosYPopa, zPosPopa);
            Inclination.Normalize();
            // Angulo entre los vectores rotacion y inclinacion
            double DotProductInclinationRotation = (double)Vector3.Dot(Inclination, FrontDirection);
            float InclinationRadians = (float)Math.Acos(DotProductInclinationRotation);
            if (WavePosYProa > WavePosYPopa) InclinationRadians *= -1;
            return InclinationRadians;
        }

        public void Move(float amount)
        {
            bIsMoving = true;
            BoundingSphere FuturePosition = new BoundingSphere(Position + FrontDirection * amount, 50);
            bool willCollide = false;
            for (var index = 0; index < OtherShips.Length && !willCollide; index++)
            {
                if (FuturePosition.Intersects(OtherShips[index].BoatBox))
                {
                    willCollide = true;
                }
            }

            for (var index = 0; index < Game.IslandColliders.Length && !willCollide; index++)
            {
                if (FuturePosition.Intersects(Game.IslandColliders[index]))
                {
                    willCollide = true;
                    BoatVelocity = 0.0f;
                }
            }

            if (!willCollide)
                Position += FrontDirection * amount;
        }
        public void MoveForward(float amount)
        {
            BoatVelocity = Math.Clamp(BoatVelocity + BoatAcceleration, -MovementSpeed, MovementSpeed);
            Move(BoatVelocity * amount);
        }
        public void MoveBackwards(float amount)
        {
            BoatVelocity = Math.Clamp(BoatVelocity - BoatAcceleration, -MovementSpeed, MovementSpeed);
            Move(BoatVelocity * amount);
        }
        public void RotateRight(float amount)
        {
            RotationRadians += RotationSpeed * amount;
        }
        public void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
        private void Decelerate(float amount)
        {
            if (BoatVelocity > 0)
                BoatVelocity = Math.Clamp(BoatVelocity - BoatAcceleration, 0.0f, MovementSpeed);

            if (BoatVelocity < 0)
                BoatVelocity = Math.Clamp(BoatVelocity + BoatAcceleration, -MovementSpeed, 0.0f);
            Move(amount * BoatVelocity);
        }
        public void AddShips()
        {
            // Agrego el resto de las ships menos esta
            OtherShips = new Ship[Game.Ships.Length - 1];
            int i = 0;
            foreach (Ship ship in Game.Ships)
            {
                if (ship != this)
                {
                    OtherShips[i] = ship;
                    i++;
                }
            }
        }
    }
}
