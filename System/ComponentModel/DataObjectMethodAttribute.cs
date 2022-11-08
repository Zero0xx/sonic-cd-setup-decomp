using System;

namespace System.ComponentModel
{
	// Token: 0x020000C6 RID: 198
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class DataObjectMethodAttribute : Attribute
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001997B File Offset: 0x0001897B
		public DataObjectMethodAttribute(DataObjectMethodType methodType) : this(methodType, false)
		{
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x00019985 File Offset: 0x00018985
		public DataObjectMethodAttribute(DataObjectMethodType methodType, bool isDefault)
		{
			this._methodType = methodType;
			this._isDefault = isDefault;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x0001999B File Offset: 0x0001899B
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x000199A3 File Offset: 0x000189A3
		public DataObjectMethodType MethodType
		{
			get
			{
				return this._methodType;
			}
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x000199AC File Offset: 0x000189AC
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectMethodAttribute dataObjectMethodAttribute = obj as DataObjectMethodAttribute;
			return dataObjectMethodAttribute != null && dataObjectMethodAttribute.MethodType == this.MethodType && dataObjectMethodAttribute.IsDefault == this.IsDefault;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000199E8 File Offset: 0x000189E8
		public override int GetHashCode()
		{
			int methodType = (int)this._methodType;
			return methodType.GetHashCode() ^ this._isDefault.GetHashCode();
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x00019A10 File Offset: 0x00018A10
		public override bool Match(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectMethodAttribute dataObjectMethodAttribute = obj as DataObjectMethodAttribute;
			return dataObjectMethodAttribute != null && dataObjectMethodAttribute.MethodType == this.MethodType;
		}

		// Token: 0x0400092F RID: 2351
		private bool _isDefault;

		// Token: 0x04000930 RID: 2352
		private DataObjectMethodType _methodType;
	}
}
