using CachedImage;
using log4net;
using PlaySharp.Toolkit.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Loader.Helpers
{
	public class SlackEmoji : CachedImage.Image
	{
		public readonly static DependencyProperty FrameIndexProperty;

		public readonly static DependencyProperty AutoStartProperty;

		private readonly static ILog Log;

		public readonly static DependencyProperty GifSourceProperty;

		private Int32Animation animation;

		private GifBitmapDecoder gifDecoder;

		private bool isInitialized;

		public bool AutoStart
		{
			get
			{
				return (bool)base.GetValue(SlackEmoji.AutoStartProperty);
			}
			set
			{
				base.SetValue(SlackEmoji.AutoStartProperty, value);
			}
		}

		public int FrameIndex
		{
			get
			{
				return (int)base.GetValue(SlackEmoji.FrameIndexProperty);
			}
			set
			{
				base.SetValue(SlackEmoji.FrameIndexProperty, value);
			}
		}

		public string GifSource
		{
			get
			{
				return (string)base.GetValue(SlackEmoji.GifSourceProperty);
			}
			set
			{
				base.SetValue(SlackEmoji.GifSourceProperty, value);
			}
		}

		public string Id
		{
			set
			{
				string item;
				string str;
				try
				{
					Dictionary<string, string> images = this.Images;
					if (images != null)
					{
						item = images[value];
					}
					else
					{
						item = null;
					}
					string image = item;
					if (image != null && image.EndsWith(".gif"))
					{
						Dictionary<string, string> strs = this.Images;
						if (strs != null)
						{
							str = strs[value];
						}
						else
						{
							str = null;
						}
						this.GifSource = str;
					}
					if (image != null)
					{
						base.Source = new BitmapImage(new Uri(image));
					}
				}
				catch (Exception exception)
				{
					SlackEmoji.Log.Error(exception);
				}
			}
		}

		public Dictionary<string, string> Images
		{
			get;
			set;
		}

		static SlackEmoji()
		{
			SlackEmoji.FrameIndexProperty = DependencyProperty.Register("FrameIndex", typeof(int), typeof(SlackEmoji), new UIPropertyMetadata((object)0, new PropertyChangedCallback(SlackEmoji.ChangingFrameIndex)));
			SlackEmoji.AutoStartProperty = DependencyProperty.Register("AutoStart", typeof(bool), typeof(SlackEmoji), new UIPropertyMetadata(true, new PropertyChangedCallback(SlackEmoji.AutoStartPropertyChanged)));
			SlackEmoji.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			SlackEmoji.GifSourceProperty = DependencyProperty.Register("GifSource", typeof(string), typeof(SlackEmoji), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(SlackEmoji.GifSourcePropertyChanged)));
			UIElement.VisibilityProperty.OverrideMetadata(typeof(SlackEmoji), new FrameworkPropertyMetadata(new PropertyChangedCallback(SlackEmoji.VisibilityPropertyChanged)));
		}

		public SlackEmoji()
		{
			Dictionary<string, string> strs = new Dictionary<string, string>();
			strs[":waiting:"] = "https://emoji.slack-edge.com/T0BP5GURX/waiting/82d23b478e4f71af.gif";
			strs[":h3h3:"] = "https://emoji.slack-edge.com/T0BP5GURX/h3h3/1d572d87426161fa.png";
			strs[":salty:"] = "https://emoji.slack-edge.com/T0BP5GURX/pjsalt/7921d7c38c9f441a.png";
			this.Images = strs;
			base();
		}

		private static void AutoStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if ((bool)e.NewValue)
			{
				SlackEmoji slackEmoji = sender as SlackEmoji;
				if (slackEmoji == null)
				{
					return;
				}
				slackEmoji.StartAnimation();
			}
		}

		private static void ChangingFrameIndex(DependencyObject obj, DependencyPropertyChangedEventArgs ev)
		{
			SlackEmoji gifImage = obj as SlackEmoji;
			if (gifImage != null)
			{
				gifImage.Source = gifImage.gifDecoder.Frames[(int)ev.NewValue];
			}
		}

		private static void GifSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			SlackEmoji slackEmoji = sender as SlackEmoji;
			if (slackEmoji == null)
			{
				return;
			}
			slackEmoji.Initialize();
		}

		private void Initialize()
		{
			this.gifDecoder = new GifBitmapDecoder(new Uri(this.GifSource), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
			this.animation = new Int32Animation(0, this.gifDecoder.Frames.Count - 1, new Duration(new TimeSpan(0, 0, 0, this.gifDecoder.Frames.Count / 10, (int)(((double)this.gifDecoder.Frames.Count / 10 - (double)(this.gifDecoder.Frames.Count / 10)) * 1000))))
			{
				RepeatBehavior = RepeatBehavior.Forever
			};
			base.Source = this.gifDecoder.Frames[0];
			this.isInitialized = true;
			this.StartAnimation();
		}

		public void StartAnimation()
		{
			if (!this.isInitialized)
			{
				this.Initialize();
			}
			base.BeginAnimation(SlackEmoji.FrameIndexProperty, this.animation);
		}

		public void StopAnimation()
		{
			base.BeginAnimation(SlackEmoji.FrameIndexProperty, null);
		}

		private static void VisibilityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			if ((System.Windows.Visibility)e.NewValue == System.Windows.Visibility.Visible)
			{
				((SlackEmoji)sender).StartAnimation();
				return;
			}
			((SlackEmoji)sender).StopAnimation();
		}
	}
}