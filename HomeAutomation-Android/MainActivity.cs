using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Home.Core.Clients;

namespace HomeAutomation_Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private GarageClient _GarageCore;
        private TextView _GarageStatusTxt;
        private bool _EventsSubscribed = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.Title = GetString(Resource.String.app_name);

            InitGarage();
        }

        protected override void OnResume()
        {
            base.OnResume();

            InitGarage();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.toolbar_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.menu_settings)
            {
                StartActivity(typeof(SettingsActivity));
            }

            return base.OnOptionsItemSelected(item);
        }

        private void InitGarage()
        {
            //_GarageCore = new GarageAppCore("http://pizzohome.ddns.net:5000", "Testing123");
            try
            {
                _GarageCore = new GarageClient(UserSettings.GarageApiEndpoint, UserSettings.Token);

                Button garageButton = FindViewById<Button>(Resource.Id.toggleBtn);
                Button statusButton = FindViewById<Button>(Resource.Id.refresh);
                _GarageStatusTxt = FindViewById<TextView>(Resource.Id.statusLbl);

                if (!_EventsSubscribed)
                {
                    garageButton.Click += ToggleGarage;
                    statusButton.Click += GetGarageStatus;
                    _EventsSubscribed = true;
                }
                
                GetGarageStatus(this, null);
            }
            catch
            {
                Toast.MakeText(this, "Please make sure Garage settings are correct.", ToastLength.Long).Show();
            }
        }

        private async void ToggleGarage(object sender, EventArgs eventArgs)
        {
            try
            {
                await _GarageCore.ToggleGarage();

                View view = (View)sender;
                Snackbar.Make(view, "Garage Button Pushed!", Snackbar.LengthLong)
                    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            }
            catch(Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }

        private async void GetGarageStatus(object sender, EventArgs eventArgs)
        {
            try
            {
                var result = await _GarageCore.GetGarageStatus();

                _GarageStatusTxt.Text = result == 0 ? GetString(Resource.String.garageStatusClosed) : GetString(Resource.String.garageStatusOpen);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}

