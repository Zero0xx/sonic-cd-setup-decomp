using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Windows.Forms
{
	// Token: 0x02000637 RID: 1591
	internal class StringSource : IEnumString
	{
		// Token: 0x06005346 RID: 21318 RVA: 0x00130A24 File Offset: 0x0012FA24
		public StringSource(string[] strings)
		{
			Array.Clear(strings, 0, this.size);
			if (strings != null)
			{
				this.strings = strings;
			}
			this.current = 0;
			this.size = ((strings == null) ? 0 : strings.Length);
			Guid guid = typeof(UnsafeNativeMethods.IAutoComplete2).GUID;
			object obj = UnsafeNativeMethods.CoCreateInstance(ref StringSource.autoCompleteClsid, null, 1, ref guid);
			this.autoCompleteObject2 = (UnsafeNativeMethods.IAutoComplete2)obj;
		}

		// Token: 0x06005347 RID: 21319 RVA: 0x00130A90 File Offset: 0x0012FA90
		public bool Bind(HandleRef edit, int options)
		{
			bool result = false;
			if (this.autoCompleteObject2 != null)
			{
				try
				{
					this.autoCompleteObject2.SetOptions(options);
					this.autoCompleteObject2.Init(edit, this, null, null);
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06005348 RID: 21320 RVA: 0x00130AE0 File Offset: 0x0012FAE0
		public void ReleaseAutoComplete()
		{
			if (this.autoCompleteObject2 != null)
			{
				Marshal.ReleaseComObject(this.autoCompleteObject2);
				this.autoCompleteObject2 = null;
			}
		}

		// Token: 0x06005349 RID: 21321 RVA: 0x00130B00 File Offset: 0x0012FB00
		public void RefreshList(string[] newSource)
		{
			Array.Clear(this.strings, 0, this.size);
			if (this.strings != null)
			{
				this.strings = newSource;
			}
			this.current = 0;
			this.size = ((this.strings == null) ? 0 : this.strings.Length);
		}

		// Token: 0x0600534A RID: 21322 RVA: 0x00130B4E File Offset: 0x0012FB4E
		void IEnumString.Clone(out IEnumString ppenum)
		{
			ppenum = new StringSource(this.strings);
		}

		// Token: 0x0600534B RID: 21323 RVA: 0x00130B60 File Offset: 0x0012FB60
		int IEnumString.Next(int celt, string[] rgelt, IntPtr pceltFetched)
		{
			if (celt < 0)
			{
				return -2147024809;
			}
			int num = 0;
			while (this.current < this.size && celt > 0)
			{
				rgelt[num] = this.strings[this.current];
				this.current++;
				num++;
				celt--;
			}
			if (pceltFetched != IntPtr.Zero)
			{
				Marshal.WriteInt32(pceltFetched, num);
			}
			if (celt != 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600534C RID: 21324 RVA: 0x00130BCE File Offset: 0x0012FBCE
		void IEnumString.Reset()
		{
			this.current = 0;
		}

		// Token: 0x0600534D RID: 21325 RVA: 0x00130BD7 File Offset: 0x0012FBD7
		int IEnumString.Skip(int celt)
		{
			this.current += celt;
			if (this.current >= this.size)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04003679 RID: 13945
		private string[] strings;

		// Token: 0x0400367A RID: 13946
		private int current;

		// Token: 0x0400367B RID: 13947
		private int size;

		// Token: 0x0400367C RID: 13948
		private UnsafeNativeMethods.IAutoComplete2 autoCompleteObject2;

		// Token: 0x0400367D RID: 13949
		private static Guid autoCompleteClsid = new Guid("{00BB2763-6A77-11D0-A535-00C04FD7D062}");
	}
}
