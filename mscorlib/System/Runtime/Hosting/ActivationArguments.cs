using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Hosting
{
	// Token: 0x02000064 RID: 100
	[ComVisible(true)]
	[Serializable]
	public sealed class ActivationArguments
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x00014DF1 File Offset: 0x00013DF1
		private ActivationArguments()
		{
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000602 RID: 1538 RVA: 0x00014DF9 File Offset: 0x00013DF9
		internal bool UseFusionActivationContext
		{
			get
			{
				return this.m_useFusionActivationContext;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00014E01 File Offset: 0x00013E01
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x00014E09 File Offset: 0x00013E09
		internal bool ActivateInstance
		{
			get
			{
				return this.m_activateInstance;
			}
			set
			{
				this.m_activateInstance = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00014E12 File Offset: 0x00013E12
		internal string ApplicationFullName
		{
			get
			{
				return this.m_appFullName;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00014E1A File Offset: 0x00013E1A
		internal string[] ApplicationManifestPaths
		{
			get
			{
				return this.m_appManifestPaths;
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00014E22 File Offset: 0x00013E22
		public ActivationArguments(ApplicationIdentity applicationIdentity) : this(applicationIdentity, null)
		{
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00014E2C File Offset: 0x00013E2C
		public ActivationArguments(ApplicationIdentity applicationIdentity, string[] activationData)
		{
			if (applicationIdentity == null)
			{
				throw new ArgumentNullException("applicationIdentity");
			}
			this.m_appFullName = applicationIdentity.FullName;
			this.m_activationData = activationData;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00014E55 File Offset: 0x00013E55
		public ActivationArguments(ActivationContext activationData) : this(activationData, null)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00014E60 File Offset: 0x00013E60
		public ActivationArguments(ActivationContext activationContext, string[] activationData)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			this.m_appFullName = activationContext.Identity.FullName;
			this.m_appManifestPaths = activationContext.ManifestPaths;
			this.m_activationData = activationData;
			this.m_useFusionActivationContext = true;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00014EAC File Offset: 0x00013EAC
		internal ActivationArguments(string appFullName, string[] appManifestPaths, string[] activationData)
		{
			if (appFullName == null)
			{
				throw new ArgumentNullException("appFullName");
			}
			this.m_appFullName = appFullName;
			this.m_appManifestPaths = appManifestPaths;
			this.m_activationData = activationData;
			this.m_useFusionActivationContext = true;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x00014EDE File Offset: 0x00013EDE
		public ApplicationIdentity ApplicationIdentity
		{
			get
			{
				return new ApplicationIdentity(this.m_appFullName);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00014EEB File Offset: 0x00013EEB
		public ActivationContext ActivationContext
		{
			get
			{
				if (!this.UseFusionActivationContext)
				{
					return null;
				}
				if (this.m_appManifestPaths == null)
				{
					return new ActivationContext(new ApplicationIdentity(this.m_appFullName));
				}
				return new ActivationContext(new ApplicationIdentity(this.m_appFullName), this.m_appManifestPaths);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x00014F26 File Offset: 0x00013F26
		public string[] ActivationData
		{
			get
			{
				return this.m_activationData;
			}
		}

		// Token: 0x040001D6 RID: 470
		private bool m_useFusionActivationContext;

		// Token: 0x040001D7 RID: 471
		private bool m_activateInstance;

		// Token: 0x040001D8 RID: 472
		private string m_appFullName;

		// Token: 0x040001D9 RID: 473
		private string[] m_appManifestPaths;

		// Token: 0x040001DA RID: 474
		private string[] m_activationData;
	}
}
