using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Globalization
{
	// Token: 0x020003C9 RID: 969
	[ComVisible(true)]
	[Serializable]
	public class TextElementEnumerator : IEnumerator
	{
		// Token: 0x0600276B RID: 10091 RVA: 0x000763B6 File Offset: 0x000753B6
		internal TextElementEnumerator(string str, int startIndex, int strLen)
		{
			this.str = str;
			this.startIndex = startIndex;
			this.strLen = strLen;
			this.Reset();
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000763D9 File Offset: 0x000753D9
		[OnDeserializing]
		private void OnDeserializing(StreamingContext ctx)
		{
			this.charLen = -1;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000763E4 File Offset: 0x000753E4
		[OnDeserialized]
		private void OnDeserialized(StreamingContext ctx)
		{
			this.strLen = this.endIndex + 1;
			this.currTextElementLen = this.nextTextElementLen;
			if (this.charLen == -1)
			{
				this.uc = CharUnicodeInfo.InternalGetUnicodeCategory(this.str, this.index, out this.charLen);
			}
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x00076431 File Offset: 0x00075431
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			this.endIndex = this.strLen - 1;
			this.nextTextElementLen = this.currTextElementLen;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x00076450 File Offset: 0x00075450
		public bool MoveNext()
		{
			if (this.index >= this.strLen)
			{
				this.index = this.strLen + 1;
				return false;
			}
			this.currTextElementLen = StringInfo.GetCurrentTextElementLen(this.str, this.index, this.strLen, ref this.uc, ref this.charLen);
			this.index += this.currTextElementLen;
			return true;
		}

		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002770 RID: 10096 RVA: 0x000764B8 File Offset: 0x000754B8
		public object Current
		{
			get
			{
				return this.GetTextElement();
			}
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x000764C0 File Offset: 0x000754C0
		public string GetTextElement()
		{
			if (this.index == this.startIndex)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
			}
			if (this.index > this.strLen)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
			}
			return this.str.Substring(this.index - this.currTextElementLen, this.currTextElementLen);
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002772 RID: 10098 RVA: 0x00076527 File Offset: 0x00075527
		public int ElementIndex
		{
			get
			{
				if (this.index == this.startIndex)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				return this.index - this.currTextElementLen;
			}
		}

		// Token: 0x06002773 RID: 10099 RVA: 0x00076554 File Offset: 0x00075554
		public void Reset()
		{
			this.index = this.startIndex;
			if (this.index < this.strLen)
			{
				this.uc = CharUnicodeInfo.InternalGetUnicodeCategory(this.str, this.index, out this.charLen);
			}
		}

		// Token: 0x040011F1 RID: 4593
		private string str;

		// Token: 0x040011F2 RID: 4594
		private int index;

		// Token: 0x040011F3 RID: 4595
		private int startIndex;

		// Token: 0x040011F4 RID: 4596
		[NonSerialized]
		private int strLen;

		// Token: 0x040011F5 RID: 4597
		[NonSerialized]
		private int currTextElementLen;

		// Token: 0x040011F6 RID: 4598
		[OptionalField(VersionAdded = 2)]
		private UnicodeCategory uc;

		// Token: 0x040011F7 RID: 4599
		[OptionalField(VersionAdded = 2)]
		private int charLen;

		// Token: 0x040011F8 RID: 4600
		private int endIndex;

		// Token: 0x040011F9 RID: 4601
		private int nextTextElementLen;
	}
}
