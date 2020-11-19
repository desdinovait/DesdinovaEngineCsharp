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

namespace EditorEngine
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
            dev.LogFilename = "EDLibitumLog.html";
            dev.ApplicationName ="..:: EdLibitum (Sors Adversa Level Editor) ::..";
            dev.PresentationParameters.BackBufferWidth = 800;
            dev.PresentationParameters.BackBufferHeight = 600;
            dev.PresentationParameters.FullScreenRefreshRateInHz = 0;
            dev.PresentationParameters.MultiSampleType = MultiSampleType.None;
            dev.PresentationParameters.MultiSampleQuality = 0;

            //XACT
            dev.AudioFileXGS = "Content\\Audio\\Audio.xgs";
            dev.AudioFileXWB = "Content\\Audio\\Wave Bank.xwb";
            dev.AudioFileXSB = "Content\\Audio\\Sound Bank.xsb";

            //Controlla i prerequisiti prima di avviare
            if (Core.CheckDXPrerequisites())
            {           
                //Loop principale
                using (Editor edit = new Editor(dev))
                {
                    //try
                    {
                        edit.Run();
                    }
                    /*catch (NoSuitableGraphicsDeviceException ex)
                    {
                        Core.ShowMessageBox("DesdinovaEngineX - Device Error", "Error exception: " + ex.Message.ToString() + "\nInner exception: " + ex.InnerException.Message + "\nPossibile solutions: Please check minimum requirements.", 0);
                    }
                    catch
                    {
                        Core.ShowMessageBox("DesdinovaEngineX - Runtime Error", "Runtime fatal error.\nPlease close all other applications, check minimum requirements and run again.", 0);
                    }//*/
                }

            }
            else
            {
                Core.ShowMessageBox("DesdinovaEngineX - Initialization Error", "XNA prerequisites NOT found in your system. Check installations of:\n\n- DirectX setup (August 2007 Update)\n- XNA 2.0 Redist\n- Framework .NET Framework 2.0 SP1\n- Visual C++ 2005 SP1 Redistributable", 0);
            }       
        }
    }
}



