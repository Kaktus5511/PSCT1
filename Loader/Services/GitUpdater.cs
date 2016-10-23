using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Loader.Helpers;
using Loader.Model;
using log4net;
using PlaySharp.Service.Messages;
using PlaySharp.Toolkit.EventAggregator;
using PlaySharp.Toolkit.Helper;
using PlaySharp.Toolkit.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Loader.Services
{
	internal class GitUpdater
	{
		private readonly static ILog Log;

		static GitUpdater()
		{
			GitUpdater.Log = Logs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		}

		public GitUpdater()
		{
		}

		private static bool Clone(string url, string directory)
		{
			bool flag;
			try
			{
				if (Directory.Exists(directory))
				{
					LoaderUtility.ClearDirectory(directory);
					Directory.Delete(directory, true);
				}
				GitUpdater.Log.Info(url);
				Repository.Clone(url, directory, new CloneOptions()
				{
					OnTransferProgress = new TransferProgressHandler(GitUpdater.OnTransferProgress)
				});
				flag = true;
			}
			catch (Exception exception)
			{
				GitUpdater.Log.Warn(exception);
				flag = false;
			}
			return flag;
		}

		public static bool HasRemoteChanges(string repository, CredentialsHandler handler = null)
		{
			bool flag;
			object url;
			try
			{
				if (Repository.IsValid(repository))
				{
					using (Repository repo = new Repository(repository))
					{
						repo.Reset(ResetMode.Hard);
						repo.RemoveUntrackedFiles();
						repo.Fetch("origin", new FetchOptions()
						{
							CredentialsProvider = handler
						});
						if (repo.Head.Tip.Sha != repo.Branches["origin/master"].Tip.Sha)
						{
							ILog log = GitUpdater.Log;
							Remote remote = repo.Network.Remotes.FirstOrDefault<Remote>();
							if (remote != null)
							{
								url = remote.Url;
							}
							else
							{
								url = null;
							}
							log.Debug(string.Format("Update Found {0}", url));
							flag = true;
							return flag;
						}
					}
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			catch (Exception exception)
			{
				GitUpdater.Log.Warn(exception);
				flag = true;
			}
			return flag;
		}

		private static bool IsValid(string directory)
		{
			bool flag;
			try
			{
				if (!Directory.Exists(directory))
				{
					flag = false;
				}
				else if (!Repository.IsValid(directory))
				{
					flag = false;
				}
				else
				{
					using (Repository repo = new Repository(directory))
					{
						if (repo.Head == null)
						{
							flag = false;
							return flag;
						}
						else if (repo.Info.IsHeadDetached)
						{
							flag = false;
							return flag;
						}
						else if (repo.Info.IsBare)
						{
							flag = false;
							return flag;
						}
					}
					return true;
				}
			}
			catch (Exception exception)
			{
				GitUpdater.Log.Warn(exception);
				flag = false;
			}
			return flag;
		}

		private static bool OnTransferProgress(TransferProgress progress)
		{
			IoC.Get<IEventAggregator>(null).PublishOnCurrentThread(new OnDownloadProgressChanged("Git", progress.ReceivedBytes, (long)0));
			return true;
		}

		private static bool Pull(string directory)
		{
			bool flag;
			try
			{
				using (Repository repo = new Repository(directory))
				{
					GitUpdater.Log.Info(directory);
					repo.Reset(ResetMode.Hard);
					repo.RemoveUntrackedFiles();
					repo.Network.Pull(new LibGit2Sharp.Signature("loader", "loader@joduska.me", DateTimeOffset.Now), new PullOptions()
					{
						MergeOptions = new MergeOptions()
						{
							FastForwardStrategy = FastForwardStrategy.Default,
							FileConflictStrategy = CheckoutFileConflictStrategy.Theirs,
							MergeFileFavor = MergeFileFavor.Theirs,
							CommitOnSuccess = true
						}
					});
					repo.Checkout(repo.Head, new CheckoutOptions()
					{
						CheckoutModifiers = CheckoutModifiers.Force
					});
					if (repo.Info.IsHeadDetached)
					{
						GitUpdater.Log.Warn("Update+Detached");
					}
				}
				flag = true;
			}
			catch (Exception exception)
			{
				GitUpdater.Log.Warn(exception);
				flag = false;
			}
			return flag;
		}

		internal static async Task<bool> UpdateAsync(string url, string destination = null)
		{
			bool flag = await Task<bool>.Factory.StartNew(() => {
				Match repositoryMatch = Regex.Match(url, "^(http[s]?)://(?<host>.*?)/(?<author>.*?)/(?<repo>.*?)(/{1}|$)");
				if (!repositoryMatch.Success)
				{
					return false;
				}
				string root = destination ?? Path.Combine(Directories.Repositories, repositoryMatch.Groups["host"].Value, repositoryMatch.Groups["author"].Value, repositoryMatch.Groups["repo"].Value);
				if (!GitUpdater.IsValid(root) && !GitUpdater.Clone(url, root))
				{
					GitUpdater.Log.Warn(string.Format("Failed to Clone - {0}", url));
					return false;
				}
				if (!GitUpdater.HasRemoteChanges(root, null))
				{
					return false;
				}
				if (GitUpdater.Pull(root))
				{
					return true;
				}
				GitUpdater.Log.Warn(string.Format("Failed to Pull Updates - {0}", url));
				return GitUpdater.Clone(url, root);
			});
			return flag;
		}
	}
}