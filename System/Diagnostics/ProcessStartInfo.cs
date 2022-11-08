using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000787 RID: 1927
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true, SelfAffectingProcessMgmt = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ProcessStartInfo
	{
		// Token: 0x06003B63 RID: 15203 RVA: 0x000FD790 File Offset: 0x000FC790
		public ProcessStartInfo()
		{
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x000FD79F File Offset: 0x000FC79F
		internal ProcessStartInfo(Process parent)
		{
			this.weakParentProcess = new WeakReference(parent);
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x000FD7BA File Offset: 0x000FC7BA
		public ProcessStartInfo(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x000FD7D0 File Offset: 0x000FC7D0
		public ProcessStartInfo(string fileName, string arguments)
		{
			this.fileName = fileName;
			this.arguments = arguments;
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x000FD7ED File Offset: 0x000FC7ED
		// (set) Token: 0x06003B68 RID: 15208 RVA: 0x000FD803 File Offset: 0x000FC803
		[NotifyParentProperty(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.VerbConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("ProcessVerb")]
		public string Verb
		{
			get
			{
				if (this.verb == null)
				{
					return string.Empty;
				}
				return this.verb;
			}
			set
			{
				this.verb = value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06003B69 RID: 15209 RVA: 0x000FD80C File Offset: 0x000FC80C
		// (set) Token: 0x06003B6A RID: 15210 RVA: 0x000FD822 File Offset: 0x000FC822
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		[RecommendedAsConfigurable(true)]
		[MonitoringDescription("ProcessArguments")]
		public string Arguments
		{
			get
			{
				if (this.arguments == null)
				{
					return string.Empty;
				}
				return this.arguments;
			}
			set
			{
				this.arguments = value;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06003B6B RID: 15211 RVA: 0x000FD82B File Offset: 0x000FC82B
		// (set) Token: 0x06003B6C RID: 15212 RVA: 0x000FD833 File Offset: 0x000FC833
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[MonitoringDescription("ProcessCreateNoWindow")]
		public bool CreateNoWindow
		{
			get
			{
				return this.createNoWindow;
			}
			set
			{
				this.createNoWindow = value;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003B6D RID: 15213 RVA: 0x000FD83C File Offset: 0x000FC83C
		[MonitoringDescription("ProcessEnvironmentVariables")]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Diagnostics.Design.StringDictionaryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public StringDictionary EnvironmentVariables
		{
			get
			{
				if (this.environmentVariables == null)
				{
					this.environmentVariables = new StringDictionary();
					if (this.weakParentProcess == null || !this.weakParentProcess.IsAlive || ((Component)this.weakParentProcess.Target).Site == null || !((Component)this.weakParentProcess.Target).Site.DesignMode)
					{
						foreach (object obj in Environment.GetEnvironmentVariables())
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							this.environmentVariables.Add((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
						}
					}
				}
				return this.environmentVariables;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003B6E RID: 15214 RVA: 0x000FD914 File Offset: 0x000FC914
		// (set) Token: 0x06003B6F RID: 15215 RVA: 0x000FD91C File Offset: 0x000FC91C
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardInput")]
		public bool RedirectStandardInput
		{
			get
			{
				return this.redirectStandardInput;
			}
			set
			{
				this.redirectStandardInput = value;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003B70 RID: 15216 RVA: 0x000FD925 File Offset: 0x000FC925
		// (set) Token: 0x06003B71 RID: 15217 RVA: 0x000FD92D File Offset: 0x000FC92D
		[NotifyParentProperty(true)]
		[MonitoringDescription("ProcessRedirectStandardOutput")]
		[DefaultValue(false)]
		public bool RedirectStandardOutput
		{
			get
			{
				return this.redirectStandardOutput;
			}
			set
			{
				this.redirectStandardOutput = value;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06003B72 RID: 15218 RVA: 0x000FD936 File Offset: 0x000FC936
		// (set) Token: 0x06003B73 RID: 15219 RVA: 0x000FD93E File Offset: 0x000FC93E
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardError")]
		public bool RedirectStandardError
		{
			get
			{
				return this.redirectStandardError;
			}
			set
			{
				this.redirectStandardError = value;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06003B74 RID: 15220 RVA: 0x000FD947 File Offset: 0x000FC947
		// (set) Token: 0x06003B75 RID: 15221 RVA: 0x000FD94F File Offset: 0x000FC94F
		public Encoding StandardErrorEncoding
		{
			get
			{
				return this.standardErrorEncoding;
			}
			set
			{
				this.standardErrorEncoding = value;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003B76 RID: 15222 RVA: 0x000FD958 File Offset: 0x000FC958
		// (set) Token: 0x06003B77 RID: 15223 RVA: 0x000FD960 File Offset: 0x000FC960
		public Encoding StandardOutputEncoding
		{
			get
			{
				return this.standardOutputEncoding;
			}
			set
			{
				this.standardOutputEncoding = value;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003B78 RID: 15224 RVA: 0x000FD969 File Offset: 0x000FC969
		// (set) Token: 0x06003B79 RID: 15225 RVA: 0x000FD971 File Offset: 0x000FC971
		[DefaultValue(true)]
		[NotifyParentProperty(true)]
		[MonitoringDescription("ProcessUseShellExecute")]
		public bool UseShellExecute
		{
			get
			{
				return this.useShellExecute;
			}
			set
			{
				this.useShellExecute = value;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003B7A RID: 15226 RVA: 0x000FD97C File Offset: 0x000FC97C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] Verbs
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				RegistryKey registryKey = null;
				string extension = Path.GetExtension(this.FileName);
				try
				{
					if (extension != null && extension.Length > 0)
					{
						registryKey = Registry.ClassesRoot.OpenSubKey(extension);
						if (registryKey != null)
						{
							string str = (string)registryKey.GetValue(string.Empty);
							registryKey.Close();
							registryKey = Registry.ClassesRoot.OpenSubKey(str + "\\shell");
							if (registryKey != null)
							{
								string[] subKeyNames = registryKey.GetSubKeyNames();
								for (int i = 0; i < subKeyNames.Length; i++)
								{
									if (string.Compare(subKeyNames[i], "new", StringComparison.OrdinalIgnoreCase) != 0)
									{
										arrayList.Add(subKeyNames[i]);
									}
								}
								registryKey.Close();
								registryKey = null;
							}
						}
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
				}
				string[] array = new string[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06003B7B RID: 15227 RVA: 0x000FDA64 File Offset: 0x000FCA64
		// (set) Token: 0x06003B7C RID: 15228 RVA: 0x000FDA7A File Offset: 0x000FCA7A
		[NotifyParentProperty(true)]
		public string UserName
		{
			get
			{
				if (this.userName == null)
				{
					return string.Empty;
				}
				return this.userName;
			}
			set
			{
				this.userName = value;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06003B7D RID: 15229 RVA: 0x000FDA83 File Offset: 0x000FCA83
		// (set) Token: 0x06003B7E RID: 15230 RVA: 0x000FDA8B File Offset: 0x000FCA8B
		public SecureString Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06003B7F RID: 15231 RVA: 0x000FDA94 File Offset: 0x000FCA94
		// (set) Token: 0x06003B80 RID: 15232 RVA: 0x000FDAAA File Offset: 0x000FCAAA
		[NotifyParentProperty(true)]
		public string Domain
		{
			get
			{
				if (this.domain == null)
				{
					return string.Empty;
				}
				return this.domain;
			}
			set
			{
				this.domain = value;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06003B81 RID: 15233 RVA: 0x000FDAB3 File Offset: 0x000FCAB3
		// (set) Token: 0x06003B82 RID: 15234 RVA: 0x000FDABB File Offset: 0x000FCABB
		[NotifyParentProperty(true)]
		public bool LoadUserProfile
		{
			get
			{
				return this.loadUserProfile;
			}
			set
			{
				this.loadUserProfile = value;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06003B83 RID: 15235 RVA: 0x000FDAC4 File Offset: 0x000FCAC4
		// (set) Token: 0x06003B84 RID: 15236 RVA: 0x000FDADA File Offset: 0x000FCADA
		[RecommendedAsConfigurable(true)]
		[DefaultValue("")]
		[Editor("System.Diagnostics.Design.StartFileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("ProcessFileName")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string FileName
		{
			get
			{
				if (this.fileName == null)
				{
					return string.Empty;
				}
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x000FDAE3 File Offset: 0x000FCAE3
		// (set) Token: 0x06003B86 RID: 15238 RVA: 0x000FDAF9 File Offset: 0x000FCAF9
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[MonitoringDescription("ProcessWorkingDirectory")]
		[Editor("System.Diagnostics.Design.WorkingDirectoryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RecommendedAsConfigurable(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string WorkingDirectory
		{
			get
			{
				if (this.directory == null)
				{
					return string.Empty;
				}
				return this.directory;
			}
			set
			{
				this.directory = value;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x000FDB02 File Offset: 0x000FCB02
		// (set) Token: 0x06003B88 RID: 15240 RVA: 0x000FDB0A File Offset: 0x000FCB0A
		[NotifyParentProperty(true)]
		[MonitoringDescription("ProcessErrorDialog")]
		[DefaultValue(false)]
		public bool ErrorDialog
		{
			get
			{
				return this.errorDialog;
			}
			set
			{
				this.errorDialog = value;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06003B89 RID: 15241 RVA: 0x000FDB13 File Offset: 0x000FCB13
		// (set) Token: 0x06003B8A RID: 15242 RVA: 0x000FDB1B File Offset: 0x000FCB1B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr ErrorDialogParentHandle
		{
			get
			{
				return this.errorDialogParentHandle;
			}
			set
			{
				this.errorDialogParentHandle = value;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06003B8B RID: 15243 RVA: 0x000FDB24 File Offset: 0x000FCB24
		// (set) Token: 0x06003B8C RID: 15244 RVA: 0x000FDB2C File Offset: 0x000FCB2C
		[DefaultValue(ProcessWindowStyle.Normal)]
		[MonitoringDescription("ProcessWindowStyle")]
		[NotifyParentProperty(true)]
		public ProcessWindowStyle WindowStyle
		{
			get
			{
				return this.windowStyle;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ProcessWindowStyle), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ProcessWindowStyle));
				}
				this.windowStyle = value;
			}
		}

		// Token: 0x04003423 RID: 13347
		private string fileName;

		// Token: 0x04003424 RID: 13348
		private string arguments;

		// Token: 0x04003425 RID: 13349
		private string directory;

		// Token: 0x04003426 RID: 13350
		private string verb;

		// Token: 0x04003427 RID: 13351
		private ProcessWindowStyle windowStyle;

		// Token: 0x04003428 RID: 13352
		private bool errorDialog;

		// Token: 0x04003429 RID: 13353
		private IntPtr errorDialogParentHandle;

		// Token: 0x0400342A RID: 13354
		private bool useShellExecute = true;

		// Token: 0x0400342B RID: 13355
		private string userName;

		// Token: 0x0400342C RID: 13356
		private string domain;

		// Token: 0x0400342D RID: 13357
		private SecureString password;

		// Token: 0x0400342E RID: 13358
		private bool loadUserProfile;

		// Token: 0x0400342F RID: 13359
		private bool redirectStandardInput;

		// Token: 0x04003430 RID: 13360
		private bool redirectStandardOutput;

		// Token: 0x04003431 RID: 13361
		private bool redirectStandardError;

		// Token: 0x04003432 RID: 13362
		private Encoding standardOutputEncoding;

		// Token: 0x04003433 RID: 13363
		private Encoding standardErrorEncoding;

		// Token: 0x04003434 RID: 13364
		private bool createNoWindow;

		// Token: 0x04003435 RID: 13365
		private WeakReference weakParentProcess;

		// Token: 0x04003436 RID: 13366
		internal StringDictionary environmentVariables;
	}
}
