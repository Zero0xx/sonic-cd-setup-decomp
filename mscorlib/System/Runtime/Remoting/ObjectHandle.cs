using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x0200072F RID: 1839
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	public class ObjectHandle : MarshalByRefObject, IObjectHandle
	{
		// Token: 0x060041ED RID: 16877 RVA: 0x000E063F File Offset: 0x000DF63F
		private ObjectHandle()
		{
		}

		// Token: 0x060041EE RID: 16878 RVA: 0x000E0647 File Offset: 0x000DF647
		public ObjectHandle(object o)
		{
			this.WrappedObject = o;
		}

		// Token: 0x060041EF RID: 16879 RVA: 0x000E0656 File Offset: 0x000DF656
		public object Unwrap()
		{
			return this.WrappedObject;
		}

		// Token: 0x060041F0 RID: 16880 RVA: 0x000E0660 File Offset: 0x000DF660
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override object InitializeLifetimeService()
		{
			MarshalByRefObject marshalByRefObject = this.WrappedObject as MarshalByRefObject;
			if (marshalByRefObject != null && marshalByRefObject.InitializeLifetimeService() == null)
			{
				return null;
			}
			return (ILease)base.InitializeLifetimeService();
		}

		// Token: 0x04002115 RID: 8469
		private object WrappedObject;
	}
}
