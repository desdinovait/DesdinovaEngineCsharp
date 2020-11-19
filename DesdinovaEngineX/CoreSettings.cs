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
using System.Reflection;

namespace DesdinovaModelPipeline
{
    public class CoreSettings
    {
        /// <summary>
        /// Inizializza la struttura con i valori base
        /// </summary>
        public CoreSettings()
        {
            //Info
            this.logFilename = "DesdinovaEngineX.dll.html";
            this.applicationName = "Desdinova Engine X " + Assembly.GetExecutingAssembly().GetName(false).Version.ToString();
            this.showCursor = true;

            //Parametri video
            this.adapterID = 0;
            this.presentationParameters.BackBufferWidth = 800;
            this.presentationParameters.BackBufferHeight = 600;
            this.presentationParameters.IsFullScreen = false;
            this.presentationParameters.PresentationInterval = PresentInterval.Immediate;
            this.presentationParameters.SwapEffect = SwapEffect.Discard;
            this.presentationParameters.EnableAutoDepthStencil = true;
            this.presentationParameters.AutoDepthStencilFormat = DepthFormat.Depth24;
            this.presentationParameters.DeviceWindowHandle = IntPtr.Zero;
            this.presentationParameters.MultiSampleType = MultiSampleType.None;
            this.presentationParameters.MultiSampleQuality = 0;
            this.presentationParameters.FullScreenRefreshRateInHz = 0;
            this.presentationParameters.BackBufferCount = 1;
            this.presentationParameters.BackBufferFormat = SurfaceFormat.Color;

            //Refresh
            this.fixedTimestep = true;
            this.fixedTimestepValue = TimeSpan.FromSeconds(1.0f / 60.0f);
            this.synchronizeWithVerticalRetrace = true;
            
            //Audio
            this.audioFileXGS = string.Empty;
            this.audioFileXWB = string.Empty;
            this.audioFileXSB = string.Empty;
        }

        /// <summary>
        /// Nome del file di log (si consiglia di usare nomefile.exe.html)
        /// </summary>
        private string logFilename;
        public string LogFilename
        {
            get { return logFilename; }
            set { logFilename = value; }
        }

        /// <summary>
        /// Nome principale dell'applicazione (solitamente il nome del gioco)
        /// </summary>
        private string applicationName;
        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        /// <summary>
        /// Sincronizza il refresh dell'applicazione con quello attuale dello schermo
        /// </summary>
        private bool synchronizeWithVerticalRetrace;
        public bool SynchronizeWithVerticalRetrace
        {
            get { return synchronizeWithVerticalRetrace; }
            set { synchronizeWithVerticalRetrace = value; }
        }

        /// <summary>
        /// Mostra o meno il cursore curante l'esecuzione dell'applicazione
        /// </summary>
        private bool showCursor;
        public bool ShowCursor
        {
            get { return showCursor; }
            set { showCursor = value; }
        }

        /// <summary>
        /// Stabilisce se il refresh è fissato ad un valore specifico (definito con FixedTimestapValue)
        /// </summary>
        private bool fixedTimestep;
        public bool FixedTimestep
        {
            get { return fixedTimestep; }
            set { fixedTimestep = value; }
        }

        /// <summary>
        /// Definisce il valore di refresh forzato (solitamente 1/60 di secondo)
        /// </summary>
        private TimeSpan fixedTimestepValue;
        public TimeSpan FixedTimestepValue
        {
            get { return fixedTimestepValue; }
            set { fixedTimestepValue = value; }
        }

        /// <summary>
        /// Indice dello schermo sul quale visualizzare l'applicazione (solitamente 0)
        /// </summary>
        private int adapterID;
        public int AdapterID
        {
            get { return adapterID; }
            set { adapterID = value; }
        }

        /// <summary>
        /// Parametri di visualizzazione dell'applicazione
        /// </summary>
        private PresentationParameters presentationParameters = new PresentationParameters();
        public PresentationParameters PresentationParameters
        {
            get { return presentationParameters; }
            set { presentationParameters = value; }
        }

        /// <summary>
        /// File .xgs per l'audio XACT da caricare
        /// </summary>
        private string audioFileXGS;
        public string AudioFileXGS
        {
            get { return audioFileXGS; }
            set { audioFileXGS = value; }
        }

        /// <summary>
        /// File .xwb per l'audio XACT da caricare
        /// </summary>
        private string audioFileXWB;
        public string AudioFileXWB
        {
            get { return audioFileXWB; }
            set { audioFileXWB = value; }
        }

        /// <summary>
        /// File .xsb per l'audio XACT da caricare
        /// </summary>
        private string audioFileXSB;
        public string AudioFileXSB
        {
            get { return audioFileXSB; }
            set { audioFileXSB = value; }
        }
    }

}
