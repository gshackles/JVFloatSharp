//  The MIT License (MIT)
//
//  Copyright (c) 2013 Greg Shackles
//  Original implementation by Jared Verdi
//	https://github.com/jverdi/JVFloatLabeledTextField
//  Original Concept by Matt D. Smith
//  http://dribbble.com/shots/1254439--GIF-Mobile-Form-Interaction?list=users
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//  the Software, and to permit persons to whom the Software is furnished to do so,
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//  FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//  COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//  IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using MonoTouch.UIKit;
using System.Drawing;

namespace JVFloatSharp
{
	public class JVFloatLabeledTextField : UITextField 
	{
		private readonly UILabel _floatingLabel;

		public UIColor FloatingLabelTextColor { get; set; }
		public UIColor FloatingLabelActiveTextColor { get; set; }
		public UIFont FloatingLabelFont
		{
			get { return _floatingLabel.Font; }
			set { _floatingLabel.Font = value; }
		}

		public JVFloatLabeledTextField(RectangleF frame)
			: base(frame)
		{
			_floatingLabel = new UILabel() 
			{
				Alpha = 0.0f
			};
			AddSubview(_floatingLabel);

			FloatingLabelTextColor = UIColor.Gray;
			FloatingLabelActiveTextColor = UIColor.Blue;
			FloatingLabelFont = UIFont.BoldSystemFontOfSize(12);
		}

		public override string Placeholder 
		{
			get { return base.Placeholder; }
			set 
			{
				base.Placeholder = value;

				_floatingLabel.Text = value;
				_floatingLabel.SizeToFit();
				_floatingLabel.Frame = new RectangleF(0, _floatingLabel.Font.LineHeight, 
				                                      _floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);
			}
		}

		public override RectangleF TextRect(RectangleF forBounds)
		{
			if (_floatingLabel == null)
				return base.TextRect(forBounds);

			return InsetRect(base.TextRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
		}

		public override RectangleF EditingRect(RectangleF forBounds)
		{
			return InsetRect(base.EditingRect(forBounds), new UIEdgeInsets(_floatingLabel.Font.LineHeight, 0, 0, 0));
		}

		public override RectangleF ClearButtonRect(RectangleF forBounds)
		{
			var rect = base.ClearButtonRect(forBounds);

			return new RectangleF(rect.X, rect.Y + _floatingLabel.Font.LineHeight / 2.0f, 
			                      rect.Size.Width, rect.Size.Height);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			if (IsFirstResponder)
			{
				if (string.IsNullOrEmpty(Text))
				{
					hideFloatingLabel();
				}
				else
				{
					_floatingLabel.TextColor = FloatingLabelActiveTextColor;
					showFloatingLabel();
				}
			}
			else
			{
				_floatingLabel.TextColor = FloatingLabelTextColor;

				if (string.IsNullOrEmpty(Text)) {
					hideFloatingLabel();
				}
				else {
					showFloatingLabel();
				}
			}
		}

		private void showFloatingLabel()
		{
			UIView.Animate(0.3f, 0.0f, UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseOut, () =>
			{
				_floatingLabel.Alpha = 1.0f;
				_floatingLabel.Frame = new RectangleF(_floatingLabel.Frame.X, 2.0f, 
				                                      _floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);
			}, null);
		}

		private void hideFloatingLabel()
		{
			UIView.Animate(0.3f, 0, UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseIn, () =>
			{
				_floatingLabel.Alpha = 0.0f;
			}, () =>
			{
				_floatingLabel.Frame = new RectangleF(_floatingLabel.Frame.X, _floatingLabel.Font.LineHeight, 
				                                      _floatingLabel.Frame.Size.Width, _floatingLabel.Frame.Size.Height);
			});
		}

		private static RectangleF InsetRect(RectangleF rect, UIEdgeInsets insets)
		{
			return new RectangleF(rect.X + insets.Left, 
			                      rect.Y + insets.Top, 
			                      rect.Width - insets.Left - insets.Right, 
			                      rect.Height - insets.Top - insets.Bottom);
		}
	}
}