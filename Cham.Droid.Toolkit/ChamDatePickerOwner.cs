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
using Android.Util;

namespace Cham.Droid.Toolkit
{
    class ChamDatePickerOwner : ChamTextOwner, IChamDatePicker
    {
        #region Fields


        #endregion

        #region Contructor

        public ChamDatePickerOwner(TextView headerTextView, TextView textView, IAttributeSet attrs, int defStyle)
            : base(headerTextView, textView, attrs, defStyle)
        {
        }

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion


        private DateTime? _value;

        public DateTime? Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    TextView.Text = value != null ? ((DateTime) value).ToShortDateString() : string.Empty;
                }
            }
        }

        public string Error
        {
            set { base.TextView.Error = value; }
        }
    }
}