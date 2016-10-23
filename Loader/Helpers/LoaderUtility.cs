using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Serialization;

namespace Loader.Helpers
{
	public class LoaderUtility
	{
		private readonly static System.Random Random;

		static LoaderUtility()
		{
			LoaderUtility.Random = new System.Random();
		}

		public LoaderUtility()
		{
		}

		public static void ClearDirectory(string directory)
		{
			int i;
			try
			{
				DirectoryInfo dir = new DirectoryInfo(directory);
				FileInfo[] files = dir.GetFiles();
				for (i = 0; i < (int)files.Length; i++)
				{
					FileInfo fi = files[i];
					try
					{
						fi.Attributes = FileAttributes.Normal;
						fi.Delete();
					}
					catch
					{
					}
				}
				DirectoryInfo[] directories = dir.GetDirectories();
				for (i = 0; i < (int)directories.Length; i++)
				{
					DirectoryInfo di = directories[i];
					try
					{
						di.Attributes = FileAttributes.Normal;
						LoaderUtility.ClearDirectory(di.FullName);
						di.Delete();
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
		}

		public static void CopyDirectory(string sourceDirName, string destDirName, bool copySubDirs = false, bool overrideFiles = false)
		{
			int i;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName)
				{
					Attributes = FileAttributes.Directory
				};
				DirectoryInfo[] dirs = directoryInfo.GetDirectories();
				if (!directoryInfo.Exists)
				{
					throw new DirectoryNotFoundException(string.Concat("Source directory does not exist or could not be found: ", sourceDirName));
				}
				if (!Directory.Exists(destDirName))
				{
					Directory.CreateDirectory(destDirName);
				}
				FileInfo[] files = directoryInfo.GetFiles();
				for (i = 0; i < (int)files.Length; i++)
				{
					FileInfo file = files[i];
					string temppath = Path.Combine(destDirName, file.Name);
					file.Attributes = FileAttributes.Normal;
					file.CopyTo(temppath, overrideFiles);
				}
				if (copySubDirs)
				{
					DirectoryInfo[] directoryInfoArray = dirs;
					for (i = 0; i < (int)directoryInfoArray.Length; i++)
					{
						DirectoryInfo subdir = directoryInfoArray[i];
						string temppath = Path.Combine(destDirName, subdir.Name);
						LoaderUtility.CopyDirectory(subdir.FullName, temppath, true, overrideFiles);
					}
				}
			}
			catch
			{
			}
		}

		public static void CreateFileFromResource(string path, string resource, bool overwrite = false)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (resource == null)
			{
				throw new ArgumentNullException("resource");
			}
			if (!overwrite && File.Exists(path))
			{
				return;
			}
			File.WriteAllBytes(path, LoaderUtility.ReadResource(resource, null));
		}

		public static string GetLatestLeagueOfLegendsExePath(string lastKnownPath)
		{
			string str;
			Version version;
			string str1;
			try
			{
				if (!lastKnownPath.EndsWith("Game\\League of Legends.exe"))
				{
					string dir = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(lastKnownPath), "..\\..\\"));
					if (Directory.Exists(dir))
					{
						string[] directories = Directory.GetDirectories(dir);
						string greatestVersionString = string.Empty;
						long greatestVersion = (long)0;
						string[] strArrays = directories;
						for (int i = 0; i < (int)strArrays.Length; i++)
						{
							string versionPath = strArrays[i];
							if (Version.TryParse(Path.GetFileName(versionPath), out version))
							{
								double test = (double)version.Build * Math.Pow(600, 4) + (double)version.Major * Math.Pow(600, 3) + (double)version.Minor * Math.Pow(600, 2) + (double)version.Revision * Math.Pow(600, 1);
								if (test > (double)greatestVersion)
								{
									greatestVersion = (long)test;
									greatestVersionString = Path.GetFileName(versionPath);
								}
							}
						}
						if (greatestVersion != 0)
						{
							string[] exe = Directory.GetFiles(Path.Combine(dir, greatestVersionString), "League of Legends.exe", SearchOption.AllDirectories);
							if (exe.Length != 0)
							{
								str1 = exe[0];
							}
							else
							{
								str1 = null;
							}
							str = str1;
							return str;
						}
					}
					return null;
				}
				else
				{
					str = lastKnownPath;
				}
			}
			catch (Exception exception)
			{
				return null;
			}
			return str;
		}

		public static string GetMultiLanguageText(string key)
		{
			string str;
			try
			{
				str = Application.Current.FindResource(key).ToString();
			}
			catch (Exception exception)
			{
				str = key;
			}
			return str;
		}

		public static string GetUniqueKey(int maxSize)
		{
			char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
			byte[] data = new byte[1];
			using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
			{
				crypto.GetNonZeroBytes(data);
				data = new byte[maxSize];
				crypto.GetNonZeroBytes(data);
			}
			StringBuilder result = new StringBuilder(maxSize);
			byte[] numArray = data;
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				byte b = numArray[i];
				result.Append(chars[b % (int)chars.Length]);
			}
			return result.ToString();
		}

		public static string MakeValidFileName(string name)
		{
			string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
			string invalidRegStr = string.Format("([{0}]*\\.+$)|([{0}]+)", invalidChars);
			return Regex.Replace(name, invalidRegStr, "_");
		}

		public static void MapClassToXmlFile(Type type, object obj, string path)
		{
			XmlSerializer serializer = new XmlSerializer(type);
			using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
			{
				serializer.Serialize(sw, obj);
			}
		}

		public static object MapXmlFileToClass(Type type, string path)
		{
			object obj;
			XmlSerializer serializer = new XmlSerializer(type);
			using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
			{
				obj = serializer.Deserialize(reader);
			}
			return obj;
		}

		public static string Md5Checksum(string filePath)
		{
			string lower;
			if (filePath == null)
			{
				throw new ArgumentNullException("filePath");
			}
			try
			{
				if (File.Exists(filePath))
				{
					using (MD5 md5 = MD5.Create())
					{
						using (FileStream stream = File.OpenRead(filePath))
						{
							lower = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty).ToLower();
						}
					}
				}
				else
				{
					lower = "-1";
				}
			}
			catch (Exception exception)
			{
				lower = "-1";
			}
			return lower;
		}

		public static string Md5Hash(string s)
		{
			StringBuilder sb = new StringBuilder();
			byte[] numArray = MD5.Create().ComputeHash(Encoding.Default.GetBytes(s));
			for (int i = 0; i < (int)numArray.Length; i++)
			{
				byte b = numArray[i];
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}

		[SecuritySafeCritical]
		public static byte[] ReadResource(string file, Assembly assembly = null)
		{
			byte[] array;
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			if (assembly == null)
			{
				assembly = Assembly.GetExecutingAssembly();
			}
			string resourceFile = assembly.GetManifestResourceNames().FirstOrDefault<string>((string f) => f.EndsWith(file));
			if (resourceFile == null)
			{
				throw new ArgumentNullException("resourceFile");
			}
			using (MemoryStream ms = new MemoryStream())
			{
				Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceFile);
				if (manifestResourceStream != null)
				{
					manifestResourceStream.CopyTo(ms);
				}
				else
				{
				}
				array = ms.ToArray();
			}
			return array;
		}

		public static string ReadResourceString(string resource)
		{
			string end;
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
			{
				if (stream == null)
				{
					return string.Empty;
				}
				else
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						end = reader.ReadToEnd();
					}
				}
			}
			return end;
		}

		public static bool RenameFileIfExists(string file, string path)
		{
			bool flag;
			try
			{
				int counter = 1;
				string fileName = Path.GetFileNameWithoutExtension(file);
				string fileExtension = Path.GetExtension(file);
				string newPath = path;
				string pathDirectory = Path.GetDirectoryName(path);
				if (pathDirectory == null)
				{
					return false;
				}
				else
				{
					if (!Directory.Exists(pathDirectory))
					{
						Directory.CreateDirectory(pathDirectory);
					}
					while (File.Exists(newPath))
					{
						int num = counter;
						counter = num + 1;
						string tmpFileName = string.Format("{0} ({1})", fileName, num);
						newPath = Path.Combine(pathDirectory, string.Concat(tmpFileName, fileExtension));
					}
					File.Move(file, newPath);
					flag = true;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public static byte[] ReplaceFilling(string file, string searchFileName, string replaceFileName, Encoding encoding = null)
		{
			if (file == null)
			{
				throw new ArgumentNullException("file");
			}
			if (searchFileName == null)
			{
				throw new ArgumentNullException("searchFileName");
			}
			if (replaceFileName == null)
			{
				throw new ArgumentNullException("replaceFileName");
			}
			return LoaderUtility.ReplaceFilling(File.ReadAllBytes(file), Encoding.ASCII.GetBytes(searchFileName), Encoding.ASCII.GetBytes(replaceFileName));
		}

		public static byte[] ReplaceFilling(byte[] content, byte[] search, byte[] replacement)
		{
			int i;
			if (content == null)
			{
				throw new ArgumentNullException("content");
			}
			if (search == null)
			{
				throw new ArgumentNullException("search");
			}
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			if (search.Length == 0)
			{
				return content;
			}
			List<byte> result = new List<byte>();
			for (i = 0; i <= (int)content.Length - (int)search.Length; i++)
			{
				bool foundMatch = true;
				int j = 0;
				while (j < (int)search.Length)
				{
					if (content[i + j] == search[j])
					{
						j++;
					}
					else
					{
						foundMatch = false;
						break;
					}
				}
				if (!foundMatch)
				{
					result.Add(content[i]);
				}
				else
				{
					result.AddRange(replacement);
					for (int k = 0; k < (int)search.Length - (int)replacement.Length; k++)
					{
						result.Add(0);
					}
					i = i + ((int)search.Length - 1);
				}
			}
			while (i < (int)content.Length)
			{
				result.Add(content[i]);
				i++;
			}
			return result.ToArray();
		}

		public static int VersionToInt(Version version)
		{
			return version.Major * 10000000 + version.Minor * 10000 + version.Build * 100 + version.Revision;
		}

		public static string WildcardToRegex(string pattern)
		{
			return string.Concat("^", Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", "."), "$");
		}
	}
}