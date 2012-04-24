﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Gadgeteer Designer.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace GadgeteerApp1
{
    public partial class Program : Gadgeteer.Program
    {
        // GTM.Module defintions
		Gadgeteer.Modules.GHIElectronics.Camera camera;
		Gadgeteer.Modules.GHIElectronics.UsbClientDP usbClient;
		Gadgeteer.Modules.Seeed.CellularRadio cellularRadio;
		Gadgeteer.Modules.GHIElectronics.MulticolorLed led;
		Gadgeteer.Modules.Seeed.OledDisplay oledDisplay;
		Gadgeteer.Modules.GHIElectronics.MulticolorLed led1;
		Gadgeteer.Modules.Seeed.Compass compass;
		Gadgeteer.Modules.GHIElectronics.Button button1;

		public static void Main()
        {
			//Important to initialize the Mainboard first
            Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();			

            Program program = new Program();
			program.InitializeModules();
            program.ProgramStarted();
            program.Run(); // Starts Dispatcher
        }

        private void InitializeModules()
        {   
			// Initialize GTM.Modules and event handlers here.		
			usbClient = new GTM.GHIElectronics.UsbClientDP(1);
		
			camera = new GTM.GHIElectronics.Camera(3);
		
			led = new GTM.GHIElectronics.MulticolorLed(4);
		
			button1 = new GTM.GHIElectronics.Button(5);
		
			oledDisplay = new GTM.Seeed.OledDisplay(6);

        }
    }
}
