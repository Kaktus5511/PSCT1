using Caliburn.Micro;
using Loader.Helpers;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace Loader.Model.Config
{
	public class HotkeyEntry : Screen, IHotkeyEntry
	{
		private Key defaultKey;

		private string description;

		private Key hotkey;

		private string name;

		[DataMember]
		public Key DefaultKey
		{
			get
			{
				return this.defaultKey;
			}
			set
			{
				if (value == this.defaultKey)
				{
					return;
				}
				this.defaultKey = value;
				base.NotifyOfPropertyChange<Key>(Expression.Lambda<Func<Key>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_DefaultKey").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[DataMember]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				if (value == this.description)
				{
					return;
				}
				this.description = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_Description").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_DisplayDescription").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public string DisplayDescription
		{
			get
			{
				return LoaderUtility.GetMultiLanguageText(this.Description);
			}
		}

		[DataMember]
		public Key Hotkey
		{
			get
			{
				return this.hotkey;
			}
			set
			{
				if (value == this.hotkey)
				{
					return;
				}
				this.hotkey = value;
				base.NotifyOfPropertyChange<Key>(Expression.Lambda<Func<Key>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_Hotkey").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<byte>(Expression.Lambda<Func<byte>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_HotkeyInt").MethodHandle)), new ParameterExpression[0]));
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_HotkeyString").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public byte HotkeyInt
		{
			get
			{
				if (this.Hotkey == Key.LeftShift || this.Hotkey == Key.RightShift)
				{
					return (byte)16;
				}
				if (this.Hotkey == Key.LeftAlt || this.Hotkey == Key.RightAlt)
				{
					return (byte)18;
				}
				if (this.Hotkey == Key.LeftCtrl || this.Hotkey == Key.RightCtrl)
				{
					return (byte)17;
				}
				return (byte)KeyInterop.VirtualKeyFromKey(this.Hotkey);
			}
		}

		public string HotkeyString
		{
			get
			{
				return this.Hotkey.ToString();
			}
		}

		[DataMember]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == this.name)
				{
					return;
				}
				this.name = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(HotkeyEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(HotkeyEntry).GetMethod("get_Name").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public HotkeyEntry()
		{
		}
	}
}