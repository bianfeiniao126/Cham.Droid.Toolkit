using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cham.Droid.Toolkit
{
	public class MaxWidthLinearLayout : LinearLayout
	{
		private readonly int maxWidth;

		public MaxWidthLinearLayout (Android.Content.Context context)
            : base (context)
		{
			maxWidth = 0;
		}

		public MaxWidthLinearLayout (Android.Content.Context context, IAttributeSet attrs)
            : base (context, attrs)
		{
			TypedArray a = Context.ObtainStyledAttributes (attrs, Resource.Styleable.MaxWidthLinearLayout);
			maxWidth = a.GetDimensionPixelSize (Resource.Styleable.MaxWidthLinearLayout_maxWidth, Int32.MaxValue);
		}

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			int measuredWidth = MeasureSpec.GetSize (widthMeasureSpec);
			if (maxWidth > 0 && maxWidth < measuredWidth)
			{
				widthMeasureSpec = (int)MeasureSpec.MakeMeasureSpec (maxWidth, MeasureSpec.GetMode (widthMeasureSpec));
			}
			base.OnMeasure (widthMeasureSpec, heightMeasureSpec);
		}
	}
}
