using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006F1 RID: 1777
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public abstract class BaseChannelObjectWithProperties : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003F65 RID: 16229 RVA: 0x000D8695 File Offset: 0x000D7695
		public virtual IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return this;
			}
		}

		// Token: 0x17000AAF RID: 2735
		public virtual object this[object key]
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06003F68 RID: 16232 RVA: 0x000D86A2 File Offset: 0x000D76A2
		public virtual ICollection Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06003F69 RID: 16233 RVA: 0x000D86A8 File Offset: 0x000D76A8
		public virtual ICollection Values
		{
			get
			{
				ICollection keys = this.Keys;
				if (keys == null)
				{
					return null;
				}
				ArrayList arrayList = new ArrayList();
				foreach (object key in keys)
				{
					arrayList.Add(this[key]);
				}
				return arrayList;
			}
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x000D8714 File Offset: 0x000D7714
		public virtual bool Contains(object key)
		{
			if (key == null)
			{
				return false;
			}
			ICollection keys = this.Keys;
			if (keys == null)
			{
				return false;
			}
			string text = key as string;
			foreach (object obj in keys)
			{
				if (text != null)
				{
					string text2 = obj as string;
					if (text2 != null)
					{
						if (string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) == 0)
						{
							return true;
						}
						continue;
					}
				}
				if (key.Equals(obj))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x000D87AC File Offset: 0x000D77AC
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x000D87AF File Offset: 0x000D77AF
		public virtual bool IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x000D87B2 File Offset: 0x000D77B2
		public virtual void Add(object key, object value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x000D87B9 File Offset: 0x000D77B9
		public virtual void Clear()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x000D87C0 File Offset: 0x000D77C0
		public virtual void Remove(object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x000D87C7 File Offset: 0x000D77C7
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x000D87CF File Offset: 0x000D77CF
		public virtual void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06003F72 RID: 16242 RVA: 0x000D87D8 File Offset: 0x000D77D8
		public virtual int Count
		{
			get
			{
				ICollection keys = this.Keys;
				if (keys == null)
				{
					return 0;
				}
				return keys.Count;
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003F73 RID: 16243 RVA: 0x000D87F7 File Offset: 0x000D77F7
		public virtual object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06003F74 RID: 16244 RVA: 0x000D87FA File Offset: 0x000D77FA
		public virtual bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003F75 RID: 16245 RVA: 0x000D87FD File Offset: 0x000D77FD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new DictionaryEnumeratorByKeys(this);
		}
	}
}
