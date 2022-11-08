using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.AccessControl
{
	// Token: 0x02000901 RID: 2305
	[Serializable]
	public sealed class PrivilegeNotHeldException : UnauthorizedAccessException, ISerializable
	{
		// Token: 0x06005391 RID: 21393 RVA: 0x0012DAB0 File Offset: 0x0012CAB0
		public PrivilegeNotHeldException() : base(Environment.GetResourceString("PrivilegeNotHeld_Default"))
		{
		}

		// Token: 0x06005392 RID: 21394 RVA: 0x0012DAC4 File Offset: 0x0012CAC4
		public PrivilegeNotHeldException(string privilege) : base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("PrivilegeNotHeld_Named"), new object[]
		{
			privilege
		}))
		{
			this._privilegeName = privilege;
		}

		// Token: 0x06005393 RID: 21395 RVA: 0x0012DB00 File Offset: 0x0012CB00
		public PrivilegeNotHeldException(string privilege, Exception inner) : base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("PrivilegeNotHeld_Named"), new object[]
		{
			privilege
		}), inner)
		{
			this._privilegeName = privilege;
		}

		// Token: 0x06005394 RID: 21396 RVA: 0x0012DB3B File Offset: 0x0012CB3B
		internal PrivilegeNotHeldException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._privilegeName = info.GetString("PrivilegeName");
		}

		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005395 RID: 21397 RVA: 0x0012DB56 File Offset: 0x0012CB56
		public string PrivilegeName
		{
			get
			{
				return this._privilegeName;
			}
		}

		// Token: 0x06005396 RID: 21398 RVA: 0x0012DB5E File Offset: 0x0012CB5E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("PrivilegeName", this._privilegeName, typeof(string));
		}

		// Token: 0x04002B4C RID: 11084
		private readonly string _privilegeName;
	}
}
