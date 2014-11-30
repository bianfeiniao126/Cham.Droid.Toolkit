using System;
using Android.Content;
using Android.Util;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Graphics;

namespace Cham.Droid.Toolkit
{
	public class ClearableEditText : EditText
	{
		private Drawable xD;
		public EventHandler AfterTextChangedEx;

		protected ClearableEditText (IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer) : base (javaReference, transfer)
		{
			Init ();
		}

		public ClearableEditText (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			Init ();
		}

		public ClearableEditText (Context context, IAttributeSet attrs) : base (context, attrs)
		{
		}

		public ClearableEditText (Context context) : base (context)
		{
			Init ();
		}

		protected bool ClearIconVisible
		{
			set
			{ 
				Drawable x = value ? xD : null;
				SetCompoundDrawables (GetCompoundDrawables () [0], GetCompoundDrawables () [1], x, GetCompoundDrawables () [3]);
			}
		}

		public override bool OnTouchEvent (MotionEvent e)
		{
			if (GetCompoundDrawables () [2] != null)
			{
				bool tappedX = e.GetX () > (Width - PaddingRight - xD.IntrinsicWidth);
				if (tappedX)
				{
					if (e.Action == MotionEventActions.Up)
					{
						Text = string.Empty;
					}
					return true;
				}
			}
			return base.OnTouchEvent (e);
		}

	    protected override void OnFocusChanged(bool gainFocus, FocusSearchDirection direction, Rect previouslyFocusedRect)
	    {
	        if (IsFocused)
	        {
	            ClearIconVisible = Text != string.Empty && KeyListener != null && Enabled;
	        }
	        else
	        {
	            ClearIconVisible = false;
	        }
	        base.OnFocusChanged(gainFocus, direction, previouslyFocusedRect);
	    }

	    protected override void OnTextChanged (Java.Lang.ICharSequence text, int start, int before, int after)
		{
			base.OnTextChanged (text, start, before, after);
			if (IsFocused)
				ClearIconVisible = Text != string.Empty;
		}

		private void Init ()
		{
			AfterTextChanged += ClearableEditTextAfterTextChanged;
			xD = GetCompoundDrawables () [2];
			if (xD == null)
			{
				xD = Resources.GetDrawable (/*GetDefaultClearIconId ()*/Resource.Drawable.clear);
			}
			xD.SetBounds (0, 0, 25, 25);
			ClearIconVisible = false;
		}

		private void ClearableEditTextAfterTextChanged (object sender, EventArgs e)
		{
			//OnAfterTextChanged ();
		}

		private void OnAfterTextChanged ()
		{
			if (AfterTextChangedEx != null)
				AfterTextChangedEx (this, EventArgs.Empty);
		}

		private int GetDefaultClearIconId ()
		{
			int id = Resources.GetIdentifier ("ic_clear", "drawable", "android");
			if (id == 0)
			{
				id = Android.Resource.Drawable.PresenceOffline;
			}
			return id;
		}

		public void ForceTextChanged()
		{
			var evt = AfterTextChangedEx;
			if (evt != null)
				evt(this, EventArgs.Empty);
		}
	}
}

