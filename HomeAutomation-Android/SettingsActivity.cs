using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HomeAutomation_Android
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        private EditText _GarageEndpointTxt;
        private EditText _GarageTokenTxt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.settings);

            _GarageEndpointTxt = FindViewById<EditText>(Resource.Id.garageEndpointTxt);
            _GarageTokenTxt = FindViewById<EditText>(Resource.Id.garageTokenTxt);

            Button saveBtn = FindViewById<Button>(Resource.Id.settingsSaveBtn);
            Button cancelBtn = FindViewById<Button>(Resource.Id.settingsCancelBtn);

            saveBtn.Click += SaveBtn_Click;
            cancelBtn.Click += CancelBtn_Click;

            InitSettings();
        }

        private void InitSettings()
        {
            _GarageEndpointTxt.Text = UserSettings.GarageApiEndpoint;
            _GarageTokenTxt.Text = UserSettings.Token;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            UserSettings.GarageApiEndpoint = _GarageEndpointTxt.Text;
            UserSettings.Token = _GarageTokenTxt.Text;

            Toast.MakeText(this, "Settings Saved!", ToastLength.Long).Show();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Finish();
        }

    }
}