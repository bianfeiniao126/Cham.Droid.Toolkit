using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cham.Droid.Toolkit
{
    public class ChamDatePicker : LinearLayout, DatePickerDialog.IOnDateSetListener, IChamDatePicker
	{
		private Button Button;

		public ChamDatePicker (Context context)
            : base (context)
		{
			((Activity)Context).LayoutInflater.Inflate (Resource.Layout.ChamDatePickerLayout, this);
		}

		public ChamDatePicker (Context context, IAttributeSet attrs)
            : this(context, attrs, Resource.Attribute.ChamDatePickerStyle)
		{

		}

        public ChamDatePicker(Context context, IAttributeSet attrs, int defStyle)
            : base (context, attrs, defStyle)
		{
            ((Activity)Context).LayoutInflater.Inflate(Resource.Layout.ChamDatePickerLayout, this);
            var headerTextView = FindViewById<TextView>(Resource.Id.ChamHeader);
            var textView = FindViewById<TextView>(Resource.Id.ChamTextView);

            ChamDatePickerOwner = new ChamDatePickerOwner(headerTextView, textView, attrs, defStyle);
            Button = FindViewById<Button>(Resource.Id.ChamDatePickerButton);
			Button.Click += Button_Click;
			Button.SetBackgroundResource (Android.Resource.Drawable.IcMenuMyCalendar);
		}

        #region Properties

        private ChamDatePickerOwner ChamDatePickerOwner { get; set; }

        public string Error
        {
            set { ChamDatePickerOwner.Error = value; }
        }

        public bool Required
        {
            get { return ChamDatePickerOwner.Required; }
            set { ChamDatePickerOwner.Required = value; }
        }

        public string Header
        {
            get { return ChamDatePickerOwner.Header; }
            set { ChamDatePickerOwner.Header = value; }
        }

        #endregion


		public DateTime? Value
		{
            get { return ChamDatePickerOwner.Value; }
			set { ChamDatePickerOwner.Value = value; }
		}

		public event EventHandler ValueChanged;

		void Button_Click (object sender, EventArgs e)
		{
		    var value = Value ?? DateTime.Today;
            var dialog = new DatePickerDialog(Context, this, value.Year, value.Month, value.Day);
			dialog.Show ();
		}

		public void OnDateSet (DatePicker view, int year, int monthOfYear, int dayOfMonth)
		{
			Value = new DateTime (year, monthOfYear, dayOfMonth);
			if (ValueChanged != null)
				ValueChanged (this, EventArgs.Empty);
		}


        
    }
}
