using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;


namespace TIMSMobile
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        TextView textMessage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            textMessage = FindViewById<TextView>(Resource.Id.message);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_home:
                    textMessage.SetText(Resource.String.title_home);
                    return true;
                case Resource.Id.navigation_notifications:
                    textMessage.SetText(Resource.String.title_notifications);
                    return true;
                case Resource.Id.navigation_settings:
                    textMessage.SetText(Resource.String.title_settings);
                    return true;
                case Resource.Id.navigation_checkins:
                    {
                        Spinner checkinSpinner = FindViewById<Spinner>(Resource.Id.selectcheckin);
                        //List<Checkin> checkins = Communication.RetrieveCheckins();
                        //string[] checkinNumbers = new string[checkins.Count];
                        //for (int i = 0; i != checkins.Count; i++)
                        //    checkinNumbers[i] = checkins[i].checkinNumber.ToString();

                        //ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, checkinNumbers);
                        //checkinSpinner.Adapter = adapter;
                        textMessage.SetText(Resource.String.title_checkins);
                        SetContentView(Resource.Layout.checkin_main);
                        return true;
                    }
            }
            return false;
        }
    }
}

