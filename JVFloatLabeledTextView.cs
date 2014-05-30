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

using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace JVFloatSharp
{
    public class JVFloatLabeledTextView : UITextView
	{
        private readonly UIColor _defaultiOSPlaceholderColor = UIColor.FromWhiteAlpha(0.702f, 1.0f);
        private readonly List<NSObject> _subscriptions = new List<NSObject>();
        private readonly float _startingTextContainerInsetTop;
        private readonly UILabel _placeholderLabel;
        private readonly UILabel _floatingLabel;

        public UIColor FloatingLabelTextColor { get; set; }
        public UIColor FloatingLabelActiveTextColor { get; set; }
        public UIFont FloatingLabelFont
        {
            get { return _floatingLabel.Font; }
            set { _floatingLabel.Font = value; }
        }

        public JVFloatLabeledTextView(RectangleF frame)
            :base(frame)
        {
            _startingTextContainerInsetTop = TextContainerInset.Top;

            _placeholderLabel = new UILabel
            {
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                BackgroundColor = UIColor.Clear,
                TextColor = _defaultiOSPlaceholderColor,

            };

            AddSubview(_placeholderLabel);

            _floatingLabel = new UILabel
            {
                Alpha = 0.0f,
            };

            AddSubview(_floatingLabel);

            // some basic default fonts/colors
            FloatingLabelFont = UIFont.BoldSystemFontOfSize(12f);
            FloatingLabelTextColor = UIColor.Gray;
            FloatingLabelActiveTextColor = UIColor.Blue;


            _subscriptions.AddRange(new []
            {
                UITextView.Notifications.ObserveTextDidChange((sender, args) => LayoutSubviews()),
                UITextView.Notifications.ObserveTextDidBeginEditing((sender, args) => LayoutSubviews()),
                UITextView.Notifications.ObserveTextDidEndEditing((sender, args) => LayoutSubviews()),
            });
        }

        public string Placeholder
        {
            get
            {
                return _placeholderLabel.Text;
            }
            set
            {
                _placeholderLabel.Text = value;
                _placeholderLabel.SizeToFit();
                _floatingLabel.Text = value;
                _floatingLabel.SizeToFit();
            }
        }

        public override UITextAlignment TextAlignment
        {
            get
            {
                return base.TextAlignment;
            }
            set
            {
                base.TextAlignment = value;
                SetNeedsLayout();
            }
        }

        public override UIFont Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                _placeholderLabel.Font = Font;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            AdjustTextContainerInsetTop();

            _placeholderLabel.Alpha = string.IsNullOrEmpty(Text) ? 1.0f: 0.0f;
            _placeholderLabel.Frame = new RectangleF(GetTextRect().X,
                GetTextRect().Y,
                _placeholderLabel.Frame.Width,
                _placeholderLabel.Frame.Height);

            SetLabelOriginForTextAlignment();


            _floatingLabel.TextColor = IsFirstResponder &&  !string.IsNullOrEmpty(Text)
                ? FloatingLabelActiveTextColor
                : FloatingLabelTextColor;

            if (string.IsNullOrEmpty(Text))
            {
                HideFloatingLabel(IsFirstResponder);
            }
            else
            {
                ShowFloatingLabel(IsFirstResponder);
            }

        }

        private void AdjustTextContainerInsetTop()
        {
            TextContainerInset = new UIEdgeInsets(_startingTextContainerInsetTop + _floatingLabel.Font.LineHeight,
                TextContainerInset.Left,
                TextContainerInset.Bottom,
                TextContainerInset.Right) ;
        }

        private RectangleF GetTextRect()
        {
            var rect = ContentInset.InsetRect(Bounds);

            if (TextContainer != null)
            {
                rect.X += TextContainer.LineFragmentPadding;
                rect.Y += TextContainerInset.Top;
            }

            return rect;
        }

        private void SetLabelOriginForTextAlignment()
        {
            var floatingLabelOriginX = GetTextRect().X;
            var placeholderLabelOriginX = floatingLabelOriginX;

            if (TextAlignment == UITextAlignment.Center)
            {
                floatingLabelOriginX = (Frame.Width / 2) - (_floatingLabel.Frame.Width / 2);
                placeholderLabelOriginX = (Frame.Width / 2) - (_placeholderLabel.Frame.Width / 2);
            }
            else if (TextAlignment == UITextAlignment.Right)
            {
                floatingLabelOriginX = Frame.Width - _floatingLabel.Frame.Width;
                placeholderLabelOriginX = Frame.Width - _placeholderLabel.Frame.Width - TextContainerInset.Right;
            }

            _floatingLabel.Frame = new RectangleF(floatingLabelOriginX,
                _floatingLabel.Frame.Y,
                _floatingLabel.Frame.Width,
                _floatingLabel.Frame.Height);

            _placeholderLabel.Frame = new RectangleF(placeholderLabelOriginX,
                _placeholderLabel.Frame.Y,
                _placeholderLabel.Frame.Width,
                _placeholderLabel.Frame.Height);
        }

        private void ShowFloatingLabel(bool animated)
        {
            NSAction showBlock = () => {
                _floatingLabel.Alpha = 1.0f;
                _floatingLabel.Frame = new RectangleF(_floatingLabel.Frame.X,
                    2.0f,
                    _floatingLabel.Frame.Width,
                    _floatingLabel.Frame.Height);
            };

            if (animated) {
                UIView.Animate(0.3f, 0.0f,
                    UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseOut,
                    showBlock, () => { });

            }
            else {
                showBlock();
            }
        }

        private void HideFloatingLabel(bool animated)
        {
            NSAction hideBlock = () => {
                _floatingLabel.Alpha = 0.0f;
                _floatingLabel.Frame = new RectangleF(_floatingLabel.Frame.X,
                    _floatingLabel.Font.LineHeight,
                    _floatingLabel.Frame.Width,
                    _floatingLabel.Frame.Height);

            };

            if (animated) {
                UIView.Animate(0.3f, 0.0f,
                    UIViewAnimationOptions.BeginFromCurrentState | UIViewAnimationOptions.CurveEaseIn,
                    hideBlock,
                    () => {});
            }
            else {
                hideBlock();
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                foreach (var subsciption in _subscriptions)
                {
                    subsciption.Dispose();
                }
                _subscriptions.Clear();
            }
        }

	}
}