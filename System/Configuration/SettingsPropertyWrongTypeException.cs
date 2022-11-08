using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x02000717 RID: 1815
	[Serializable]
	public class SettingsPropertyWrongTypeException : Exception
	{
		// Token: 0x06003796 RID: 14230 RVA: 0x000EBAB6 File Offset: 0x000EAAB6
		public SettingsPropertyWrongTypeException(string message) : base(message)
		{
		}

		// Token: 0x06003797 RID: 14231 RVA: 0x000EBABF File Offset: 0x000EAABF
		public SettingsPropertyWrongTypeException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x000EBAC9 File Offset: 0x000EAAC9
		protected SettingsPropertyWrongTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x000EBAD3 File Offset: 0x000EAAD3
		public SettingsPropertyWrongTypeException()
		{
		}
	}
}
