using Caliburn.Micro;
using log4net;
using PlaySharp.Toolkit.Logging;
using System;

namespace Loader.Helpers
{
	internal class Log4NetLogger : Caliburn.Micro.ILog
	{
		private readonly log4net.ILog logger;

		public Log4NetLogger(Type type)
		{
			this.logger = Logs.GetLogger(type);
		}

		public void Error(Exception exception)
		{
			this.logger.Error(exception);
		}

		public void Info(string format, params object[] args)
		{
			this.logger.InfoFormat(format, args);
		}

		public void Warn(string format, params object[] args)
		{
			this.logger.WarnFormat(format, args);
		}
	}
}