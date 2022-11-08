using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007F0 RID: 2032
	internal sealed class ValueFixup
	{
		// Token: 0x060047BD RID: 18365 RVA: 0x000F5F53 File Offset: 0x000F4F53
		internal ValueFixup(Array arrayObj, int[] indexMap)
		{
			this.valueFixupEnum = ValueFixupEnum.Array;
			this.arrayObj = arrayObj;
			this.indexMap = indexMap;
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x000F5F70 File Offset: 0x000F4F70
		internal ValueFixup(object memberObject, string memberName, ReadObjectInfo objectInfo)
		{
			this.valueFixupEnum = ValueFixupEnum.Member;
			this.memberObject = memberObject;
			this.memberName = memberName;
			this.objectInfo = objectInfo;
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x000F5F94 File Offset: 0x000F4F94
		internal void Fixup(ParseRecord record, ParseRecord parent)
		{
			object prnewObj = record.PRnewObj;
			switch (this.valueFixupEnum)
			{
			case ValueFixupEnum.Array:
				this.arrayObj.SetValue(prnewObj, this.indexMap);
				return;
			case ValueFixupEnum.Header:
			{
				Type typeFromHandle = typeof(Header);
				if (ValueFixup.valueInfo == null)
				{
					MemberInfo[] member = typeFromHandle.GetMember("Value");
					if (member.Length != 1)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_HeaderReflection"), new object[]
						{
							member.Length
						}));
					}
					ValueFixup.valueInfo = member[0];
				}
				FormatterServices.SerializationSetValue(ValueFixup.valueInfo, this.header, prnewObj);
				return;
			}
			case ValueFixupEnum.Member:
			{
				if (this.objectInfo.isSi)
				{
					this.objectInfo.objectManager.RecordDelayedFixup(parent.PRobjectId, this.memberName, record.PRobjectId);
					return;
				}
				MemberInfo memberInfo = this.objectInfo.GetMemberInfo(this.memberName);
				if (memberInfo != null)
				{
					this.objectInfo.objectManager.RecordFixup(parent.PRobjectId, memberInfo, record.PRobjectId);
				}
				return;
			}
			default:
				return;
			}
		}

		// Token: 0x0400248E RID: 9358
		internal ValueFixupEnum valueFixupEnum;

		// Token: 0x0400248F RID: 9359
		internal Array arrayObj;

		// Token: 0x04002490 RID: 9360
		internal int[] indexMap;

		// Token: 0x04002491 RID: 9361
		internal object header;

		// Token: 0x04002492 RID: 9362
		internal object memberObject;

		// Token: 0x04002493 RID: 9363
		internal static MemberInfo valueInfo;

		// Token: 0x04002494 RID: 9364
		internal ReadObjectInfo objectInfo;

		// Token: 0x04002495 RID: 9365
		internal string memberName;
	}
}
