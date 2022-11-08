using System;
using System.Configuration;

namespace System.Windows.Forms
{
	// Token: 0x0200073C RID: 1852
	public sealed class WindowsFormsSection : ConfigurationSection
	{
		// Token: 0x0600629D RID: 25245 RVA: 0x00166928 File Offset: 0x00165928
		internal static WindowsFormsSection GetSection()
		{
			WindowsFormsSection result = null;
			try
			{
				result = (WindowsFormsSection)PrivilegedConfigurationManager.GetSection("system.windows.forms");
			}
			catch
			{
				result = new WindowsFormsSection();
			}
			return result;
		}

		// Token: 0x0600629E RID: 25246 RVA: 0x00166964 File Offset: 0x00165964
		private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
		{
			if (WindowsFormsSection.s_properties == null)
			{
				WindowsFormsSection.s_propJitDebugging = new ConfigurationProperty("jitDebugging", typeof(bool), false, ConfigurationPropertyOptions.None);
				WindowsFormsSection.s_properties = new ConfigurationPropertyCollection
				{
					WindowsFormsSection.s_propJitDebugging
				};
			}
			return WindowsFormsSection.s_properties;
		}

		// Token: 0x0600629F RID: 25247 RVA: 0x001669B4 File Offset: 0x001659B4
		public WindowsFormsSection()
		{
			WindowsFormsSection.EnsureStaticPropertyBag();
		}

		// Token: 0x170014BC RID: 5308
		// (get) Token: 0x060062A0 RID: 25248 RVA: 0x001669C2 File Offset: 0x001659C2
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return WindowsFormsSection.EnsureStaticPropertyBag();
			}
		}

		// Token: 0x170014BD RID: 5309
		// (get) Token: 0x060062A1 RID: 25249 RVA: 0x001669C9 File Offset: 0x001659C9
		// (set) Token: 0x060062A2 RID: 25250 RVA: 0x001669DB File Offset: 0x001659DB
		[ConfigurationProperty("jitDebugging", DefaultValue = false)]
		public bool JitDebugging
		{
			get
			{
				return (bool)base[WindowsFormsSection.s_propJitDebugging];
			}
			set
			{
				base[WindowsFormsSection.s_propJitDebugging] = value;
			}
		}

		// Token: 0x04003B22 RID: 15138
		internal const bool JitDebuggingDefault = false;

		// Token: 0x04003B23 RID: 15139
		private static ConfigurationPropertyCollection s_properties;

		// Token: 0x04003B24 RID: 15140
		private static ConfigurationProperty s_propJitDebugging;
	}
}
