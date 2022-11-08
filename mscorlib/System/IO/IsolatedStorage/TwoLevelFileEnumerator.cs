using System;
using System.Collections;

namespace System.IO.IsolatedStorage
{
	// Token: 0x020007AF RID: 1967
	internal sealed class TwoLevelFileEnumerator : IEnumerator
	{
		// Token: 0x06004638 RID: 17976 RVA: 0x000EFCFC File Offset: 0x000EECFC
		public TwoLevelFileEnumerator(string root)
		{
			this.m_Root = root;
			this.Reset();
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x000EFD14 File Offset: 0x000EED14
		public bool MoveNext()
		{
			lock (this)
			{
				if (this.m_fReset)
				{
					this.m_fReset = false;
					return this.AdvanceRootDir();
				}
				if (this.m_RootDir.Length == 0)
				{
					return false;
				}
				this.m_nSubDir++;
				if (this.m_nSubDir >= this.m_SubDir.Length)
				{
					this.m_nSubDir = this.m_SubDir.Length;
					return this.AdvanceRootDir();
				}
				this.UpdateCurrent();
			}
			return true;
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x000EFDA8 File Offset: 0x000EEDA8
		private bool AdvanceRootDir()
		{
			this.m_nRootDir++;
			if (this.m_nRootDir >= this.m_RootDir.Length)
			{
				this.m_nRootDir = this.m_RootDir.Length;
				return false;
			}
			this.m_SubDir = Directory.GetDirectories(this.m_RootDir[this.m_nRootDir]);
			if (this.m_SubDir.Length == 0)
			{
				return this.AdvanceRootDir();
			}
			this.m_nSubDir = 0;
			this.UpdateCurrent();
			return true;
		}

		// Token: 0x0600463B RID: 17979 RVA: 0x000EFE1A File Offset: 0x000EEE1A
		private void UpdateCurrent()
		{
			this.m_Current.Path1 = Path.GetFileName(this.m_RootDir[this.m_nRootDir]);
			this.m_Current.Path2 = Path.GetFileName(this.m_SubDir[this.m_nSubDir]);
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x000EFE56 File Offset: 0x000EEE56
		public object Current
		{
			get
			{
				if (this.m_fReset)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (this.m_nRootDir >= this.m_RootDir.Length)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
				return this.m_Current;
			}
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x000EFE98 File Offset: 0x000EEE98
		public void Reset()
		{
			this.m_RootDir = null;
			this.m_nRootDir = -1;
			this.m_SubDir = null;
			this.m_nSubDir = -1;
			this.m_Current = new TwoPaths();
			this.m_fReset = true;
			this.m_RootDir = Directory.GetDirectories(this.m_Root);
		}

		// Token: 0x040022F5 RID: 8949
		private string m_Root;

		// Token: 0x040022F6 RID: 8950
		private TwoPaths m_Current;

		// Token: 0x040022F7 RID: 8951
		private bool m_fReset;

		// Token: 0x040022F8 RID: 8952
		private string[] m_RootDir;

		// Token: 0x040022F9 RID: 8953
		private int m_nRootDir;

		// Token: 0x040022FA RID: 8954
		private string[] m_SubDir;

		// Token: 0x040022FB RID: 8955
		private int m_nSubDir;
	}
}
