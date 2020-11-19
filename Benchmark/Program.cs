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
using Benchmark.Properties;

namespace Benchmark
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //Impostazioni
            CoreSettings dev = new CoreSettings();

            //Fullscreen
            dev.LogFilename = "Benchmark.html";
            dev.ApplicationName = "..:: Desdinova Engine Benchmark ::..";
            dev.AdapterID = Settings.Default.AdapterID;
            dev.PresentationParameters.IsFullScreen = Settings.Default.IsFullscreen;
            dev.PresentationParameters.BackBufferWidth = Settings.Default.Width;
            dev.PresentationParameters.BackBufferHeight = Settings.Default.Height;
            dev.PresentationParameters.FullScreenRefreshRateInHz = Settings.Default.FullScreenRefreshRateInHz;
            dev.PresentationParameters.MultiSampleType = Settings.Default.MultiSampleType;
            dev.PresentationParameters.MultiSampleQuality = Settings.Default.MultiSampleQuality;//*/

            //TimeStep
            dev.FixedTimestep = false;
            dev.SynchronizeWithVerticalRetrace = true;//*/

            /*XACT
            dev.xgsXACTFile = "Content\\Audio\\Audio.xgs";
            dev.xwbXACTFile = "Content\\Audio\\Wave Bank.xwb";
            dev.xsbXACTFile = "Content\\Audio\\Sound Bank.xsb";*/

            //Controlla i prerequisiti prima di avviare
            if (Core.CheckDXPrerequisites())
            {
                //Loop principale
                using (Benchmark game = new Benchmark(dev))
                {
                    //try
                    {
                        game.Run();
                    }
                    /*catch (NoSuitableGraphicsDeviceException ex)
                    {
                        game.ShowMessageBox("Sors Adversa - Device Error", "Error exception: " + ex.Message.ToString() + "\nInner exception: " + ex.InnerException.Message + "\nPossibile solutions: Please check minimum requirements.", 0);
                    }
                    catch
                    {
                        game.ShowMessageBox("Sors Adversa - Runtime Error", "Runtime fatal error.\nPlease close all other applications, check minimum requirements and run again.", 0);
                    }*/
                }
            }
            else
            {
                Core.ShowMessageBox("Desdinova Engine X Benchmark - Initialization Error", "XNA prerequisites NOT found in your system. Check installations of:\n\n- DirectX setup (August 2007 Update)\n- XNA 2.0 Redist\n- Framework .NET Framework 2.0 SP1\n- Visual C++ 2005 SP1 Redistributable", 0);
            }
        }
    }
}


