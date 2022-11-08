using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Cache;
using System.Net.Configuration;
using System.Security;
using System.Text;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000427 RID: 1063
	internal sealed class NetWebProxyFinder : BaseWebProxyFinder
	{
		// Token: 0x06002149 RID: 8521 RVA: 0x0008365C File Offset: 0x0008265C
		public NetWebProxyFinder(AutoWebProxyScriptEngine engine) : base(engine)
		{
			this.backupCache = new SingleItemRequestCache(RequestCacheManager.IsCachingEnabled);
			this.lockObject = new object();
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x00083680 File Offset: 0x00082680
		public override bool GetProxies(Uri destination, out IList<string> proxyList)
		{
			bool result;
			try
			{
				proxyList = null;
				this.EnsureEngineAvailable();
				if (base.State != BaseWebProxyFinder.AutoWebProxyState.Completed)
				{
					result = false;
				}
				else
				{
					bool flag = false;
					try
					{
						string scriptReturn = this.scriptInstance.FindProxyForURL(destination.ToString(), destination.Host);
						proxyList = NetWebProxyFinder.ParseScriptResult(scriptReturn);
						flag = true;
					}
					catch (Exception ex)
					{
						if (Logging.On)
						{
							Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_script_execution_error", new object[]
							{
								ex
							}));
						}
					}
					result = flag;
				}
			}
			finally
			{
				this.aborted = false;
			}
			return result;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x00083724 File Offset: 0x00082724
		public override void Abort()
		{
			lock (this.lockObject)
			{
				this.aborted = true;
				if (this.request != null)
				{
					ThreadPool.UnsafeQueueUserWorkItem(NetWebProxyFinder.abortWrapper, this.request);
				}
			}
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x00083780 File Offset: 0x00082780
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.scriptInstance != null)
			{
				this.scriptInstance.Close();
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x00083798 File Offset: 0x00082798
		private void EnsureEngineAvailable()
		{
			if (base.State == BaseWebProxyFinder.AutoWebProxyState.Uninitialized || this.engineScriptLocation == null)
			{
				if (base.Engine.AutomaticallyDetectSettings)
				{
					this.DetectScriptLocation();
					if (this.scriptLocation != null)
					{
						if (this.scriptLocation.Equals(this.engineScriptLocation))
						{
							base.State = BaseWebProxyFinder.AutoWebProxyState.Completed;
							return;
						}
						BaseWebProxyFinder.AutoWebProxyState autoWebProxyState = this.DownloadAndCompile(this.scriptLocation);
						if (autoWebProxyState == BaseWebProxyFinder.AutoWebProxyState.Completed)
						{
							base.State = BaseWebProxyFinder.AutoWebProxyState.Completed;
							this.engineScriptLocation = this.scriptLocation;
							return;
						}
					}
				}
				if (base.Engine.AutomaticConfigurationScript != null && !this.aborted)
				{
					if (base.Engine.AutomaticConfigurationScript.Equals(this.engineScriptLocation))
					{
						base.State = BaseWebProxyFinder.AutoWebProxyState.Completed;
						return;
					}
					base.State = this.DownloadAndCompile(base.Engine.AutomaticConfigurationScript);
					if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
					{
						this.engineScriptLocation = base.Engine.AutomaticConfigurationScript;
						return;
					}
				}
			}
			else
			{
				base.State = this.DownloadAndCompile(this.engineScriptLocation);
				if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
				{
					return;
				}
				if (!this.engineScriptLocation.Equals(base.Engine.AutomaticConfigurationScript) && !this.aborted)
				{
					base.State = this.DownloadAndCompile(base.Engine.AutomaticConfigurationScript);
					if (base.State == BaseWebProxyFinder.AutoWebProxyState.Completed)
					{
						this.engineScriptLocation = base.Engine.AutomaticConfigurationScript;
						return;
					}
				}
			}
			base.State = BaseWebProxyFinder.AutoWebProxyState.DiscoveryFailure;
			if (this.scriptInstance != null)
			{
				this.scriptInstance.Close();
				this.scriptInstance = null;
			}
			this.engineScriptLocation = null;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00083930 File Offset: 0x00082930
		private BaseWebProxyFinder.AutoWebProxyState DownloadAndCompile(Uri location)
		{
			BaseWebProxyFinder.AutoWebProxyState autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.DownloadFailure;
			WebResponse webResponse = null;
			TimerThread.Timer timer = null;
			AutoWebProxyScriptWrapper autoWebProxyScriptWrapper = null;
			ExceptionHelper.WebPermissionUnrestricted.Assert();
			try
			{
				lock (this.lockObject)
				{
					if (this.aborted)
					{
						throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					}
					this.request = WebRequest.Create(location);
				}
				this.request.Timeout = -1;
				this.request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
				this.request.ConnectionGroupName = "__WebProxyScript";
				if (this.request.CacheProtocol != null)
				{
					this.request.CacheProtocol = new RequestCacheProtocol(this.backupCache, this.request.CacheProtocol.Validator);
				}
				HttpWebRequest httpWebRequest = this.request as HttpWebRequest;
				if (httpWebRequest != null)
				{
					httpWebRequest.Accept = "*/*";
					httpWebRequest.UserAgent = base.GetType().FullName + "/" + Environment.Version;
					httpWebRequest.KeepAlive = false;
					httpWebRequest.Pipelined = false;
					httpWebRequest.InternalConnectionGroup = true;
				}
				else
				{
					FtpWebRequest ftpWebRequest = this.request as FtpWebRequest;
					if (ftpWebRequest != null)
					{
						ftpWebRequest.KeepAlive = false;
					}
				}
				this.request.Proxy = null;
				this.request.Credentials = base.Engine.Credentials;
				if (NetWebProxyFinder.timerQueue == null)
				{
					NetWebProxyFinder.timerQueue = TimerThread.GetOrCreateQueue(SettingsSectionInternal.Section.DownloadTimeout);
				}
				timer = NetWebProxyFinder.timerQueue.CreateTimer(NetWebProxyFinder.timerCallback, this.request);
				webResponse = this.request.GetResponse();
				DateTime dateTime = DateTime.MinValue;
				HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
				if (httpWebResponse != null)
				{
					dateTime = httpWebResponse.LastModified;
				}
				else
				{
					FtpWebResponse ftpWebResponse = webResponse as FtpWebResponse;
					if (ftpWebResponse != null)
					{
						dateTime = ftpWebResponse.LastModified;
					}
				}
				if (this.scriptInstance != null && dateTime != DateTime.MinValue && this.scriptInstance.LastModified == dateTime)
				{
					autoWebProxyScriptWrapper = this.scriptInstance;
					autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
				}
				else
				{
					string text = null;
					byte[] array = null;
					using (Stream responseStream = webResponse.GetResponseStream())
					{
						SingleItemRequestCache.ReadOnlyStream readOnlyStream = responseStream as SingleItemRequestCache.ReadOnlyStream;
						if (readOnlyStream != null)
						{
							array = readOnlyStream.Buffer;
						}
						if (this.scriptInstance != null && array != null && array == this.scriptInstance.Buffer)
						{
							this.scriptInstance.LastModified = dateTime;
							autoWebProxyScriptWrapper = this.scriptInstance;
							autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
						}
						else
						{
							using (StreamReader streamReader = new StreamReader(responseStream))
							{
								text = streamReader.ReadToEnd();
							}
						}
					}
					WebResponse webResponse2 = webResponse;
					webResponse = null;
					webResponse2.Close();
					timer.Cancel();
					timer = null;
					if (autoWebProxyState != BaseWebProxyFinder.AutoWebProxyState.Completed)
					{
						if (this.scriptInstance != null && text == this.scriptInstance.ScriptBody)
						{
							this.scriptInstance.LastModified = dateTime;
							if (array != null)
							{
								this.scriptInstance.Buffer = array;
							}
							autoWebProxyScriptWrapper = this.scriptInstance;
							autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
						}
						else
						{
							autoWebProxyScriptWrapper = new AutoWebProxyScriptWrapper();
							autoWebProxyScriptWrapper.LastModified = dateTime;
							if (autoWebProxyScriptWrapper.Compile(location, text, array))
							{
								autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.Completed;
							}
							else
							{
								autoWebProxyState = BaseWebProxyFinder.AutoWebProxyState.CompilationFailure;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_script_download_compile_error", new object[]
					{
						ex
					}));
				}
			}
			finally
			{
				if (timer != null)
				{
					timer.Cancel();
				}
				try
				{
					if (webResponse != null)
					{
						webResponse.Close();
					}
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
					this.request = null;
				}
			}
			if (autoWebProxyState == BaseWebProxyFinder.AutoWebProxyState.Completed && this.scriptInstance != autoWebProxyScriptWrapper)
			{
				if (this.scriptInstance != null)
				{
					this.scriptInstance.Close();
				}
				this.scriptInstance = autoWebProxyScriptWrapper;
			}
			return autoWebProxyState;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x00083D5C File Offset: 0x00082D5C
		private static IList<string> ParseScriptResult(string scriptReturn)
		{
			IList<string> list = new List<string>();
			if (scriptReturn == null)
			{
				return list;
			}
			string[] array = scriptReturn.Split(NetWebProxyFinder.splitChars);
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string text = array2[i];
				string text2 = text.Trim(new char[]
				{
					' '
				});
				string text3;
				if (!text2.StartsWith("PROXY ", StringComparison.OrdinalIgnoreCase))
				{
					if (string.Compare("DIRECT", text2, StringComparison.OrdinalIgnoreCase) == 0)
					{
						text3 = null;
						goto IL_100;
					}
				}
				else
				{
					text3 = text2.Substring(6).TrimStart(new char[]
					{
						' '
					});
					Uri uri = null;
					bool flag = Uri.TryCreate("http://" + text3, UriKind.Absolute, out uri);
					if (flag && uri.UserInfo.Length <= 0 && uri.HostNameType != UriHostNameType.Basic && uri.AbsolutePath.Length == 1 && text3[text3.Length - 1] != '/' && text3[text3.Length - 1] != '#' && text3[text3.Length - 1] != '?')
					{
						goto IL_100;
					}
				}
				IL_107:
				i++;
				continue;
				IL_100:
				list.Add(text3);
				goto IL_107;
			}
			return list;
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x00083E84 File Offset: 0x00082E84
		private void DetectScriptLocation()
		{
			if (this.scriptDetectionFailed || this.scriptLocation != null)
			{
				return;
			}
			this.scriptLocation = NetWebProxyFinder.SafeDetectAutoProxyUrl(UnsafeNclNativeMethods.WinHttp.AutoDetectType.Dhcp);
			if (this.scriptLocation == null)
			{
				this.scriptLocation = NetWebProxyFinder.SafeDetectAutoProxyUrl(UnsafeNclNativeMethods.WinHttp.AutoDetectType.DnsA);
			}
			if (this.scriptLocation == null)
			{
				this.scriptDetectionFailed = true;
			}
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x00083EE4 File Offset: 0x00082EE4
		private unsafe static Uri SafeDetectAutoProxyUrl(UnsafeNclNativeMethods.WinHttp.AutoDetectType discoveryMethod)
		{
			Uri result = null;
			string text = null;
			if (ComNetOS.IsWinHttp51)
			{
				SafeGlobalFree safeGlobalFree;
				if (!UnsafeNclNativeMethods.WinHttp.WinHttpDetectAutoProxyConfigUrl(discoveryMethod, out safeGlobalFree))
				{
					if (safeGlobalFree != null)
					{
						safeGlobalFree.SetHandleAsInvalid();
					}
				}
				else
				{
					text = new string((char*)((void*)safeGlobalFree.DangerousGetHandle()));
					safeGlobalFree.Close();
				}
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder(2058);
				bool flag = UnsafeNclNativeMethods.WinInet.DetectAutoProxyUrl(stringBuilder, 2058, (int)discoveryMethod);
				if (flag)
				{
					text = stringBuilder.ToString();
				}
			}
			if (text != null)
			{
				if (!Uri.TryCreate(text, UriKind.Absolute, out result) && Logging.On)
				{
					Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_autodetect_script_location_parse_error", new object[]
					{
						ValidationHelper.ToString(text)
					}));
				}
			}
			else if (Logging.On)
			{
				Logging.PrintWarning(Logging.Web, SR.GetString("net_log_proxy_autodetect_failed"));
			}
			return result;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00083FB2 File Offset: 0x00082FB2
		private static void RequestTimeoutCallback(TimerThread.Timer timer, int timeNoticed, object context)
		{
			ThreadPool.UnsafeQueueUserWorkItem(NetWebProxyFinder.abortWrapper, context);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x00083FC0 File Offset: 0x00082FC0
		private static void AbortWrapper(object context)
		{
			if (context != null)
			{
				((WebRequest)context).Abort();
			}
		}

		// Token: 0x04002172 RID: 8562
		private const int MaximumProxyStringLength = 2058;

		// Token: 0x04002173 RID: 8563
		private static readonly char[] splitChars = new char[]
		{
			';'
		};

		// Token: 0x04002174 RID: 8564
		private static TimerThread.Queue timerQueue;

		// Token: 0x04002175 RID: 8565
		private static readonly TimerThread.Callback timerCallback = new TimerThread.Callback(NetWebProxyFinder.RequestTimeoutCallback);

		// Token: 0x04002176 RID: 8566
		private static readonly WaitCallback abortWrapper = new WaitCallback(NetWebProxyFinder.AbortWrapper);

		// Token: 0x04002177 RID: 8567
		private RequestCache backupCache;

		// Token: 0x04002178 RID: 8568
		private AutoWebProxyScriptWrapper scriptInstance;

		// Token: 0x04002179 RID: 8569
		private Uri engineScriptLocation;

		// Token: 0x0400217A RID: 8570
		private Uri scriptLocation;

		// Token: 0x0400217B RID: 8571
		private bool scriptDetectionFailed;

		// Token: 0x0400217C RID: 8572
		private object lockObject;

		// Token: 0x0400217D RID: 8573
		private volatile WebRequest request;

		// Token: 0x0400217E RID: 8574
		private volatile bool aborted;
	}
}
