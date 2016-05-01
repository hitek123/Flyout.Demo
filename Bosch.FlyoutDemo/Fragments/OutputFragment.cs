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
    public class OutputFragment : Fragment, OptionsDialog.IOptionDialogListener
    {
        private ListView _listView;

        public const int OutputOnRequestCode = 0;
        public const int OutputOffRequestCode = 1;

        private List<Output> _outputs = new List<Output>
            {
                new Output(1, "A", true),
                new Output(2, "B", false),
                new Output(3, "C", true),
                new Output(4, "D", false)
            };


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.Outputs, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.ouputListView);
            _listView.Adapter = new OutputsScreenAdapter(Activity, _outputs);
            _listView.ItemClick += (sender, args) =>
            {
                Console.WriteLine();
                var adapter = sender as OutputsScreenAdapter;
                var output = _outputs[args.Position];
                FragmentTransaction ft = Activity.FragmentManager.BeginTransaction();
                Fragment prev = Activity.FragmentManager.FindFragmentByTag("outputdialog");
                if (prev != null)
                {
                    ft.Remove(prev);
                }
                ft.AddToBackStack(null);

                var options = new OptionsDialog(output.Name, output.Id,
                                                output.IsOn
                                                    ? new List<string> { "Turn Off", "Cancel" }
                                                    : new List<string> { "Turn On", "Cancel" });
                options.SetTargetFragment(this, output.IsOn ? OutputOnRequestCode : OutputOffRequestCode);
                options.Show(ft, "outputdialog");
            };
            return view;
        }

        public override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            var outputNumber = data.GetIntExtra("ItemNumber", -1);
            switch (requestCode)
            {
                case OutputOnRequestCode:
                    if (resultCode == 0) //Turn off
                        Toast.MakeText(Activity, "Turn off output " +outputNumber, ToastLength.Long).Show();
                    break;
                case OutputOffRequestCode:
                    if (resultCode == 0) //Turn On
                        Toast.MakeText(Activity, "Turn on output " +outputNumber, ToastLength.Long).Show();
                    break;
            }
        }

        public void OnOptionSelected(int chosenOption)
        {
            throw new NotImplementedException();
        }
    }
}