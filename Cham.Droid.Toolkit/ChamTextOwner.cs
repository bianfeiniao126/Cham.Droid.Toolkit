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
    public class ChamTextOwner : ChamHeaderOwner, IChamText
    {
        #region Fields

        public readonly TextView TextView;

        #endregion

        #region Contructor

        public ChamTextOwner(TextView headerTextView, TextView textView, IAttributeSet attrs, int defStyle)
            : base(headerTextView, attrs, defStyle)
        {
            TextView = textView;
            var a = TextView.Context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.ChamText, defStyle, Resource.Style.ChamTextView);
            var text = string.Empty;
            var textStyle = Android.Resource.Style.TextAppearance;
            try
            {
                for (var i = 0; i < a.IndexCount; i++)
                {
                    var attr = a.GetIndex(i);
                    if (attr == Resource.Styleable.ChamText_Text)
                        text = a.GetString(attr);
                    else if (attr == Resource.Styleable.ChamText_TextStyle)
                        textStyle = a.GetResourceId(attr, textStyle);
                    else if (attr == Resource.Styleable.ChamText_Currency)
                        Currency = a.GetBoolean(attr, false);
                }
            }
            finally
            {
                a.Recycle();
            }

            TextView.SetTextAppearance(TextView.Context, textStyle);
            Text = text;
        }

        #endregion

        #region Properties

        public string Text
        {
            get { return TextView.Text; }
            set { TextView.Text = value; }
        }

        private bool _currency;
        public bool Currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                UpdateHeader();
            }
        }

        #endregion

        #region Methods

        protected override void UpdateHeader()
        {
            base.UpdateHeader();
            HeaderTextView.Text = string.Format("{0}{1}{2}"
                , _header
                , _currency ? string.Format(" ({0})", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol) : string.Empty
                , _required ? RequiredSuffix : string.Empty);
        }

		public bool RequestFocus()
		{
			return TextView.RequestFocus ();
		}

        #endregion
    }
}