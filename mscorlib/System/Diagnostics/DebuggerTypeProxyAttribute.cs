using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x020002BD RID: 701
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
	[ComVisible(true)]
	public sealed class DebuggerTypeProxyAttribute : Attribute
	{
		// Token: 0x06001B25 RID: 6949 RVA: 0x00046F29 File Offset: 0x00045F29
		public DebuggerTypeProxyAttribute(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00046F4B File Offset: 0x00045F4B
		public DebuggerTypeProxyAttribute(string typeName)
		{
			this.typeName = typeName;
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06001B27 RID: 6951 RVA: 0x00046F5A File Offset: 0x00045F5A
		public string ProxyTypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06001B29 RID: 6953 RVA: 0x00046F85 File Offset: 0x00045F85
		// (set) Token: 0x06001B28 RID: 6952 RVA: 0x00046F62 File Offset: 0x00045F62
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

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001B2A RID: 6954 RVA: 0x00046F8D File Offset: 0x00045F8D
		// (set) Token: 0x06001B2B RID: 6955 RVA: 0x00046F95 File Offset: 0x00045F95
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

		// Token: 0x04000A5F RID: 2655
		private string typeName;

		// Token: 0x04000A60 RID: 2656
		private string targetName;

		// Token: 0x04000A61 RID: 2657
		private Type target;
	}
}
