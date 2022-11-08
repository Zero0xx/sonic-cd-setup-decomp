using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Threading
{
	// Token: 0x0200013F RID: 319
	[ComVisible(false)]
	[Serializable]
	public class AbandonedMutexException : SystemException
	{
		// Token: 0x06001192 RID: 4498 RVA: 0x00031590 File Offset: 0x00030590
		public AbandonedMutexException() : base(Environment.GetResourceString("Threading.AbandonedMutexException"))
		{
			base.SetErrorCode(-2146233043);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x000315B4 File Offset: 0x000305B4
		public AbandonedMutexException(string message) : base(message)
		{
			base.SetErrorCode(-2146233043);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x000315CF File Offset: 0x000305CF
		public AbandonedMutexException(string message, Exception inner) : base(message, inner)
		{
			base.SetErrorCode(-2146233043);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x000315EB File Offset: 0x000305EB
		public AbandonedMutexException(int location, WaitHandle handle) : base(Environment.GetResourceString("Threading.AbandonedMutexException"))
		{
			base.SetErrorCode(-2146233043);
			this.SetupException(location, handle);
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00031617 File Offset: 0x00030617
		public AbandonedMutexException(string message, int location, WaitHandle handle) : base(message)
		{
			base.SetErrorCode(-2146233043);
			this.SetupException(location, handle);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0003163A File Offset: 0x0003063A
		public AbandonedMutexException(string message, Exception inner, int location, WaitHandle handle) : base(message, inner)
		{
			base.SetErrorCode(-2146233043);
			this.SetupException(location, handle);
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0003165F File Offset: 0x0003065F
		private void SetupException(int location, WaitHandle handle)
		{
			this.m_MutexIndex = location;
			if (handle != null)
			{
				this.m_Mutex = (handle as Mutex);
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00031677 File Offset: 0x00030677
		protected AbandonedMutexException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00031688 File Offset: 0x00030688
		public Mutex Mutex
		{
			get
			{
				return this.m_Mutex;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00031690 File Offset: 0x00030690
		public int MutexIndex
		{
			get
			{
				return this.m_MutexIndex;
			}
		}

		// Token: 0x04000612 RID: 1554
		private int m_MutexIndex = -1;

		// Token: 0x04000613 RID: 1555
		private Mutex m_Mutex;
	}
}
