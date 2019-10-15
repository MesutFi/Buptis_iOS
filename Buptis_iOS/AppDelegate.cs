using Buptis_iOS.Database;
using Buptis_iOS.GenericClass;
using Buptis_iOS.Lokasyonlar;
using Foundation;
using Google.Maps;
using Google.SignIn;
using System;
using UIKit;
using static Buptis_iOS.GirisKayit.GirisVC;

namespace Buptis_iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        const string MapsApiKey = "AIzaSyD6F8c4Mf4a4lMLj_XLYvLKnBd_SP4_jVE";
        public override UIWindow Window
        {
            get;
            set;
        }

        public UIStoryboard Storyboard_Lokasyonlar
        {
            get { return UIStoryboard.FromName("LokasyonlarBaseVC", NSBundle.MainBundle); }
        }
        public UIStoryboard Storyboard_Splash
        {
            get { return UIStoryboard.FromName("GirisKayit", NSBundle.MainBundle); }
        }
        public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
        {
            return storyboard.InstantiateViewController(viewControllerName);
        }
        public void SetRootViewController(UIViewController rootViewController, bool animate)
        {
            if (animate)
            {
                var transitionType = UIViewAnimationOptions.TransitionFlipFromRight;

                Window.RootViewController = rootViewController;
                UIView.Transition(Window, 0.5, transitionType,
                                  () => Window.RootViewController = rootViewController,
                                  null);
            }
            else
            {
                Window.RootViewController = rootViewController;
            }
        }

        public void SetRootLokasyonlarViewController()
        {
            var LokasyonlarBase = GetViewController(Storyboard_Lokasyonlar, "LokasyonlarBaseVC") as LokasyonlarBaseVC;
            SetRootViewController(LokasyonlarBase, true);
        }

        public void SetRootSplashViewController()
        {
            var GirisYapBase = GetViewController(Storyboard_Splash, "GirisYapBaseVC") as GirisYapBaseVC;
            SetRootViewController(GirisYapBase, true);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            new DataBase();
            MapServices.ProvideAPIKey(MapsApiKey);
            Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();
            //var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
            //SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();
            return true;
        }
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            //var uri_netfx = new Uri(url.AbsoluteString);

            //GoogleAuthenticatorHelper.Auth?.OnPageLoading(uri_netfx);


            // Convert NSUrl to Uri
            var uri = new Uri(url.AbsoluteString);

            // Load redirectUrl page
            AuthenticationState.Authenticator.OnPageLoading(uri);

            return true;
        }
        public override void OnResignActivation(UIApplication application)
        {
        }

        public override void DidEnterBackground(UIApplication application)
        {
        }

        public override void WillEnterForeground(UIApplication application)
        {
        }

        public override void OnActivated(UIApplication application)
        {
        }

        public override void WillTerminate(UIApplication application)
        {
        }
    }
}

