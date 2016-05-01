using Android.App;
using Android.OS;
using Android.Views;

namespace Bosch.FlyoutDemo.Fragments
{
    public class DealerFragment : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Dealer, null);

            return view;

        }
    }
}