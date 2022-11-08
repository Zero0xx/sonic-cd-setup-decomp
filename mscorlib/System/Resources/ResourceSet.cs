using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Resources
{
	// Token: 0x0200043A RID: 1082
	[ComVisible(true)]
	[Serializable]
	public class ResourceSet : IDisposable, IEnumerable
	{
		// Token: 0x06002C34 RID: 11316 RVA: 0x00095A95 File Offset: 0x00094A95
		protected ResourceSet()
		{
			this.Table = new Hashtable(0);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x00095AA9 File Offset: 0x00094AA9
		internal ResourceSet(bool junk)
		{
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x00095AB1 File Offset: 0x00094AB1
		public ResourceSet(string fileName)
		{
			this.Reader = new ResourceReader(fileName);
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00095AD6 File Offset: 0x00094AD6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public ResourceSet(Stream stream)
		{
			this.Reader = new ResourceReader(stream);
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x00095AFB File Offset: 0x00094AFB
		public ResourceSet(IResourceReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.Reader = reader;
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x00095B29 File Offset: 0x00094B29
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x00095B34 File Offset: 0x00094B34
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				IResourceReader reader = this.Reader;
				this.Reader = null;
				if (reader != null)
				{
					reader.Close();
				}
			}
			this.Reader = null;
			this._caseInsensitiveTable = null;
			this.Table = null;
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x00095B70 File Offset: 0x00094B70
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00095B79 File Offset: 0x00094B79
		public virtual Type GetDefaultReader()
		{
			return typeof(ResourceReader);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x00095B85 File Offset: 0x00094B85
		public virtual Type GetDefaultWriter()
		{
			return typeof(ResourceWriter);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00095B91 File Offset: 0x00094B91
		[ComVisible(false)]
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x00095B99 File Offset: 0x00094B99
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x00095BA4 File Offset: 0x00094BA4
		private IDictionaryEnumerator GetEnumeratorHelper()
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			return table.GetEnumerator();
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x00095BD4 File Offset: 0x00094BD4
		public virtual string GetString(string name)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string result;
			try
			{
				result = (string)table[name];
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Name", new object[]
				{
					name
				}));
			}
			return result;
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x00095C48 File Offset: 0x00094C48
		public virtual string GetString(string name, bool ignoreCase)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string text;
			try
			{
				text = (string)table[name];
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Name", new object[]
				{
					name
				}));
			}
			if (text != null || !ignoreCase)
			{
				return text;
			}
			Hashtable hashtable = this._caseInsensitiveTable;
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				IDictionaryEnumerator enumerator = table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					hashtable.Add(enumerator.Key, enumerator.Value);
				}
				this._caseInsensitiveTable = hashtable;
			}
			string result;
			try
			{
				result = (string)hashtable[name];
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Name", new object[]
				{
					name
				}));
			}
			return result;
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x00095D44 File Offset: 0x00094D44
		public virtual object GetObject(string name)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return table[name];
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00095D84 File Offset: 0x00094D84
		public virtual object GetObject(string name, bool ignoreCase)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = table[name];
			if (obj != null || !ignoreCase)
			{
				return obj;
			}
			Hashtable hashtable = this._caseInsensitiveTable;
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				IDictionaryEnumerator enumerator = table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					hashtable.Add(enumerator.Key, enumerator.Value);
				}
				this._caseInsensitiveTable = hashtable;
			}
			return hashtable[name];
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x00095E10 File Offset: 0x00094E10
		protected virtual void ReadResources()
		{
			IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
			while (enumerator.MoveNext())
			{
				object value = enumerator.Value;
				this.Table.Add(enumerator.Key, value);
			}
		}

		// Token: 0x04001591 RID: 5521
		[NonSerialized]
		protected IResourceReader Reader;

		// Token: 0x04001592 RID: 5522
		protected Hashtable Table;

		// Token: 0x04001593 RID: 5523
		private Hashtable _caseInsensitiveTable;
	}
}
