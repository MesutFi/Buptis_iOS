using CoreGraphics;
using Foundation;
using Google.Maps;
using System;
using System.IO;
using UIKit;

namespace Buptis_iOS
{
    public partial class TestVcBase : UIViewController
    {
        public TestVcBase (IntPtr handle) : base (handle)
        {
        }
        MapView mapView;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            GetMap(this.View);
        }

        #region Map
        public void GetMap(UIView _mapView)
        {
            CameraPosition camera = CameraPosition.FromCamera(37.797865, -122.402526, 6);

            mapView = MapView.FromCamera(CGRect.Empty, camera);
            mapView.MyLocationEnabled = true;


            mapView.MapStyle = MapStyle.FromJson(ReadFile(), null);
            mapView.Frame = this.View.Bounds;
            this.View.AddSubview(mapView);
        }
        private string ReadFile()
        {
            using (StreamReader reader = new StreamReader("MapStyle/mapstyle1.txt"))
            {
                return reader.ReadToEnd();
            }

        }
        #endregion
    }
}