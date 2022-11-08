using System;

namespace System.ComponentModel
{
	// Token: 0x020000C4 RID: 196
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DataObjectAttribute : Attribute
	{
		// Token: 0x060006C2 RID: 1730 RVA: 0x00019827 File Offset: 0x00018827
		public DataObjectAttribute() : this(true)
		{
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x00019830 File Offset: 0x00018830
		public DataObjectAttribute(bool isDataObject)
		{
			this._isDataObject = isDataObject;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0001983F File Offset: 0x0001883F
		public bool IsDataObject
		{
			get
			{
				return this._isDataObject;
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x00019848 File Offset: 0x00018848
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectAttribute dataObjectAttribute = obj as DataObjectAttribute;
			return dataObjectAttribute != null && dataObjectAttribute.IsDataObject == this.IsDataObject;
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x00019875 File Offset: 0x00018875
		public override int GetHashCode()
		{
			return this._isDataObject.GetHashCode();
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00019882 File Offset: 0x00018882
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DataObjectAttribute.Default);
		}

		// Token: 0x04000927 RID: 2343
		public static readonly DataObjectAttribute DataObject = new DataObjectAttribute(true);

		// Token: 0x04000928 RID: 2344
		public static readonly DataObjectAttribute NonDataObject = new DataObjectAttribute(false);

		// Token: 0x04000929 RID: 2345
		public static readonly DataObjectAttribute Default = DataObjectAttribute.NonDataObject;

		// Token: 0x0400092A RID: 2346
		private bool _isDataObject;
	}
}
