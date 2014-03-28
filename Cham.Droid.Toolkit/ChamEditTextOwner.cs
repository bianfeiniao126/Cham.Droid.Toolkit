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
	public class ChamEditTextOwner : ChamTextOwner, IChamEditText
	{
		#region Fields

		public readonly EditText EditText;

		#endregion

		#region Contructor

		public ChamEditTextOwner (TextView headerTextView, EditText editText, IAttributeSet attrs, int defStyle)
			: base (headerTextView, editText, attrs, defStyle)
		{
			EditText = editText;
			var a = EditText.Context.Theme.ObtainStyledAttributes (attrs, Resource.Styleable.ChamEditText, defStyle, Resource.Style.ChamEditText);
			var readOnly = false;
			var inputType = -1;
			var imeOptions = -1;
			var lines = -1;
			var minLines = -1;
			var maxLines = -1;
			var singleLine = false;

			try
			{
				for (var i = 0; i < a.IndexCount; i++)
				{
					var attr = a.GetIndex (i);
                   
					if (attr == Resource.Styleable.ChamEditText_ReadOnly)
						readOnly = a.GetBoolean (attr, false);
					else if (attr == Resource.Styleable.ChamEditText_android_inputType)
						inputType = a.GetInt (attr, -1);
					else if (attr == Resource.Styleable.ChamEditText_android_imeOptions)
						imeOptions = a.GetInt (attr, -1);
					else if (attr == Resource.Styleable.ChamEditText_android_lines)
						lines = a.GetInt (attr, -1);
					else if (attr == Resource.Styleable.ChamEditText_android_singleLine)
						singleLine = a.GetBoolean (attr, false);
					else if (attr == Resource.Styleable.ChamEditText_android_minLines)
						minLines = a.GetInt (attr, -1);
					else if (attr == Resource.Styleable.ChamEditText_android_maxLines)
						maxLines = a.GetInt (attr, -1);
				}

			} finally
			{
				a.Recycle ();
			}
			if (readOnly)
				EditText.KeyListener = null;
			if (inputType != -1)
				EditText.InputType = (Android.Text.InputTypes)inputType;
			if (imeOptions != -1)
				EditText.ImeOptions = (Android.Views.InputMethods.ImeAction)imeOptions;
			if (lines != -1)
				EditText.SetLines (lines);
			if (singleLine)
				EditText.SetSingleLine ();
		}

		#endregion

		#region Properties

		public string Error
		{
			set { EditText.Error = value; }
		}

		#endregion
	}
}