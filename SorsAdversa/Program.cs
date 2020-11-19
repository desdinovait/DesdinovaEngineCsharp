using System;
//Using XNA
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
//Desdinova Engine
using DesdinovaModelPipeline;
using DesdinovaModelPipeline.Helpers;
using SorsAdversa.Properties;
using System.Reflection;

namespace SorsAdversa
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //Impostazioni
            CoreSettings coreSettings = new CoreSettings();

            //Fullscreen
            coreSettings.ApplicationName = "Sors Adversa - 2.5D Shooter";
            coreSettings.LogFilename = Assembly.GetExecutingAssembly().GetName(false).Name.ToString() + ".exe" + ".html";
            coreSettings.AdapterID = Settings.Default.AdapterID;
            coreSettings.PresentationParameters.IsFullScreen = Settings.Default.IsFullscreen;
            coreSettings.PresentationParameters.BackBufferWidth = Settings.Default.Width;
            coreSettings.PresentationParameters.BackBufferHeight = Settings.Default.Height;
            coreSettings.PresentationParameters.FullScreenRefreshRateInHz = Settings.Default.FullScreenRefreshRateInHz;
            coreSettings.PresentationParameters.MultiSampleType = Settings.Default.MultiSampleType;
            coreSettings.PresentationParameters.MultiSampleQuality = Settings.Default.MultiSampleQuality;//*/

            //TimeStep
            coreSettings.FixedTimestep = false;
            coreSettings.SynchronizeWithVerticalRetrace = true;//*/
       
            //XACT Audio
            coreSettings.AudioFileXGS = "Content\\Audio\\Audio.xgs";
            coreSettings.AudioFileXWB = "Content\\Audio\\Wave Bank.xwb";
            coreSettings.AudioFileXSB = "Content\\Audio\\Sound Bank.xsb";

            //Controlla i prerequisiti prima di avviare
            if (Core.CheckDXPrerequisites())
            {
                //Loop principale
                using (SorsAdversa game = new SorsAdversa(coreSettings))
                {
                    #if DEBUG
                    {
                        //In modalità DEBUG non fa il controllo delle eccezioni
                        game.Run();
                    }
                    #else
                    {
                        try
                        {
                            game.Run();
                        }
                        catch (NoSuitableGraphicsDeviceException ex)
                        {
                            Core.ShowMessageBox("Sors Adversa - Device Error", "Error exception: " + ex.Message.ToString() + "\nInner exception: " + ex.InnerException.Message + "\nPossibile solutions: Please check minimum requirements.", 0);
                        }
                        catch
                        {
                            Core.ShowMessageBox("Sors Adversa - Runtime Error", "Runtime fatal error.\nPlease close all other applications, check minimum requirements and run again.", 0);
                        }
                    }
                    #endif
                }
            }
            else
            {
                Core.ShowMessageBox("Sors Adversa - Initialization Error", "XNA prerequisites NOT found in your system. Check installations of:\n\n- DirectX setup (August 2007 Update)\n- XNA 2.0 Redist\n- Framework .NET Framework 2.0 SP1\n- Visual C++ 2005 SP1 Redistributable", 0);
            }
        }
    }
}

