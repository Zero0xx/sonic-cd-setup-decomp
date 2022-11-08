using System;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007F2 RID: 2034
	internal sealed class NameInfo
	{
		// Token: 0x060047C1 RID: 18369 RVA: 0x000F60B1 File Offset: 0x000F50B1
		internal NameInfo()
		{
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x000F60BC File Offset: 0x000F50BC
		internal void Init()
		{
			this.NIFullName = null;
			this.NIobjectId = 0L;
			this.NIassemId = 0L;
			this.NIprimitiveTypeEnum = InternalPrimitiveTypeE.Invalid;
			this.NItype = null;
			this.NIisSealed = false;
			this.NItransmitTypeOnObject = false;
			this.NItransmitTypeOnMember = false;
			this.NIisParentTypeOnObject = false;
			this.NIisArray = false;
			this.NIisArrayItem = false;
			this.NIarrayEnum = InternalArrayTypeE.Empty;
			this.NIsealedStatusChecked = false;
		}

		// Token: 0x17000C7D RID: 3197
		// (get) Token: 0x060047C3 RID: 18371 RVA: 0x000F6126 File Offset: 0x000F5126
		public bool IsSealed
		{
			get
			{
				if (!this.NIsealedStatusChecked)
				{
					this.NIisSealed = this.NItype.IsSealed;
					this.NIsealedStatusChecked = true;
				}
				return this.NIisSealed;
			}
		}

		// Token: 0x17000C7E RID: 3198
		// (get) Token: 0x060047C4 RID: 18372 RVA: 0x000F614E File Offset: 0x000F514E
		// (set) Token: 0x060047C5 RID: 18373 RVA: 0x000F616F File Offset: 0x000F516F
		public string NIname
		{
			get
			{
				if (this.NIFullName == null)
				{
					this.NIFullName = this.NItype.FullName;
				}
				return this.NIFullName;
			}
			set
			{
				this.NIFullName = value;
			}
		}

		// Token: 0x0400249A RID: 9370
		internal string NIFullName;

		// Token: 0x0400249B RID: 9371
		internal long NIobjectId;

		// Token: 0x0400249C RID: 9372
		internal long NIassemId;

		// Token: 0x0400249D RID: 9373
		internal InternalPrimitiveTypeE NIprimitiveTypeEnum;

		// Token: 0x0400249E RID: 9374
		internal Type NItype;

		// Token: 0x0400249F RID: 9375
		internal bool NIisSealed;

		// Token: 0x040024A0 RID: 9376
		internal bool NIisArray;

		// Token: 0x040024A1 RID: 9377
		internal bool NIisArrayItem;

		// Token: 0x040024A2 RID: 9378
		internal bool NItransmitTypeOnObject;

		// Token: 0x040024A3 RID: 9379
		internal bool NItransmitTypeOnMember;

		// Token: 0x040024A4 RID: 9380
		internal bool NIisParentTypeOnObject;

		// Token: 0x040024A5 RID: 9381
		internal InternalArrayTypeE NIarrayEnum;

		// Token: 0x040024A6 RID: 9382
		private bool NIsealedStatusChecked;
	}
}
