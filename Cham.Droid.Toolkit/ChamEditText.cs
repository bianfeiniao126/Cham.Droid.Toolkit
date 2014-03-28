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
	public class ChamEditText : LinearLayout, IChamEditText
	{
		public EventHandler AfterTextChanged;

		public ChamEditText (Context context) : this (context, null)
		{
			((Activity)Context).LayoutInflater.Inflate (LayoutId, this);
		}

		public ChamEditText (Context context, IAttributeSet attrs)
            : this (context, attrs, Resource.Attribute.ChamEditTextStyle)
		{
            
		}

		public ChamEditText (Context context, IAttributeSet attrs, int defStyle)
            : base (context, attrs, defStyle)
		{
			((Activity)Context).LayoutInflater.Inflate (LayoutId, this);
			var headerTextView = FindViewById<TextView> (Resource.Id.ChamHeader);
			var editText = FindViewById<ClearableEditText> (Resource.Id.ChamTextEdit_EditText);
			ChamEditTextOwner = new ChamEditTextOwner (headerTextView, editText, attrs, defStyle);
			editText.FocusChange += editText_FocusChange;
			editText.AfterTextChanged += EditTextAfterTextChanged;
		}

		void editText_FocusChange (object sender, View.FocusChangeEventArgs e)
		{
			if (!IsFocused)
			{
				OnAfterTextChanged ();
			}
		}

		#region Properties

		protected virtual int LayoutId { get { return Resource.Layout.ChamEditTextLayout; } }

		protected ChamEditTextOwner ChamEditTextOwner { get; set; }

		public string Text
		{
			get { return ChamEditTextOwner.Text; }
			set { ChamEditTextOwner.Text = value; }
		}

		public string Header
		{
			get { return ChamEditTextOwner.Header; }
			set { ChamEditTextOwner.Header = value; }
		}

		public string Error
		{
			set { ChamEditTextOwner.Error = value; }
		}

		public bool Required
		{
			get { return ChamEditTextOwner.Required; }
			set { ChamEditTextOwner.Required = value; }
		}

		public bool Currency
		{
			get { return ChamEditTextOwner.Currency; }
			set { ChamEditTextOwner.Currency = value; }
		}

		#endregion

		#region Methods

		private void OnAfterTextChanged ()
		{
			if (AfterTextChanged != null)
				AfterTextChanged (this, EventArgs.Empty);
		}

		#endregion

		#region Events

		private void EditTextAfterTextChanged (object sender, EventArgs e)
		{
			//OnAfterTextChanged ();
		}

		#endregion
	}
}