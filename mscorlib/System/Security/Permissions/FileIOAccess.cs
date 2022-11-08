using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Util;

namespace System.Security.Permissions
{
	// Token: 0x0200062E RID: 1582
	[Serializable]
	internal sealed class FileIOAccess
	{
		// Token: 0x0600390E RID: 14606 RVA: 0x000C0C48 File Offset: 0x000BFC48
		public FileIOAccess()
		{
			this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
			this.m_allFiles = false;
			this.m_allLocalFiles = false;
			this.m_pathDiscovery = false;
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x000C0C7E File Offset: 0x000BFC7E
		public FileIOAccess(bool pathDiscovery)
		{
			this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
			this.m_allFiles = false;
			this.m_allLocalFiles = false;
			this.m_pathDiscovery = pathDiscovery;
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x000C0CB4 File Offset: 0x000BFCB4
		public FileIOAccess(string value)
		{
			if (value == null)
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
				this.m_allFiles = false;
				this.m_allLocalFiles = false;
			}
			else if (value.Length >= "*AllFiles*".Length && string.Compare("*AllFiles*", value, StringComparison.Ordinal) == 0)
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
				this.m_allFiles = true;
				this.m_allLocalFiles = false;
			}
			else if (value.Length >= "*AllLocalFiles*".Length && string.Compare("*AllLocalFiles*", 0, value, 0, "*AllLocalFiles*".Length, StringComparison.Ordinal) == 0)
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, value.Substring("*AllLocalFiles*".Length), true);
				this.m_allFiles = false;
				this.m_allLocalFiles = true;
			}
			else
			{
				this.m_set = new StringExpressionSet(this.m_ignoreCase, value, true);
				this.m_allFiles = false;
				this.m_allLocalFiles = false;
			}
			this.m_pathDiscovery = false;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x000C0DBE File Offset: 0x000BFDBE
		public FileIOAccess(bool allFiles, bool allLocalFiles, bool pathDiscovery)
		{
			this.m_set = new StringExpressionSet(this.m_ignoreCase, true);
			this.m_allFiles = allFiles;
			this.m_allLocalFiles = allLocalFiles;
			this.m_pathDiscovery = pathDiscovery;
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x000C0DF4 File Offset: 0x000BFDF4
		public FileIOAccess(StringExpressionSet set, bool allFiles, bool allLocalFiles, bool pathDiscovery)
		{
			this.m_set = set;
			this.m_set.SetThrowOnRelative(true);
			this.m_allFiles = allFiles;
			this.m_allLocalFiles = allLocalFiles;
			this.m_pathDiscovery = pathDiscovery;
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x000C0E2C File Offset: 0x000BFE2C
		private FileIOAccess(FileIOAccess operand)
		{
			this.m_set = operand.m_set.Copy();
			this.m_allFiles = operand.m_allFiles;
			this.m_allLocalFiles = operand.m_allLocalFiles;
			this.m_pathDiscovery = operand.m_pathDiscovery;
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x000C0E7B File Offset: 0x000BFE7B
		public void AddExpressions(ArrayList values, bool checkForDuplicates)
		{
			this.m_allFiles = false;
			this.m_set.AddExpressions(values, checkForDuplicates);
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003915 RID: 14613 RVA: 0x000C0E91 File Offset: 0x000BFE91
		// (set) Token: 0x06003916 RID: 14614 RVA: 0x000C0E99 File Offset: 0x000BFE99
		public bool AllFiles
		{
			get
			{
				return this.m_allFiles;
			}
			set
			{
				this.m_allFiles = value;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003917 RID: 14615 RVA: 0x000C0EA2 File Offset: 0x000BFEA2
		// (set) Token: 0x06003918 RID: 14616 RVA: 0x000C0EAA File Offset: 0x000BFEAA
		public bool AllLocalFiles
		{
			get
			{
				return this.m_allLocalFiles;
			}
			set
			{
				this.m_allLocalFiles = value;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (set) Token: 0x06003919 RID: 14617 RVA: 0x000C0EB3 File Offset: 0x000BFEB3
		public bool PathDiscovery
		{
			set
			{
				this.m_pathDiscovery = value;
			}
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x000C0EBC File Offset: 0x000BFEBC
		public bool IsEmpty()
		{
			return !this.m_allFiles && !this.m_allLocalFiles && (this.m_set == null || this.m_set.IsEmpty());
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x000C0EE5 File Offset: 0x000BFEE5
		public FileIOAccess Copy()
		{
			return new FileIOAccess(this);
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x000C0EF0 File Offset: 0x000BFEF0
		public FileIOAccess Union(FileIOAccess operand)
		{
			if (operand == null)
			{
				if (!this.IsEmpty())
				{
					return this.Copy();
				}
				return null;
			}
			else
			{
				if (this.m_allFiles || operand.m_allFiles)
				{
					return new FileIOAccess(true, false, this.m_pathDiscovery);
				}
				return new FileIOAccess(this.m_set.Union(operand.m_set), false, this.m_allLocalFiles || operand.m_allLocalFiles, this.m_pathDiscovery);
			}
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x000C0F60 File Offset: 0x000BFF60
		public FileIOAccess Intersect(FileIOAccess operand)
		{
			if (operand == null)
			{
				return null;
			}
			if (this.m_allFiles)
			{
				if (operand.m_allFiles)
				{
					return new FileIOAccess(true, false, this.m_pathDiscovery);
				}
				return new FileIOAccess(operand.m_set.Copy(), false, operand.m_allLocalFiles, this.m_pathDiscovery);
			}
			else
			{
				if (operand.m_allFiles)
				{
					return new FileIOAccess(this.m_set.Copy(), false, this.m_allLocalFiles, this.m_pathDiscovery);
				}
				StringExpressionSet stringExpressionSet = new StringExpressionSet(this.m_ignoreCase, true);
				if (this.m_allLocalFiles)
				{
					string[] array = operand.m_set.ToStringArray();
					if (array != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							string root = FileIOAccess.GetRoot(array[i]);
							if (root != null && FileIOAccess._LocalDrive(FileIOAccess.GetRoot(root)))
							{
								stringExpressionSet.AddExpressions(new string[]
								{
									array[i]
								}, true, false);
							}
						}
					}
				}
				if (operand.m_allLocalFiles)
				{
					string[] array2 = this.m_set.ToStringArray();
					if (array2 != null)
					{
						for (int j = 0; j < array2.Length; j++)
						{
							string root2 = FileIOAccess.GetRoot(array2[j]);
							if (root2 != null && FileIOAccess._LocalDrive(FileIOAccess.GetRoot(root2)))
							{
								stringExpressionSet.AddExpressions(new string[]
								{
									array2[j]
								}, true, false);
							}
						}
					}
				}
				string[] array3 = this.m_set.Intersect(operand.m_set).ToStringArray();
				if (array3 != null)
				{
					stringExpressionSet.AddExpressions(array3, !stringExpressionSet.IsEmpty(), false);
				}
				return new FileIOAccess(stringExpressionSet, false, this.m_allLocalFiles && operand.m_allLocalFiles, this.m_pathDiscovery);
			}
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x000C10EC File Offset: 0x000C00EC
		public bool IsSubsetOf(FileIOAccess operand)
		{
			if (operand == null)
			{
				return this.IsEmpty();
			}
			if (operand.m_allFiles)
			{
				return true;
			}
			if ((!this.m_pathDiscovery || !this.m_set.IsSubsetOfPathDiscovery(operand.m_set)) && !this.m_set.IsSubsetOf(operand.m_set))
			{
				if (!operand.m_allLocalFiles)
				{
					return false;
				}
				string[] array = this.m_set.ToStringArray();
				for (int i = 0; i < array.Length; i++)
				{
					string root = FileIOAccess.GetRoot(array[i]);
					if (root == null || !FileIOAccess._LocalDrive(FileIOAccess.GetRoot(root)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x000C1180 File Offset: 0x000C0180
		private static string GetRoot(string path)
		{
			string text = path.Substring(0, 3);
			if (text.EndsWith(":\\", StringComparison.Ordinal))
			{
				return text;
			}
			return null;
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x000C11A8 File Offset: 0x000C01A8
		public override string ToString()
		{
			if (this.m_allFiles)
			{
				return "*AllFiles*";
			}
			if (this.m_allLocalFiles)
			{
				string text = "*AllLocalFiles*";
				string text2 = this.m_set.ToString();
				if (text2 != null && text2.Length > 0)
				{
					text = text + ";" + text2;
				}
				return text;
			}
			return this.m_set.ToString();
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x000C1203 File Offset: 0x000C0203
		public string[] ToStringArray()
		{
			return this.m_set.ToStringArray();
		}

		// Token: 0x06003922 RID: 14626
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool _LocalDrive(string path);

		// Token: 0x06003923 RID: 14627 RVA: 0x000C1210 File Offset: 0x000C0210
		public override bool Equals(object obj)
		{
			FileIOAccess fileIOAccess = obj as FileIOAccess;
			if (fileIOAccess == null)
			{
				return this.IsEmpty() && obj == null;
			}
			if (this.m_pathDiscovery)
			{
				return (this.m_allFiles && fileIOAccess.m_allFiles) || (this.m_allLocalFiles == fileIOAccess.m_allLocalFiles && this.m_set.IsSubsetOf(fileIOAccess.m_set) && fileIOAccess.m_set.IsSubsetOf(this.m_set));
			}
			return this.IsSubsetOf(fileIOAccess) && fileIOAccess.IsSubsetOf(this);
		}

		// Token: 0x06003924 RID: 14628 RVA: 0x000C129F File Offset: 0x000C029F
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04001D9A RID: 7578
		private const string m_strAllFiles = "*AllFiles*";

		// Token: 0x04001D9B RID: 7579
		private const string m_strAllLocalFiles = "*AllLocalFiles*";

		// Token: 0x04001D9C RID: 7580
		private bool m_ignoreCase = true;

		// Token: 0x04001D9D RID: 7581
		private StringExpressionSet m_set;

		// Token: 0x04001D9E RID: 7582
		private bool m_allFiles;

		// Token: 0x04001D9F RID: 7583
		private bool m_allLocalFiles;

		// Token: 0x04001DA0 RID: 7584
		private bool m_pathDiscovery;
	}
}
