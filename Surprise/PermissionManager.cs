using Android.Content.PM;
using AndroidX.Core.App;

namespace Surprise
{
    public partial class MainActivity
    {
        class PermissionManager
        {
            // [ Handles All Permission Releated Things ]

            // Get SPecific Permissions
            public void GainPermissions(MainActivity activity, string[] permissions)
            {
                ActivityCompat.RequestPermissions(activity, permissions, 1);
            }

            // Specif Permisison is Gained or Not
            public bool CheckPermissionGranted(MainActivity activity, string Permissions)
            {
                // Check if the permission is already available.
                if (ActivityCompat.CheckSelfPermission(activity, Permissions) != Permission.Granted)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
