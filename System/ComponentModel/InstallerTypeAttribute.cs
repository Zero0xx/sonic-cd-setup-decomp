using System;

namespace System.ComponentModel
{
	// Token: 0x020000F8 RID: 248
	[AttributeUsage(AttributeTargets.Class)]
	public class InstallerTypeAttribute : Attribute
	{
		// Token: 0x06000803 RID: 2051 RVA: 0x0001BF8B File Offset: 0x0001AF8B
		public InstallerTypeAttribute(Type installerType)
		{
			this._typeName = installerType.AssemblyQualifiedName;
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x0001BF9F File Offset: 0x0001AF9F
		public InstallerTypeAttribute(string typeName)
		{
			this._typeName = typeName;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000805 RID: 2053 RVA: 0x0001BFAE File Offset: 0x0001AFAE
		public virtual Type InstallerType
		{
			get
			{
				return Type.GetType(this._typeName);
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0001BFBC File Offset: 0x0001AFBC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			InstallerTypeAttribute installerTypeAttribute = obj as InstallerTypeAttribute;
			return installerTypeAttribute != null && installerTypeAttribute._typeName == this._typeName;
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0001BFEC File Offset: 0x0001AFEC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000981 RID: 2433
		private string _typeName;
	}
}
