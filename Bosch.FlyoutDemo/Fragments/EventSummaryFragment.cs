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
    public class EventSummaryFragment : Fragment
    {

        public interface IEventSummaryListener
        {
            void OnPrioritySelected(int piority, string name );
        }

        private string[] _priorities;
        public static EventSummaryFragment NewInstance()
        {
            var args = new Bundle();
            var frag1 = new EventSummaryFragment { Arguments = args };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            
            var view = inflater.Inflate(Resource.Layout.EventSummary, null);

            var listView = view.FindViewById<ListView>(Resource.Id.EventSummaryListView);
            _priorities = new[] {"Fire Alarm", "Gas Alarm", "Panic Alarm"};
            var adadpter = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1, _priorities);

            listView.Adapter = adadpter;

            listView.ItemClick += (sender, args) =>
            {
                var activity = Activity as IEventSummaryListener;
                if (activity != null)
                    activity.OnPrioritySelected(args.Position, _priorities[args.Position]);
            };
            return view;
        }
    }
}