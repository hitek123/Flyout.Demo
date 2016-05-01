using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Bosch.FlyoutDemo.Fragments
{
    public class AboutAppFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static AboutAppFragment NewInstance(string versiomText)
        {
            var args = new Bundle();
            args.PutString("version", versiomText);
            var fragment = new AboutAppFragment {Arguments = args};
            return fragment;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var versionText = Arguments.GetString("version");
            var view = inflater.Inflate(Resource.Layout.aboutApp, null);
            (view.FindViewById<TextView>(Resource.Id.version)).Text = versionText;
            (view.FindViewById<TextView>(Resource.Id.legalText)).Text = LicenseStore.LegalText;
            return view;
        }
    }
}