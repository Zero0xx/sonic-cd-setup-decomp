using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EF RID: 751
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyKeyNameAttribute : Attribute
	{
		// Token: 0x06001D1F RID: 7455 RVA: 0x00049E97 File Offset: 0x00048E97
		public AssemblyKeyNameAttribute(string keyName)
		{
			this.m_keyName = keyName;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x06001D20 RID: 7456 RVA: 0x00049EA6 File Offset: 0x00048EA6
		public string KeyName
		{
			get
			{
				return this.m_keyName;
			}
		}

		// Token: 0x04000ADB RID: 2779
		private string m_keyName;
	}
}
