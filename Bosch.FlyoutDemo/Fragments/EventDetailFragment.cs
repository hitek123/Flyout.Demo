using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Security;
using Fragment = Android.App.Fragment;

namespace Bosch.FlyoutDemo.Fragments
{
    public class EventDetailFragment : Fragment
    {

        public interface IEventDetailListener
        {
            void OnFinished();
        }
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetHasOptionsMenu(true);
            Activity.ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public static EventDetailFragment NewInstance(string text)
        {
            var args = new Bundle();
            args.PutString("text", text);
            var frag1 = new EventDetailFragment { Arguments = args };
            return frag1;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.fragment1, null);
            var label = view.FindViewById<TextView>(Resource.Id.textView1);
            var labelText = Arguments.GetString("text");
            label.Text = "Events for " + labelText;
            return view;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    var activity = Activity as IEventDetailListener;
                    if (activity != null)
                        activity.OnFinished();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}