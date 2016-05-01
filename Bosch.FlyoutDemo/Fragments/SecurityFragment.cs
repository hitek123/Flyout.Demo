using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace Bosch.FlyoutDemo.Fragments
{
    public class SecurityFragment : Fragment
    {

        public interface ISecurityFragmentListener
        {
            void OnAdvancedButtonClick();
        }

        private Button _advancedButton;

        public static SecurityFragment NewInstance()
        {
            var args = new Bundle();
            var frag1 = new SecurityFragment { Arguments = args };
            return frag1;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Security, null);

            _advancedButton = view.FindViewById<Button>(Resource.Id.button1);
            _advancedButton.Click += (sender, args) =>
            {
                var listener = Activity as ISecurityFragmentListener;
                if (listener != null)
                    listener.OnAdvancedButtonClick();
            };
            
            return view;

        }
    }
}