using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using TGC.MonoGame.TP.Cameras;
using TGC.MonoGame.TP.Skydome;
using TGC.MonoGame.TP.Geometries;
using TGC.MonoGame.TP.Ships;
using TGC.MonoGame.TP.UI;
using TGC.MonoGame.TP.Gizmos;
using System.Collections.Generic;

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

            Gizmos = new Gizmos.Gizmos();
        }
        public Gizmos.Gizmos Gizmos { get; }
        public SpherePrimitive DebugSphere;
        /// <summary>
        /// Isla
        /// </summary>
        private GraphicsDeviceManager Graphics { get; }
        private SpriteBatch SpriteBatch { get; set; }

        /// Isla 1 ///
        private Model ModelIsland { get; set; }
        private Effect VolcanEffect1 { get; set; }
        public Texture2D volcanTexture;
        public Texture2D volcanNormalTexture;


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


        private Model ModelPiso { get; set; }
        private Effect PisoEffect { get; set; }
        public Texture2D PisoTexture;


        public Texture2D IslandTexture;
        public Texture2D IslandMiscTexture;

        public Texture2D WaterTexture;
        public Texture2D WaterFoamTexture;
        public Texture2D WaterNormalTexture;

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

        Matrix MatrixPiso;



        /// <summary>
        /// Barcos
        /// </summary>
        public Ship SM { get; set; }
        public Ship Patrol { get; set; }
        public Ship Cruiser { get; set; }
        public Ship Barquito { get; set; }
        public Ship PlayerBoat { get; set; }

        public Ship PlayerControlledShip { get; set; }

        /// <summary>
        /// Camara
        /// </summary>
        private BoatCamera Camera { get; set; }
        public Matrix World { get; set; }
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

        public Ship[] Ships { get; set; }

        BoundingSphere IslandSphere;


        public BoundingSphere[] IslandColliders;
        public BoundingBox[] WaterColliders;

        //BoundingBox TestBox;

        public bool godModeEnabled = false;

        //BoundingBox TestBox;
        public int TiempoEntreDisparos = 0;
        // Iluminacion
        private Effect LightEffect { get; set; }
        private Matrix LightBoxWorld { get; set; } = Matrix.Identity;
        private Matrix LightBoxWorld2 { get; set; } = Matrix.Identity;

        private CubePrimitive lightBox;
        private CubePrimitive lightBox2;
        private float Timer { get; set; }

        public Model BulletModel;
        public List<Bullet.Bullet> BulletsToDelete;
        public List<Bullet.Bullet> Bullets;
        public List<Bullet.Bullet> PoolBullets;
        private int amountBullets = 20;
        public int availableBullets;
        private GameUI _ui;
        private MouseState lastMouseState = new MouseState();
        public Vector3 Orientacion { get; set; }

        public SpriteBatch spriteBatch;
        public SpriteFont font;
        public KeyboardState lastState;

        // OVERLAY GOTAS //
        private Texture2D dropsTexture { get; set; }
        private Texture2D dropsTexture2 { get; set; }
        private Effect dropsEffect { get; set; }
        private Effect nightVisionEffect { get; set; }
        private FullScreenQuad FullScreenQuad { get; set; }
        private RenderTarget2D SceneRenderTarget { get; set; }
        private RenderTarget2D ScreenRenderTarget { get; set; }

        // ENV MAP //
        private const int EnvironmentmapSize = 2048;
        private StaticCamera CubeMapCamera { get; set; }
        private RenderTargetCube EnvironmentMapRenderTarget { get; set; }
        private Vector3 refPosition { get; }
        //private Effect WaterEnvEffect { get; set; }
        //Sonido 
        private SoundEffect Waves { get; set; }
        private SoundEffect ShipShoot { get; set; }
        private SoundEffect InvencibleSound { get; set; }
        public SoundEffect Explosion { get; set; }
        private SoundEffectInstance Instance { get; set; }
        private SoundEffectInstance ShootInstance { get; set; }
        public SoundEffectInstance ExplosionInstance { get; set; }

        public SoundEffectInstance InvencibleInstance { get; set; }

        public BoundingFrustum boundingFrustum = new BoundingFrustum(Matrix.Identity);


        // pal debuggin
        //SpriteBatch spriteBatch;
        //SpriteFont font;



        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aqui el codigo de inicializacion: el procesamiento que podemos pre calcular para nuestro juego.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "_.~\"~._.~\"~.__.~\"~._.~\"~._.~\"~._.~\"~.__.~\"~._.~\"~._.~\"~._.~\"~.__.~\"(_.~\"(_.~\"(_.~\"(_.~\"(    11 ANCLAS 11" +
                            "    _.~\"(_.~\"(_.~\"(_.~\"(_.~\"(_.~\"~._.~\"~._.~\"~._.~\"~.__.~\"~._.~\"~._.~\"~._.~\"~._";
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

            CameraArm = 40.0f;
            shotCam = new BoatCamera(GraphicsDevice.Viewport.AspectRatio, new Vector3(0, CameraArm, 440), screenSize);

            Window.AllowUserResizing = true;

            //Graphics.IsFullScreen = true;
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(Graphics.GraphicsDevice);

            IslandSphere = new BoundingSphere(Vector3.Zero, 400);

            CubeMapCamera = new StaticCamera(1f, refPosition, Vector3.UnitX, Vector3.Up);
            CubeMapCamera.BuildProjection(1f, 1f, 3000f, MathHelper.PiOver2);

            PoolBullets = new List<Bullet.Bullet>();
            for (int i = 0; i < amountBullets; i++)
            {
                Bullet.Bullet bullet = new Bullet.Bullet();
                PoolBullets.Add(bullet);
            }
            availableBullets = amountBullets;

            _ui = new GameUI(this);

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


            //TODO: use this.Content to load your game content here

            //Gizmos.LoadContent(GraphicsDevice, this.Content);
            Gizmos.LoadContent(GraphicsDevice, Content);
            //DebugSphere = new SpherePrimitive(GraphicsDevice, 1);

            // Cargo el modelos /// ISLA ///
            ModelIsland = Content.Load<Model>(ContentFolder3D + "Island/isla_volcan1");
            VolcanEffect1 = Content.Load<Effect>(ContentFolderEffects + "islasShader");
            volcanTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/Isla1/isla_geo_DefaultMaterial_Diffuse");
            volcanNormalTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/Isla1/isla_geo_DefaultMaterial_Normal");
            VolcanEffect1.Parameters["ambientColor"].SetValue(new Vector3(.2f, 1f, 0.2f));
            //VolcanEffect1.Parameters["diffuseColor"].SetValue(new Vector3(1f, 1f, 0.0f));
            VolcanEffect1.Parameters["specularColor"].SetValue(new Vector3(1f, 1f, 1f));
            VolcanEffect1.Parameters["KAmbient"].SetValue(0.3f);
            VolcanEffect1.Parameters["KDiffuse"].SetValue(1f);
            VolcanEffect1.Parameters["KSpecular"].SetValue(0.2f);
            VolcanEffect1.Parameters["shininess"].SetValue(5.0f);

            IslandEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland02Diffuse");
            ModelIsland2 = Content.Load<Model>(ContentFolder3D + "Island/Isla2Geo");
            ModelIsland3 = Content.Load<Model>(ContentFolder3D + "Island/Isla3Geo");

            IslandMiscEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            IslandMiscTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TropicalIsland01Diffuse");

            PisoEffect = Content.Load<Effect>(ContentFolderEffects + "BasicShader");
            PisoTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TexturesCom_SandPebbles0067_1_seamless_S");
            ModelPiso = Content.Load<Model>(ContentFolder3D + "Island/plano_baja");
            
            ModelWater = Content.Load<Model>(ContentFolder3D + "Island/waterAltaGeo");
            WaterEffect = Content.Load<Effect>(ContentFolderEffects + "WaterShader");
            WaterTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/TexturesCom_WaterPlain0012_1_seamless_S");
            WaterFoamTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/53_sea water foam texture-seamless");
            WaterNormalTexture = Content.Load<Texture2D>(ContentFolderTextures + "Island/normalAgua");
            WaterEffect.Parameters["ambientColor"]?.SetValue(new Vector3(0f, 0.17f, 0.37f));
            WaterEffect.Parameters["diffuseColor"]?.SetValue(new Vector3(0f, 0.25f, 0.48f));
            WaterEffect.Parameters["specularColor"]?.SetValue(new Vector3(0.95f, 0.95f, 0.95f));
            WaterEffect.Parameters["KAmbient"]?.SetValue(0.5f);
            WaterEffect.Parameters["KFoam"]?.SetValue(0.15f);
            WaterEffect.Parameters["KDiffuse"]?.SetValue(0.8f);
            WaterEffect.Parameters["KSpecular"]?.SetValue(0.42f);
            WaterEffect.Parameters["shininess"]?.SetValue(15f);
            WaterEffect.Parameters["KReflection"]?.SetValue(0.52f);

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

            MatrixIsland1 = Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(1300, 0, 2800f);
            MatrixIsland2 = Matrix.CreateScale(0.2f);
            MatrixIsland3 = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(1.54f) * Matrix.CreateTranslation(2000, 0, -1000);
            MatrixIsland5 = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-1650, -2, -300);

            MatrixIsland4 = Matrix.CreateScale(0.1f) * Matrix.CreateRotationY(2.5f) * Matrix.CreateTranslation(800, -2, 600);
            MatrixCasa = Matrix.CreateScale(0.07f) * Matrix.CreateTranslation(780, 56, 620);


            MatrixRock1 = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(2350, -10, 3350);
            MatrixRock2 = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(-1350, -10, 2350);
            MatrixRock3 = Matrix.CreateScale(0.5f) * Matrix.CreateRotationY(0.8f) * Matrix.CreateTranslation(-1350, -10, -2680);
            MatrixRock4 = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(3f) * Matrix.CreateTranslation(850, -10, 50);
            MatrixRock5 = Matrix.CreateScale(0.2f) * Matrix.CreateTranslation(100, -10, -780);
            MatrixRock6 = Matrix.CreateScale(0.18f) * Matrix.CreateRotationY(2.5f) * Matrix.CreateTranslation(530, -10, 780);
            MatrixRock7 = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(1050, -10, 300);

            MatrixPiso = Matrix.CreateScale(100f) * Matrix.CreateTranslation(0, -200, 0);



            //// BOTES ////

            SM = new Ship(this, new Vector3(-1000f, 0.01f, 400f), new Vector3(0f, 0f, 0f), new Vector3(0.04f, 0.04f, 0.04f), 50.0f, 30.0f, "Botes/SMGeo", "ShipsShader", "Botes/SM_T_Boat_M_Boat_BaseColor", "Botes/SM_T_Boat_M_Boat_OcclusionRoughnessMetallic", "Botes/SM_T_Boat_M_Boat_Normal");
            SM.LoadContent();

            Patrol = new Ship(this, new Vector3(-1600f, 0.01f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0.07f, 0.07f, 0.07f), 50.0f, 350.0f, "Botes/PatrolGeo", "ShipsShader", "Botes/T_Patrol_Ship_1K_BaseColor", "Botes/T_Patrol_Ship_1K_OcclusionRoughnessMetallic", "Botes/T_Patrol_Ship_1K_Normal");
            Patrol.LoadContent();

            Cruiser = new Ship(this, new Vector3(-1000f, 0.01f, -100f), new Vector3(0f, 0.0f, 0f), new Vector3(0.03f, 0.03f, 0.03f), 50.0f, 350.0f, "Botes/CruiserGeo", "ShipsShader", "Botes/T_Cruiser_M_Cruiser_BaseColor", "Botes/T_Cruiser_M_Cruiser_OcclusionRoughnessMetallic", "Botes/T_Cruiser_M_Cruiser_Normal");
            Cruiser.LoadContent();

            Barquito = new Ship(this, new Vector3(-200f, 0.01f, 200f), new Vector3(0f, 0f, 0f), new Vector3(0.05f, 0.05f, 0.05f), 50.0f, 20.0f, "Botes/BarquitoGeo", "ShipsShader", "Botes/Barquito_BaseColor", "Botes/blanco", "Island/normalAgua");
            Barquito.LoadContent();


            PlayerBoat = new Ship(this, new Vector3(-1000f, 0.01f, 1000f), new Vector3(0f, 0f, 0f), new Vector3(0.02f, 0.02f, 0.02f), 100.0f, 350.0f, "Botes/CruiserGeo", "ShipsShader", "Botes/T_Cruiser_M_Cruiser_BaseColor", "Botes/T_Cruiser_M_Cruiser_OcclusionRoughnessMetallic", "Botes/T_Cruiser_M_Cruiser_Normal");
            //PlayerBoat = new Ship(this, new Vector3(0f, 0.01f, 600f), new Vector3(0f, MathHelper.PiOver2, 0f), new Vector3(0.07f, 0.07f, 0.07f), 100.0f, 350.0f, "Botes/PatrolGeo", "ShipsShader", "Botes/T_Patrol_Ship_1K_BaseColor", "Botes/T_Patrol_Ship_1K_OcclusionRoughnessMetallic", "Botes/T_Patrol_Ship_1K_Normal");
            //PlayerBoat = new Ship(this, new Vector3(0f, 0.01f, 600f), new Vector3(0f, MathHelper.PiOver2, 0f), new Vector3(0.1f, 0.1f, 0.1f), 100.0f, 200.0f, "ShipB/Source/Ship", "ShipsShader", "Botes/Battleship_lambert1_AlbedoTransparency.tga", "Botes/Battleship_lambert1_SpecularSmoothness.tga", "Island/normalAgua");
            PlayerBoat.playerMode = true;
            PlayerBoat.LoadContent();

            PlayerControlledShip = PlayerBoat;
            //PlayerControlledShip = Barquito;

            Ships = new Ship[]
            {
                PlayerControlledShip, SM, Patrol, Cruiser, Barquito
            };

            // Tienen que agregarse despues de que las demas sean creadas.
            foreach (Ship ship in Ships)
                ship.AddShips();

            float radius = 40f;
            IslandColliders = new BoundingSphere[]
            {
                new BoundingSphere(MatrixIsland1.Translation, 100), new BoundingSphere(MatrixIsland2.Translation, 300), new BoundingSphere(MatrixIsland3.Translation, radius),
                new BoundingSphere(MatrixIsland4.Translation, 100), new BoundingSphere(MatrixIsland5.Translation, radius),
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

            //Sonido
            Waves = Content.Load<SoundEffect>(ContentFolderSounds + "Waves");
            ShipShoot = Content.Load<SoundEffect>(ContentFolderSounds + "CannonShot");
            ShootInstance = ShipShoot.CreateInstance();
            Instance = Waves.CreateInstance();

            Explosion = Content.Load<SoundEffect>(ContentFolderSounds + "Explosion");
            ExplosionInstance = Explosion.CreateInstance();

            InvencibleSound = Content.Load<SoundEffect>(ContentFolderSounds + "Invencible");
            InvencibleInstance = InvencibleSound.CreateInstance();
            BulletModel = Content.Load<Model>(ContentFolder3D + "Bullets/Bullet");

            font = Content.Load<SpriteFont>("Fonts/Font");

            // OVERLAY GOTAS //


            dropsTexture = Content.Load<Texture2D>(ContentFolderTextures + "Dirty");
            dropsTexture2 = Content.Load<Texture2D>(ContentFolderTextures + "Smudgy");
            dropsEffect = Content.Load<Effect>(ContentFolderEffects + "TextureMerge");
            dropsEffect.Parameters["overlayTexture"]?.SetValue(dropsTexture);
            dropsEffect.Parameters["overlayTexture2"]?.SetValue(dropsTexture2);

            nightVisionEffect = Content.Load<Effect>(ContentFolderEffects + "NightVision");

            // Create a full screen quad to post-process
            FullScreenQuad = new FullScreenQuad(GraphicsDevice);

            // Create a render target for the scene
            SceneRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24, 0,
                RenderTargetUsage.DiscardContents);


            ScreenRenderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.None, 0,
                RenderTargetUsage.DiscardContents);

            // Create a render target for the scene
            EnvironmentMapRenderTarget = new RenderTargetCube(GraphicsDevice, EnvironmentmapSize, false,
                SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);
            GraphicsDevice.BlendState = BlendState.Opaque;

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
            TiempoEntreDisparos++;

            boundingFrustum.Matrix = shotCam.View * shotCam.Projection;

            Gizmos.UpdateViewProjection(shotCam.View, shotCam.Projection);

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

            VolcanEffect1.Parameters["lightPosition"].SetValue(lightPosition);
            VolcanEffect1.Parameters["eyePosition"].SetValue(shotCam.Position);

            WaterEffect.Parameters["lightPosition"]?.SetValue(lightPosition);
            WaterEffect.Parameters["eyePosition"]?.SetValue(shotCam.Position);

            // Aca deberiamos poner toda la logica de actualizacion del juego.
            if (SM.Life > 0)
            {
                SM.Update(gameTime, shotCam, lightPosition);
                Bullet.Bullet bullet = PoolBullets.Find(b => b._available);
                if (bullet != null && (TiempoEntreDisparos % 500 == 0))
                {

                    dispararAlJugador(SM, bullet);
                }
            }
            else
            {
                Vector3 hundimiento;
                hundimiento.X = 0;
                hundimiento.Y = 0.2f;
                hundimiento.Z = 0;
                SM.Position -= hundimiento;
                SM.BoatMatrix = Matrix.CreateScale(SM.Scale) * Matrix.CreateRotationX(SM.FrontDirection.X * (time / 10)) * Matrix.CreateRotationZ(SM.FrontDirection.Z * (time / 10)) * Matrix.CreateTranslation(SM.Position);
                if (SM.Position.Y <= -200)
                {
                    SM.BoatMatrix = Matrix.CreateTranslation(800f, 0.01f, 500f) * Matrix.CreateScale(SM.Scale);
                    SM.Life = 100;
                }
            }
            if (Patrol.Life > 0)
            {
                Patrol.Update(gameTime, shotCam, lightPosition);

                Bullet.Bullet bullet = PoolBullets.Find(b => b._available);
                if (bullet != null && (TiempoEntreDisparos % 275 == 0))
                {
                    dispararAlJugador(Patrol, bullet);
                }
            }
            else
            {
                Vector3 hundimiento;
                hundimiento.X = 0;
                hundimiento.Y = 0.2f;
                hundimiento.Z = 0;
                Patrol.Position -= hundimiento;
                Patrol.BoatMatrix = Matrix.CreateScale(Patrol.Scale) * Matrix.CreateRotationX(Patrol.FrontDirection.X * (time / 10)) * Matrix.CreateRotationZ(Patrol.FrontDirection.Z * (time / 10)) * Matrix.CreateTranslation(Patrol.Position);
                if (Patrol.Position.Y <= -200)
                {
                    Patrol.BoatMatrix = Matrix.CreateTranslation(1000f, 0.01f, 400f) * Matrix.CreateScale(Patrol.Scale);
                    Patrol.Life = 100;
                }
            }
            if (Barquito.Life > 0)
            {
                Barquito.Update(gameTime, shotCam, lightPosition);
            }
            else
            {
                Vector3 hundimiento;
                hundimiento.X = 0;
                hundimiento.Y = 0.2f;
                hundimiento.Z = 0;
                Barquito.Position -= hundimiento;
                Barquito.BoatMatrix = Matrix.CreateScale(Barquito.Scale) * Matrix.CreateRotationX(Barquito.FrontDirection.X * (time / 10)) * Matrix.CreateRotationZ(Barquito.FrontDirection.Z * (time / 10)) * Matrix.CreateTranslation(Barquito.Position);
                if (Barquito.Position.Y <= -200)
                {
                    Barquito.BoatMatrix = Matrix.CreateTranslation(-800f, 0.01f, 1000f) * Matrix.CreateScale(Barquito.Scale);
                    Barquito.Life = 100;
                }
            }
            if (Cruiser.Life > 0)
            {
                Cruiser.Update(gameTime, shotCam, lightPosition);
                Bullet.Bullet bullet = PoolBullets.Find(b => b._available);
                if (bullet != null && (TiempoEntreDisparos % 600 == 0))
                {
                    dispararAlJugador(Cruiser, bullet);
                }

            }
            else
            {
                Vector3 hundimiento;
                hundimiento.X = 0;
                hundimiento.Y = 0.2f;
                hundimiento.Z = 0;
                Cruiser.Position -= hundimiento;
                Cruiser.BoatMatrix = Matrix.CreateScale(Cruiser.Scale) * Matrix.CreateRotationX(Cruiser.FrontDirection.X * (time / 10)) * Matrix.CreateRotationZ(Cruiser.FrontDirection.Z * (time / 10)) * Matrix.CreateTranslation(Cruiser.Position);
                if (Cruiser.Position.Y <= -200)
                {
                    Cruiser.BoatMatrix = Matrix.CreateTranslation(1000f, 0.01f, 1000f) * Matrix.CreateScale(Cruiser.Scale);
                    Cruiser.Life = 100;
                }
            }

            PlayerBoat.Update(gameTime, shotCam, lightPosition);
            shotCam.Update(gameTime);
            shotCam.Position = PlayerBoat.Position + new Vector3(0, CameraArm, 0) - shotCam.FrontDirection *175f;
            CubeMapCamera.Position = shotCam.Position + new Vector3(0, -30, 0);

            Bullets = PoolBullets.FindAll(b => b._active);

            foreach (var bullet in Bullets)
            {
                bullet.Update();
            }
            if (Instance.State != SoundState.Playing)
            {
                Instance.IsLooped = true;
                Instance.Play();
                Instance.Volume = (float)0.45;
            }

            //if (PlayerControlledShip._currentLife < 1)
            //{
            //    Dispose();
            //}

            base.Update(gameTime);

        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aqui el codigo referido al renderizado.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            time += Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            #region Pass 1-6

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch.GraphicsDevice.BlendState = BlendState.Opaque;
            // Draw to our cubemap from the robot position
            for (var face = CubeMapFace.PositiveX; face <= CubeMapFace.NegativeZ; face++)
            {
                // Set the render target as our cubemap face, we are drawing the scene in this texture
                GraphicsDevice.SetRenderTarget(EnvironmentMapRenderTarget, face);
                GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

                SetCubemapCameraForOrientation(face);
                CubeMapCamera.BuildView();

                // Draw our scene. Do not draw our tank as it would be occluded by itself 
                // (if it has backface culling on)
                // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
                IslandEffect.Parameters["ModelTexture"].SetValue(IslandTexture);
                LightEffect.Parameters["baseTexture"].SetValue(IslandTexture);

                VolcanEffect1.Parameters["baseTexture"].SetValue(volcanTexture);
                VolcanEffect1.Parameters["normalTexture"].SetValue(volcanNormalTexture);

                DrawModelLight(ModelIsland, MatrixIsland1, VolcanEffect1, CubeMapCamera);
                DrawModelLight(ModelIsland, MatrixIsland2, VolcanEffect1, CubeMapCamera);
                DrawModelLight(ModelIsland, MatrixIsland3, VolcanEffect1, CubeMapCamera);
                DrawModelLight(ModelCasa, MatrixCasa, LightEffect, CubeMapCamera);

                DrawModelLight(ModelRock1, MatrixRock1, LightEffect, CubeMapCamera);
                DrawModelLight(ModelRock2, MatrixRock2, LightEffect, CubeMapCamera);
                DrawModelLight(ModelRock2, MatrixRock3, LightEffect, CubeMapCamera);
                DrawModelLight(ModelRock3, MatrixRock4, LightEffect, CubeMapCamera);
                DrawModelLight(ModelRock5, MatrixRock5, LightEffect, CubeMapCamera);
                DrawModelLight(ModelRock2, MatrixRock6, LightEffect, CubeMapCamera);
                DrawModelLight(ModelRock1, MatrixRock7, LightEffect, CubeMapCamera);
                LightEffect.Parameters["baseTexture"].SetValue(IslandMiscTexture);
                DrawModelLight(ModelIsland2, MatrixIsland4, LightEffect, CubeMapCamera);
                DrawModelLight(ModelIsland3, MatrixIsland5, LightEffect, CubeMapCamera);


                DrawModelLight(ModelPalm1, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(60, 10, 280), LightEffect, CubeMapCamera);
                DrawModelLight(ModelPalm2, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(110, 0, 300), LightEffect, CubeMapCamera);
                DrawModelLight(ModelPalm3, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(-50, 48, 150), LightEffect, CubeMapCamera);
                DrawModelLight(ModelPalm4, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(750, 0, -60), LightEffect, CubeMapCamera);
                DrawModelLight(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(580, 0, -150), LightEffect, CubeMapCamera);
                DrawModelLight(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(-650, 30, -100), LightEffect, CubeMapCamera);


                /// Dibujo Botes

                SM.Draw(CubeMapCamera);
                Patrol.Draw(CubeMapCamera);
                Cruiser.Draw(CubeMapCamera);
                Barquito.Draw(CubeMapCamera);
                //PlayerBoat.Draw(CubeMapCamera);

                //Iluminacion
                lightBox.Draw(LightBoxWorld, CubeMapCamera.View, CubeMapCamera.Projection);
                lightBox2.Draw(LightBoxWorld2, CubeMapCamera.View, CubeMapCamera.Projection);
                //DrawModel(PlayerBoatModel, Matrix.CreateRotationY((float)PlayerRotation)* PlayerBoatMatrix  , PlayerBoatEffect);

                /// Skydome
                Skydome.Draw(CubeMapCamera.View, CubeMapCamera.Projection, CubeMapCamera.Position);
                SkyDomeEffect.Parameters["Time"].SetValue(time);


            }

            #endregion

            #region Pass 7

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            // Set the render target as our shadow map, we are drawing the depth into this texture
            GraphicsDevice.SetRenderTarget(SceneRenderTarget);
            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1f, 0);

            // Para dibujar le modelo necesitamos pasarle informacion que el efecto esta esperando.
            IslandEffect.Parameters["ModelTexture"].SetValue(IslandTexture);
            LightEffect.Parameters["baseTexture"].SetValue(IslandTexture);

            VolcanEffect1.Parameters["baseTexture"].SetValue(volcanTexture);
            VolcanEffect1.Parameters["normalTexture"].SetValue(volcanNormalTexture);

            DrawModelLight(ModelIsland, MatrixIsland1, VolcanEffect1, shotCam);
            
            DrawModelLight(ModelIsland, MatrixIsland2, VolcanEffect1, shotCam);
            DrawModelLight(ModelIsland, MatrixIsland3, VolcanEffect1, shotCam);
            DrawModelLight(ModelCasa, MatrixCasa, LightEffect, shotCam);

            DrawModelLight(ModelRock1, MatrixRock1, LightEffect, shotCam);
            DrawModelLight(ModelRock2, MatrixRock2, LightEffect, shotCam);
            DrawModelLight(ModelRock2, MatrixRock3, LightEffect, shotCam);
            DrawModelLight(ModelRock3, MatrixRock4, LightEffect, shotCam);
            DrawModelLight(ModelRock5, MatrixRock5, LightEffect, shotCam);
            DrawModelLight(ModelRock2, MatrixRock6, LightEffect, shotCam);
            DrawModelLight(ModelRock1, MatrixRock7, LightEffect, shotCam);
            LightEffect.Parameters["baseTexture"].SetValue(IslandMiscTexture);
            DrawModelLight(ModelIsland2, MatrixIsland4, LightEffect, shotCam);
            DrawModelLight(ModelIsland3, MatrixIsland5, LightEffect, shotCam);



            DrawModelLight(ModelPalm1, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(60, 10, 280), LightEffect, shotCam);
            DrawModelLight(ModelPalm2, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(110, 0, 300), LightEffect, shotCam);
            DrawModelLight(ModelPalm3, Matrix.CreateScale(0.08f) * Matrix.CreateTranslation(-50, 48, 150), LightEffect, shotCam);
            DrawModelLight(ModelPalm4, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(750, 0, -60), LightEffect, shotCam);
            DrawModelLight(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateTranslation(580, 0, -150), LightEffect, shotCam);
            DrawModelLight(ModelPalm5, Matrix.CreateScale(0.09f) * Matrix.CreateRotationY(4f) * Matrix.CreateTranslation(-650, 30, -100), LightEffect, shotCam);


            SM.Draw(shotCam);
            Patrol.Draw(shotCam);
            Cruiser.Draw(shotCam);
            Barquito.Draw(shotCam);
            PlayerBoat.Draw(shotCam);


            /// Skydome
            Skydome.Draw(shotCam.View, shotCam.Projection, shotCam.Position);
            SkyDomeEffect.Parameters["Time"].SetValue(time);

            PisoEffect.Parameters["ModelTexture"]?.SetValue(PisoTexture);
            PisoEffect.Parameters["World"].SetValue(MatrixPiso);
            PisoEffect.Parameters["View"].SetValue(shotCam.View);
            PisoEffect.Parameters["Projection"].SetValue(shotCam.Projection);

            foreach (var mesh in ModelPiso.Meshes)
            {
                foreach (var meshPart in mesh.MeshParts)
                {
                    meshPart.Effect = PisoEffect;
                }
                mesh.Draw();
            }

            spriteBatch.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // Set up our Effect to draw the water
            WaterEffect.Parameters["baseTexture"]?.SetValue(WaterTexture);
            WaterEffect.Parameters["foamTexture"]?.SetValue(WaterFoamTexture);
            WaterEffect.Parameters["normalTexture"]?.SetValue(WaterNormalTexture);
            WaterEffect.Parameters["Time"]?.SetValue(time);
            WaterEffect.Parameters["environmentMap"]?.SetValue(EnvironmentMapRenderTarget);
            WaterEffect.Parameters["eyePosition"]?.SetValue(shotCam.Position);
            WaterEffect.Parameters["eyePosition"]?.SetValue(shotCam.Position);

            int offset = 40;

            for (int i = -offset; i < offset; i++)
                for (int j = -offset; j < offset; j++)
                {
                    Matrix MatrixWater = Matrix.Identity * Matrix.CreateScale(10f, 0f, 10f) * Matrix.CreateTranslation(i * 200, 0, j * 200);
                    WaterEffect.Parameters["World"].SetValue(MatrixWater);
                    WaterEffect.Parameters["View"].SetValue(shotCam.View);
                    WaterEffect.Parameters["Projection"].SetValue(shotCam.Projection);
                    WaterEffect.Parameters["InverseTransposeWorld"]?.SetValue(Matrix.Transpose(Matrix.Invert(MatrixWater)));

                    DrawModel(ModelWater, MatrixWater, WaterEffect, shotCam);
                }


            spriteBatch.GraphicsDevice.BlendState = BlendState.Opaque;


            //Gizmos.DrawSphere(collider.Center, collider.Radius * 10 * Vector3.One, Color.Yellow);
            //Gizmos.DrawFrustum(shotCam.Projection);
            //Gizmos.DrawCube(Matrix.Identity * 100000f, Color.Green);
            //DebugSphere.Draw(Matrix.Identity * Matrix.CreateTranslation(ProaPos), Game.CurrentCamera.View, Game.CurrentCamera.Projection);

            foreach (BoundingSphere collider in IslandColliders)
                Gizmos.DrawSphere(collider.Center, collider.Radius * Vector3.One);

            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Draw(gameTime);
            }

            #endregion

            #region Pass 8


            // No depth needed
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            // Set the render target to null, we are drawing to the screen


            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.R))
            {
                GraphicsDevice.SetRenderTarget(ScreenRenderTarget);
                dropsEffect.Parameters["time"]?.SetValue(time);
                dropsEffect.Parameters["baseTexture"]?.SetValue(SceneRenderTarget);
                FullScreenQuad.Draw(dropsEffect);

                GraphicsDevice.SetRenderTarget(null);
                nightVisionEffect.Parameters["time"]?.SetValue(time);
                nightVisionEffect.Parameters["baseTexture"]?.SetValue(ScreenRenderTarget);
                FullScreenQuad.Draw(nightVisionEffect);

            } else
            {
                GraphicsDevice.SetRenderTarget(null);
                dropsEffect.Parameters["time"]?.SetValue(time);
                dropsEffect.Parameters["baseTexture"]?.SetValue(SceneRenderTarget);
                FullScreenQuad.Draw(dropsEffect);
                Gizmos.DrawFrustum(shotCam.View * shotCam.Projection);

            }

            #endregion

            _ui.Draw();


            //wGizmos.Draw();
            base.Draw(gameTime);
        }

        private void DrawModel(Model geometry, Matrix transform, Effect effect, Camera cam)
        {
            BoundingSphere FuturePosition = new BoundingSphere(transform.Translation, 200);
            bool willCollide = false;
            if (boundingFrustum.Intersects(FuturePosition))
                willCollide = true;

            if (willCollide)
            {
                foreach (var mesh in geometry.Meshes)
                {
                    foreach (var meshPart in mesh.MeshParts)
                    {
                        meshPart.Effect = effect;

                    }
                    mesh.Draw();
                }
            }

        }


        private void DrawModelLight(Model geometry, Matrix transform, Effect light, Camera cam)
        {
            BoundingSphere FuturePosition = new BoundingSphere(transform.Translation, 250);
            bool willCollide = false;
            if (boundingFrustum.Intersects(FuturePosition))
                willCollide = true;

            if (willCollide)
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
                    light.Parameters["WorldViewProjection"].SetValue(transform * cam.View * cam.Projection);
                    foreach (var meshPart in modelMesh.MeshParts)
                    {
                        meshPart.Effect = light;
                    }
                    // Once we set these matrices we draw
                    modelMesh.Draw();
                }
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

        private void SetCubemapCameraForOrientation(CubeMapFace face)
        {
            switch (face)
            {
                default:
                case CubeMapFace.PositiveX:
                    CubeMapCamera.FrontDirection = -Vector3.UnitX;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;

                case CubeMapFace.NegativeX:
                    CubeMapCamera.FrontDirection = Vector3.UnitX;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;

                case CubeMapFace.PositiveY:
                    CubeMapCamera.FrontDirection = Vector3.Down;
                    CubeMapCamera.UpDirection = Vector3.UnitZ;
                    break;

                case CubeMapFace.NegativeY:
                    CubeMapCamera.FrontDirection = Vector3.Up;
                    CubeMapCamera.UpDirection = -Vector3.UnitZ;
                    break;

                case CubeMapFace.PositiveZ:
                    CubeMapCamera.FrontDirection = -Vector3.UnitZ;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;

                case CubeMapFace.NegativeZ:
                    CubeMapCamera.FrontDirection = Vector3.UnitZ;
                    CubeMapCamera.UpDirection = Vector3.Down;
                    break;
            }
        }
        /// <summary>
        ///     Libero los recursos que se cargaron en el juego.
        /// </summary>
        protected override void UnloadContent()
        {
            // Libero los recursos.
            Gizmos.Dispose();
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

            if (keyboardState.IsKeyDown(Keys.W))
            {
                PlayerControlledShip.bIsApplyingMovement = true;
                PlayerControlledShip.MoveForward(elapsedTime);
            }
            
            if (keyboardState.IsKeyDown(Keys.S))
            {
                PlayerControlledShip.bIsApplyingMovement = true;
                PlayerControlledShip.MoveBackwards(elapsedTime);
            }

            // Setteo bIsMoving a false para que el barco desacelere
            if (!keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.S))
            {
                PlayerControlledShip.bIsApplyingMovement = false;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                PlayerControlledShip.RotateRight(elapsedTime);
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                PlayerControlledShip.RotateLeft(elapsedTime);
            }

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton.Equals(ButtonState.Pressed) &&
                lastMouseState.LeftButton.Equals(ButtonState.Released))
            {
                Bullet.Bullet bullet = PoolBullets.Find(b => b._available);

                if (bullet != null)
                {

                    Vector3 offset = PlayerBoat.Position;
                    offset.Y += 20f;
                    Vector3 correccionPitch = shotCam.FrontDirection;
                    correccionPitch.Y += 0.2f;
                    bullet.Init(this, offset, correccionPitch, PlayerBoat);


                    bullet._available = false;

                    availableBullets = PoolBullets.FindAll(b => b._available).Count;

                    if (ShootInstance.State == SoundState.Playing)
                        ShootInstance.Stop();

                    ShootInstance.Play();
                    ShootInstance.Volume = 0.2f;
                }
            }

            lastMouseState = mouseState;

            if (keyboardState.IsKeyDown(Keys.M) && Instance.State == SoundState.Playing && Instance.Volume <= 0.98)
            {
                Instance.Volume += (float)0.02;
            }
            if (keyboardState.IsKeyDown(Keys.N) && Instance.State == SoundState.Playing && Instance.Volume >= 0.03)
            {
                Instance.Volume -= (float)0.02;
                
            }
            if (keyboardState.IsKeyDown(Keys.G) && !godModeEnabled && !lastState.IsKeyDown(Keys.G))
            {
                godModeEnabled = true;
                InvencibleInstance.Play();
                InvencibleInstance.IsLooped = true;
                InvencibleInstance.Volume = 0.25f;
                
                
            }
            if (keyboardState.IsKeyDown(Keys.L) && godModeEnabled && !lastState.IsKeyDown(Keys.L))
            {
                godModeEnabled = false;
                InvencibleInstance.Stop();

            }
            lastState = keyboardState;
        }

        public void dispararAlJugador(Ship barcoOrigen,Bullet.Bullet bullet)
        {
            Vector3 offset = barcoOrigen.Position;
            offset.Y += 20f;
            Vector3 DirectionToPlayer = Vector3.Normalize(PlayerControlledShip.Position - barcoOrigen.Position);
            bullet.Init(this, offset, DirectionToPlayer, barcoOrigen);

            bullet._available = false;

            availableBullets = PoolBullets.FindAll(b => b._available).Count;

            if (ShootInstance.State == SoundState.Playing)
                ShootInstance.Stop();

            ShootInstance.Play();
        }

    }

}