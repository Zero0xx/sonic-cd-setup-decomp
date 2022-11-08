using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200060B RID: 1547
	[Serializable]
	public sealed class RuntimeWrappedException : Exception
	{
		// Token: 0x06003808 RID: 14344 RVA: 0x000BBD75 File Offset: 0x000BAD75
		private RuntimeWrappedException(object thrownObject) : base(Environment.GetResourceString("RuntimeWrappedException"))
		{
			base.SetErrorCode(-2146233026);
			this.m_wrappedException = thrownObject;
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x06003809 RID: 14345 RVA: 0x000BBD99 File Offset: 0x000BAD99
		public object WrappedException
		{
			get
			{
				return this.m_wrappedException;
			}
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000BBDA1 File Offset: 0x000BADA1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("WrappedException", this.m_wrappedException, typeof(object));
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000BBDD4 File Offset: 0x000BADD4
		internal RuntimeWrappedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.m_wrappedException = info.GetValue("WrappedException", typeof(object));
		}

		// Token: 0x04001D09 RID: 7433
		private object m_wrappedException;
	}
}
