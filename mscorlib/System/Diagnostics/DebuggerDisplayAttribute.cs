using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002BE RID: 702
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Delegate, AllowMultiple = true)]
	[ComVisible(true)]
	public sealed class DebuggerDisplayAttribute : Attribute
	{
		// Token: 0x06001B2C RID: 6956 RVA: 0x00046F9E File Offset: 0x00045F9E
		public DebuggerDisplayAttribute(string value)
		{
			if (value == null)
			{
				this.value = "";
			}
			else
			{
				this.value = value;
			}
			this.name = "";
			this.type = "";
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x00046FD3 File Offset: 0x00045FD3
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001B2E RID: 6958 RVA: 0x00046FDB File Offset: 0x00045FDB
		// (set) Token: 0x06001B2F RID: 6959 RVA: 0x00046FE3 File Offset: 0x00045FE3
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001B30 RID: 6960 RVA: 0x00046FEC File Offset: 0x00045FEC
		// (set) Token: 0x06001B31 RID: 6961 RVA: 0x00046FF4 File Offset: 0x00045FF4
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001B33 RID: 6963 RVA: 0x00047020 File Offset: 0x00046020
		// (set) Token: 0x06001B32 RID: 6962 RVA: 0x00046FFD File Offset: 0x00045FFD
		public Type Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.targetName = value.AssemblyQualifiedName;
				this.target = value;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001B34 RID: 6964 RVA: 0x00047028 File Offset: 0x00046028
		// (set) Token: 0x06001B35 RID: 6965 RVA: 0x00047030 File Offset: 0x00046030
		public string TargetTypeName
		{
			get
			{
				return this.targetName;
			}
			set
			{
				this.targetName = value;
			}
		}

		// Token: 0x04000A62 RID: 2658
		private string name;

		// Token: 0x04000A63 RID: 2659
		private string value;

		// Token: 0x04000A64 RID: 2660
		private string type;

		// Token: 0x04000A65 RID: 2661
		private string targetName;

		// Token: 0x04000A66 RID: 2662
		private Type target;
	}
}
