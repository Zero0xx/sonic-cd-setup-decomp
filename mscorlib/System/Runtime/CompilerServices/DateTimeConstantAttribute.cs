using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005E1 RID: 1505
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
	[Serializable]
	public sealed class DateTimeConstantAttribute : CustomConstantAttribute
	{
		// Token: 0x060037DA RID: 14298 RVA: 0x000BBB40 File Offset: 0x000BAB40
		public DateTimeConstantAttribute(long ticks)
		{
			this.date = new DateTime(ticks);
		}

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x060037DB RID: 14299 RVA: 0x000BBB54 File Offset: 0x000BAB54
		public override object Value
		{
			get
			{
				return this.date;
			}
		}

		// Token: 0x04001CE6 RID: 7398
		private DateTime date;
	}
}
