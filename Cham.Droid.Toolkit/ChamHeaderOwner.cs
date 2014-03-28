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
    public class ChamHeaderOwner : IChamHeader
    {
        #region Fields

        public readonly TextView HeaderTextView;
        protected const string RequiredSuffix = " *";

        #endregion

        #region Contructor

        public ChamHeaderOwner(TextView headerTextView, IAttributeSet attrs, int defStyle)
        {
            HeaderTextView = headerTextView;
            var a = HeaderTextView.Context.Theme.ObtainStyledAttributes(attrs, Resource.Styleable.ChamHeader, defStyle, Resource.Style.Header);
            try
            {
                for (var i = 0; i < a.IndexCount; i++)
                {
                    var attr = a.GetIndex(i);
                    if (attr == Resource.Styleable.ChamHeader_Header)
                        Header = a.GetString(attr);
                    else if (attr == Resource.Styleable.ChamHeader_HeaderStyle)
                    {
                        HeaderTextView.SetTextAppearance(HeaderTextView.Context, a.GetResourceId(attr, Android.Resource.Style.TextAppearance));
                    }
                }
            }
            finally
            {
                a.Recycle();
            }
        }

        #endregion

        #region Properties

        protected string _header;

        public string Header
        {
            get { return HeaderTextView.Text; }
            set
            {
                _header = value;
                UpdateHeader();
            }
        }

        protected bool _required;

        public bool Required
        {
            get { return _required; }
            set
            {
                _required = value;
                UpdateHeader();
            }
        }

        #endregion

        #region Methods

        protected virtual void UpdateHeader()
        {
            HeaderTextView.Text = string.Format("{0}{1}"
                , _header
                ,_required ? RequiredSuffix : string.Empty);
        }

        #endregion
    }
}