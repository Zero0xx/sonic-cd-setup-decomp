using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x0200074C RID: 1868
	internal abstract class BaseCAMarshaler
	{
		// Token: 0x06006338 RID: 25400 RVA: 0x0016A37C File Offset: 0x0016937C
		protected BaseCAMarshaler(NativeMethods.CA_STRUCT caStruct)
		{
			if (caStruct == null)
			{
				this.count = 0;
			}
			this.count = caStruct.cElems;
			this.caArrayAddress = caStruct.pElems;
		}

		// Token: 0x06006339 RID: 25401 RVA: 0x0016A3A8 File Offset: 0x001693A8
		protected override void Finalize()
		{
			try
			{
				if (this.itemArray == null && this.caArrayAddress != IntPtr.Zero)
				{
					object[] items = this.Items;
				}
			}
			catch
			{
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0600633A RID: 25402
		protected abstract Array CreateArray();

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x0600633B RID: 25403
		public abstract Type ItemType { get; }

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x0600633C RID: 25404 RVA: 0x0016A400 File Offset: 0x00169400
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x0600633D RID: 25405 RVA: 0x0016A408 File Offset: 0x00169408
		public virtual object[] Items
		{
			get
			{
				try
				{
					if (this.itemArray == null)
					{
						this.itemArray = this.Get_Items();
					}
				}
				catch (Exception)
				{
				}
				return this.itemArray;
			}
		}

		// Token: 0x0600633E RID: 25406
		protected abstract object GetItemFromAddress(IntPtr addr);

		// Token: 0x0600633F RID: 25407 RVA: 0x0016A444 File Offset: 0x00169444
		private object[] Get_Items()
		{
			Array array = new object[this.Count];
			for (int i = 0; i < this.count; i++)
			{
				try
				{
					IntPtr addr = Marshal.ReadIntPtr(this.caArrayAddress, i * IntPtr.Size);
					object itemFromAddress = this.GetItemFromAddress(addr);
					if (itemFromAddress != null && this.ItemType.IsInstanceOfType(itemFromAddress))
					{
						array.SetValue(itemFromAddress, i);
					}
				}
				catch (Exception)
				{
				}
			}
			Marshal.FreeCoTaskMem(this.caArrayAddress);
			this.caArrayAddress = IntPtr.Zero;
			return (object[])array;
		}

		// Token: 0x04003B56 RID: 15190
		private static TraceSwitch CAMarshalSwitch = new TraceSwitch("CAMarshal", "BaseCAMarshaler: Debug CA* struct marshaling");

		// Token: 0x04003B57 RID: 15191
		private IntPtr caArrayAddress;

		// Token: 0x04003B58 RID: 15192
		private int count;

		// Token: 0x04003B59 RID: 15193
		private object[] itemArray;
	}
}
