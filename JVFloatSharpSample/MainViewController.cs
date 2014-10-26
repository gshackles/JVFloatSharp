using MonoTouch.UIKit;
using System.Drawing;
using JVFloatSharp;

namespace JVFloatSharpSample
{
	public class MainViewController : UIViewController
	{
        const float JVTextViewHeight = 100.0f;
		const float JVFieldHeight = 44.0f;
		const float JVFieldHMargin = 10.0f;
		const float JVFieldFontSize = 16.0f;
		const float JVFieldFloatingLabelFontSize = 11.0f;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			Title = "Floating Label Demo";

			View.BackgroundColor = UIColor.White;

			float topOffset = UIApplication.SharedApplication.StatusBarFrame.Size.Height + NavigationController.NavigationBar.Frame.Size.Height;
			UIColor floatingLabelColor = UIColor.Gray, floatingLabelActiveColor = UIColor.Blue;

			var titleField = new JVFloatLabeledTextField(new RectangleF(JVFieldHMargin, topOffset, 
			                                                            View.Frame.Size.Width - 2 * JVFieldHMargin, 
			                                                            JVFieldHeight))
			{
				Placeholder = "Title",
				Font = UIFont.SystemFontOfSize(JVFieldFontSize),
				ClearButtonMode = UITextFieldViewMode.WhileEditing,
				FloatingLabelFont = UIFont.BoldSystemFontOfSize(JVFieldFloatingLabelFontSize),
				FloatingLabelTextColor = floatingLabelColor,
				FloatingLabelActiveTextColor = floatingLabelActiveColor
			};
			View.AddSubview(titleField);

			var div1 = new UIView(new RectangleF(JVFieldHMargin, 
			                                     titleField.Frame.Y + titleField.Frame.Size.Height, 
			                                     View.Frame.Size.Width - 2 * JVFieldHMargin, 1))
			{
				BackgroundColor = UIColor.LightGray.ColorWithAlpha(0.3f)
			};
			View.AddSubview(div1);

			var priceField = new JVFloatLabeledTextField(new RectangleF(JVFieldHMargin, 
			                                                            div1.Frame.Y + div1.Frame.Size.Height, 
			                                                            80, JVFieldHeight))
			{
				Placeholder = "Price",
				Font = UIFont.SystemFontOfSize(JVFieldFontSize),
				FloatingLabelFont = UIFont.BoldSystemFontOfSize(JVFieldFloatingLabelFontSize),
				FloatingLabelTextColor = floatingLabelColor,
				FloatingLabelActiveTextColor = floatingLabelActiveColor,
				Text = "3.14"
			};
			View.AddSubview(priceField);

			var div2 = new UIView(new RectangleF(JVFieldHMargin + priceField.Frame.Size.Width, 
			                                     titleField.Frame.Y + titleField.Frame.Size.Height, 
			                                     1, JVFieldHeight))
			{
				BackgroundColor = UIColor.LightGray.ColorWithAlpha(0.3f)
			};
			View.AddSubview(div2);

			var locationField = new JVFloatLabeledTextField(new RectangleF(JVFieldHMargin + JVFieldHMargin + priceField.Frame.Size.Width + 1.0f, 
			                                                               div1.Frame.Y + div1.Frame.Size.Height,
			                                                               View.Frame.Size.Width - 3 * JVFieldHMargin - priceField.Frame.Size.Width - 1.0f, 
			                                                               JVFieldHeight))
			{
				Placeholder = "Specific Location (optional)",
				Font = UIFont.SystemFontOfSize(JVFieldFontSize),
				FloatingLabelFont = UIFont.BoldSystemFontOfSize(JVFieldFloatingLabelFontSize),
				FloatingLabelTextColor = floatingLabelColor,
				FloatingLabelActiveTextColor = floatingLabelActiveColor
			};
			View.AddSubview(locationField);

			var div3 = new UIView(new RectangleF(JVFieldHMargin, 
			                                     priceField.Frame.Y + priceField.Frame.Size.Height,
			                                     View.Frame.Size.Width - 2 * JVFieldHMargin, 1))
			{
				BackgroundColor = UIColor.LightGray.ColorWithAlpha(0.3f)
			};
			View.AddSubview(div3);

            var descriptionField = new JVFloatLabeledTextView(new RectangleF(JVFieldHMargin, 
			                                                                  div3.Frame.Y + div3.Frame.Size.Height,
			                                                                  View.Frame.Size.Width - 2 * JVFieldHMargin, 
                                                                              JVTextViewHeight))
			{
				Placeholder = "Description",
				Font = UIFont.SystemFontOfSize(JVFieldFontSize),
				FloatingLabelFont = UIFont.BoldSystemFontOfSize(JVFieldFloatingLabelFontSize),
				FloatingLabelTextColor = floatingLabelColor,
				FloatingLabelActiveTextColor = floatingLabelActiveColor
			};
			View.AddSubview(descriptionField);

			titleField.BecomeFirstResponder();

			NavigationItem.RightBarButtonItem = new UIBarButtonItem("Dialog Demo", UIBarButtonItemStyle.Plain, delegate
			{
				this.NavigationController.PushViewController(new DialogDemoViewController(), true);
			});
		}
	}
}