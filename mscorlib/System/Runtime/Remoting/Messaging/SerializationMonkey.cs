using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Messaging
{
	// Token: 0x02000720 RID: 1824
	[Serializable]
	internal class SerializationMonkey : ISerializable, IFieldInfo
	{
		// Token: 0x06004164 RID: 16740 RVA: 0x000DEDBC File Offset: 0x000DDDBC
		internal SerializationMonkey(SerializationInfo info, StreamingContext ctx)
		{
			this._obj.RootSetObjectData(info, ctx);
		}

		// Token: 0x06004165 RID: 16741 RVA: 0x000DEDD1 File Offset: 0x000DDDD1
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_Method"));
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x000DEDE2 File Offset: 0x000DDDE2
		// (set) Token: 0x06004167 RID: 16743 RVA: 0x000DEDEA File Offset: 0x000DDDEA
		public string[] FieldNames
		{
			get
			{
				return this.fieldNames;
			}
			set
			{
				this.fieldNames = value;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06004168 RID: 16744 RVA: 0x000DEDF3 File Offset: 0x000DDDF3
		// (set) Token: 0x06004169 RID: 16745 RVA: 0x000DEDFB File Offset: 0x000DDDFB
		public Type[] FieldTypes
		{
			get
			{
				return this.fieldTypes;
			}
			set
			{
				this.fieldTypes = value;
			}
		}

		// Token: 0x040020E1 RID: 8417
		internal ISerializationRootObject _obj;

		// Token: 0x040020E2 RID: 8418
		internal string[] fieldNames;

		// Token: 0x040020E3 RID: 8419
		internal Type[] fieldTypes;
	}
}
