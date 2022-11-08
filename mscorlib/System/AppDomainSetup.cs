using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Security.Util;
using System.Text;

namespace System
{
	// Token: 0x0200005D RID: 93
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public sealed class AppDomainSetup : IAppDomainSetup
	{
		// Token: 0x06000571 RID: 1393 RVA: 0x00013100 File Offset: 0x00012100
		internal AppDomainSetup(AppDomainSetup copy, bool copyDomainBoundData)
		{
			string[] value = this.Value;
			if (copy != null)
			{
				string[] value2 = copy.Value;
				int num = this._Entries.Length;
				int num2 = value2.Length;
				int num3 = (num2 < num) ? num2 : num;
				for (int i = 0; i < num3; i++)
				{
					value[i] = value2[i];
				}
				if (num3 < num)
				{
					for (int j = num3; j < num; j++)
					{
						value[j] = null;
					}
				}
				this._LoaderOptimization = copy._LoaderOptimization;
				this._AppDomainInitializerArguments = copy.AppDomainInitializerArguments;
				this._ActivationArguments = copy.ActivationArguments;
				this._ApplicationTrust = copy._ApplicationTrust;
				if (copyDomainBoundData)
				{
					this._AppDomainInitializer = copy.AppDomainInitializer;
				}
				else
				{
					this._AppDomainInitializer = null;
				}
				this._ConfigurationBytes = copy.GetConfigurationBytes();
				this._DisableInterfaceCache = copy._DisableInterfaceCache;
				return;
			}
			this._LoaderOptimization = LoaderOptimization.NotSpecified;
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x000131DA File Offset: 0x000121DA
		public AppDomainSetup()
		{
			this._LoaderOptimization = LoaderOptimization.NotSpecified;
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x000131E9 File Offset: 0x000121E9
		public AppDomainSetup(ActivationContext activationContext) : this(new ActivationArguments(activationContext))
		{
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x000131F8 File Offset: 0x000121F8
		public AppDomainSetup(ActivationArguments activationArguments)
		{
			if (activationArguments == null)
			{
				throw new ArgumentNullException("activationArguments");
			}
			this._LoaderOptimization = LoaderOptimization.NotSpecified;
			this.ActivationArguments = activationArguments;
			string entryPointFullPath = CmsUtils.GetEntryPointFullPath(activationArguments);
			if (!string.IsNullOrEmpty(entryPointFullPath))
			{
				this.SetupDefaultApplicationBase(entryPointFullPath);
				return;
			}
			this.ApplicationBase = activationArguments.ActivationContext.ApplicationDirectory;
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00013250 File Offset: 0x00012250
		internal void SetupDefaultApplicationBase(string imageLocation)
		{
			char[] anyOf = new char[]
			{
				'\\',
				'/'
			};
			int num = imageLocation.LastIndexOfAny(anyOf);
			string text = null;
			StringBuilder stringBuilder;
			if (num == -1)
			{
				stringBuilder = new StringBuilder(imageLocation);
			}
			else
			{
				text = imageLocation.Substring(0, num + 1);
				stringBuilder = new StringBuilder(imageLocation.Substring(num + 1));
			}
			string text2;
			if (num == -1)
			{
				text2 = imageLocation;
			}
			else
			{
				text2 = imageLocation.Substring(num + 1);
			}
			stringBuilder.Append(AppDomainSetup.ConfigurationExtension);
			if (stringBuilder != null)
			{
				this.ConfigurationFile = stringBuilder.ToString();
			}
			if (text != null)
			{
				this.ApplicationBase = text;
			}
			if (text2 != null)
			{
				this.ApplicationName = text2;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x000132EC File Offset: 0x000122EC
		internal string[] Value
		{
			get
			{
				if (this._Entries == null)
				{
					this._Entries = new string[16];
				}
				return this._Entries;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00013309 File Offset: 0x00012309
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x0001331A File Offset: 0x0001231A
		public string ApplicationBase
		{
			get
			{
				return this.VerifyDir(this.Value[0], false);
			}
			set
			{
				this.Value[0] = this.NormalizePath(value, false);
			}
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001332C File Offset: 0x0001232C
		private string NormalizePath(string path, bool useAppBase)
		{
			if (path == null)
			{
				return null;
			}
			if (!useAppBase)
			{
				path = URLString.PreProcessForExtendedPathRemoval(path, false);
			}
			int num = path.Length;
			if (num == 0)
			{
				return null;
			}
			bool flag = false;
			if (num > 7 && string.Compare(path, 0, "file:", 0, 5, StringComparison.OrdinalIgnoreCase) == 0)
			{
				int num2;
				if (path[6] == '\\')
				{
					if (path[7] == '\\' || path[7] == '/')
					{
						if (num > 8 && (path[8] == '\\' || path[8] == '/'))
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPathChars"));
						}
						num2 = 8;
					}
					else
					{
						num2 = 5;
						flag = true;
					}
				}
				else if (path[7] == '/')
				{
					num2 = 8;
				}
				else
				{
					if (num > 8 && path[7] == '\\' && path[8] == '\\')
					{
						num2 = 7;
					}
					else
					{
						num2 = 5;
						StringBuilder stringBuilder = new StringBuilder(num);
						for (int i = 0; i < num; i++)
						{
							char c = path[i];
							if (c == '/')
							{
								stringBuilder.Append('\\');
							}
							else
							{
								stringBuilder.Append(c);
							}
						}
						path = stringBuilder.ToString();
					}
					flag = true;
				}
				path = path.Substring(num2);
				num -= num2;
			}
			bool flag2;
			if (flag || (num > 1 && (path[0] == '/' || path[0] == '\\') && (path[1] == '/' || path[1] == '\\')))
			{
				flag2 = false;
			}
			else
			{
				int num3 = path.IndexOf(':') + 1;
				flag2 = (num3 == 0 || num <= num3 + 1 || (path[num3] != '/' && path[num3] != '\\') || (path[num3 + 1] != '/' && path[num3 + 1] != '\\'));
			}
			if (flag2)
			{
				if (useAppBase && (num == 1 || path[1] != ':'))
				{
					string text = this.Value[0];
					if (text == null || text.Length == 0)
					{
						throw new MemberAccessException(Environment.GetResourceString("AppDomain_AppBaseNotSet"));
					}
					StringBuilder stringBuilder2 = new StringBuilder();
					bool flag3 = false;
					if (path[0] == '/' || path[0] == '\\')
					{
						string text2 = Path.GetPathRoot(text);
						if (text2.Length == 0)
						{
							int j = text.IndexOf(":/", StringComparison.Ordinal);
							if (j == -1)
							{
								j = text.IndexOf(":\\", StringComparison.Ordinal);
							}
							int length = text.Length;
							for (j++; j < length; j++)
							{
								if (text[j] != '/' && text[j] != '\\')
								{
									break;
								}
							}
							while (j < length && text[j] != '/' && text[j] != '\\')
							{
								j++;
							}
							text2 = text.Substring(0, j);
						}
						stringBuilder2.Append(text2);
						flag3 = true;
					}
					else
					{
						stringBuilder2.Append(text);
					}
					int num4 = stringBuilder2.Length - 1;
					if (stringBuilder2[num4] != '/' && stringBuilder2[num4] != '\\')
					{
						if (!flag3)
						{
							if (text.IndexOf(":/", StringComparison.Ordinal) == -1)
							{
								stringBuilder2.Append('\\');
							}
							else
							{
								stringBuilder2.Append('/');
							}
						}
					}
					else if (flag3)
					{
						stringBuilder2.Remove(num4, 1);
					}
					stringBuilder2.Append(path);
					path = stringBuilder2.ToString();
				}
				else
				{
					path = Path.GetFullPathInternal(path);
				}
			}
			return path;
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00013676 File Offset: 0x00012676
		private bool IsFilePath(string path)
		{
			return path[1] == ':' || (path[0] == '\\' && path[1] == '\\');
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0001369D File Offset: 0x0001269D
		internal static string ApplicationBaseKey
		{
			get
			{
				return "APPBASE";
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x000136A4 File Offset: 0x000126A4
		// (set) Token: 0x0600057D RID: 1405 RVA: 0x000136B5 File Offset: 0x000126B5
		public string ConfigurationFile
		{
			get
			{
				return this.VerifyDir(this.Value[1], true);
			}
			set
			{
				this.Value[1] = value;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x000136C0 File Offset: 0x000126C0
		internal string ConfigurationFileInternal
		{
			get
			{
				return this.NormalizePath(this.Value[1], true);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x000136D1 File Offset: 0x000126D1
		internal static string ConfigurationFileKey
		{
			get
			{
				return "APP_CONFIG_FILE";
			}
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000136D8 File Offset: 0x000126D8
		public byte[] GetConfigurationBytes()
		{
			if (this._ConfigurationBytes == null)
			{
				return null;
			}
			return (byte[])this._ConfigurationBytes.Clone();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000136F4 File Offset: 0x000126F4
		public void SetConfigurationBytes(byte[] value)
		{
			this._ConfigurationBytes = value;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000136FD File Offset: 0x000126FD
		private static string ConfigurationBytesKey
		{
			get
			{
				return "APP_CONFIG_BLOB";
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00013704 File Offset: 0x00012704
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00013718 File Offset: 0x00012718
		public string DynamicBase
		{
			get
			{
				return this.VerifyDir(this.Value[2], true);
			}
			set
			{
				if (value == null)
				{
					this.Value[2] = null;
					return;
				}
				if (this.ApplicationName == null)
				{
					throw new MemberAccessException(Environment.GetResourceString("AppDomain_RequireApplicationName"));
				}
				StringBuilder stringBuilder = new StringBuilder(this.NormalizePath(value, false));
				stringBuilder.Append('\\');
				string value2 = ParseNumbers.IntToString(this.ApplicationName.GetHashCode(), 16, 8, '0', 256);
				stringBuilder.Append(value2);
				this.Value[2] = stringBuilder.ToString();
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00013792 File Offset: 0x00012792
		internal static string DynamicBaseKey
		{
			get
			{
				return "DYNAMIC_BASE";
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x00013799 File Offset: 0x00012799
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x000137AA File Offset: 0x000127AA
		public bool DisallowPublisherPolicy
		{
			get
			{
				return this.Value[11] != null;
			}
			set
			{
				if (value)
				{
					this.Value[11] = "true";
					return;
				}
				this.Value[11] = null;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x000137C8 File Offset: 0x000127C8
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x000137D9 File Offset: 0x000127D9
		public bool DisallowBindingRedirects
		{
			get
			{
				return this.Value[13] != null;
			}
			set
			{
				if (value)
				{
					this.Value[13] = "true";
					return;
				}
				this.Value[13] = null;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x000137F7 File Offset: 0x000127F7
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00013808 File Offset: 0x00012808
		public bool DisallowCodeDownload
		{
			get
			{
				return this.Value[12] != null;
			}
			set
			{
				if (value)
				{
					this.Value[12] = "true";
					return;
				}
				this.Value[12] = null;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x00013826 File Offset: 0x00012826
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x00013837 File Offset: 0x00012837
		public bool DisallowApplicationBaseProbing
		{
			get
			{
				return this.Value[14] != null;
			}
			set
			{
				if (value)
				{
					this.Value[14] = "true";
					return;
				}
				this.Value[14] = null;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00013855 File Offset: 0x00012855
		private string VerifyDir(string dir, bool normalize)
		{
			if (dir != null)
			{
				if (dir.Length == 0)
				{
					dir = null;
				}
				else
				{
					if (normalize)
					{
						dir = this.NormalizePath(dir, true);
					}
					if (this.IsFilePath(dir))
					{
						new FileIOPermission(FileIOPermissionAccess.PathDiscovery, dir).Demand();
					}
				}
			}
			return dir;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001388C File Offset: 0x0001288C
		private void VerifyDirList(string dirs)
		{
			if (dirs != null)
			{
				string[] array = dirs.Split(new char[]
				{
					';'
				});
				int num = array.Length;
				for (int i = 0; i < num; i++)
				{
					this.VerifyDir(array[i], true);
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x000138CC File Offset: 0x000128CC
		// (set) Token: 0x06000591 RID: 1425 RVA: 0x000138EC File Offset: 0x000128EC
		internal string DeveloperPath
		{
			get
			{
				string text = this.Value[3];
				this.VerifyDirList(text);
				return text;
			}
			set
			{
				if (value == null)
				{
					this.Value[3] = null;
					return;
				}
				string[] array = value.Split(new char[]
				{
					';'
				});
				int num = array.Length;
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				for (int i = 0; i < num; i++)
				{
					if (array[i].Length != 0)
					{
						if (flag)
						{
							stringBuilder.Append(";");
						}
						else
						{
							flag = true;
						}
						stringBuilder.Append(Path.GetFullPathInternal(array[i]));
					}
				}
				string text = stringBuilder.ToString();
				if (text.Length == 0)
				{
					this.Value[3] = null;
					return;
				}
				this.Value[3] = stringBuilder.ToString();
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00013990 File Offset: 0x00012990
		internal static string DisallowPublisherPolicyKey
		{
			get
			{
				return "DISALLOW_APP";
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00013997 File Offset: 0x00012997
		internal static string DisallowCodeDownloadKey
		{
			get
			{
				return "CODE_DOWNLOAD_DISABLED";
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0001399E File Offset: 0x0001299E
		internal static string DisallowBindingRedirectsKey
		{
			get
			{
				return "DISALLOW_APP_REDIRECTS";
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x000139A5 File Offset: 0x000129A5
		internal static string DeveloperPathKey
		{
			get
			{
				return "DEV_PATH";
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x000139AC File Offset: 0x000129AC
		internal static string DisallowAppBaseProbingKey
		{
			get
			{
				return "DISALLOW_APP_BASE_PROBING";
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x000139B3 File Offset: 0x000129B3
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x000139BD File Offset: 0x000129BD
		public string ApplicationName
		{
			get
			{
				return this.Value[4];
			}
			set
			{
				this.Value[4] = value;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x000139C8 File Offset: 0x000129C8
		internal static string ApplicationNameKey
		{
			get
			{
				return "APP_NAME";
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x000139CF File Offset: 0x000129CF
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x000139D7 File Offset: 0x000129D7
		[XmlIgnoreMember]
		public AppDomainInitializer AppDomainInitializer
		{
			get
			{
				return this._AppDomainInitializer;
			}
			set
			{
				this._AppDomainInitializer = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x000139E0 File Offset: 0x000129E0
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x000139E8 File Offset: 0x000129E8
		public string[] AppDomainInitializerArguments
		{
			get
			{
				return this._AppDomainInitializerArguments;
			}
			set
			{
				this._AppDomainInitializerArguments = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x000139F1 File Offset: 0x000129F1
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x000139FC File Offset: 0x000129FC
		[XmlIgnoreMember]
		public ActivationArguments ActivationArguments
		{
			get
			{
				return this._ActivationArguments;
			}
			set
			{
				lock (this)
				{
					if (value != null && this._ApplicationTrust != null && !CmsUtils.CompareIdentities(this.ApplicationTrust.ApplicationIdentity, value.ApplicationIdentity, ApplicationVersionMatch.MatchExactVersion))
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ActivationArgsAppTrustMismatch"));
					}
					this._ActivationArguments = value;
				}
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00013A68 File Offset: 0x00012A68
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00013A9C File Offset: 0x00012A9C
		[XmlIgnoreMember]
		public ApplicationTrust ApplicationTrust
		{
			get
			{
				if (this._ApplicationTrust == null)
				{
					return null;
				}
				SecurityElement element = SecurityElement.FromString(this._ApplicationTrust);
				ApplicationTrust applicationTrust = new ApplicationTrust();
				applicationTrust.FromXml(element);
				return applicationTrust;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value", Environment.GetResourceString("ArgumentNull_ApplicationTrust"));
				}
				lock (this)
				{
					if (this._ActivationArguments != null && !CmsUtils.CompareIdentities(value.ApplicationIdentity, this._ActivationArguments.ApplicationIdentity, ApplicationVersionMatch.MatchExactVersion))
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ActivationArgsAppTrustMismatch"));
					}
					this._ApplicationTrust = value.ToXml().ToString();
				}
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00013B24 File Offset: 0x00012B24
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00013B42 File Offset: 0x00012B42
		public string PrivateBinPath
		{
			get
			{
				string text = this.Value[5];
				this.VerifyDirList(text);
				return text;
			}
			set
			{
				this.Value[5] = value;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00013B4D File Offset: 0x00012B4D
		internal static string PrivateBinPathKey
		{
			get
			{
				return "PRIVATE_BINPATH";
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00013B54 File Offset: 0x00012B54
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00013B5E File Offset: 0x00012B5E
		public string PrivateBinPathProbe
		{
			get
			{
				return this.Value[6];
			}
			set
			{
				this.Value[6] = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00013B69 File Offset: 0x00012B69
		internal static string PrivateBinPathProbeKey
		{
			get
			{
				return "BINPATH_PROBE_ONLY";
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x00013B70 File Offset: 0x00012B70
		// (set) Token: 0x060005A9 RID: 1449 RVA: 0x00013B8E File Offset: 0x00012B8E
		public string ShadowCopyDirectories
		{
			get
			{
				string text = this.Value[7];
				this.VerifyDirList(text);
				return text;
			}
			set
			{
				this.Value[7] = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060005AA RID: 1450 RVA: 0x00013B99 File Offset: 0x00012B99
		internal static string ShadowCopyDirectoriesKey
		{
			get
			{
				return "SHADOW_COPY_DIRS";
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00013BA0 File Offset: 0x00012BA0
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x00013BAA File Offset: 0x00012BAA
		public string ShadowCopyFiles
		{
			get
			{
				return this.Value[8];
			}
			set
			{
				if (value != null && string.Compare(value, "true", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.Value[8] = value;
					return;
				}
				this.Value[8] = null;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00013BD0 File Offset: 0x00012BD0
		internal static string ShadowCopyFilesKey
		{
			get
			{
				return "FORCE_CACHE_INSTALL";
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00013BD7 File Offset: 0x00012BD7
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x00013BE9 File Offset: 0x00012BE9
		public string CachePath
		{
			get
			{
				return this.VerifyDir(this.Value[9], false);
			}
			set
			{
				this.Value[9] = this.NormalizePath(value, false);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x00013BFC File Offset: 0x00012BFC
		internal static string CachePathKey
		{
			get
			{
				return "CACHE_BASE";
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060005B1 RID: 1457 RVA: 0x00013C03 File Offset: 0x00012C03
		// (set) Token: 0x060005B2 RID: 1458 RVA: 0x00013C15 File Offset: 0x00012C15
		public string LicenseFile
		{
			get
			{
				return this.VerifyDir(this.Value[10], true);
			}
			set
			{
				this.Value[10] = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060005B3 RID: 1459 RVA: 0x00013C21 File Offset: 0x00012C21
		// (set) Token: 0x060005B4 RID: 1460 RVA: 0x00013C29 File Offset: 0x00012C29
		public LoaderOptimization LoaderOptimization
		{
			get
			{
				return this._LoaderOptimization;
			}
			set
			{
				this._LoaderOptimization = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060005B5 RID: 1461 RVA: 0x00013C32 File Offset: 0x00012C32
		internal static string LoaderOptimizationKey
		{
			get
			{
				return "LOADER_OPTIMIZATION";
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060005B6 RID: 1462 RVA: 0x00013C39 File Offset: 0x00012C39
		internal static string ConfigurationExtension
		{
			get
			{
				return ".config";
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060005B7 RID: 1463 RVA: 0x00013C40 File Offset: 0x00012C40
		internal static string PrivateBinPathEnvironmentVariable
		{
			get
			{
				return "RELPATH";
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060005B8 RID: 1464 RVA: 0x00013C47 File Offset: 0x00012C47
		internal static string RuntimeConfigurationFile
		{
			get
			{
				return "config\\machine.config";
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x00013C4E File Offset: 0x00012C4E
		internal static string MachineConfigKey
		{
			get
			{
				return "MACHINE_CONFIG";
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060005BA RID: 1466 RVA: 0x00013C55 File Offset: 0x00012C55
		internal static string HostBindingKey
		{
			get
			{
				return "HOST_CONFIG";
			}
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00013C5C File Offset: 0x00012C5C
		internal void SetupFusionContext(IntPtr fusionContext)
		{
			string text = this.Value[0];
			if (text != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ApplicationBaseKey, text);
			}
			string text2 = this.Value[5];
			if (text2 != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.PrivateBinPathKey, text2);
			}
			string text3 = this.Value[3];
			if (text3 != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DeveloperPathKey, text3);
			}
			if (this.DisallowPublisherPolicy)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DisallowPublisherPolicyKey, "true");
			}
			if (this.DisallowCodeDownload)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DisallowCodeDownloadKey, "true");
			}
			if (this.DisallowBindingRedirects)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DisallowBindingRedirectsKey, "true");
			}
			if (this.DisallowApplicationBaseProbing)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DisallowAppBaseProbingKey, "true");
			}
			if (this.ShadowCopyFiles != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ShadowCopyFilesKey, this.ShadowCopyFiles);
				if (this.Value[7] == null)
				{
					this.ShadowCopyDirectories = this.BuildShadowCopyDirectories();
				}
				string text4 = this.Value[7];
				if (text4 != null)
				{
					AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ShadowCopyDirectoriesKey, text4);
				}
			}
			string text5 = this.Value[9];
			if (text5 != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.CachePathKey, text5);
			}
			if (this.PrivateBinPathProbe != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.PrivateBinPathProbeKey, this.PrivateBinPathProbe);
			}
			string text6 = this.Value[1];
			if (text6 != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ConfigurationFileKey, text6);
			}
			if (this._ConfigurationBytes != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ConfigurationBytesKey, this._ConfigurationBytes);
			}
			if (this.ApplicationName != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.ApplicationNameKey, this.ApplicationName);
			}
			string text7 = this.Value[2];
			if (text7 != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.DynamicBaseKey, text7);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(RuntimeEnvironment.GetRuntimeDirectoryImpl());
			stringBuilder.Append(AppDomainSetup.RuntimeConfigurationFile);
			AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.MachineConfigKey, stringBuilder.ToString());
			string hostBindingFile = RuntimeEnvironment.GetHostBindingFile();
			if (hostBindingFile != null)
			{
				AppDomainSetup.UpdateContextProperty(fusionContext, AppDomainSetup.HostBindingKey, hostBindingFile);
			}
		}

		// Token: 0x060005BC RID: 1468
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void UpdateContextProperty(IntPtr fusionContext, string key, object value);

		// Token: 0x060005BD RID: 1469 RVA: 0x00013E40 File Offset: 0x00012E40
		internal static int Locate(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return -1;
			}
			char c = s[0];
			if (c <= 'L')
			{
				switch (c)
				{
				case 'A':
					if (s == "APP_CONFIG_FILE")
					{
						return 1;
					}
					if (s == "APP_NAME")
					{
						return 4;
					}
					if (s == "APPBASE")
					{
						return 0;
					}
					if (s == "APP_CONFIG_BLOB")
					{
						return 15;
					}
					break;
				case 'B':
					if (s == "BINPATH_PROBE_ONLY")
					{
						return 6;
					}
					break;
				case 'C':
					if (s == "CACHE_BASE")
					{
						return 9;
					}
					if (s == "CODE_DOWNLOAD_DISABLED")
					{
						return 12;
					}
					break;
				case 'D':
					if (s == "DEV_PATH")
					{
						return 3;
					}
					if (s == "DYNAMIC_BASE")
					{
						return 2;
					}
					if (s == "DISALLOW_APP")
					{
						return 11;
					}
					if (s == "DISALLOW_APP_REDIRECTS")
					{
						return 13;
					}
					if (s == "DISALLOW_APP_BASE_PROBING")
					{
						return 14;
					}
					break;
				case 'E':
					break;
				case 'F':
					if (s == "FORCE_CACHE_INSTALL")
					{
						return 8;
					}
					break;
				default:
					if (c == 'L')
					{
						if (s == "LICENSE_FILE")
						{
							return 10;
						}
					}
					break;
				}
			}
			else if (c != 'P')
			{
				if (c == 'S')
				{
					if (s == "SHADOW_COPY_DIRS")
					{
						return 7;
					}
				}
			}
			else if (s == "PRIVATE_BINPATH")
			{
				return 5;
			}
			return -1;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00013FA8 File Offset: 0x00012FA8
		private string BuildShadowCopyDirectories()
		{
			string text = this.Value[5];
			if (text == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text2 = this.Value[0];
			if (text2 != null)
			{
				char[] separator = new char[]
				{
					';'
				};
				string[] array = text.Split(separator);
				int num = array.Length;
				bool flag = text2[text2.Length - 1] != '/' && text2[text2.Length - 1] != '\\';
				if (num == 0)
				{
					stringBuilder.Append(text2);
					if (flag)
					{
						stringBuilder.Append('\\');
					}
					stringBuilder.Append(text);
				}
				else
				{
					for (int i = 0; i < num; i++)
					{
						stringBuilder.Append(text2);
						if (flag)
						{
							stringBuilder.Append('\\');
						}
						stringBuilder.Append(array[i]);
						if (i < num - 1)
						{
							stringBuilder.Append(';');
						}
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00014090 File Offset: 0x00013090
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00014098 File Offset: 0x00013098
		public bool SandboxInterop
		{
			get
			{
				return this._DisableInterfaceCache;
			}
			set
			{
				this._DisableInterfaceCache = value;
			}
		}

		// Token: 0x040001B3 RID: 435
		private string[] _Entries;

		// Token: 0x040001B4 RID: 436
		private LoaderOptimization _LoaderOptimization;

		// Token: 0x040001B5 RID: 437
		private string _AppBase;

		// Token: 0x040001B6 RID: 438
		[OptionalField(VersionAdded = 2)]
		private AppDomainInitializer _AppDomainInitializer;

		// Token: 0x040001B7 RID: 439
		[OptionalField(VersionAdded = 2)]
		private string[] _AppDomainInitializerArguments;

		// Token: 0x040001B8 RID: 440
		[OptionalField(VersionAdded = 2)]
		private ActivationArguments _ActivationArguments;

		// Token: 0x040001B9 RID: 441
		[OptionalField(VersionAdded = 2)]
		internal string _ApplicationTrust;

		// Token: 0x040001BA RID: 442
		[OptionalField(VersionAdded = 2)]
		private byte[] _ConfigurationBytes;

		// Token: 0x040001BB RID: 443
		[OptionalField(VersionAdded = 3)]
		private bool _DisableInterfaceCache;

		// Token: 0x0200005E RID: 94
		[Serializable]
		internal enum LoaderInformation
		{
			// Token: 0x040001BD RID: 445
			ApplicationBaseValue,
			// Token: 0x040001BE RID: 446
			ConfigurationFileValue,
			// Token: 0x040001BF RID: 447
			DynamicBaseValue,
			// Token: 0x040001C0 RID: 448
			DevPathValue,
			// Token: 0x040001C1 RID: 449
			ApplicationNameValue,
			// Token: 0x040001C2 RID: 450
			PrivateBinPathValue,
			// Token: 0x040001C3 RID: 451
			PrivateBinPathProbeValue,
			// Token: 0x040001C4 RID: 452
			ShadowCopyDirectoriesValue,
			// Token: 0x040001C5 RID: 453
			ShadowCopyFilesValue,
			// Token: 0x040001C6 RID: 454
			CachePathValue,
			// Token: 0x040001C7 RID: 455
			LicenseFileValue,
			// Token: 0x040001C8 RID: 456
			DisallowPublisherPolicyValue,
			// Token: 0x040001C9 RID: 457
			DisallowCodeDownloadValue,
			// Token: 0x040001CA RID: 458
			DisallowBindingRedirectsValue,
			// Token: 0x040001CB RID: 459
			DisallowAppBaseProbingValue,
			// Token: 0x040001CC RID: 460
			ConfigurationBytesValue,
			// Token: 0x040001CD RID: 461
			LoaderMaximum
		}
	}
}
