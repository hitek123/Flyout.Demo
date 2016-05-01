using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace Bosch.FlyoutDemo.Fragments
{
    public class OutputsScreenAdapter : BaseAdapter<Output>
    {
        private readonly Activity _context;
        private readonly List<Output> _outputStatuses;

        public OutputsScreenAdapter(Activity context, List<Output> outputStatuses)
        {
            _context = context;
            _outputStatuses = outputStatuses;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var outputStatus = _outputStatuses[position];
            var view = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.OutputRow, null);
            view.FindViewById<TextView>(Resource.Id.OutputNumber).Text = outputStatus.IdText;
            view.FindViewById<TextView>(Resource.Id.OutputName).Text = outputStatus.Name;
            view.FindViewById<ImageView>(Resource.Id.OutputState).SetImageResource(outputStatus.IsOn ? Resource.Drawable.OutputOnIndicator : Resource.Drawable.OutputOffIndicator);
            return view;
        }

        public override int Count
        {
            get { return _outputStatuses.Count; }
        }

        public override Output this[int position]
        {
            get { return _outputStatuses[position]; }
        }
    }
}