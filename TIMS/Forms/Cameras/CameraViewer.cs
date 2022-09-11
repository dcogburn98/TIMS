using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Windows.Forms;
using Onvif.Core.Client;
using Onvif.Core.Client.Imaging;
using Onvif.Core.Client.Common;
using Onvif.Core.Client.Media;

namespace TIMS.Forms.Cameras
{
    public partial class CameraViewer : Form
    {
        public CameraViewer()
        {
            InitializeComponent();

            GetVideo();
        }

        public async void GetVideo()
        {
            MediaClient cf = await OnvifClientFactory.CreateMediaClientAsync("192.168.138.105:8080", "zosi", "");
            
            
            var account = new Account("192.168.138.105", "zosi", "");
            var camera = Camera.Create(account, ex =>
            {
                MessageBox.Show("Unable to connect");
            });

            if (camera != null)
            {
                //Onvif.Core.Client.OnvifClientFactory cf = OnvifClientFactory.
                //move...
                var vector1 = new PTZVector { PanTilt = new Vector2D { x = 0.5f } };
                var speed1 = new PTZSpeed { PanTilt = new Vector2D { x = 1f, y = 1f } };
                //await camera.MoveAsync(MoveType.Absolute, vector1, speed1, 0);

                //zoom...
                var vector2 = new PTZVector { Zoom = new Vector1D { x = 1f } };
                var speed2 = new PTZSpeed { Zoom = new Vector1D { x = 1f } };
                //await camera.MoveAsync(MoveType.Absolute, vector2, speed2, 0);

                //focus...
                //var focusMove = new FocusMove { Absolute=new AbsoluteFocus {   } };
                //await camera.FocusAsync(focusMove);
                //Media cf = camera.Media.ChannelFactory.CreateChannel();
                //GetVideoSourcesResponse r = await cf.GetVideoSourcesAsync(new GetVideoSourcesRequest());
            }
        }
    }
}
