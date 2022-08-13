using System;
using StereoKit;

namespace StoreViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize StereoKit
            SKSettings settings = new SKSettings
            {
                appName = "StoreViewer",
                assetsFolder = "Assets",
            };
            if (!SK.Initialize(settings))
                Environment.Exit(1);
            DemoSky g = new DemoSky();

			g.Initialize();


            // Core application loop
            while (SK.Step(() =>
            {
                
                Matrix newCam = Renderer.CameraRoot;
                Vec3 newPos = Renderer.CameraRoot.Translation;
                newPos.x +=  + ((Input.Controller(Handed.Left).stick.x) / 25);
                newPos.z += Input.Head.orientation.z * ((Input.Controller(Handed.Left).stick.y) / 25);

                newCam.Translation = newPos;
                Renderer.CameraRoot = newCam;
                g.Update();
            })) ;
            g.Shutdown();
		}
	}
}
