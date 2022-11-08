using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections
{
	// Token: 0x0200025F RID: 607
	[ComVisible(true)]
	[Serializable]
	public sealed class Comparer : IComparer, ISerializable
	{
		// Token: 0x060017BE RID: 6078 RVA: 0x0003CEA5 File Offset: 0x0003BEA5
		private Comparer()
		{
			this.m_compareInfo = null;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x0003CEB4 File Offset: 0x0003BEB4
		public Comparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.m_compareInfo = culture.CompareInfo;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x0003CED8 File Offset: 0x0003BED8
		private Comparer(SerializationInfo info, StreamingContext context)
		{
			this.m_compareInfo = null;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null && name == "CompareInfo")
				{
					this.m_compareInfo = (CompareInfo)info.GetValue("CompareInfo", typeof(CompareInfo));
				}
			}
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x0003CF3C File Offset: 0x0003BF3C
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			if (this.m_compareInfo != null)
			{
				string text = a as string;
				string text2 = b as string;
				if (text != null && text2 != null)
				{
					return this.m_compareInfo.Compare(text, text2);
				}
			}
			IComparable comparable = a as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(b);
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_ImplementIComparable"));
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x0003CFA4 File Offset: 0x0003BFA4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.m_compareInfo != null)
			{
				info.AddValue("CompareInfo", this.m_compareInfo);
			}
		}

		// Token: 0x0400098A RID: 2442
		private const string CompareInfoName = "CompareInfo";

		// Token: 0x0400098B RID: 2443
		private CompareInfo m_compareInfo;

		// Token: 0x0400098C RID: 2444
		public static readonly Comparer Default = new Comparer(CultureInfo.CurrentCulture);

		// Token: 0x0400098D RID: 2445
		public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);
	}
}
