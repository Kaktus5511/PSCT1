using log4net;
using Microsoft.VisualStudio.Threading;
using PlaySharp.Service.WebService;
using PlaySharp.Service.WebService.Endpoints;
using PlaySharp.Toolkit.Extensions;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Loader.Model
{
	public static class ConfigFactory
	{
		private readonly static ILog Log;

		static ConfigFactory()
		{
			ConfigFactory.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public static ConfigFactory.ConfigLoad<T> Load<T>(string name)
		where T : class, new()
		{
			return new ConfigFactory.ConfigLoad<T>(name);
		}

		public static ConfigFactory.ConfigSave<T> Save<T>(string name, T instance)
		where T : class
		{
			return new ConfigFactory.ConfigSave<T>(name, instance);
		}

		public class ConfigLoad<T>
		where T : class, new()
		{
			private T Config
			{
				get;
				set;
			}

			public string FilePath
			{
				get;
				private set;
			}

			public DateTime Modified
			{
				get;
				private set;
			}

			public string Name
			{
				get;
				set;
			}

			private Assembly ResourceAssembly
			{
				get;
				set;
			}

			private string ResourceFilePath
			{
				get;
				set;
			}

			[Import(typeof(IWebServiceClient))]
			private Lazy<IWebServiceClient> ServiceClient
			{
				get;
				set;
			}

			private bool UseCloud
			{
				get;
				set;
			}

			private bool UseFile
			{
				get;
				set;
			}

			private bool UseResource
			{
				get;
				set;
			}

			public ConfigLoad(string name)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				this.Name = name;
				this.FilePath = Path.Combine(Directories.Config, string.Format("{0}.json", name));
				this.ResourceFilePath = string.Format("{0}.json", name);
				try
				{
					IoC.BuildUp(this);
				}
				catch
				{
				}
			}

			public T Execute()
			{
				if (this.UseCloud && this.LoadCloud())
				{
					return this.Config;
				}
				if (this.UseFile && this.LoadFile())
				{
					return this.Config;
				}
				if (this.UseResource && this.LoadResource())
				{
					return this.Config;
				}
				T config = this.Config;
				if (config == null)
				{
					config = Activator.CreateInstance<T>();
				}
				return config;
			}

			private bool LoadCloud()
			{
				return ThreadHelper.JoinableTaskFactory.Run<bool>(async () => {
					bool flag;
					try
					{
						CloudData<string> cloudDatum = await this.ServiceClient.Value.StorageDownloadAsync<string>(this.Name);
						if (!cloudDatum.Modified.IsUnixTimeNull())
						{
							ConfigFactory.Log.Debug(string.Format("{0} - R:{1} - L:{2}", this.Name, cloudDatum.Modified, this.Modified));
							if (cloudDatum.Modified > this.Modified || !File.Exists(this.FilePath))
							{
								ConfigFactory.Log.Info(string.Format("Load Cloud {0}", this.Name));
								T t = JsonFactory.FromString<T>(cloudDatum.Data, null);
								if (t != null)
								{
									this.Config = t;
									flag = true;
									return flag;
								}
							}
						}
						else
						{
							flag = false;
							return flag;
						}
					}
					catch (Exception exception)
					{
						ConfigFactory.Log.Warn(exception);
					}
					flag = false;
					return flag;
				});
			}

			private bool LoadFile()
			{
				try
				{
					if (File.Exists(this.FilePath))
					{
						ConfigFactory.Log.Info(string.Format("Load File {0}", this.FilePath));
						this.Config = JsonFactory.FromFile<T>(this.FilePath, null);
						return true;
					}
				}
				catch (Exception exception)
				{
					ConfigFactory.Log.Warn(exception);
				}
				return false;
			}

			private bool LoadResource()
			{
				bool flag;
				try
				{
					ConfigFactory.Log.Info(string.Format("Load Resource {0}", this.ResourceFilePath));
					string content = Utility.ReadResourceString(this.ResourceFilePath, this.ResourceAssembly);
					this.Config = JsonFactory.FromString<T>(content, null);
					flag = true;
				}
				catch (Exception exception)
				{
					ConfigFactory.Log.Warn(exception);
					return false;
				}
				return flag;
			}

			public ConfigFactory.ConfigLoad<T> WithCloud()
			{
				this.UseCloud = true;
				if (File.Exists(this.FilePath))
				{
					this.Modified = File.GetLastWriteTimeUtc(this.FilePath);
				}
				return this;
			}

			public ConfigFactory.ConfigLoad<T> WithCloud(DateTime modified)
			{
				this.UseCloud = true;
				this.Modified = modified;
				return this;
			}

			public ConfigFactory.ConfigLoad<T> WithFile(string file = null)
			{
				if (file != null)
				{
					this.FilePath = Path.Combine(Directories.Config, file);
				}
				this.UseFile = true;
				return this;
			}

			public ConfigFactory.ConfigLoad<T> WithResource(string file = null, Assembly assembly = null)
			{
				if (file != null)
				{
					this.ResourceFilePath = file;
				}
				if (assembly != null)
				{
					this.ResourceAssembly = assembly;
				}
				this.UseResource = true;
				return this;
			}
		}

		public class ConfigSave<T>
		where T : class
		{
			private string FilePath
			{
				get;
			}

			private T Instance
			{
				get;
			}

			private DateTime Modified
			{
				get;
				set;
			}

			public string Name
			{
				get;
			}

			[Import(typeof(IWebServiceClient))]
			private Lazy<IWebServiceClient> ServiceClient
			{
				get;
				set;
			}

			private bool UseCloud
			{
				get;
				set;
			}

			private bool UseFile
			{
				get;
				set;
			}

			public ConfigSave(string name, T instance)
			{
				this.<Name>k__BackingField = name;
				this.<Instance>k__BackingField = instance;
				this.<FilePath>k__BackingField = Path.Combine(Directories.Config, string.Format("{0}.json", name));
				try
				{
					IoC.BuildUp(this);
				}
				catch
				{
				}
			}

			public void Execute()
			{
				if (this.UseCloud)
				{
					this.SaveCloud();
				}
				if (this.UseFile)
				{
					this.SaveFile();
				}
			}

			private void SaveCloud()
			{
				ConfigFactory.Log.Info(string.Format("Save Cloud {0}", this.Name));
				ThreadHelper.JoinableTaskFactory.Run(async () => {
					try
					{
						await this.ServiceClient.Value.StorageUploadAsync(this.Instance, this.Name, this.Modified);
					}
					catch (Exception exception)
					{
						ConfigFactory.Log.Warn(exception);
					}
				});
			}

			private void SaveFile()
			{
				try
				{
					ConfigFactory.Log.Info(string.Format("Save File {0}", this.FilePath));
					JsonFactory.ToFile(this.FilePath, this.Instance, null);
					File.SetLastWriteTime(this.FilePath, this.Modified);
				}
				catch (Exception exception)
				{
					ConfigFactory.Log.Warn(exception);
				}
			}

			public ConfigFactory.ConfigSave<T> WithCloud()
			{
				this.UseCloud = true;
				return this;
			}

			public ConfigFactory.ConfigSave<T> WithFile()
			{
				this.UseFile = true;
				return this;
			}

			public ConfigFactory.ConfigSave<T> WithModified(DateTime time)
			{
				this.Modified = time;
				return this;
			}
		}
	}
}