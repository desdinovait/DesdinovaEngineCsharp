//Using di sistema
using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//Using Desdinova Engine X
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using System.Management;

namespace DesdinovaModelPipeline
{
    public enum BlendMode
    {
        None = 0,
        AlphaBlend = 1,
        Additive = 2,
    }

    public enum LogType
    {
        HtmlSection = 0,
        HtmlSubSection = 1,
        HtmlInfo = 2,
        HtmlWarning = 3,
        HtmlError = 4,
        TextWriteline = 5,
        TextWrite = 6
    }

    public partial class Core : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Show the classic modal messagebox
        /// </summary>
        /// <param name="hWnd">Parent window handle</param>
        /// <param name="text">Message text</param>
        /// <param name="caption">Title of message box</param>
        /// <param name="type">Button type</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBox(IntPtr hWnd, String text, String caption, uint type);

        //File di log
        private static StreamWriter logWriter = null;

        //Device settings definiti da utente
        private static CoreSettings deviceSettings = null;

        //Device manager per egli elementi grafici
        protected static GraphicsDeviceManager gameGraphics = null;
        public static GraphicsDeviceManager Graphics
        {
            get { return gameGraphics; }
        }

        //Service container
        protected static GameServiceContainer gameService= null;
        public static GameServiceContainer Service
        {
            get { return gameService; }
        }
	
        //Content manager per gli elementi da caricare
        protected static ContentManager gameContent = null;
        public new static ContentManager Content
        {
            get { return gameContent; }
        }

        //Versione della libreria
        public static string EngineVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName(false).Version.ToString(); }
        }

        //Versione della libreria
        public static string EngineName
        {
            get { return Assembly.GetExecutingAssembly().GetName(false).Name.ToString(); }
        }

        //Tipo di compliazione
        public static string EngineCompileMode
        {
            get 
            {
                #if DEBUG
                {
                    return "DEBUG";
                }
                #else
                {
                    return "RELEASE";
                }
                #endif
            }
        }
	

        //Indica se il device è stato creato
        private bool isCreated = false;
        public bool IsCreated
        {
            get { return isCreated; }
        }

        //Titolo finestra
        public string WindowTitle
        {
            get { return Window.Title; }
            set { Window.Title = value; }
        }
	
        //Colore di default della schermata di loading (se non viene trovata)
        private Color defaultLoadingColor = Color.GreenYellow;
        public Color DefaultLoadingColor
        {
            get { return defaultLoadingColor; }
            set { defaultLoadingColor = value; }
        }
	
        //Texture vuota (con immagine)
        private static Texture2D nullTextureImage = null;
        public static Texture2D NullTextureImage
        {
            get { return nullTextureImage; }
        }

        //Texture vuota (colore bianco)
        private static Texture2D nullTextureColor = null;
        public static Texture2D NullTextureColor
        {
            get { return nullTextureColor; }
        }

        //Framerates
        private static int frameRateFPS = 0;
        public static int FPS
        {
            get { return frameRateFPS; }
        }
        private static int frameRateUPS = 0;
        public static int UPS
        {
            get { return frameRateUPS; }
        }
        private int frameCounterUPS = 0;
        private int frameCounterFPS = 0;
        private TimeSpan elapsedTime = TimeSpan.Zero;

        //Info disegno
        static internal int loadedVertices = 0;
        public static int LoadedVertices
        {
            get { return loadedVertices; }
        }
        static internal int loadedPrimitives = 0;
        public static int LoadedPrimitives
        {
            get { return loadedPrimitives; }
        }
        static internal int loadedModels = 0;
        public static int LoadedModels
        {
            get { return loadedModels; }
        }
        static internal int loadedMeshes = 0;
        public static int LoadedMeshes
        {
            get { return loadedMeshes; }
        }
        static internal int loadedSprites = 0;
        public static int LoadedSprites
        {
            get { return loadedSprites; }
        }
        static internal int loadedFonts = 0;
        public static int LoadedFonts
        {
            get { return loadedFonts; }
        }

        static internal int currentVertices = 0;
        static public int CurrentVertices
        {
            get { return currentVertices; }
        }
        static internal int currentPrimitives = 0;
        public static int CurrentPrimitives
        {
            get { return currentPrimitives; }
        }
        static internal int currentModels = 0;
        public static int CurrentModels
        {
            get { return currentModels; }
        }
        static internal int currentMeshes = 0;
        public static int CurrentMeshes
        {
            get { return currentMeshes; }
        }
        static internal int currentSprites = 0;
        public static int CurrentSprites
        {
            get { return currentSprites; }
        }
        static internal int currentFonts = 0;
        public static int CurrentFonts
        {
            get { return currentFonts; }
        }

        //Gestione scene
        private static Scene currentScene = null;
        private static Scene sceneLoading = null;
        private static ResolveTexture2D sceneTexture = null;
        public static ResolveTexture2D SceneTexture
        {
            get { return sceneTexture; }
        }
	
        //Adapter video corrente
        private GraphicsAdapter currentAdapter = null;

        //Screenshot
        private static ResolveTexture2D screenshotTexture = null;

        /// 
        /// <summary>
        /// MessageBox
        /// </summary>
        /// <param name="caption">Title of message box</param>
        /// <param name="text">Message text</param>
        /// <param name="buttonmode">Button type</param>
        /// <returns></returns>
        public static uint ShowMessageBox(string caption, string text, uint buttonmode)
        {
            //Mostra una messagebox di sistema
            return MessageBox(new IntPtr(0), text, caption, buttonmode);
        }

        /// <summary>
        /// Check DirectX prerequistes for XNA installed on current system
        /// </summary>
        /// <returns>True if requistes are present</returns>
        public static bool CheckDXPrerequisites()
        {
            if (!System.IO.File.Exists(System.Environment.SystemDirectory + "\\xactengine2_9.dll")) return false;
            if (!System.IO.File.Exists(System.Environment.SystemDirectory + "\\d3dx9_31.dll")) return false;
            if (!System.IO.File.Exists(System.Environment.SystemDirectory + "\\x3daudio1_2.dll")) return false;
            if (!System.IO.File.Exists(System.Environment.SystemDirectory + "\\xinput1_3.dll")) return false;

            //Tutti i requisti sono raggiunti
            return true;
        }

        /// <summary>
        /// Executed before application exits
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="args">Event arguments</param>
        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            Core.Exit();
        }

        /// <summary>
        /// Main DesdinovaEngineX initialization
        /// </summary>
        /// <param name="mainDeviceSettings">DeviceSettings structure with initialization parameters</param>
        public Core(CoreSettings mainDeviceSettings)
        {
            try
            {
                //Crea il file di log con l'intestazione
                logWriter = new StreamWriter(mainDeviceSettings.LogFilename);
                logWriter.WriteLine("<html>");
                logWriter.Write("<head><title>");
                logWriter.Write(mainDeviceSettings.ApplicationName);
                logWriter.WriteLine("</title></head>");
                logWriter.WriteLine("<body>");
                logWriter.Write("<font face=\"Verdana\" size=\"4\" color=\"#0000FF\"><b><u>");
                logWriter.Write(mainDeviceSettings.ApplicationName);
                logWriter.Write("</b></u></font><br>");

                //Log engine
                Core.Log(LogType.HtmlSection, "Engine Initialization");

                //Imposta il Device
                deviceSettings = mainDeviceSettings;

                //Crea il service
                gameService = Services;

                //Crea il Content
                gameContent = new ContentManager(gameService);

                //Crea il Device
                gameGraphics = new GraphicsDeviceManager(this);

                //Imposta il Device
                gameGraphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
                gameGraphics.SynchronizeWithVerticalRetrace = deviceSettings.SynchronizeWithVerticalRetrace;
                gameGraphics.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;
                gameGraphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;

                try
                {
                    //Imposta l'adepater corrente da usare
                    currentAdapter = GraphicsAdapter.Adapters[deviceSettings.AdapterID];
                }
                catch
                {
                    //Se succede qualche problema con l'adapter selezionato (id fuori dall'array)
                    //imposta l'adapter come quello di default
                    currentAdapter = GraphicsAdapter.DefaultAdapter;
                }

                //Log system information
                Core.Log(LogType.HtmlSubSection, "System Informations");
                Core.Log(LogType.HtmlInfo, "Date : " + DateTime.Today.ToShortDateString());
                Core.Log(LogType.HtmlInfo, "Time : " + DateTime.Now.TimeOfDay.Hours.ToString() + ":" + DateTime.Now.TimeOfDay.Minutes.ToString() + ":" + DateTime.Now.TimeOfDay.Seconds.ToString());
                //CPU info
                try
                {
                    System.Management.ManagementClass mgmt = new ManagementClass("Win32_Processor");
                    System.Management.ManagementObjectCollection objCol = mgmt.GetInstances();
                    foreach (ManagementObject obj in objCol)
                    {
                        //Cpu info
                        Core.Log(LogType.HtmlInfo, "Processor : " + obj.Properties["Name"].Value.ToString());
                        Core.Log(LogType.HtmlInfo, "Current/Max Clock Speed : " + obj.Properties["CurrentClockSpeed"].Value.ToString() + "MHz / " + obj.Properties["MaxClockSpeed"].Value.ToString() + "MHz");
                        obj.Dispose();
                    }
                }
                catch (System.Management.ManagementException)
                {
                    Core.Log(LogType.HtmlWarning, "Processor : Unknown");
                }
                Core.Log(LogType.HtmlInfo, "OS Version : " + System.Environment.OSVersion.ToString());
                Core.Log(LogType.HtmlInfo, "Machine Name : " + System.Environment.MachineName.ToString());
                Core.Log(LogType.HtmlInfo, "Current Directory : " + System.Environment.CurrentDirectory.ToString());
                Core.Log(LogType.HtmlInfo, "Engine Version : " + Core.EngineName + " " + Core.EngineVersion + " (" + Core.EngineCompileMode + ")");

                //Log adapter
                Core.Log(LogType.HtmlSubSection, "Adapter Informations");
                Core.Log(LogType.HtmlInfo, "Description : " + currentAdapter.Description.ToString());
                Core.Log(LogType.HtmlInfo, "Driver : " + currentAdapter.DriverDll.ToString() + " - " + currentAdapter.DriverVersion.ToString());
                Core.Log(LogType.HtmlInfo, "Display Mode : " + currentAdapter.CurrentDisplayMode.Width.ToString() + " x " + currentAdapter.CurrentDisplayMode.Height.ToString() + " x " + currentAdapter.CurrentDisplayMode.Format.ToString() + " (" + currentAdapter.CurrentDisplayMode.RefreshRate.ToString() + "Hz)");

                //Controlla l'accelerazione hardware, altrimenti esce
                //Qualsiasi applicazione creata deve supportare l'accelerazione hardware (necessariamente)
                if (currentAdapter.IsDeviceTypeAvailable(DeviceType.Hardware))
                {
                    Core.Log(LogType.HtmlInfo, "Supported Device Type: Hardware");
                }
                else
                {
                    if (currentAdapter.IsDeviceTypeAvailable(DeviceType.Reference))
                    {
                        Core.Log(LogType.HtmlInfo, "Supported Device Type: Reference");
                        Core.Log(LogType.HtmlError, "Supported Device Type Error. Only hardware device type allowed.");
                        isCreated = false;
                        Exit();
                        return;
                    }
                }

                //Controlla le capacità dell'adapter sul VertexShader e PixelSHader
                GraphicsDeviceCapabilities caps = currentAdapter.GetCapabilities(DeviceType.Hardware);
                if ((caps.MaxPixelShaderProfile < ShaderProfile.PS_2_0) || (caps.MaxVertexShaderProfile < ShaderProfile.VS_2_0))
                {
                    //Device non creato (non supporta i pixel/vertex shader richiesti)
                    Core.Log(LogType.HtmlInfo, "Max Vertex Shader : " + caps.MaxVertexShaderProfile.ToString());
                    Core.Log(LogType.HtmlInfo, "Max Pixel Shader : " + caps.MaxPixelShaderProfile.ToString());
                    Core.Log(LogType.HtmlError, "Vertex/Pixel shader minimum version not supported. Requested VS " + gameGraphics.MinimumPixelShaderProfile + " and PS " + gameGraphics.MinimumVertexShaderProfile + " versions.");

                    //Fallito
                    isCreated = false;
                    Exit();
                    return;
                }
                else
                {
                    Core.Log(LogType.HtmlInfo, "Max Vertex Shader : " + caps.MaxVertexShaderProfile.ToString());
                    Core.Log(LogType.HtmlInfo, "Max Pixel Shader : " + caps.MaxPixelShaderProfile.ToString());
                }


                //Log adapter
                Core.Log(LogType.HtmlSubSection, "Device and Presentation Parameters");
                Core.Log(LogType.HtmlInfo, "BackBuffer Width : " + deviceSettings.PresentationParameters.BackBufferWidth.ToString());
                Core.Log(LogType.HtmlInfo, "BackBuffer Height : " + deviceSettings.PresentationParameters.BackBufferHeight.ToString());
                Core.Log(LogType.HtmlInfo, "BackBuffer Format : " + deviceSettings.PresentationParameters.BackBufferFormat.ToString());
                Core.Log(LogType.HtmlInfo, "BackBuffer Count : " + deviceSettings.PresentationParameters.BackBufferCount.ToString());
                Core.Log(LogType.HtmlInfo, "Presentation Interval : " + deviceSettings.PresentationParameters.PresentationInterval.ToString());
                Core.Log(LogType.HtmlInfo, "Presentation Flags : " + deviceSettings.PresentationParameters.PresentOptions.ToString());

                //Stencil formats (per ombre)
                SurfaceFormat format = currentAdapter.CurrentDisplayMode.Format;
                if (currentAdapter.CheckDepthStencilMatch(DeviceType.Hardware, format, format, DepthFormat.Depth24Stencil8))
                {
                    gameGraphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
                    Core.Log(LogType.HtmlInfo, "Depth Stencil Formats : " + gameGraphics.PreferredDepthStencilFormat.ToString());
                }
                else if (currentAdapter.CheckDepthStencilMatch(DeviceType.Hardware, format, format, DepthFormat.Depth24Stencil8Single))
                {
                    gameGraphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8Single;
                    Core.Log(LogType.HtmlInfo, "Depth Stencil Formats : " + gameGraphics.PreferredDepthStencilFormat.ToString());
                }
                else if (currentAdapter.CheckDepthStencilMatch(DeviceType.Hardware, format, format, DepthFormat.Depth24Stencil4))
                {
                    gameGraphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil4;
                    Core.Log(LogType.HtmlInfo, "Depth Stencil Formats : " + gameGraphics.PreferredDepthStencilFormat.ToString());
                }
                else if (currentAdapter.CheckDepthStencilMatch(DeviceType.Hardware, format, format, DepthFormat.Depth15Stencil1))
                {
                    gameGraphics.PreferredDepthStencilFormat = DepthFormat.Depth15Stencil1;
                    Core.Log(LogType.HtmlInfo, "Depth Stencil Formats : " + gameGraphics.PreferredDepthStencilFormat.ToString());
                }
                else 
                {
                    gameGraphics.PreferredDepthStencilFormat = DepthFormat.Unknown;
                    Core.Log(LogType.HtmlWarning, "Depth Stencil Formats : " + gameGraphics.PreferredDepthStencilFormat.ToString());
                }

                //Impostazioni del refresh
                if (deviceSettings.PresentationParameters.IsFullScreen)
                {
                    //In fullscreen il refresh rate deve sempre essere impostato maggiore di 0 (70 è default)
                    if (deviceSettings.PresentationParameters.FullScreenRefreshRateInHz <= 0)
                    {
                        deviceSettings.PresentationParameters.FullScreenRefreshRateInHz = 70;
                    }
                    Core.Log(LogType.HtmlInfo, "Screen Mode : Fullscreen (" + deviceSettings.PresentationParameters.FullScreenRefreshRateInHz.ToString() + "Hz)");
                }
                else
                { 
                    //In finestra il refresh rate deve sempre essere 0
                    if (deviceSettings.PresentationParameters.FullScreenRefreshRateInHz != 0)
                    {
                        deviceSettings.PresentationParameters.FullScreenRefreshRateInHz = 0;
                    }
                    Core.Log(LogType.HtmlInfo, "Screen Mode : Windowed (" + deviceSettings.PresentationParameters.FullScreenRefreshRateInHz.ToString() + "Hz)");
                }

                //Controlla il supporto al multisampling
                int qLevels = 0; //Numero di livelli supportati (non è il numero del livello)
                if (deviceSettings.PresentationParameters.MultiSampleType != MultiSampleType.None)
                {
                    bool supportMulti = currentAdapter.CheckDeviceMultiSampleType(DeviceType.Hardware, deviceSettings.PresentationParameters.BackBufferFormat, deviceSettings.PresentationParameters.IsFullScreen, deviceSettings.PresentationParameters.MultiSampleType, out qLevels);
                    if (supportMulti)
                    {
                        //Supporta il mulsampling, controlla se il livello è supportato
                        if (qLevels - 1 < deviceSettings.PresentationParameters.MultiSampleQuality)
                        {
                            Core.Log(LogType.HtmlInfo, "Multisample Quality Not Suppoted. Forced to default quality");
                            deviceSettings.PresentationParameters.MultiSampleQuality = qLevels - 1;
                        }
                    }
                    else
                    {
                        //Se la modalità non è supportata allora imposta a 0 il multisampling
                        Core.Log(LogType.HtmlInfo, "Multisample Capability Not Suppoted. Forced to MultiSampleType.None");
                        deviceSettings.PresentationParameters.MultiSampleType = MultiSampleType.None;
                        deviceSettings.PresentationParameters.MultiSampleQuality = 0;
                    }
                }
                Core.Log(LogType.HtmlInfo, "Multisample Type : " + deviceSettings.PresentationParameters.MultiSampleType.ToString());
                Core.Log(LogType.HtmlInfo, "Multisample Quality : " + deviceSettings.PresentationParameters.MultiSampleQuality.ToString());
                Core.Log(LogType.HtmlInfo, "RenderTarget Usage : " + deviceSettings.PresentationParameters.RenderTargetUsage.ToString());
                Core.Log(LogType.HtmlInfo, "Swap Effect : " + deviceSettings.PresentationParameters.SwapEffect.ToString());

                //Imposta le proprietà della finestra
                Window.AllowUserResizing = false;
                this.IsMouseVisible = deviceSettings.ShowCursor;
                this.IsFixedTimeStep = deviceSettings.FixedTimestep;
                this.TargetElapsedTime = deviceSettings.FixedTimestepValue;

                Core.Log(LogType.HtmlInfo, "Fixed TimeStep : " + this.IsFixedTimeStep.ToString());
                Core.Log(LogType.HtmlInfo, "Fixed TimeStep Value : " + this.TargetElapsedTime.ToString());
                Core.Log(LogType.HtmlInfo, "Allow User Resizing : " + Window.AllowUserResizing.ToString());
                Core.Log(LogType.HtmlInfo, "Mouse Visible : " + deviceSettings.ShowCursor.ToString());

                //Device creato correttamente
                isCreated = true;
                Core.Log(LogType.HtmlInfo, "Device Engine Created Successfully");
            }
            catch
            {
                //Device non creato (qualcosa è andato storto)
                isCreated = false;
                Core.Log(LogType.HtmlError, "Device Engine NOT Created");
                Exit();
            }
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.Adapter = currentAdapter;
            e.GraphicsDeviceInformation.DeviceType = DeviceType.Hardware;
            e.GraphicsDeviceInformation.PresentationParameters = deviceSettings.PresentationParameters;
        }

        /// <summary>
        /// Engine components initialization (sounds, static elements, etc)
        /// </summary>
        protected override void Initialize()
        {
            if (isCreated)
            {
                //Texture vuote generiche
                Core.Log(LogType.HtmlSubSection, "Static Textures Initialization");
                try
                {
                    nullTextureImage = gameContent.Load<Texture2D>("Content\\Texture\\NullTexture");
                    Core.Log(LogType.HtmlInfo, "Null Texture Image Created Successfully");
                }
                catch
                {
                    Core.Log(LogType.HtmlError, "Null Texture Image NOT Created");
                }
                try
                {
                    nullTextureColor = GraphicsHelper.CreateColorTexture(Core.Graphics.GraphicsDevice, 1, 1, 0, Color.White, TextureUsage.None, SurfaceFormat.Color);
                    Core.Log(LogType.HtmlInfo, "Null Texture Color Created Successfully");
                }
                catch
                {
                    Core.Log(LogType.HtmlError, "Null Texture Color NOT Created");
                }

                //Screenshot
                try
                {
                    screenshotTexture = new ResolveTexture2D(Core.Graphics.GraphicsDevice, Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight, 1, Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferFormat);
                    Core.Log(LogType.HtmlInfo, "Screenshot Texture Created Successfully");
                }
                catch
                {
                    Core.Log(LogType.HtmlError, "Screenshot Texture NOT Created");
                }

                //Azzeramento scene
                sceneLoading = null;
                currentScene = null;

                //Log engine
                Core.Log(LogType.HtmlSection, "Scenes Management");
            }
        }

        /// <summary>
        /// Main update function
        /// </summary>
        /// <param name="gameTime">GameTime class by XNA pipeline</param>
        protected override void Update(GameTime gameTime)
        {
            //Aggiorna il framerate
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRateFPS = frameCounterFPS;
                frameRateUPS = frameCounterUPS;
                frameCounterUPS = 0;
                frameCounterFPS = 0;
            }
            frameCounterUPS++;

            //Azzeramento vertici
            Core.currentVertices = 0;
            Core.currentPrimitives = 0;
            Core.currentModels = 0;
            Core.currentMeshes = 0;
            Core.currentSprites = 0;
            Core.currentFonts = 0;

            //Gestione scene
            if (currentScene != null)
            {
                if (currentScene.Loaded)
                {
                    //Esegue l'update della scena corrente (se caricata)
                    currentScene.Update(gameTime);
                }
                else
                {
                    //Si è deciso di usare la scehrmata di loading (cioè so prevede un caricamento lungo)
                    if (currentScene.InLoading)
                    {
                        //La scena corrente si sta caricando, mostra la schermata di loading (se definita)
                        if (sceneLoading != null)
                        {
                            if (sceneLoading.Loaded)
                            {
                                sceneLoading.Update(gameTime);
                            }
                            else
                            {
                                sceneLoading.InitializeBegin();
                                sceneLoading.Initialize();
                                sceneLoading.InitializeEnd();
                            }
                        }
                    }
                    else
                    {
                        //La scena corrente non è stata caricata, avvia il thread per il caricamento
                        Thread loadThread = new Thread(LoadSceneByThread);
                        loadThread.Name = "Temporary load-scene thread";
                        loadThread.Start();
                        Thread.Sleep(500);
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            //Gestione scene
            if (currentScene != null)
            {
                if (currentScene.Loaded)
                {
                    //Disegna la scena corrente (se caricata)
                    currentScene.Draw();
                }
                else
                {
                    //Si è deciso di usare la schermata di loading (cioè si prevede un caricamento lungo)
                    if (sceneLoading != null)
                    {
                        if (sceneLoading.Loaded)
                        {
                            //Disegna la scena usata per le fasi di loading (se caricata e definita)
                            sceneLoading.Draw();
                        }
                    }
                    else
                    {
                        //Non è stata definita la scena usata per le fasi di loading, mostra una schermata verde di avvertimento
                        Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, defaultLoadingColor, 1.0f, 0);
                    }
                }
            }
            else
            {
                //Non è stata definita la scena corrente, mostra una schermata rossa di avvertimento
                Core.Graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Red, 1.0f, 0);
            }

            //Aumenta il framecounter
            frameCounterFPS++;
        }

        public static void SetLoadingScene(Scene newScene)
        {
            try
            {
                sceneLoading = newScene;
                sceneLoading.InitializeBegin();
                sceneLoading.Initialize();
                sceneLoading.InitializeEnd();
            }
            catch
            {
                sceneLoading = null;
            }
        }

        public static void SetCurrentScene(Scene scene, bool releaseCurrent)
        {
            try
            {
                //Salva l'immagine corrente della scena che sta per essere cambiata
                //Può essere usata per creare effetti di sovrapposizione delle scene
                sceneTexture = null;
                sceneTexture = new ResolveTexture2D(Core.Graphics.GraphicsDevice, Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferWidth, Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferHeight, 1, Core.Graphics.GraphicsDevice.PresentationParameters.BackBufferFormat);
                Core.Graphics.GraphicsDevice.ResolveBackBuffer(sceneTexture);
            }
            finally
            {
                if ((currentScene != null) && (releaseCurrent))
                {
                    //Rilascia la scena corrente
                    currentScene.PreRelease();
                    currentScene.Release();
                    currentScene.PostRelease();
                }
                //Attende qualche istante
                Thread.Sleep(400);
                //Forza il Garbage Collector
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                //Attende qualche istante
                Thread.Sleep(100);
            }
            currentScene = scene;
            Core.Service.RemoveService(typeof(EngineScene));
            Core.Service.AddService(typeof(EngineScene), scene);
        }

        public static void LoadSceneByThread()
        {
            //Esegue il caricanto dal thread separato
            currentScene.InitializeBegin();
            currentScene.Initialize();
            currentScene.InitializeEnd();
        }

        public static bool CaptureScreenshot(string fileNameWithoutExtension, bool useTimestamp)
        {
            string finalFilename = string.Empty;
            if (useTimestamp)
            {
                finalFilename = fileNameWithoutExtension + "_" + DateTime.Now.ToString(@"dd.MM.yyyy") + "_" + DateTime.Now.ToLongTimeString() + ".bmp";
            }
            else
            {
                finalFilename = fileNameWithoutExtension + ".bmp";
            }

            Core.Log(LogType.HtmlSubSection, "Capture Screenshot");
            Core.Log(LogType.HtmlInfo, "Filename : " + finalFilename);
            Core.Log(LogType.HtmlInfo, "Directory : " + System.Environment.CurrentDirectory.ToString());     
            try
            {
                //Cattura il backbuffer
                Core.Graphics.GraphicsDevice.ResolveBackBuffer(screenshotTexture);
                screenshotTexture.Save(finalFilename, ImageFileFormat.Bmp);
                Core.Log(LogType.HtmlInfo, "Screenshot Created Succesfully");  
                return true;
            }
            catch
            {
                Core.Log(LogType.HtmlError, "Screenshot NOT Created");
                return false;
            }
        }


        public static new void Exit()
        {
            //Log system information
            Core.Log(LogType.HtmlSubSection, "Final Informations");
            Core.Log(LogType.HtmlInfo, "Date : " + DateTime.Today.ToShortDateString());
            Core.Log(LogType.HtmlInfo, "Time : " + DateTime.Now.TimeOfDay.Hours.ToString() + ":" + DateTime.Now.TimeOfDay.Minutes.ToString() + ":" + DateTime.Now.TimeOfDay.Seconds.ToString());

            //Chiude il log
            logWriter.WriteLine();

            //Informazioni di uscita
		    logWriter.WriteLine("<br><br><hr>");
            logWriter.WriteLine("<font face=\"Arial\" size=\"1\" color=\"#000000\">Copyright (C) 2009 by Ferla Daniele. All rights reserved.</font><br>");
            logWriter.WriteLine("<font face=\"Arial\" size=\"1\" color=\"#000000\">Informations at <a href='http://www.desdinova.it'>www.desdinova.it</a></font>");
            logWriter.WriteLine("</body>");
            logWriter.WriteLine("</html>");
            logWriter.Close();

            //Forza il GC (di prova)
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            //Uscita (non è possibile usare base.Exit() perchè la funzione è statica)
            Environment.Exit(1);
        }

        public static void Log(LogType type, string message)
        {
            if (type == LogType.HtmlSection)
            {
                logWriter.WriteLine();
                logWriter.Write("<font face=\"Arial\" size=\"3\" color=\"#0000FF\"><b><u><br><br>");
                logWriter.Write(message);
                logWriter.Write("</b></u></font>");
            }
            if (type == LogType.HtmlSubSection)
            {
                logWriter.WriteLine();
                logWriter.Write("<font face=\"Arial\" size=\"2\" color=\"#000000\"><b><br><br>");
                logWriter.Write(message);
                logWriter.Write("</b></font>");
            }
            else if (type == LogType.HtmlInfo)
            {
                logWriter.WriteLine();
                logWriter.Write("<font face=\"Arial\" size=\"2\" color=\"#000000\">");
                logWriter.Write("<li>" + message);
                logWriter.Write("</font>");
            }
            else if (type == LogType.HtmlWarning)
            {
                logWriter.WriteLine();
                logWriter.Write("<font face=\"Arial\" size=\"2\" color=\"#0000FF\">");
                logWriter.Write("<li>" + message);
                logWriter.Write("</font>");
            }
            else if (type == LogType.HtmlError)
            {
                logWriter.WriteLine();
                logWriter.WriteLine("<font face=\"Arial\" size=\"2\" color=\"#FF0000\">");
                logWriter.Write("<li>" + message);
                logWriter.Write("</font>");
            }
            else if (type == LogType.TextWriteline)
            {
                logWriter.WriteLine(message);
            }
            else if (type == LogType.TextWrite)
            {
                logWriter.Write(message);
            }

            //Esegue il flash dello stream
            //In questo caso se avviene qualche errore lo strem è già stato scritto e il file di log risulta comunque leggibile)
            logWriter.Flush();
        }

        public static void ToggleFullscreen()
        {
            int newWidth = 0;
            int newHeight = 0;

            Core.Graphics.IsFullScreen = !Core.Graphics.IsFullScreen;

            //if (Core.Graphics.IsFullScreen)
            {
                newWidth = Core.Graphics.GraphicsDevice.DisplayMode.Width;
                newHeight = Core.Graphics.GraphicsDevice.DisplayMode.Height;
            }
            //else
            {
                //newWidth = 800;
                //newHeight = 600;
            }

            Core.Graphics.PreferredBackBufferWidth = newWidth;
            Core.Graphics.PreferredBackBufferHeight = newHeight;
            Core.Graphics.PreferMultiSampling = true;
            Core.Graphics.ApplyChanges();
        }

    }
}
