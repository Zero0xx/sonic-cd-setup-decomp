using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000130 RID: 304
	[ComVisible(true)]
	[Serializable]
	public sealed class TypeInitializationException : SystemException
	{
		// Token: 0x060010C0 RID: 4288 RVA: 0x0002EFE1 File Offset: 0x0002DFE1
		private TypeInitializationException() : base(Environment.GetResourceString("TypeInitialization_Default"))
		{
			base.SetErrorCode(-2146233036);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x0002EFFE File Offset: 0x0002DFFE
		private TypeInitializationException(string message) : base(message)
		{
			base.SetErrorCode(-2146233036);
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x0002F014 File Offset: 0x0002E014
		public TypeInitializationException(string fullTypeName, Exception innerException) : base(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("TypeInitialization_Type"), new object[]
		{
			fullTypeName
		}), innerException)
		{
			this._typeName = fullTypeName;
			base.SetErrorCode(-2146233036);
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x0002F05A File Offset: 0x0002E05A
		internal TypeInitializationException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._typeName = info.GetString("TypeName");
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0002F075 File Offset: 0x0002E075
		public string TypeName
		{
			get
			{
				if (this._typeName == null)
				{
					return string.Empty;
				}
				return this._typeName;
			}
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x0002F08B File Offset: 0x0002E08B
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("TypeName", this.TypeName, typeof(string));
		}

		// Token: 0x040005B9 RID: 1465
		private string _typeName;
	}
}
