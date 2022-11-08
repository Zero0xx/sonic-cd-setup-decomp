using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.IO.IsolatedStorage
{
	// Token: 0x020007B1 RID: 1969
	[ComVisible(true)]
	[Serializable]
	public class IsolatedStorageException : Exception
	{
		// Token: 0x0600465F RID: 18015 RVA: 0x000F06BC File Offset: 0x000EF6BC
		public IsolatedStorageException() : base(Environment.GetResourceString("IsolatedStorage_Exception"))
		{
			base.SetErrorCode(-2146233264);
		}

		// Token: 0x06004660 RID: 18016 RVA: 0x000F06D9 File Offset: 0x000EF6D9
		public IsolatedStorageException(string message) : base(message)
		{
			base.SetErrorCode(-2146233264);
		}

		// Token: 0x06004661 RID: 18017 RVA: 0x000F06ED File Offset: 0x000EF6ED
		public IsolatedStorageException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233264);
		}

		// Token: 0x06004662 RID: 18018 RVA: 0x000F0702 File Offset: 0x000EF702
		protected IsolatedStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
