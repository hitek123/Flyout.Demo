using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;

namespace Bosch.FlyoutDemo.Fragments
{
    public class OptionsDialog : DialogFragment
    {
        private readonly string _title;
        private readonly int _itemNumber;
        private readonly List<string> _options;
        private ListView _listView;

        public interface IOptionDialogListener
        {
            void OnOptionSelected(int chosenOption);
        }

        public OptionsDialog(string title, int itemNumber,  List<string> options)
        {
            if (options == null || options.Count == 0)
                throw new ArgumentException("Options dialog: Options cannot be empty", "options");
            _title = title;
            _itemNumber = itemNumber;
            _options = options;
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            Dialog.SetTitle(_title);
            var view = inflater.Inflate(Resource.Layout.Options, container, false);
            _listView = view.FindViewById<ListView>(Resource.Id.OptionsList);
            var adapter = new ArrayAdapter<string>(Activity, Android.Resource.Layout.SimpleListItem1, _options);
            _listView.Adapter = adapter;
            _listView.ItemClick += (sender, args) =>
                {
                    var intent = new Intent();
                    intent.PutExtra("ItemNumber", _itemNumber);
                    TargetFragment.OnActivityResult(TargetRequestCode, Result.Ok, intent);
                    Dismiss();
                };
            return view;
        }

       
    }
}