using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Android.OS;
using Bosch.FlyoutDemo.Adapters;
using Bosch.FlyoutDemo.Fragments;
using Bosch.FlyoutDemo.Helpers;
using Android.Content.Res;

namespace Bosch.FlyoutDemo
{
    [Activity(Label = "Remote Security Control", MainLauncher = true, Icon = "@drawable/rscicon", Theme = "@style/ThemeLight")]
    public class MainActivity : Activity, FragmentManager.IOnBackStackChangedListener
    {
        private DrawerToggle _drawerToggle;
        private string _drawerTitle;
        private string _currentSectionTitle;

        private DrawerLayout _drawerLayout;
        
        private ListView _leftDrawerListView;
        private List<NavigationDrawerItem> _sections;
        private int _lastSelectedSection = -1;

        private ListView _rightDrawerListView;
        private string[] _certs;
        private int _lastSelectedCert = -1;

        private bool _firstTime = true;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main);
            FragmentManager.AddOnBackStackChangedListener(this);

            _sections = new List<NavigationDrawerItem>();
            _sections.Add(new NavigationDrawerItem("Connect", "", Resource.Drawable.ic_action_content_forward_dark));
            _sections.Add(new NavigationDrawerItem("Dealer info", "", Resource.Drawable.ic_action_info_outline_dark));
            _sections.Add(new NavigationDrawerItem("Demo", "", Resource.Drawable.RSCIcon));
            _sections.Add(new NavigationDrawerItem("About app", "", Resource.Drawable.ic_action_help_dark));
            _certs = new[] {"B9512", "B5512"};

            _currentSectionTitle = _drawerTitle = Title;

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            _leftDrawerListView = FindViewById<ListView>(Resource.Id.left_drawer);
            _leftDrawerListView.Adapter = new NavigationDrawerListAdapter(this, _sections);
            _leftDrawerListView.ItemClick += (sender, args) => ListItemClicked(args.Position);

            _rightDrawerListView = FindViewById<ListView>(Resource.Id.right_drawer);
            _rightDrawerListView.Adapter = new ArrayAdapter<string>(this, Resource.Layout.item_menu, _certs);
            _rightDrawerListView.ItemClick += (sender, args) =>
            {
                if (_lastSelectedCert != args.Position)
                {
                    _lastSelectedCert = args.Position;
                    if (_lastSelectedSection == 0)
                    {
                        _lastSelectedSection = -1;
                        ListItemClicked(0);
                    }
                }
                _drawerLayout.CloseDrawers();
            };

            _drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow_light, (int)GravityFlags.Start);

            //DrawerToggle is the animation that happens with the indicator next to the actionbar
            _drawerToggle = new DrawerToggle(this, _drawerLayout,
                                                        Resource.Drawable.ic_navigation_drawer_light,
                                                        Resource.String.drawer_open,
                                                        Resource.String.drawer_close);

            //Display the current fragments title and update the options menu
            _drawerToggle.DrawerClosed += (o, args) =>
            {
                ActionBar.Title = _currentSectionTitle;
                InvalidateOptionsMenu();
            };

            //Display the drawer title and update the options menu
            _drawerToggle.DrawerOpened += (o, args) =>
            {
                ActionBar.Title = _drawerTitle;
                InvalidateOptionsMenu();
            };

            //Set the drawer lister to be the toggle.
            _drawerLayout.SetDrawerListener(_drawerToggle);

            //If first time you will want to go ahead and click first item.
            if (savedInstanceState == null)
            {
                ListItemClicked(0);
            }

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        // Pass the event to ActionBarDrawerToggle, if it returns
        // true, then it has handled the app icon touch event
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_drawerToggle.OnOptionsItemSelected(item))
            {
                if (_drawerLayout.IsDrawerOpen(_rightDrawerListView))
                    _drawerLayout.CloseDrawer(_rightDrawerListView);
                return true;
            }

            switch (item.ItemId)
            {
                case Resource.Id.action_edit:
                    if (_drawerLayout.IsDrawerOpen(_rightDrawerListView))
                        _drawerLayout.CloseDrawer(_rightDrawerListView);
                    else
                        _drawerLayout.OpenDrawer(_rightDrawerListView);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ListItemClicked(int position)
        {
            if (_lastSelectedSection == position)
            {
                _drawerLayout.CloseDrawers();
                return;
            }

            _lastSelectedSection = position;

            Fragment fragment = null;

            switch (position)
            {
                case 0:
                    switch (_lastSelectedCert)
                    {
                        case 0:
                            fragment = new LoginFragment(_lastSelectedCert, _certs[_lastSelectedCert], "US1A B9512",
                                "Valid 12/13/2014 - 12/31/2035");
                            break;
                        case 1:
                            fragment = new LoginFragment(_lastSelectedCert, _certs[_lastSelectedCert], "US1B B5512",
                                "Valid 3/13/2013 - 12/31/2035");
                            break;
                        default:
                            fragment = new LoginFragment();
                            break;
                    }
                    break;
                case 2:
                    var intent = new Intent(this, typeof (ConnectedActivity));
                    intent.PutExtra("id", 999);
                    StartActivity(intent);
                    break;
                case 3:
                    fragment = AboutAppFragment.NewInstance(string.Format("Version {0} ({1})",
                        PackageManager.GetPackageInfo(PackageName, 0).VersionName,
                        PackageManager.GetPackageInfo(PackageName, 0).VersionCode));
                    break;
                default:
                    fragment = Fragment1.NewInstance(_sections[position].Title);
                    break;
            }

            if (fragment == null) { _drawerLayout.CloseDrawer(_leftDrawerListView); return; }

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.content_frame, fragment)
                .Commit();

            _leftDrawerListView.SetItemChecked(position, true);
            ActionBar.Title = _currentSectionTitle = _sections[position].Title;
            _drawerLayout.CloseDrawer(_leftDrawerListView);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {

            var drawerOpen = _drawerLayout.IsDrawerOpen(_leftDrawerListView);

            //When we open the drawer we usually do not want to show any menu options
            for (int i = 0; i < menu.Size(); i++)
                menu.GetItem(i).SetVisible(!drawerOpen);

            return base.OnPrepareOptionsMenu(menu);
        }

        public void OnBackStackChanged()
        {
            var count = FragmentManager.BackStackEntryCount;

            var stack = string.Empty;
            for (var i = count - 1; i >= 0; i--)
            {
                var entry = FragmentManager.GetBackStackEntryAt(i);
                stack += " | " + entry.Name;

            }
            Console.WriteLine(stack);
            if (count == 0) return;

            var nextEntry = FragmentManager.GetBackStackEntryAt(count - 1);
            ActionBar.Title = _currentSectionTitle = nextEntry.Name;
            var position = _sections.IndexOf(_sections.FirstOrDefault(s => s.Title == nextEntry.Name));
            _leftDrawerListView.SetItemChecked(position, true);
        }
    }
}

