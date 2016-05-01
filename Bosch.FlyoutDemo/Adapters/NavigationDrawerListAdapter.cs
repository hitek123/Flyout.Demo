using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Accessibility;
using Android.Widget;

namespace Bosch.FlyoutDemo.Adapters
{
    public class NavigationDrawerListAdapter : BaseAdapter<NavigationDrawerItem>
    {
        private readonly Activity _context;
        private List<NavigationDrawerItem> _items;

        public NavigationDrawerListAdapter(Activity context, List<NavigationDrawerItem> items )
        {
            _context = context;
            _items = items;
        }

        public void UpdateData(List<NavigationDrawerItem> items)
        {
            _items = items;
            NotifyDataSetChanged();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            var item = _items[position];

            if (view == null)
                view = _context.LayoutInflater.Inflate(Resource.Layout.item_menu_icon, null);

            (view.FindViewById<ImageView>(Resource.Id.icon)).SetImageResource(item.ImageId);
            (view.FindViewById<TextView>(Resource.Id.text1)).Text = item.Title;
            if (item.IsCounterVisible)
                (view.FindViewById<TextView>(Resource.Id.count)).Text = item.Count;
            else
                (view.FindViewById<TextView>(Resource.Id.count)).Visibility = ViewStates.Gone;
            return view;
        }

        public override int Count
        {
            get { return _items.Count; }
        }

        public override NavigationDrawerItem this[int position]
        {
            get { return _items[position]; }
        }
    }
}