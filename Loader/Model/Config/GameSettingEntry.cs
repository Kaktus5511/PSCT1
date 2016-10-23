using Caliburn.Micro;
using Loader.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Loader.Model.Config
{
	public class GameSettingEntry : Screen, IGameSettingEntry
	{
		private string name;

		private ObservableCollection<string> posibleValues;

		private string selectedValue;

		[JsonIgnore]
		[XmlIgnore]
		public override string DisplayName
		{
			get
			{
				return LoaderUtility.GetMultiLanguageText(this.name);
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
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(GameSettingEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GameSettingEntry).GetMethod("get_Name").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[DataMember]
		public ObservableCollection<string> PosibleValues
		{
			get
			{
				return this.posibleValues;
			}
			set
			{
				if (object.Equals(value, this.posibleValues))
				{
					return;
				}
				this.posibleValues = value;
				base.NotifyOfPropertyChange<ObservableCollection<string>>(Expression.Lambda<Func<ObservableCollection<string>>>(Expression.Property(Expression.Constant(this, typeof(GameSettingEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GameSettingEntry).GetMethod("get_PosibleValues").MethodHandle)), new ParameterExpression[0]));
			}
		}

		[DataMember]
		public string SelectedValue
		{
			get
			{
				return this.selectedValue;
			}
			set
			{
				if (value == this.selectedValue)
				{
					return;
				}
				this.selectedValue = value;
				base.NotifyOfPropertyChange<string>(Expression.Lambda<Func<string>>(Expression.Property(Expression.Constant(this, typeof(GameSettingEntry)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(GameSettingEntry).GetMethod("get_SelectedValue").MethodHandle)), new ParameterExpression[0]));
			}
		}

		public GameSettingEntry()
		{
		}
	}
}