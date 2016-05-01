using Android.App;
using Android.Content;

namespace Bosch.FlyoutDemo.Helpers
{
    public static class DialogHelpers
    {
         public static void ShowAlert(Context context, string title, string message, string buttonText)
         {
             var alertdialog = new AlertDialog.Builder(context);
             alertdialog.SetTitle(title);
             alertdialog.SetMessage(message);
             alertdialog.SetPositiveButton(buttonText, (sender, args) => { });
             alertdialog.Show();
         }
    }
}