using System;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x020006DC RID: 1756
	internal class TrackingStringDictionary : StringDictionary
	{
		// Token: 0x0600362B RID: 13867 RVA: 0x000E767D File Offset: 0x000E667D
		internal TrackingStringDictionary() : this(false)
		{
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000E7686 File Offset: 0x000E6686
		internal TrackingStringDictionary(bool isReadOnly)
		{
			this.isReadOnly = isReadOnly;
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x000E7695 File Offset: 0x000E6695
		// (set) Token: 0x0600362E RID: 13870 RVA: 0x000E769D File Offset: 0x000E669D
		internal bool IsChanged
		{
			get
			{
				return this.isChanged;
			}
			set
			{
				this.isChanged = value;
			}
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000E76A6 File Offset: 0x000E66A6
		public override void Add(string key, string value)
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Add(key, value);
			this.isChanged = true;
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000E76CF File Offset: 0x000E66CF
		public override void Clear()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Clear();
			this.isChanged = true;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000E76F6 File Offset: 0x000E66F6
		public override void Remove(string key)
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Remove(key);
			this.isChanged = true;
		}

		// Token: 0x17000C89 RID: 3209
		public override string this[string key]
		{
			get
			{
				return base[key];
			}
			set
			{
				if (this.isReadOnly)
				{
					throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
				}
				base[key] = value;
				this.isChanged = true;
			}
		}

		// Token: 0x04003163 RID: 12643
		private bool isChanged;

		// Token: 0x04003164 RID: 12644
		private bool isReadOnly;
	}
}
