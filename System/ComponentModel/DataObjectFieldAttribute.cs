using System;

namespace System.ComponentModel
{
	// Token: 0x020000C5 RID: 197
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DataObjectFieldAttribute : Attribute
	{
		// Token: 0x060006C9 RID: 1737 RVA: 0x000198B1 File Offset: 0x000188B1
		public DataObjectFieldAttribute(bool primaryKey) : this(primaryKey, false, false, -1)
		{
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x000198BD File Offset: 0x000188BD
		public DataObjectFieldAttribute(bool primaryKey, bool isIdentity) : this(primaryKey, isIdentity, false, -1)
		{
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x000198C9 File Offset: 0x000188C9
		public DataObjectFieldAttribute(bool primaryKey, bool isIdentity, bool isNullable) : this(primaryKey, isIdentity, isNullable, -1)
		{
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x000198D5 File Offset: 0x000188D5
		public DataObjectFieldAttribute(bool primaryKey, bool isIdentity, bool isNullable, int length)
		{
			this._primaryKey = primaryKey;
			this._isIdentity = isIdentity;
			this._isNullable = isNullable;
			this._length = length;
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x000198FA File Offset: 0x000188FA
		public bool IsIdentity
		{
			get
			{
				return this._isIdentity;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x00019902 File Offset: 0x00018902
		public bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x0001990A File Offset: 0x0001890A
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x00019912 File Offset: 0x00018912
		public bool PrimaryKey
		{
			get
			{
				return this._primaryKey;
			}
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001991C File Offset: 0x0001891C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectFieldAttribute dataObjectFieldAttribute = obj as DataObjectFieldAttribute;
			return dataObjectFieldAttribute != null && dataObjectFieldAttribute.IsIdentity == this.IsIdentity && dataObjectFieldAttribute.IsNullable == this.IsNullable && dataObjectFieldAttribute.Length == this.Length && dataObjectFieldAttribute.PrimaryKey == this.PrimaryKey;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x00019973 File Offset: 0x00018973
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0400092B RID: 2347
		private bool _primaryKey;

		// Token: 0x0400092C RID: 2348
		private bool _isIdentity;

		// Token: 0x0400092D RID: 2349
		private bool _isNullable;

		// Token: 0x0400092E RID: 2350
		private int _length;
	}
}
