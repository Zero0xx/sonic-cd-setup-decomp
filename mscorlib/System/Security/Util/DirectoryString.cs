using System;
using System.Collections;

namespace System.Security.Util
{
	// Token: 0x0200048B RID: 1163
	[Serializable]
	internal class DirectoryString : SiteString
	{
		// Token: 0x06002E42 RID: 11842 RVA: 0x0009BD5C File Offset: 0x0009AD5C
		public DirectoryString()
		{
			this.m_site = "";
			this.m_separatedSite = new ArrayList();
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x0009BD7A File Offset: 0x0009AD7A
		public DirectoryString(string directory, bool checkForIllegalChars)
		{
			this.m_site = directory;
			this.m_checkForIllegalChars = checkForIllegalChars;
			this.m_separatedSite = this.CreateSeparatedString(directory);
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0009BDA0 File Offset: 0x0009ADA0
		private ArrayList CreateSeparatedString(string directory)
		{
			ArrayList arrayList = new ArrayList();
			if (directory == null || directory.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDirectoryOnUrl"));
			}
			string[] array = directory.Split(DirectoryString.m_separators);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && !array[i].Equals(""))
				{
					if (array[i].Equals("*"))
					{
						if (i != array.Length - 1)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDirectoryOnUrl"));
						}
						arrayList.Add(array[i]);
					}
					else
					{
						if (this.m_checkForIllegalChars && array[i].IndexOfAny(DirectoryString.m_illegalDirectoryCharacters) != -1)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidDirectoryOnUrl"));
						}
						arrayList.Add(array[i]);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x0009BE65 File Offset: 0x0009AE65
		public virtual bool IsSubsetOf(DirectoryString operand)
		{
			return this.IsSubsetOf(operand, true);
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x0009BE70 File Offset: 0x0009AE70
		public virtual bool IsSubsetOf(DirectoryString operand, bool ignoreCase)
		{
			if (operand == null)
			{
				return false;
			}
			if (operand.m_separatedSite.Count == 0)
			{
				return this.m_separatedSite.Count == 0 || (this.m_separatedSite.Count > 0 && string.Compare((string)this.m_separatedSite[0], "*", StringComparison.Ordinal) == 0);
			}
			if (this.m_separatedSite.Count == 0)
			{
				return string.Compare((string)operand.m_separatedSite[0], "*", StringComparison.Ordinal) == 0;
			}
			return base.IsSubsetOf(operand, ignoreCase);
		}

		// Token: 0x040017C5 RID: 6085
		private bool m_checkForIllegalChars;

		// Token: 0x040017C6 RID: 6086
		private new static char[] m_separators = new char[]
		{
			'/'
		};

		// Token: 0x040017C7 RID: 6087
		protected static char[] m_illegalDirectoryCharacters = new char[]
		{
			'\\',
			':',
			'*',
			'?',
			'"',
			'<',
			'>',
			'|'
		};
	}
}
