using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000108 RID: 264
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class LicenseException : SystemException
	{
		// Token: 0x06000841 RID: 2113 RVA: 0x0001C234 File Offset: 0x0001B234
		public LicenseException(Type type) : this(type, null, SR.GetString("LicExceptionTypeOnly", new object[]
		{
			type.FullName
		}))
		{
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001C264 File Offset: 0x0001B264
		public LicenseException(Type type, object instance) : this(type, null, SR.GetString("LicExceptionTypeAndInstance", new object[]
		{
			type.FullName,
			instance.GetType().FullName
		}))
		{
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001C2A2 File Offset: 0x0001B2A2
		public LicenseException(Type type, object instance, string message) : base(message)
		{
			this.type = type;
			this.instance = instance;
			base.HResult = -2146232063;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001C2C4 File Offset: 0x0001B2C4
		public LicenseException(Type type, object instance, string message, Exception innerException) : base(message, innerException)
		{
			this.type = type;
			this.instance = instance;
			base.HResult = -2146232063;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001C2E8 File Offset: 0x0001B2E8
		protected LicenseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.type = (Type)info.GetValue("type", typeof(Type));
			this.instance = info.GetValue("instance", typeof(object));
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001C338 File Offset: 0x0001B338
		public Type LicensedType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001C340 File Offset: 0x0001B340
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("type", this.type);
			info.AddValue("instance", this.instance);
			base.GetObjectData(info, context);
		}

		// Token: 0x04000984 RID: 2436
		private Type type;

		// Token: 0x04000985 RID: 2437
		private object instance;
	}
}
