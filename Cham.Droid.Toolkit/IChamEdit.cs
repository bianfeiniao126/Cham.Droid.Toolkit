using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Cham.Droid.Toolkit
{
    public interface IChamValidation : IChamHeader
    {
        string Error { set; }

        bool Required { get; set; }
    }
}