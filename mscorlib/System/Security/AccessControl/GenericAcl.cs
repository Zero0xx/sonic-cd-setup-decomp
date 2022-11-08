using System;
using System.Collections;

namespace System.Security.AccessControl
{
	// Token: 0x020008D5 RID: 2261
	public abstract class GenericAcl : ICollection, IEnumerable
	{
		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06005220 RID: 21024
		public abstract byte Revision { get; }

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x06005221 RID: 21025
		public abstract int BinaryLength { get; }

		// Token: 0x17000E47 RID: 3655
		public abstract GenericAce this[int index]
		{
			get;
			set;
		}

		// Token: 0x06005224 RID: 21028
		public abstract void GetBinaryForm(byte[] binaryForm, int offset);

		// Token: 0x06005225 RID: 21029 RVA: 0x00127850 File Offset: 0x00126850
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new RankException(Environment.GetResourceString("Rank_MultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < this.Count)
			{
				throw new ArgumentOutOfRangeException("array", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index + i);
			}
		}

		// Token: 0x06005226 RID: 21030 RVA: 0x001278E3 File Offset: 0x001268E3
		public void CopyTo(GenericAce[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x06005227 RID: 21031
		public abstract int Count { get; }

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x06005228 RID: 21032 RVA: 0x001278ED File Offset: 0x001268ED
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x06005229 RID: 21033 RVA: 0x001278F0 File Offset: 0x001268F0
		public object SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600522A RID: 21034 RVA: 0x001278F3 File Offset: 0x001268F3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new AceEnumerator(this);
		}

		// Token: 0x0600522B RID: 21035 RVA: 0x001278FB File Offset: 0x001268FB
		public AceEnumerator GetEnumerator()
		{
			return ((IEnumerable)this).GetEnumerator() as AceEnumerator;
		}

		// Token: 0x04002A79 RID: 10873
		internal const int HeaderLength = 8;

		// Token: 0x04002A7A RID: 10874
		public static readonly byte AclRevision = 2;

		// Token: 0x04002A7B RID: 10875
		public static readonly byte AclRevisionDS = 4;

		// Token: 0x04002A7C RID: 10876
		public static readonly int MaxBinaryLength = 65535;
	}
}
