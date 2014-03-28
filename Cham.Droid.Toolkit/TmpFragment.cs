using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace Cham.Droid.Toolkit
{
    class TmpFragment : Fragment
    {
        public event ActivityResultHandler ActivityResult; 

        public TmpFragment()
        {
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            if (ActivityResult != null)
            {
                ActivityResult(this, new ActivityResultEventArgs(requestCode, resultCode, data));
            }
            base.OnActivityResult(requestCode, resultCode, data);
            var fm = ((FragmentActivity)Activity).SupportFragmentManager;
            fm.BeginTransaction().Remove(this).Commit();
        }
    }

    class ActivityResultEventArgs : EventArgs
    {
       
        public  readonly int RequestCode;
        public readonly int ResultCode;
        public readonly Intent Data;

        public ActivityResultEventArgs(int requestCode, int resultCode, Intent data)
        {
            Data = data;
            ResultCode = resultCode;
            RequestCode = requestCode;
        }
    }

    delegate void ActivityResultHandler(object sender, ActivityResultEventArgs data);
}