using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x0200033D RID: 829
	[ComVisible(true)]
	[Serializable]
	public sealed class ReflectionTypeLoadException : SystemException, ISerializable
	{
		// Token: 0x06001FBB RID: 8123 RVA: 0x0004FB7B File Offset: 0x0004EB7B
		private ReflectionTypeLoadException() : base(Environment.GetResourceString("ReflectionTypeLoad_LoadFailed"))
		{
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x0004FB98 File Offset: 0x0004EB98
		private ReflectionTypeLoadException(string message) : base(message)
		{
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x0004FBAC File Offset: 0x0004EBAC
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions) : base(null)
		{
			this._classes = classes;
			this._exceptions = exceptions;
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0004FBCE File Offset: 0x0004EBCE
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions, string message) : base(message)
		{
			this._classes = classes;
			this._exceptions = exceptions;
			base.SetErrorCode(-2146232830);
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0004FBF0 File Offset: 0x0004EBF0
		internal ReflectionTypeLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._classes = (Type[])info.GetValue("Types", typeof(Type[]));
			this._exceptions = (Exception[])info.GetValue("Exceptions", typeof(Exception[]));
		}

		// Token: 0x17000542 RID: 1346
		// (get) Token: 0x06001FC0 RID: 8128 RVA: 0x0004FC45 File Offset: 0x0004EC45
		public Type[] Types
		{
			get
			{
				return this._classes;
			}
		}

		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x0004FC4D File Offset: 0x0004EC4D
		public Exception[] LoaderExceptions
		{
			get
			{
				return this._exceptions;
			}
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x0004FC58 File Offset: 0x0004EC58
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("Types", this._classes, typeof(Type[]));
			info.AddValue("Exceptions", this._exceptions, typeof(Exception[]));
		}

		// Token: 0x04000DB9 RID: 3513
		private Type[] _classes;

		// Token: 0x04000DBA RID: 3514
		private Exception[] _exceptions;
	}
}
