using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000334 RID: 820
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate, AllowMultiple = true, Inherited = false)]
	public sealed class ObfuscationAttribute : Attribute
	{
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x0004F6CA File Offset: 0x0004E6CA
		// (set) Token: 0x06001F94 RID: 8084 RVA: 0x0004F6D2 File Offset: 0x0004E6D2
		public bool StripAfterObfuscation
		{
			get
			{
				return this.m_strip;
			}
			set
			{
				this.m_strip = value;
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x0004F6DB File Offset: 0x0004E6DB
		// (set) Token: 0x06001F96 RID: 8086 RVA: 0x0004F6E3 File Offset: 0x0004E6E3
		public bool Exclude
		{
			get
			{
				return this.m_exclude;
			}
			set
			{
				this.m_exclude = value;
			}
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x0004F6EC File Offset: 0x0004E6EC
		// (set) Token: 0x06001F98 RID: 8088 RVA: 0x0004F6F4 File Offset: 0x0004E6F4
		public bool ApplyToMembers
		{
			get
			{
				return this.m_applyToMembers;
			}
			set
			{
				this.m_applyToMembers = value;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x0004F6FD File Offset: 0x0004E6FD
		// (set) Token: 0x06001F9A RID: 8090 RVA: 0x0004F705 File Offset: 0x0004E705
		public string Feature
		{
			get
			{
				return this.m_feature;
			}
			set
			{
				this.m_feature = value;
			}
		}

		// Token: 0x04000D86 RID: 3462
		private bool m_strip = true;

		// Token: 0x04000D87 RID: 3463
		private bool m_exclude = true;

		// Token: 0x04000D88 RID: 3464
		private bool m_applyToMembers = true;

		// Token: 0x04000D89 RID: 3465
		private string m_feature = "all";
	}
}
