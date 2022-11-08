using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x0200006E RID: 110
	[ComVisible(true)]
	[Serializable]
	public class ArgumentNullException : ArgumentException
	{
		// Token: 0x06000651 RID: 1617 RVA: 0x00015A66 File Offset: 0x00014A66
		public ArgumentNullException() : base(Environment.GetResourceString("ArgumentNull_Generic"))
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00015A83 File Offset: 0x00014A83
		public ArgumentNullException(string paramName) : base(Environment.GetResourceString("ArgumentNull_Generic"), paramName)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00015AA1 File Offset: 0x00014AA1
		public ArgumentNullException(string message, Exception innerException) : base(message, innerException)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00015AB6 File Offset: 0x00014AB6
		public ArgumentNullException(string paramName, string message) : base(message, paramName)
		{
			base.SetErrorCode(-2147467261);
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x00015ACB File Offset: 0x00014ACB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		protected ArgumentNullException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
