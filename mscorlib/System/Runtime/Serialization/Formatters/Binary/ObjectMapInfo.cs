using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007D2 RID: 2002
	internal sealed class ObjectMapInfo
	{
		// Token: 0x06004718 RID: 18200 RVA: 0x000F34EB File Offset: 0x000F24EB
		internal ObjectMapInfo(int objectId, int numMembers, string[] memberNames, Type[] memberTypes)
		{
			this.objectId = objectId;
			this.numMembers = numMembers;
			this.memberNames = memberNames;
			this.memberTypes = memberTypes;
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x000F3510 File Offset: 0x000F2510
		internal bool isCompatible(int numMembers, string[] memberNames, Type[] memberTypes)
		{
			bool result = true;
			if (this.numMembers == numMembers)
			{
				for (int i = 0; i < numMembers; i++)
				{
					if (!this.memberNames[i].Equals(memberNames[i]))
					{
						result = false;
						break;
					}
					if (memberTypes != null && this.memberTypes[i] != memberTypes[i])
					{
						result = false;
						break;
					}
				}
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x040023EA RID: 9194
		internal int objectId;

		// Token: 0x040023EB RID: 9195
		private int numMembers;

		// Token: 0x040023EC RID: 9196
		private string[] memberNames;

		// Token: 0x040023ED RID: 9197
		private Type[] memberTypes;
	}
}
