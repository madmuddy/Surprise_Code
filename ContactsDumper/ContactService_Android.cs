using Android.Provider;
using System.Collections.Generic;

namespace Surprise
{
    public partial class MainActivity
    {
        public class ContactService_Android
        {
            public IEnumerable<PhoneContact> GetAllContacts()
            {
                var phoneContacts = new List<PhoneContact>();

                using (var phones = Android.App.Application.Context.ContentResolver.Query(ContactsContract.CommonDataKinds.Phone.ContentUri, null, null, null, null))
                {
                    if (phones != null)
                    {
                        while (phones.MoveToNext())
                        {
                            try
                            {
                                string name = phones.GetString(phones.GetColumnIndex(ContactsContract.Contacts.InterfaceConsts.DisplayName));
                                string phoneNumber = phones.GetString(phones.GetColumnIndex(ContactsContract.CommonDataKinds.Phone.Number));

                                string[] words = name.Split(' ');
                                var contact = new PhoneContact();
                                contact.FirstName = words[0];
                                if (words.Length > 1)
                                    contact.LastName = words[1];
                                else
                                    contact.LastName = ""; //no last name
                                contact.PhoneNumber = phoneNumber;
                                phoneContacts.Add(contact);
                            }
                            catch
                            {
                                //something wrong with one contact, may be display name is completely empty, decide what to do
                            }
                        }
                        phones.Close();
                    }
                    // if we get here, we can't access the contacts. Consider throwing an exception to display to the user
                }

                return phoneContacts;
            }
        }
    }

    public class PhoneContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        public string Name { get => $"{FirstName} {LastName}"; }

    }
}
