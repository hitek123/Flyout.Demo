using Android.Animation;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace Bosch.FlyoutDemo.Views
{
	public class BadgeDrawable : Drawable
	{
	    readonly Drawable _child;
	    readonly Paint _badgePaint;
	    readonly Paint _textPaint;
	    readonly RectF _badgeBounds = new RectF ();
	    readonly Rect _txtBounds = new Rect ();
		int _count;
		int _alpha = 0xFF;

		ValueAnimator alphaAnimator;

		public BadgeDrawable (Drawable child)
		{
			this._child = child;
			_badgePaint = new Paint {
				AntiAlias = true,
				Color = Color.Blue,
			};
			_textPaint = new Paint {
				AntiAlias = true,
				Color = Android.Graphics.Color.White,
				TextSize = 16,
				TextAlign = Paint.Align.Center
			};
		}

		public int Count {
			get { return _count; }
			set {
				_count = value;
				InvalidateSelf ();
			}
		}

		public void SetCountAnimated (int count)
		{
			if (alphaAnimator != null) {
				alphaAnimator.Cancel ();
				alphaAnimator = null;
			}
			const int Duration = 300;

			alphaAnimator = ObjectAnimator.OfInt (this, "alpha", 0xFF, 0);
			alphaAnimator.SetDuration (Duration);
			alphaAnimator.RepeatMode = ValueAnimatorRepeatMode.Reverse;
			alphaAnimator.RepeatCount = 1;
			alphaAnimator.AnimationRepeat += (sender, e) => {
				((Animator)sender).RemoveAllListeners ();
				this._count = count;
			};
			alphaAnimator.Start ();
		}

		public override void Draw (Canvas canvas)
		{
			_child.Draw (canvas);
			if (_count <= 0)
				return;
			_badgePaint.Alpha = _textPaint.Alpha = _alpha;
			_badgeBounds.Set (0, 0, Bounds.Width () / 2, Bounds.Height () / 2);
			canvas.DrawRoundRect (_badgeBounds, 8, 8, _badgePaint);
			_textPaint.TextSize = (8 * _badgeBounds.Height ()) / 10;
			var text = _count.ToString ();
			_textPaint.GetTextBounds (text, 0, text.Length, _txtBounds);
			canvas.DrawText (
				text,
				_badgeBounds.CenterX (),
				_badgeBounds.Bottom - (_badgeBounds.Height () - _txtBounds.Height ()) / 2 - 1,
				_textPaint
			);
		}

		protected override void OnBoundsChange (Rect bounds)
		{
			base.OnBoundsChange (bounds);
			_child.SetBounds (bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
		}

		public override int IntrinsicWidth {
			get {
				return _child.IntrinsicWidth;
			}
		}

		public override int IntrinsicHeight {
			get {
				return _child.IntrinsicHeight;
			}
		}

		public override void SetAlpha (int alpha)
		{
			this._alpha = alpha;
			InvalidateSelf ();
		}

		public override void SetColorFilter (ColorFilter cf)
		{
			_child.SetColorFilter (cf);
		}

		public override int Opacity {
			get {
				return _child.Opacity;
			}
		}
	}
}

