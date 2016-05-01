using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Bosch.FlyoutDemo.Fragments
{
    public class DisconnectFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Disconnect, container, false);

            var disconnectButton = view.FindViewById<Button>(Resource.Id.DisconnectButton);
            disconnectButton.Click += (sender, args) =>
            {
                Activity.Finish();
            };

            return view;
        }
    }
}