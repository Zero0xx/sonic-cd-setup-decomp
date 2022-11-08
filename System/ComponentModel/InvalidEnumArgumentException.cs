using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020000FF RID: 255
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class InvalidEnumArgumentException : ArgumentException
	{
		// Token: 0x06000823 RID: 2083 RVA: 0x0001C1AE File Offset: 0x0001B1AE
		public InvalidEnumArgumentException() : this(null)
		{
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0001C1B7 File Offset: 0x0001B1B7
		public InvalidEnumArgumentException(string message) : base(message)
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0001C1C0 File Offset: 0x0001B1C0
		public InvalidEnumArgumentException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0001C1CC File Offset: 0x0001B1CC
		public InvalidEnumArgumentException(string argumentName, int invalidValue, Type enumClass) : base(SR.GetString("InvalidEnumArgument", new object[]
		{
			argumentName,
			invalidValue.ToString(CultureInfo.CurrentCulture),
			enumClass.Name
		}), argumentName)
		{
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0001C20E File Offset: 0x0001B20E
		protected InvalidEnumArgumentException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
