using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.App;

namespace Cham.Droid.Toolkit
{
	class TmpFragment : Fragment
	{
		public event ActivityResultHandler ActivityResult;

		public TmpFragment ()
		{
		}

		public override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (ActivityResult != null)
			{
				ActivityResult (this, new ActivityResultEventArgs (requestCode, resultCode, data));
			}
			base.OnActivityResult (requestCode, resultCode, data);
			var fm = ((Activity)Activity).FragmentManager;
			fm.BeginTransaction ().Remove (this).Commit ();
		}
	}

	class ActivityResultEventArgs : EventArgs
	{
		public  readonly int RequestCode;
		public readonly Result ResultCode;
		public readonly Intent Data;

		public ActivityResultEventArgs (int requestCode, Result resultCode, Intent data)
		{
			Data = data;
			ResultCode = resultCode;
			RequestCode = requestCode;
		}
	}
	delegate void ActivityResultHandler (object sender, ActivityResultEventArgs data);
}