using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Bosch.FlyoutDemo.Helpers;

namespace Bosch.FlyoutDemo.Fragments
{
    public class LoginFragment : Fragment
    {

        private Button _connectButton;
        private readonly int _certificateId;
        private readonly string _panelName;
        private readonly string _panelType;
        private readonly string _dates;

        public LoginFragment() :
            this (-1, "Please choose a panel", "", "")
        {
            
        }
        public LoginFragment(int certificateId, string panelName, string panelType, string dates)
        {
            _certificateId = certificateId;
            _panelName = panelName;
            _panelType = panelType;
            _dates = dates;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            SetHasOptionsMenu(true);
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Login, null);
            _connectButton = view.FindViewById<Button>(Resource.Id.ConnectButton);
            _connectButton.Click += (sender, args) =>
            {
                if (_certificateId < 0)
                {
                    DialogHelpers.ShowAlert(Activity, "No Certificate Chosen",
                        "Please choose a panel", "OK");
                    return;
                }
                var intent = new Intent(Activity, typeof(ConnectedActivity));
                intent.PutExtra("id", _certificateId);
                StartActivity(intent);
            };
            var panelName = view.FindViewById<TextView>(Resource.Id.PanelName);
            panelName.Text = _panelName;

            var panelType = view.FindViewById<TextView>(Resource.Id.PanelType);
            panelType.Text = _panelType;

            var dates = view.FindViewById<TextView>(Resource.Id.ValidDateRange);
            dates.Text = _dates;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.editCert, menu);
        }
    }
}