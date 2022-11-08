using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000071 RID: 113
	[ComVisible(true)]
	[Serializable]
	public class ArithmeticException : SystemException
	{
		// Token: 0x0600066E RID: 1646 RVA: 0x00015CE6 File Offset: 0x00014CE6
		public ArithmeticException() : base(Environment.GetResourceString("Arg_ArithmeticException"))
		{
			base.SetErrorCode(-2147024362);
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00015D03 File Offset: 0x00014D03
		public ArithmeticException(string message) : base(message)
		{
			base.SetErrorCode(-2147024362);
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00015D17 File Offset: 0x00014D17
		public ArithmeticException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147024362);
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00015D2C File Offset: 0x00014D2C
		protected ArithmeticException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
