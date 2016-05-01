using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Timers;
using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using Bosch.FlyoutDemo.Adapters;
using Bosch.FlyoutDemo.Fragments;
using Bosch.FlyoutDemo.Helpers;
using Bosch.FlyoutDemo.Views;
using Fragment = Android.App.Fragment;

namespace Bosch.FlyoutDemo
{
    [Activity(Label = "ConnectedActivity", Icon = "@drawable/rscicon", Theme = "@style/ThemeLight")]
    public class ConnectedActivity : FragmentActivity, 
        EventSummaryFragment.IEventSummaryListener, 
        EventDetailFragment.IEventDetailListener, 
        AreasFragment.IAreasListener,
        SecurityFragment.ISecurityFragmentListener
    {
        private DrawerToggle _drawerToggle;
        private string _drawerTitle;
        private string _currentSectionTitle;

        private DrawerLayout _drawerLayout;

        private ListView _leftDrawerListView;
        private List<NavigationDrawerItem> _sections;
        private NavigationDrawerListAdapter _adapter;

        private int _certificateId;
        private BadgeDrawable _alarmsBadge;
        private BadgeDrawable _eventsBadge;
        private Timer _alarmTimer;
        private int _lastAlarmCount;
        private Timer _eventTimer;
        private int _lastEventCount;
        private readonly Random _random = new Random();
        
        private int _disconnectPosition;
        private int _eventsPosition;
        private int _areasPosition;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var firstTime = Intent.GetBooleanExtra("firstTime", false);

            _certificateId = Intent.GetIntExtra("id", 0);
            switch (_certificateId)
            {
                case 0:
                    Title = "B9512";
                    break;
                case 1:
                    Title = "B5512";
                    break;
                default:
                    Title = "Demo panel";
                    break;
            }
           
            // Create your application here
            SetContentView(Resource.Layout.Connected);

            _sections = new List<NavigationDrawerItem>();
            _sections.Add(new NavigationDrawerItem("Security", "", Resource.Drawable.ic_action_lock_32_dark, true) 
                {Count = _lastAlarmCount.ToString(CultureInfo.InvariantCulture)});
            //_sections.Add(new NavigationDrawerItem("Areas", "", Resource.Drawable.ic_action_editor_mode_edit));
            _sections.Add(new NavigationDrawerItem("Events", "", Resource.Drawable.ic_action_image_flash_on_dark, true)
                {Count = _lastEventCount.ToString(CultureInfo.InvariantCulture)});

            _eventsPosition = _sections.Count - 1;
            //_sections.Add(new NavigationDrawerItem("Faults", "", Resource.Drawable.ic_action_editor_mode_edit));
            _sections.Add(new NavigationDrawerItem("Outputs", "", Resource.Drawable.ic_action_led_diode_dark));
            if (_certificateId != 1)
                _sections.Add(new NavigationDrawerItem("Doors", "", Resource.Drawable.ic_action_mydoor_dark));
            _sections.Add(new NavigationDrawerItem("Cameras", "", Resource.Drawable.ic_action_web_camera_25));
            _sections.Add(new NavigationDrawerItem("Disconnect", "", Resource.Drawable.ic_action_navigation_close_dark));
            _disconnectPosition = _sections.Count - 1;
            _sections.Add(new NavigationDrawerItem("Dealer info", "", Resource.Drawable.ic_action_info_outline_dark));

            _currentSectionTitle = _drawerTitle = Title;

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            _leftDrawerListView = FindViewById<ListView>(Resource.Id.left_drawer);
            _adapter = new NavigationDrawerListAdapter(this, _sections);
            _leftDrawerListView.Adapter = _adapter;
            _leftDrawerListView.ItemClick += (sender, args) => UpdateDrawer(args.Position, _sections[args.Position].Title);

            _drawerLayout.SetDrawerShadow(Resource.Drawable.drawer_shadow_light, (int) GravityFlags.Start);

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

            UpdateDrawer(0, _sections[0].Title);
            if (firstTime)
                _drawerLayout.OpenDrawer(_leftDrawerListView);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
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

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_drawerToggle.OnOptionsItemSelected(item))
                return true;

            switch (item.ItemId)
            {
                case Resource.Id.action_edit:
                    var fragment = AreasFragment.NewInstance();
                    FragmentManager.BeginTransaction()
                        .Replace(Resource.Id.frame_container, fragment)
                        .Commit();
                    ActionBar.Title = _currentSectionTitle = "Areas";
                    if (!_drawerToggle.DrawerIndicatorEnabled)
                        _drawerToggle.DrawerIndicatorEnabled = true;
                    break;
                case Resource.Id.action_save:
                    UpdateDrawer(_eventsPosition, _sections[_eventsPosition].Title);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
        
        private void UpdateDrawer(int position, string text)
        {
            Fragment fragment = null;

            if (position == _eventsPosition)
                    fragment = new EventSummaryFragment();
            else if (position == 2)
                    fragment = new OutputFragment();
            else if (position == _disconnectPosition)
                    fragment = new DisconnectFragment();
            else if (position == 0)
                    fragment = SecurityFragment.NewInstance();
            else
                    fragment = Fragment1.NewInstance(text);

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_container, fragment)
                .Commit();
            _leftDrawerListView.SetItemChecked(position, true);
            ActionBar.Title = _currentSectionTitle = text;
            _drawerLayout.CloseDrawers();
            if (!_drawerToggle.DrawerIndicatorEnabled)
                _drawerToggle.DrawerIndicatorEnabled = true;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ConnectedMenu, menu);

            var alarmsItem = menu.FindItem(Resource.Id.action_edit);
            alarmsItem.SetIcon(_alarmsBadge = new BadgeDrawable(alarmsItem.Icon));

            if (_alarmTimer == null)
            {
                _alarmTimer = new Timer(10021);
                _alarmTimer.Elapsed +=
                    (sender, args) => RunOnUiThread(() =>
                    {
                        var count = _random.Next(0, 15);
                        if (_lastAlarmCount == count) return;

                        _lastAlarmCount = count;
                        _sections[0].Count = _lastAlarmCount.ToString(CultureInfo.InvariantCulture);
                        _adapter.UpdateData(_sections);
                        _alarmsBadge.SetCountAnimated(_lastAlarmCount);
                    });
                _alarmTimer.Start();
            }

            var eventsItem = menu.FindItem(Resource.Id.action_save);
            eventsItem.SetIcon(_eventsBadge = new BadgeDrawable(eventsItem.Icon));
            if (_eventTimer == null)
            {
                _eventTimer = new Timer(12825);
                _eventTimer.Elapsed +=
                    (sender, args) => RunOnUiThread(() =>
                    {
                        var count = _random.Next(0, 9);
                        if (_lastEventCount == count) return;

                        _lastEventCount = count;
                        _sections[_eventsPosition].Count = _lastEventCount.ToString(CultureInfo.InvariantCulture);
                        _adapter.UpdateData(_sections);
                        _eventsBadge.SetCountAnimated(_lastEventCount);
                    });
                _eventTimer.Start();
            }

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {

            var drawerOpen = _drawerLayout.IsDrawerOpen(_leftDrawerListView);
            if (!drawerOpen)
            {
                _alarmsBadge.SetCountAnimated(_lastAlarmCount);
                _eventsBadge.SetCountAnimated(_lastEventCount);
            }

            //When we open the drawer we usually do not want to show any menu options
            for (int i = 0; i < menu.Size(); i++)
                menu.GetItem(i).SetVisible(!drawerOpen);

            return base.OnPrepareOptionsMenu(menu);
        }

        public override void OnBackPressed()
        {
            UpdateDrawer(_disconnectPosition, _sections[_disconnectPosition].Title);
        }

        public class ConnectedFragmentTypes
        {
            public ConnectedFragmentIds Id { get; set; }
            public string Name { get; set; }

            public ConnectedFragmentTypes(ConnectedFragmentIds id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        public enum ConnectedFragmentIds
        {

            SecurityFragment = 0,
            AreasFragment = 1,
            FaultsFragment = 2,
            OutputsFragment = 3,
            DoorsFragment = 4,
            CamerasFragment = 5,
            EventsFragment = 6,
            DisconnectFragment = 7,
            DealerInfoFragment = 8,
        }

        public void OnPrioritySelected(int piority, string name)
        {
            var eventDetailFragment = EventDetailFragment.NewInstance(name);
            _drawerToggle.DrawerIndicatorEnabled = false;
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_container, eventDetailFragment)
                .AddToBackStack("eventDetail")
                .Commit();
        }

        public void OnFinished()
        {
            UpdateDrawer(_eventsPosition, _sections[_eventsPosition].Title);
        }

        public void OnLeavingAreas()
        {
            UpdateDrawer(0, _sections[0].Title);
        }

        public void OnAdvancedButtonClick()
        {
            var securityFragment = AreasFragment.NewInstance();
            _drawerToggle.DrawerIndicatorEnabled = false;
            ActionBar.Title = _currentSectionTitle = "Areas";
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.frame_container, securityFragment)
                .Commit();
        }
    }
}