using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002DF RID: 735
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyCopyrightAttribute : Attribute
	{
		// Token: 0x06001CFB RID: 7419 RVA: 0x00049CE4 File Offset: 0x00048CE4
		public AssemblyCopyrightAttribute(string copyright)
		{
			this.m_copyright = copyright;
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x00049CF3 File Offset: 0x00048CF3
		public string Copyright
		{
			get
			{
				return this.m_copyright;
			}
		}

		// Token: 0x04000ACB RID: 2763
		private string m_copyright;
	}
}
