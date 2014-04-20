using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Net;
using System.IO;

namespace Cham.Droid.Toolkit
{
	public class ChamImageView : ImageView
	{
		#region Fields

		protected static int CAMERA_REQUEST = 0;
		protected static int GALLERY_PICTURE = 1;
		string selectedImagePath;
		public System.EventHandler ImageChanged;

		#endregion

		#region Constructors

		protected ChamImageView (System.IntPtr javaReference, Android.Runtime.JniHandleOwnership transfer)
            : base (javaReference, transfer)
		{
			
		}

		public ChamImageView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			Click += ChamImageView_Click;
		}

		public ChamImageView (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			Click += ChamImageView_Click;
		}

		public ChamImageView (Context context)
            : base (context)
		{
			
		}

		#endregion

		#region Properties

		private byte[] _image;

		public byte[] Image
		{
			get { return _image; }
			set
			{
				_image = value;
				if (_image != null)
				{
					BitmapFactory.Options options = new BitmapFactory.Options ();
					options.InJustDecodeBounds = true;
					var bitmap = BitmapFactory.DecodeByteArray (_image, 0, _image.Length, options);
					SetImageBitmap (bitmap);
				}
				if (ImageChanged != null)
					ImageChanged (this, System.EventArgs.Empty);
				if (_image == null)
					SetImageResource (Resource.Drawable.picture);
			}
		}

		#endregion

		#region Methods

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);
			var paint = new Paint ();
			paint.SetStyle (Paint.Style.Stroke);
			paint.Color = Color.Black;
			canvas.DrawRect (0, 0, Width - 1, Height - 1, paint);
		}

		private void StartDialog ()
		{
			var myAlertDialog = new AlertDialog.Builder (Context);
			myAlertDialog.SetTitle (Resource.String.Options);
			myAlertDialog.SetMessage (Resource.String.HowDoYouWantToSetYourPicture);

			Intent pictureActionIntent = null;
			int code;

			var res = Context.Resources;

			myAlertDialog.SetPositiveButton (res.GetString (Resource.String.Gallery), delegate
			{
				pictureActionIntent = new Intent (Intent.ActionGetContent, null);
				pictureActionIntent.SetType ("image/*");
				pictureActionIntent.PutExtra ("return-data", true);
				code = GALLERY_PICTURE;
				var fm = ((Activity)Context).FragmentManager;
				var tmpFragment = new TmpFragment ();
				tmpFragment.ActivityResult += tmpFragment_ActivityResult;
				fm.BeginTransaction ().Add (tmpFragment, "FRAGMENT_TAG").Commit ();
				fm.ExecutePendingTransactions ();
				tmpFragment.StartActivityForResult (pictureActionIntent, code);
			}).SetNegativeButton (res.GetString (Resource.String.Camera), delegate
			{
				pictureActionIntent = new Intent (Android.Provider.MediaStore.ActionImageCapture);
				code = CAMERA_REQUEST;
				var fm = ((Activity)Context).FragmentManager;
				var tmpFragment = new TmpFragment ();
				tmpFragment.ActivityResult += tmpFragment_ActivityResult;
				fm.BeginTransaction ().Add (tmpFragment, "FRAGMENT_TAG").Commit ();
				fm.ExecutePendingTransactions ();
				tmpFragment.StartActivityForResult (pictureActionIntent, code);
			});

			if (Image != null)
			{
				myAlertDialog.SetNeutralButton (res.GetString (Resource.String.Delete), delegate
				{
					Image = null;
					SetImageBitmap (null);
					DestroyDrawingCache ();
				});
			}
			myAlertDialog.Show ();
		}

		#endregion

		#region Events

		void ChamImageView_Click (object sender, System.EventArgs e)
		{
			StartDialog ();
		}

		private void tmpFragment_ActivityResult (object sender, ActivityResultEventArgs e)
		{
			if (e.RequestCode == GALLERY_PICTURE)
			{
				if (e.ResultCode == Result.Ok)
				{
					if (e.Data != null)
					{
						// our BitmapDrawable for the thumbnail
						BitmapDrawable bmpDrawable = null;
						// try to retrieve the image using the data from the intent
						var cursor = Context.ContentResolver.Query (e.Data.Data,
							             null, null, null, null);
						if (cursor != null)
						{
							cursor.MoveToFirst ();
							int idx = cursor.GetColumnIndex (MediaStore.Images.ImageColumns.Data);
							var fileSrc = cursor.GetString (idx);
							var bitmap = BitmapFactory.DecodeFile (fileSrc); // load
							// preview
							// image
							var stream = new MemoryStream ();
							bitmap.Compress (Bitmap.CompressFormat.Png, 100, stream);
							Image = stream.ToArray ();

							//bitmap = Bitmap.CreateScaledBitmap (bitmap, 100, 100, false);
							//SetImageBitmap (bitmap);

						} else
						{
							bmpDrawable = new BitmapDrawable (Resources, e.Data.Data.Path);
							SetImageDrawable (bmpDrawable);
						}

					} else
					{
						Toast.MakeText (Context, "Cancelled", ToastLength.Short).Show ();
					}
				} else if (e.ResultCode == 0)
				{
					Toast.MakeText (Context, "Cancelled",
						ToastLength.Short).Show ();
				}
			} else if (e.RequestCode == CAMERA_REQUEST)
			{
				if (e.ResultCode == Result.Ok)
				{
					if (e.Data.HasExtra ("data"))
					{
						// retrieve the bitmap from the intent
						var bitmap = (Bitmap)e.Data.Extras.Get ("data");
						var cursor = Context.ContentResolver
                            .Query (Android.Provider.MediaStore.Images.Media.ExternalContentUri,
							             new string[] {
								MediaStore.Images.ImageColumns.Data,
								MediaStore.Images.ImageColumns.DateAdded,
								MediaStore.Images.ImageColumns.Orientation
							},
							             MediaStore.Images.ImageColumns.DateAdded, null, "date_added ASC");
						if (cursor != null && cursor.MoveToFirst ())
						{
							do
							{
								Uri uri = Uri.Parse (cursor.GetString (cursor
                                    .GetColumnIndex (MediaStore.Images.ImageColumns.Data)));
								selectedImagePath = uri.ToString ();
							} while (cursor.MoveToNext ());
							cursor.Close ();
						}

						Log.Info ("path of the image from camera ====> ", selectedImagePath);


						bitmap = Bitmap.CreateScaledBitmap (bitmap, 100, 100, false);
						// update the image view with the bitmap
						SetImageBitmap (bitmap);
					} else if (e.Data.Extras == null)
					{
						Toast.MakeText (Context, "No extras to retrieve!", ToastLength.Short).Show ();
						var thumbnail = new BitmapDrawable (Resources, e.Data.Data.Path);

						SetImageDrawable (thumbnail);

					}
				}
			}
		}

		#endregion
	}
}