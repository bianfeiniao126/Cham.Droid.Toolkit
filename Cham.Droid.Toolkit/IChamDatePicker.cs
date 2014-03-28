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
using Java.Sql;

namespace Cham.Droid.Toolkit
{
    public interface IChamDatePicker : IChamValidation
    {
        DateTime? Value { get; set; }
    }
}