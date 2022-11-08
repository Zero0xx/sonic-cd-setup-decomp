using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x02000712 RID: 1810
	public class SettingsPropertyCollection : ICloneable, ICollection, IEnumerable
	{
		// Token: 0x0600375D RID: 14173 RVA: 0x000EAF70 File Offset: 0x000E9F70
		public SettingsPropertyCollection()
		{
			this._Hashtable = new Hashtable(10, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
		}

		// Token: 0x0600375E RID: 14174 RVA: 0x000EAF90 File Offset: 0x000E9F90
		public void Add(SettingsProperty property)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this.OnAdd(property);
			this._Hashtable.Add(property.Name, property);
			try
			{
				this.OnAddComplete(property);
			}
			catch
			{
				this._Hashtable.Remove(property.Name);
				throw;
			}
		}

		// Token: 0x0600375F RID: 14175 RVA: 0x000EAFF4 File Offset: 0x000E9FF4
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			SettingsProperty settingsProperty = (SettingsProperty)this._Hashtable[name];
			if (settingsProperty == null)
			{
				return;
			}
			this.OnRemove(settingsProperty);
			this._Hashtable.Remove(name);
			try
			{
				this.OnRemoveComplete(settingsProperty);
			}
			catch
			{
				this._Hashtable.Add(name, settingsProperty);
				throw;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		public SettingsProperty this[string name]
		{
			get
			{
				return this._Hashtable[name] as SettingsProperty;
			}
		}

		// Token: 0x06003761 RID: 14177 RVA: 0x000EB077 File Offset: 0x000EA077
		public IEnumerator GetEnumerator()
		{
			return this._Hashtable.Values.GetEnumerator();
		}

		// Token: 0x06003762 RID: 14178 RVA: 0x000EB089 File Offset: 0x000EA089
		public object Clone()
		{
			return new SettingsPropertyCollection(this._Hashtable);
		}

		// Token: 0x06003763 RID: 14179 RVA: 0x000EB096 File Offset: 0x000EA096
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
		}

		// Token: 0x06003764 RID: 14180 RVA: 0x000EB0A8 File Offset: 0x000EA0A8
		public void Clear()
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			this.OnClear();
			this._Hashtable.Clear();
			this.OnClearComplete();
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000EB0CF File Offset: 0x000EA0CF
		protected virtual void OnAdd(SettingsProperty property)
		{
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x000EB0D1 File Offset: 0x000EA0D1
		protected virtual void OnAddComplete(SettingsProperty property)
		{
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x000EB0D3 File Offset: 0x000EA0D3
		protected virtual void OnClear()
		{
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x000EB0D5 File Offset: 0x000EA0D5
		protected virtual void OnClearComplete()
		{
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000EB0D7 File Offset: 0x000EA0D7
		protected virtual void OnRemove(SettingsProperty property)
		{
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000EB0D9 File Offset: 0x000EA0D9
		protected virtual void OnRemoveComplete(SettingsProperty property)
		{
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x0600376B RID: 14187 RVA: 0x000EB0DB File Offset: 0x000EA0DB
		public int Count
		{
			get
			{
				return this._Hashtable.Count;
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x0600376C RID: 14188 RVA: 0x000EB0E8 File Offset: 0x000EA0E8
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x0600376D RID: 14189 RVA: 0x000EB0EB File Offset: 0x000EA0EB
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000EB0EE File Offset: 0x000EA0EE
		public void CopyTo(Array array, int index)
		{
			this._Hashtable.Values.CopyTo(array, index);
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x000EB102 File Offset: 0x000EA102
		private SettingsPropertyCollection(Hashtable h)
		{
			this._Hashtable = (Hashtable)h.Clone();
		}

		// Token: 0x040031C2 RID: 12738
		private Hashtable _Hashtable;

		// Token: 0x040031C3 RID: 12739
		private bool _ReadOnly;
	}
}
