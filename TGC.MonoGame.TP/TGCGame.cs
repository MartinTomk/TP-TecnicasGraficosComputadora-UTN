﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Skydome;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Ships;


namespace TGC.MonoGame.TP
{
    /// <summary>
    ///     Esta es la clase principal  del juego.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar la clase que ejecuta Program <see cref="Program.Main()" /> linea 10.
    /// </summary>
    public class TGCGame : Game
    {
        private float time;

        public const string ContentFolder3D = "Models/";
        public const string ContentFolderEffects = "Effects/";
        public const string ContentFolderMusic = "Music/";
        public const string ContentFolderSounds = "Sounds/";
        public const string ContentFolderSpriteFonts = "SpriteFonts/";
        public const string ContentFolderTextures = "Textures/";

        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        public TGCGame()
        {
            // Maneja la configuracion y la administracion del dispositivo grafico.
            Graphics = new GraphicsDeviceManager(this);
            // Descomentar para que el juego sea pantalla completa.
            // Graphics.IsFullScreen = true;
            // Carpeta raiz donde va a estar toda la Media.
            Content.RootDirectory = "Content";
            // Hace que el mouse sea visible.
            IsMouseVisible = true;
        }

        /// <summary>
        /// Isla
        /// </summary>
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }
        private Model ModelIsland { get; set; }
        private Model ModelIsland2 { get; set; }
        private Model ModelIsland3 { get; set; }
        private Model ModelCasa { get; set; }
        private Effect IslandEffect { get; set; }
        private Model ModelWater { get; set; }
        private Effect WaterEffect { get; set; }
        private Model ModelPalm1 { get; set; }
        private Model ModelPalm2 { get; set; }
        private Model ModelPalm3 { get; set; }
        private Model ModelPalm4 { get; set; }
        private Model ModelPalm5 { get; set; }
        private Model ModelRock1 { get; set; }
        private Model ModelRock2 { get; set; }
        private Model ModelRock3 { get; set; }
        //private Model ModelRock4 { get; set; }
        private Model ModelRock5 { get; set; }
        private Effect IslandMiscEffect { get; set; }
        public Texture2D IslandTexture;
        public Texture2D IslandMiscTexture;
        public Texture2D WaterTexture;

        Matrix MatrixIsland1;
        Matrix MatrixIsland2;
        Matrix MatrixIsland3;
        Matrix MatrixIsland4;
        Matrix MatrixIsland5;
        Matrix MatrixCasa;
        Matrix MatrixRock1;
        Matrix MatrixRock2;
        Matrix MatrixRock3;
        Matrix MatrixRock4;
        Matrix MatrixRock5;
        Matrix MatrixRock6;
        Matrix MatrixRock7;


        /// <summary>
        /// Barcos
        /// </summary>
        private Ship SM { get; set; }
        private Ship Patrol { get; set; }
        private Ship Cruiser { get; set; }
        private Ship Barquito { get; set; }
        private Ship PlayerBoat { get; set; }

        private Ship PlayerControlledShip { get; set; }

        /// <summary>
        /// Camara
        /// </summary>
        private BoatCamera Camera { get; set; }
        private Matrix World { get; set; }
        private Matrix View { get; set; }
        private Matrix Projection { get; set; }

        private float CameraArm;
        
        public Camera shotCam;
        public Camera CurrentCamera => shotCam;
      
        /// <summary>
        /// Skydome
        /// </summary>
        private SkyDome Skydome { get; set; }
        private Model SkyDomeModel { get; set; }
        private Effect SkyDomeEffect { get; set; }
        public Texture2D SkyDomeTexture;

        private Ship[] Ships;
        float IslandScaleCollisionTest;
        BoundingSphere IslandSphere;

        private BoundingSphere[] IslandColliders;

        

        BoundingBox TestBox;

        // Iluminacion
        private Effect LightEffect{ get; set; }
        private Matrix LightBoxWorld { get; set; } = Matrix.Identity;
        private Matrix LightBoxWorld2 { get; set; } = Matrix.Identity;

        private CubePrimitive lightBox;
        private CubePrimitive lightBox2;
        private float Timer { get; set; }


        public SpriteBatch spriteBatch;
        public SpriteFont font;

        // pal debuggin
        //SpriteBatch spriteBatch;
        //SpriteFont font;

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            // La logica de inicializacion que no depende del contenido se recomienda poner en este metodo.

            // Apago el backface culling.
            // Esto se hace por un problema en el diseno del modelo del logo de la materia.
            // Una vez que empiecen su juego, esto no es mas necesario y lo pueden sacar.
            //var rasterizerState = new RasterizerState();
            //rasterizerState.CullMode = CullMode.None;
            //GraphicsDevice.RasterizerState = rasterizerState;
            // Seria hasta aca.

            // Configuramos nuestras matrices de la escena.
            World = Matrix.Identity;
            View = Matrix.CreateLookAt(Vector3.UnitZ * 150, Vector3.Zero, Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1, 50);
            var screenSize = new Point(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);

            CameraArm = 30.0f;
            shotCam = new BoatCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, CameraArm, 600), screenSize);

            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);

            IslandScaleCollisionTest = 0.2f;
            IslandSphere = new BoundingSphere(Vector3.Zero, 400);

            base.Initialize();
            //Colliders.
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo, despues de Initialize.
        ///     Escribir aqui el codigo de inicializacion: cargar modelos, texturas, estructuras de optimizacion, el procesamiento
        ///     que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void LoadContent()
        {
            // Aca es donde deberiamos cargar todos los contenido necesarios antes de iniciar el juego.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Cargo el modelos /// ISLA ///
            ModelIsland = Content.Load<Model>(ContentFolder3D + "Island/IslaGeo");
            ModelIsland2 = Content.Load<Model>(ContentFolder3D + "Island/Isla2Geo");
            ModelIsland3 = Content.Load<Model>(ContentFolder3D + "Island/Isla3Geo");
            IslandEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland02Diffuse");

            IslandMiscEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandMiscTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland01Diffuse");

            ModelWater = Content.Load<Model>(ContentFolder3D + "Island/waterAltaGeo");
            WaterEffect = Content.Load<Effect>(ContentFolderEffects + "WaterShader");
            WaterTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TexturesCom_WaterPlain0012_1_seamless_S");
            WaterEffect.Parameters["ambientColor"]?.SetValue(new Vector3(0f, 0.1f, 0.27f));
            WaterEffect.Parameters["diffuseColor"]?.SetValue(new Vector3(0f, 0.25f, 0.48f));
            WaterEffect.Parameters["specularColor"]?.SetValue(new Vector3(0.95f, 0.95f, 0.95f));
            WaterEffect.Parameters["KAmbient"]?.SetValue(0.4f);
            WaterEffect.Parameters["KDiffuse"]?.SetValue(1f);
            WaterEffect.Parameters["KSpecular"]?.SetValue(0.08f);
            WaterEffect.Parameters["shininess"]?.SetValue(1f);


            ModelCasa = Content.Load<Model>(ContentFolder3D + "Island/CasaGeo");

            ModelPalm1 = Content.Load<Model>(ContentFolder3D + "Island/Palmera1Geo");
            ModelPalm2 = Content.Load<Model>(ContentFolder3D + "Island/Palmera2Geo");
            ModelPalm3 = Content.Load<Model>(ContentFolder3D + "Island/Palmera3Geo");
            ModelPalm4 = Content.Load<Model>(ContentFolder3D + "Island/Palmera4Geo");
            ModelPalm5 = Content.Load<Model>(ContentFolder3D + "Island/Palmera5Geo");

            ModelRock1 = Content.Load<Model>(ContentFolder3D + "Island/Roca1Geo");
            ModelRock2 = Content.Load<Model>(ContentFolder3D + "Island/Roca2Geo");
            ModelRock3 = Content.Load<Model>(ContentFolder3D + "Island/Roca3Geo");
            //ModelRock4 = Content.Load<Model>(ContentFolder3D + "Island/Roca4Geo");
            ModelRock5 = Content.Load<Model>(ContentFolder3D + "Island/Roca5Geo");

            MatrixIsland1 = Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(300, 0, 800f);
            MatrixIsland2 = Matrix.CreateScale(0.2f);
            MatrixIsland3 = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(1.54f) * Matrix.CreateTranslation(800, 0, -300);
            MatrixIsland4 = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(2.5f) * Matrix.CreateTranslation(800, -2, 600);
            MatrixIsland5 = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-650, -2, -00);

            MatrixCasa = Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(780, 56, 620);

            MatrixRock1 = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(350, -10, 350);
            MatrixRock2 = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-350, -10, 350);
            MatrixRock3 = Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(0.8f) * Matrix.CreateTranslation(-350, -10, -680);
            MatrixRock4 = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(3f) * Matrix.CreateTranslation(850, -10, 50);
            MatrixRock5 = Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(100, -10, -780);
            MatrixRock6 = Matrix.CreateScale(0.18f) * Matrix.CreateRotationY(2.5f) * Matrix.CreateTranslation(530, -10, 780);
            MatrixRock7 = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(1050, -10, 300);


            //// BOTES ////

            SM = new Ship(this, new Vector3(-100f, 0.01f, 400f), new Vector3(0f, MathHelper.PiOver2, 0f), new Vector3(0.04f, 0.04f, 0.04f), 0.5f, "Botes/SMGeo", "BasicShader", "Botes/SM_T_Boat_M_Boat_BaseColor");
            SM.LoadContent();

            Patrol = new Ship(this, new Vector3(-300f, 0.01f, 500f), new Vector3(0f, MathHelper.PiOver2, 0f), new Vector3(0.07f, 0.07f, 0.07f), 0.5f, "Botes/PatrolGeo", "BasicShader", "Botes/T_Patrol_Ship_1K_BaseColor");
            Patrol.LoadContent();

            Cruiser = new Ship(this, new Vector3(-100f, 0.01f, 900f), new Vector3(0f, MathHelper.PiOver2, 0f), new Vector3(0.03f, 0.03f, 0.03f), 0.5f, "Botes/CruiserGeo", "BasicShader", "Botes/T_Cruiser_M_Cruiser_BaseColor");
            Cruiser.LoadContent();

            Barquito = new Ship(this, new Vector3(-200f, 0.01f, 700f), new Vector3(0f, MathHelper.PiOver2, 0f), new Vector3(0.05f, 0.05f, 0.05f), 0.5f, "Botes/BarquitoGeo", "BasicShader", "Botes/Barquito_BaseColor");
            Barquito.LoadContent();

            PlayerBoat = new Ship(this, new Vector3(0f, 0.01f, 600f), new Vector3(0f, 0f, 0f), new Vector3(0.1f, 0.1f, 0.1f), 0.5f, "ShipB/Source/Ship", "BasicShader", "Botes/Battleship_lambert1_AlbedoTransparency.tga");
            PlayerBoat.playerMode = true;
            PlayerBoat.LoadContent();

            PlayerControlledShip = PlayerBoat;

            Ships = new Ship[]
            {
                SM, Patrol, Cruiser, Barquito
            };

            float radius = 50f;
            IslandColliders = new BoundingSphere[]
            {
                new BoundingSphere(MatrixIsland1.Translation, 100), new BoundingSphere(MatrixIsland2.Translation, 300), new BoundingSphere(MatrixIsland3.Translation, radius),
                new BoundingSphere(MatrixIsland4.Translation, radius), new BoundingSphere(MatrixIsland5.Translation, radius), 
                new BoundingSphere(MatrixCasa.Translation, radius), 
                new BoundingSphere(MatrixRock1.Translation, radius), new BoundingSphere(MatrixRock2.Translation, radius), new BoundingSphere(MatrixRock3.Translation, radius), 
                new BoundingSphere(MatrixRock4.Translation, radius), new BoundingSphere(MatrixRock5.Translation, radius), new BoundingSphere(MatrixRock6.Translation, radius), 
                new BoundingSphere(MatrixRock7.Translation, radius), 
            };


            SkyDomeModel = Content.Load<Model>(ContentFolder3D + "Skydome/SkyDome");
            SkyDomeTexture = Content.Load<Texture2D>(ContentFolder3D + "Skydome/Sky");
            SkyDomeEffect = Content.Load<Effect>(ContentFolderEffects + "SkyDome");
            Skydome = new SkyDome(SkyDomeModel, SkyDomeTexture, SkyDomeEffect, 200);

            //Valores de Iluminacion 
            LightEffect = Content.Load<Effect>(ContentFolderEffects + "BlinnPhong"); 

            LightEffect.Parameters["ambientColor"].SetValue(new Vector3(1f, 1f, 0.0f));
            LightEffect.Parameters["diffuseColor"].SetValue(new Vector3(1f, 1f, 0.0f));
            LightEffect.Parameters["specularColor"].SetValue(new Vector3(1f, 1f, 1f));
            LightEffect.Parameters["KAmbient"].SetValue(0.6f);
            LightEffect.Parameters["KDiffuse"].SetValue(1f);
            LightEffect.Parameters["KSpecular"].SetValue(0.08f);
            LightEffect.Parameters["shininess"].SetValue(2.0f);


            lightBox = new CubePrimitive(GraphicsDevice, 70, Color.Yellow);
            lightBox2 = new CubePrimitive(GraphicsDevice, 25, Color.White);

            font = Content.Load<SpriteFont>("Fonts/Font");

            base.LoadContent();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la logica de computo del modelo, asi como tambien verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        protected override void Update(GameTime gameTime)
        {
            var elapsedTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            ProcessKeyboard(elapsedTime);

            // Aca deberiamos poner toda la logica de actualizacion del juego.
            SM.Update(gameTime);
            Patrol.Update(gameTime);
            Cruiser.Update(gameTime);
            Barquito.Update(gameTime);
            PlayerBoat.Update(gameTime);
            shotCam.Update(gameTime);
            shotCam.Position = PlayerBoat.Position + new Vector3(0, CameraArm, 0);


            //Iluminacion 
            var posicionY = (float)MathF.Cos(Timer / 5) * 1500f;
            var posicionZ = (float)MathF.Sin(Timer / 5) * 1500f;
            var lightPosition = new Vector3(1000f, posicionY, posicionZ);
            Timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var lightPosition2 = new Vector3(1000f, posicionY * -1f, posicionZ * -1);
            LightBoxWorld = Matrix.CreateTranslation(lightPosition);
            LightBoxWorld2 = Matrix.CreateTranslation(lightPosition2);
            LightEffect.Parameters["lightPosition"].SetValue(lightPosition);
            LightEffect.Parameters["eyePosition"].SetValue(shotCam.Position);

            WaterEffect.Parameters["lightPosition"]?.SetValue(lightPosition);
            WaterEffect.Parameters["eyePosition"]?.SetValue(shotCam.Position);

            base.Update(gameTime);

        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            // Aca deberiamos poner toda la logia de renderizado del juego.
            GraphicsDevice.Clear(Color.White);

            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            IslandEffect.Parameters["ModelTexture"].SetValue(IslandTexture);
            LightEffect.Parameters["baseTexture"].SetValue(IslandTexture);


            DrawModelLight(ModelIsland, MatrixIsland1, LightEffect);
            DrawModelLight(ModelIsland, MatrixIsland2, LightEffect);
            DrawModelLight(ModelIsland, MatrixIsland3, LightEffect);
            DrawModelLight(ModelCasa, MatrixCasa, LightEffect);
            
            DrawModelLight(ModelRock1, MatrixRock1, LightEffect);
            DrawModelLight(ModelRock2, MatrixRock2, LightEffect);
            DrawModelLight(ModelRock2, MatrixRock3, LightEffect);
            DrawModelLight(ModelRock3, MatrixRock4, LightEffect);
            DrawModelLight(ModelRock5, MatrixRock5, LightEffect);
            DrawModelLight(ModelRock2, MatrixRock6, LightEffect);
            DrawModelLight(ModelRock1, MatrixRock7, LightEffect);
            LightEffect.Parameters["baseTexture"].SetValue(IslandMiscTexture);
            DrawModelLight(ModelIsland2, MatrixIsland4, LightEffect);
            DrawModelLight(ModelIsland3, MatrixIsland5, LightEffect);
            

            DrawModelLight(ModelPalm1, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(60, 10, 280), LightEffect);
            DrawModelLight(ModelPalm2, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(110, 0, 300), LightEffect);
            DrawModelLight(ModelPalm3, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(-50, 48, 150), LightEffect);
            DrawModelLight(ModelPalm4, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(750, 0, -60), LightEffect);
            DrawModelLight(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(580, 0, -150), LightEffect);
            DrawModelLight(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(-650, 30, -100), LightEffect);

            WaterEffect.Parameters["baseTexture"]?.SetValue(WaterTexture);
            WaterEffect.Parameters["Time"]?.SetValue(time);
            for (int i = -8; i < 8; i++)
                for (int j = -8; j < 8; j++)
                {
                    Matrix MatrixWater = Matrix.Identity * Matrix.CreateScale(10f, 0f, 10f) * Matrix.CreateTranslation(i * 200, 0, j * 200);
                    WaterEffect.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(MatrixWater)));
                    DrawModel(ModelWater, MatrixWater, WaterEffect);
                }
            //DrawModel(ModelWater, Matrix.CreateScale(10f, 0f, 10f), WaterEffect);

            /// Dibujo Botes

            SM.Draw();
            Patrol.Draw();
            Cruiser.Draw();
            Barquito.Draw();
            PlayerBoat.Draw();
            
            //Iluminacion
            lightBox.Draw(LightBoxWorld, shotCam.View, shotCam.Projection);
            lightBox2.Draw(LightBoxWorld2, shotCam.View, shotCam.Projection);
            //DrawModel(PlayerBoatModel, Matrix.CreateRotationY((float)PlayerRotation)* PlayerBoatMatrix  , PlayerBoatEffect);

            /// Skydome
            Skydome.Draw(shotCam.View, shotCam.Projection, shotCam.Position);
            SkyDomeEffect.Parameters["Time"].SetValue(time);

            base.Draw(gameTime);
        }

        private void DrawModel(Model geometry, Matrix transform, Effect effect)
        {
            foreach (var mesh in geometry.Meshes)
            {
                effect.Parameters["World"].SetValue(transform);
                effect.Parameters["View"].SetValue(shotCam.View);
                effect.Parameters["Projection"].SetValue(shotCam.Projection);
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = effect;

                }
                mesh.Draw();
            }
        }
        private void DrawModelLight(Model geometry, Matrix transform, Effect light)
        {
            var modelMeshesBaseTransforms = new Matrix[geometry.Bones.Count];
            geometry.CopyAbsoluteBoneTransformsTo(modelMeshesBaseTransforms);
            foreach (var modelMesh in geometry.Meshes)
            {
                // We set the main matrices for each mesh to draw
                // World is used to transform from model space to world space
                light.Parameters["World"].SetValue(transform);
                // InverseTransposeWorld is used to rotate normals
                light.Parameters["InverseTransposeWorld"].SetValue(Matrix.Transpose(Matrix.Invert(transform)));
                // WorldViewProjection is used to transform from model space to clip space
                light.Parameters["WorldViewProjection"].SetValue(transform * shotCam.View * shotCam.Projection);
                    foreach (var meshPart in modelMesh.MeshParts) {
                        meshPart.Effect = light;
                }
                // Once we set these matrices we draw
                modelMesh.Draw();
            }
        }

        public void DrawCenterTextY(string msg, float Y, float escala)
        {
            var W = GraphicsDevice.Viewport.Width;
            var H = GraphicsDevice.Viewport.Height;
            var size = font.MeasureString(msg) * escala;
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                Matrix.CreateScale(escala) * Matrix.CreateTranslation((W - size.X) / 2, Y, 0));
            spriteBatch.DrawString(font, msg, new Vector2(0, 0), Color.YellowGreen);
            spriteBatch.End();
        }

        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Content.Unload();

            base.UnloadContent();
        }

        private void ProcessKeyboard(float elapsedTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            //var currentMovementSpeed = MovementSpeed;

            if (keyboardState.IsKeyDown(Keys.W))
            {
                MoveForward(PlayerControlledShip.MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                MoveBackwards(PlayerControlledShip.MovementSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                RotateRight(PlayerControlledShip.RotationSpeed * elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                RotateLeft(PlayerControlledShip.RotationSpeed * elapsedTime);
            }


        }

        private void MoveForward(float amount)
        {
            BoundingSphere FuturePosition = new BoundingSphere(PlayerControlledShip.Position + PlayerControlledShip.FrontDirection * amount, 50);
            bool willCollide = false;
            for (var index = 0; index < Ships.Length && !willCollide; index++)
            {
                if (FuturePosition.Intersects(Ships[index].BoatBox))
                {
                    willCollide = true;
                }
            }

            for (var index = 0; index < IslandColliders.Length && !willCollide; index++)
            {
                if (FuturePosition.Intersects(IslandColliders[index]))
                {
                    willCollide = true;
                }
            }

            if (!willCollide)
                PlayerControlledShip.Position += PlayerControlledShip.FrontDirection * amount;
        }
        private void MoveBackwards(float amount)
        {
            MoveForward(-amount);
        }
        private void RotateRight(float amount)
        {
            PlayerControlledShip.Rotation = new Vector3(PlayerControlledShip.Rotation.X, PlayerControlledShip.Rotation.Y + amount, PlayerControlledShip.Rotation.Z);
            PlayerControlledShip.RotationRadians += amount;
        }
        private void RotateLeft(float amount)
        {
            RotateRight(-amount);
        }
    }
}