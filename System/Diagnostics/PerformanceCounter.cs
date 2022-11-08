using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x02000761 RID: 1889
	[SRDescription("PerformanceCounterDesc")]
	[InstallerType("System.Diagnostics.PerformanceCounterInstaller,System.Configuration.Install, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SharedState = true)]
	public sealed class PerformanceCounter : Component, ISupportInitialize
	{
		// Token: 0x060039F4 RID: 14836 RVA: 0x000F5328 File Offset: 0x000F4328
		public PerformanceCounter()
		{
			this.machineName = ".";
			this.categoryName = string.Empty;
			this.counterName = string.Empty;
			this.instanceName = string.Empty;
			this.isReadOnly = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x000F5388 File Offset: 0x000F4388
		public PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName)
		{
			this.MachineName = machineName;
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = true;
			this.Initialize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060039F6 RID: 14838 RVA: 0x000F53E0 File Offset: 0x000F43E0
		internal PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName, bool skipInit)
		{
			this.MachineName = machineName;
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = true;
			this.initialized = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x060039F7 RID: 14839 RVA: 0x000F5436 File Offset: 0x000F4436
		public PerformanceCounter(string categoryName, string counterName, string instanceName) : this(categoryName, counterName, instanceName, true)
		{
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x000F5444 File Offset: 0x000F4444
		public PerformanceCounter(string categoryName, string counterName, string instanceName, bool readOnly)
		{
			this.MachineName = ".";
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = readOnly;
			this.Initialize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x000F549D File Offset: 0x000F449D
		public PerformanceCounter(string categoryName, string counterName) : this(categoryName, counterName, true)
		{
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x000F54A8 File Offset: 0x000F44A8
		public PerformanceCounter(string categoryName, string counterName, bool readOnly) : this(categoryName, counterName, "", readOnly)
		{
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x060039FB RID: 14843 RVA: 0x000F54B8 File Offset: 0x000F44B8
		// (set) Token: 0x060039FC RID: 14844 RVA: 0x000F54C0 File Offset: 0x000F44C0
		[ReadOnly(true)]
		[SRDescription("PCCategoryName")]
		[RecommendedAsConfigurable(true)]
		[TypeConverter("System.Diagnostics.Design.CategoryValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.categoryName == null || string.Compare(this.categoryName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.categoryName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x060039FD RID: 14845 RVA: 0x000F54F4 File Offset: 0x000F44F4
		[MonitoringDescription("PC_CounterHelp")]
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CounterHelp
		{
			get
			{
				string category = this.categoryName;
				string machine = this.machineName;
				PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machine, category);
				performanceCounterPermission.Demand();
				this.Initialize();
				if (this.helpMsg == null)
				{
					this.helpMsg = PerformanceCounterLib.GetCounterHelp(machine, category, this.counterName);
				}
				return this.helpMsg;
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x060039FE RID: 14846 RVA: 0x000F5545 File Offset: 0x000F4545
		// (set) Token: 0x060039FF RID: 14847 RVA: 0x000F554D File Offset: 0x000F454D
		[TypeConverter("System.Diagnostics.Design.CounterNameConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SRDescription("PCCounterName")]
		[ReadOnly(true)]
		[DefaultValue("")]
		[RecommendedAsConfigurable(true)]
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.counterName == null || string.Compare(this.counterName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.counterName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06003A00 RID: 14848 RVA: 0x000F5584 File Offset: 0x000F4584
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("PC_CounterType")]
		public PerformanceCounterType CounterType
		{
			get
			{
				if (this.counterType == -1)
				{
					string category = this.categoryName;
					string machine = this.machineName;
					PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machine, category);
					performanceCounterPermission.Demand();
					this.Initialize();
					CategorySample categorySample = PerformanceCounterLib.GetCategorySample(machine, category);
					CounterDefinitionSample counterDefinitionSample = categorySample.GetCounterDefinitionSample(this.counterName);
					this.counterType = counterDefinitionSample.CounterType;
				}
				return (PerformanceCounterType)this.counterType;
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003A01 RID: 14849 RVA: 0x000F55E6 File Offset: 0x000F45E6
		// (set) Token: 0x06003A02 RID: 14850 RVA: 0x000F55EE File Offset: 0x000F45EE
		[SRDescription("PCInstanceLifetime")]
		[DefaultValue(PerformanceCounterInstanceLifetime.Global)]
		public PerformanceCounterInstanceLifetime InstanceLifetime
		{
			get
			{
				return this.instanceLifetime;
			}
			set
			{
				if (value > PerformanceCounterInstanceLifetime.Process || value < PerformanceCounterInstanceLifetime.Global)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("CantSetLifetimeAfterInitialized"));
				}
				this.instanceLifetime = value;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003A03 RID: 14851 RVA: 0x000F5622 File Offset: 0x000F4622
		// (set) Token: 0x06003A04 RID: 14852 RVA: 0x000F562A File Offset: 0x000F462A
		[ReadOnly(true)]
		[RecommendedAsConfigurable(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.InstanceNameConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SRDescription("PCInstanceName")]
		public string InstanceName
		{
			get
			{
				return this.instanceName;
			}
			set
			{
				if (value == null && this.instanceName == null)
				{
					return;
				}
				if ((value == null && this.instanceName != null) || (value != null && this.instanceName == null) || string.Compare(this.instanceName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.instanceName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003A05 RID: 14853 RVA: 0x000F566A File Offset: 0x000F466A
		// (set) Token: 0x06003A06 RID: 14854 RVA: 0x000F5672 File Offset: 0x000F4672
		[Browsable(false)]
		[MonitoringDescription("PC_ReadOnly")]
		[DefaultValue(true)]
		public bool ReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
			set
			{
				if (value != this.isReadOnly)
				{
					this.isReadOnly = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x000F568A File Offset: 0x000F468A
		// (set) Token: 0x06003A08 RID: 14856 RVA: 0x000F5694 File Offset: 0x000F4694
		[Browsable(false)]
		[DefaultValue(".")]
		[SRDescription("PCMachineName")]
		[RecommendedAsConfigurable(true)]
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidParameter", new object[]
					{
						"machineName",
						value
					}));
				}
				if (this.machineName != value)
				{
					this.machineName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06003A09 RID: 14857 RVA: 0x000F56E8 File Offset: 0x000F46E8
		// (set) Token: 0x06003A0A RID: 14858 RVA: 0x000F571D File Offset: 0x000F471D
		[MonitoringDescription("PC_RawValue")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public long RawValue
		{
			get
			{
				if (this.ReadOnly)
				{
					return this.NextSample().RawValue;
				}
				this.Initialize();
				return this.sharedCounter.Value;
			}
			set
			{
				if (this.ReadOnly)
				{
					this.ThrowReadOnly();
				}
				this.Initialize();
				this.sharedCounter.Value = value;
			}
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x000F573F File Offset: 0x000F473F
		public void BeginInit()
		{
			this.Close();
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x000F5747 File Offset: 0x000F4747
		public void Close()
		{
			this.helpMsg = null;
			this.oldSample = CounterSample.Empty;
			this.sharedCounter = null;
			this.initialized = false;
			this.counterType = -1;
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x000F5770 File Offset: 0x000F4770
		public static void CloseSharedResources()
		{
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, ".", "*");
			performanceCounterPermission.Demand();
			PerformanceCounterLib.CloseAllLibraries();
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x000F5799 File Offset: 0x000F4799
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x000F57AB File Offset: 0x000F47AB
		public long Decrement()
		{
			if (this.ReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.Decrement();
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x000F57CC File Offset: 0x000F47CC
		public void EndInit()
		{
			this.Initialize();
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x000F57D4 File Offset: 0x000F47D4
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public long IncrementBy(long value)
		{
			if (this.isReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.IncrementBy(value);
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x000F57F6 File Offset: 0x000F47F6
		public long Increment()
		{
			if (this.isReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.Increment();
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x000F5817 File Offset: 0x000F4817
		private void ThrowReadOnly()
		{
			throw new InvalidOperationException(SR.GetString("ReadOnlyCounter"));
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x000F5828 File Offset: 0x000F4828
		private void Initialize()
		{
			if (!this.initialized && !base.DesignMode)
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						Monitor.Enter(this);
						flag = true;
					}
					if (!this.initialized)
					{
						string text = this.categoryName;
						string text2 = this.machineName;
						if (text == string.Empty)
						{
							throw new InvalidOperationException(SR.GetString("CategoryNameMissing"));
						}
						if (this.counterName == string.Empty)
						{
							throw new InvalidOperationException(SR.GetString("CounterNameMissing"));
						}
						if (this.ReadOnly)
						{
							PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
							performanceCounterPermission.Demand();
							if (!PerformanceCounterLib.CounterExists(text2, text, this.counterName))
							{
								throw new InvalidOperationException(SR.GetString("CounterExists", new object[]
								{
									text,
									this.counterName
								}));
							}
							PerformanceCounterCategoryType categoryType = PerformanceCounterLib.GetCategoryType(text2, text);
							if (categoryType == PerformanceCounterCategoryType.MultiInstance)
							{
								if (string.IsNullOrEmpty(this.instanceName))
								{
									throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[]
									{
										text
									}));
								}
							}
							else if (categoryType == PerformanceCounterCategoryType.SingleInstance && !string.IsNullOrEmpty(this.instanceName))
							{
								throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[]
								{
									text
								}));
							}
							if (this.instanceLifetime != PerformanceCounterInstanceLifetime.Global)
							{
								throw new InvalidOperationException(SR.GetString("InstanceLifetimeProcessonReadOnly"));
							}
							this.initialized = true;
						}
						else
						{
							PerformanceCounterPermission performanceCounterPermission2 = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Write, text2, text);
							performanceCounterPermission2.Demand();
							if (text2 != "." && string.Compare(text2, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) != 0)
							{
								throw new InvalidOperationException(SR.GetString("RemoteWriting"));
							}
							SharedUtils.CheckNtEnvironment();
							if (!PerformanceCounterLib.IsCustomCategory(text2, text))
							{
								throw new InvalidOperationException(SR.GetString("NotCustomCounter"));
							}
							PerformanceCounterCategoryType categoryType2 = PerformanceCounterLib.GetCategoryType(text2, text);
							if (categoryType2 == PerformanceCounterCategoryType.MultiInstance)
							{
								if (string.IsNullOrEmpty(this.instanceName))
								{
									throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[]
									{
										text
									}));
								}
							}
							else if (categoryType2 == PerformanceCounterCategoryType.SingleInstance && !string.IsNullOrEmpty(this.instanceName))
							{
								throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[]
								{
									text
								}));
							}
							if (string.IsNullOrEmpty(this.instanceName) && this.InstanceLifetime == PerformanceCounterInstanceLifetime.Process)
							{
								throw new InvalidOperationException(SR.GetString("InstanceLifetimeProcessforSingleInstance"));
							}
							this.sharedCounter = new SharedPerformanceCounter(text.ToLower(CultureInfo.InvariantCulture), this.counterName.ToLower(CultureInfo.InvariantCulture), this.instanceName.ToLower(CultureInfo.InvariantCulture), this.instanceLifetime);
							this.initialized = true;
						}
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x000F5B08 File Offset: 0x000F4B08
		public CounterSample NextSample()
		{
			string category = this.categoryName;
			string machine = this.machineName;
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, machine, category);
			performanceCounterPermission.Demand();
			this.Initialize();
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(machine, category);
			CounterDefinitionSample counterDefinitionSample = categorySample.GetCounterDefinitionSample(this.counterName);
			this.counterType = counterDefinitionSample.CounterType;
			if (!categorySample.IsMultiInstance)
			{
				if (this.instanceName != null && this.instanceName.Length != 0)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameProhibited", new object[]
					{
						this.instanceName
					}));
				}
				return counterDefinitionSample.GetSingleValue();
			}
			else
			{
				if (this.instanceName == null || this.instanceName.Length == 0)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameRequired"));
				}
				return counterDefinitionSample.GetInstanceValue(this.instanceName);
			}
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x000F5BD8 File Offset: 0x000F4BD8
		public float NextValue()
		{
			CounterSample nextCounterSample = this.NextSample();
			float result = CounterSample.Calculate(this.oldSample, nextCounterSample);
			this.oldSample = nextCounterSample;
			return result;
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x000F5C08 File Offset: 0x000F4C08
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void RemoveInstance()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("ReadOnlyRemoveInstance"));
			}
			this.Initialize();
			this.sharedCounter.RemoveInstance(this.instanceName.ToLower(CultureInfo.InvariantCulture), this.instanceLifetime);
		}

		// Token: 0x040032E6 RID: 13030
		private string machineName;

		// Token: 0x040032E7 RID: 13031
		private string categoryName;

		// Token: 0x040032E8 RID: 13032
		private string counterName;

		// Token: 0x040032E9 RID: 13033
		private string instanceName;

		// Token: 0x040032EA RID: 13034
		private PerformanceCounterInstanceLifetime instanceLifetime;

		// Token: 0x040032EB RID: 13035
		private bool isReadOnly;

		// Token: 0x040032EC RID: 13036
		private bool initialized;

		// Token: 0x040032ED RID: 13037
		private string helpMsg;

		// Token: 0x040032EE RID: 13038
		private int counterType = -1;

		// Token: 0x040032EF RID: 13039
		private CounterSample oldSample = CounterSample.Empty;

		// Token: 0x040032F0 RID: 13040
		private SharedPerformanceCounter sharedCounter;

		// Token: 0x040032F1 RID: 13041
		[Obsolete("This field has been deprecated and is not used.  Use machine.config or an application configuration file to set the size of the PerformanceCounter file mapping.")]
		public static int DefaultFileMappingSize = 524288;
	}
}
