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
            DemoPointCloud g = new DemoPointCloud();

			g.Initialize();


            // Core application loop
            while (SK.Step(() =>
            {
				g.Update();
            })) ;
            g.Shutdown();
		}
	}
}
