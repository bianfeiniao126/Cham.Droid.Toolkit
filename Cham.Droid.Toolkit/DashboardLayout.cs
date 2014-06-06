using System;
using Android.Views;
using Android.Content;
using Android.Util;

namespace Cham.Droid.Toolkit
{
    public class DashboardLayout : ViewGroup
    {

		private int UNEVEN_GRID_PENALTY_MULTIPLIER = 10;

        private int mMaxChildWidth = 0;
        private int mMaxChildHeight = 0;

        public DashboardLayout(Context context) : base(context, null)
        {

        }

        public DashboardLayout(Context context, IAttributeSet attrs) : base(context, attrs, 0)
        {
        }

        public DashboardLayout(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {

            mMaxChildWidth = 0;
            mMaxChildHeight = 0;

            // Measure once to find the maximum child size.

            int childWidthMeasureSpec = MeasureSpec.MakeMeasureSpec(
                MeasureSpec.GetSize(widthMeasureSpec), MeasureSpecMode.AtMost);
            int childHeightMeasureSpec = MeasureSpec.MakeMeasureSpec(
                MeasureSpec.GetSize(widthMeasureSpec), MeasureSpecMode.AtMost);

            int count = ChildCount;
            for (int i = 0; i < count; i++)
            {
                View child = GetChildAt(i);
                if (child.Visibility == ViewStates.Gone)
                {
                    continue;
                }

                child.Measure(childWidthMeasureSpec, childHeightMeasureSpec);

                mMaxChildWidth = Math.Max(mMaxChildWidth, child.MeasuredWidth);
                mMaxChildHeight = Math.Max(mMaxChildHeight, child.MeasuredHeight);
            }

            // Measure again for each child to be exactly the same size.

            childWidthMeasureSpec = MeasureSpec.MakeMeasureSpec(mMaxChildWidth, MeasureSpecMode.Exactly);
            childHeightMeasureSpec = MeasureSpec.MakeMeasureSpec(mMaxChildHeight, MeasureSpecMode.Exactly);

            for (int i = 0; i < count; i++)
            {
                View child = GetChildAt(i);
                if (child.Visibility == ViewStates.Gone)
                {
                    continue;
                }

                child.Measure(childWidthMeasureSpec, childHeightMeasureSpec);
            }

            SetMeasuredDimension(
                ResolveSize(mMaxChildWidth, widthMeasureSpec),
                ResolveSize(mMaxChildHeight, heightMeasureSpec));
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            int width = r - l;
            int height = b - t;

            int count = ChildCount;

            // Calculate the number of visible children.
            int visibleCount = 0;
            for (int i = 0; i < count; i++)
            {
                View child = GetChildAt(i);
                if (child.Visibility == ViewStates.Gone)
                {
                    continue;
                }
                ++visibleCount;
            }

            if (visibleCount == 0)
            {
                return;
            }

            // Calculate what number of rows and columns will optimize for even horizontal and
            // vertical whitespace between items. Start with a 1 x N grid, then try 2 x N, and so on.
            int bestSpaceDifference = Int32.MaxValue;
            int spaceDifference;

            // Horizontal and vertical space between items
            int hSpace = 0;
            int vSpace = 0;

            int cols = 1;
            int rows;

            while (true)
            {
                rows = (visibleCount - 1)/cols + 1;

                hSpace = ((width - mMaxChildWidth*cols)/(cols + 1));
                vSpace = ((height - mMaxChildHeight*rows)/(rows + 1));

                spaceDifference = Math.Abs(vSpace - hSpace);
                if (rows*cols != visibleCount)
                {
                    spaceDifference *= UNEVEN_GRID_PENALTY_MULTIPLIER;
                }

                if (spaceDifference < bestSpaceDifference)
                {
                    // Found a better whitespace squareness/ratio
                    bestSpaceDifference = spaceDifference;

                    // If we found a better whitespace squareness and there's only 1 row, this is
                    // the best we can do.
                    if (rows == 1)
                    {
                        break;
                    }
                }
                else
                {
                    // This is a worse whitespace ratio, use the previous value of cols and exit.
                    --cols;
                    rows = (visibleCount - 1)/cols + 1;
                    hSpace = ((width - mMaxChildWidth*cols)/(cols + 1));
                    vSpace = ((height - mMaxChildHeight*rows)/(rows + 1));
                    break;
                }

                ++cols;
            }

            // Lay out children based on calculated best-fit number of rows and cols.

            // If we chose a layout that has negative horizontal or vertical space, force it to zero.
            hSpace = Math.Max(0, hSpace);
            vSpace = Math.Max(0, vSpace);

            // Re-use width/height variables to be child width/height.
            width = (width - hSpace*(cols + 1))/cols;
            height = (height - vSpace*(rows + 1))/rows;

            int left, top;
            int col, row;
            int visibleIndex = 0;
            for (int i = 0; i < count; i++)
            {
                View child = GetChildAt(i);
                if (child.Visibility == ViewStates.Gone)
                {
                    continue;
                }

                row = visibleIndex/cols;
                col = visibleIndex%cols;

                left = hSpace*(col + 1) + width*col;
                top = vSpace*(row + 1) + height*row;

                child.Layout(left, top,
                    (hSpace == 0 && col == cols - 1) ? r : (left + width),
                    (vSpace == 0 && row == rows - 1) ? b : (top + height));
                ++visibleIndex;
            }
        }
    }
}

