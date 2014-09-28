using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace Cham.Droid.Toolkit
{
	public class ChamTextView : LinearLayout, IChamText
	{
		#region Constructors

		public ChamTextView (Context context) : this (context, null)
		{
			((Activity)Context).LayoutInflater.Inflate (Resource.Layout.ChamTextViewLayout, this);
		}

		public ChamTextView (Context context, IAttributeSet attrs) : this (context, attrs, Resource.Attribute.ChamTextStyle)
		{
            
		}

		public ChamTextView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			((Activity)Context).LayoutInflater.Inflate (Resource.Layout.ChamTextViewLayout, this);
			var headerTextView = FindViewById<TextView> (Resource.Id.ChamHeader);
            //headerTextView.PaintFlags = headerTextView.PaintFlags | PaintFlags.UnderlineText;
			var textView = FindViewById<TextView> (Resource.Id.ChamTextView);
			ChamTextOwner = new ChamTextOwner (headerTextView, textView, attrs, defStyle);
		}

		#endregion

		#region Properties

		private ChamTextOwner ChamTextOwner { get; set; }

		public string Text
		{
			get { return ChamTextOwner.Text; }
			set { ChamTextOwner.Text = value; }
		}

		public string Header
		{
			get { return ChamTextOwner.Header; }
			set { ChamTextOwner.Header = value; }
		}

		public bool Currency
		{
			get { return ChamTextOwner.Currency; }
			set { ChamTextOwner.Currency = value; }
		}

		#endregion
	}
}