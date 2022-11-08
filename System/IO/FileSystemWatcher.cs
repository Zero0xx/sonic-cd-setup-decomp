using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.IO
{
	// Token: 0x02000728 RID: 1832
	[IODescription("FileSystemWatcherDesc")]
	[DefaultEvent("Changed")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class FileSystemWatcher : Component, ISupportInitialize
	{
		// Token: 0x060037D9 RID: 14297 RVA: 0x000EBFA0 File Offset: 0x000EAFA0
		static FileSystemWatcher()
		{
			foreach (object obj in Enum.GetValues(typeof(NotifyFilters)))
			{
				int num = (int)obj;
				FileSystemWatcher.notifyFiltersValidMask |= num;
			}
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000EC024 File Offset: 0x000EB024
		public FileSystemWatcher()
		{
			this.directory = string.Empty;
			this.filter = "*.*";
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000EC055 File Offset: 0x000EB055
		public FileSystemWatcher(string path) : this(path, "*.*")
		{
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000EC064 File Offset: 0x000EB064
		public FileSystemWatcher(string path, string filter)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			if (filter == null)
			{
				throw new ArgumentNullException("filter");
			}
			if (path.Length == 0 || !Directory.Exists(path))
			{
				throw new ArgumentException(SR.GetString("InvalidDirName", new object[]
				{
					path
				}));
			}
			this.directory = path;
			this.filter = filter;
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x060037DD RID: 14301 RVA: 0x000EC0E0 File Offset: 0x000EB0E0
		// (set) Token: 0x060037DE RID: 14302 RVA: 0x000EC0E8 File Offset: 0x000EB0E8
		[DefaultValue(NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite)]
		[IODescription("FSW_ChangedFilter")]
		public NotifyFilters NotifyFilter
		{
			get
			{
				return this.notifyFilters;
			}
			set
			{
				if ((value & (NotifyFilters)(~(NotifyFilters)FileSystemWatcher.notifyFiltersValidMask)) != (NotifyFilters)0)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(NotifyFilters));
				}
				if (this.notifyFilters != value)
				{
					this.notifyFilters = value;
					this.Restart();
				}
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x060037DF RID: 14303 RVA: 0x000EC120 File Offset: 0x000EB120
		// (set) Token: 0x060037E0 RID: 14304 RVA: 0x000EC128 File Offset: 0x000EB128
		[IODescription("FSW_Enabled")]
		[DefaultValue(false)]
		public bool EnableRaisingEvents
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (this.enabled == value)
				{
					return;
				}
				this.enabled = value;
				if (!this.IsSuspended())
				{
					if (this.enabled)
					{
						this.StartRaisingEvents();
						return;
					}
					this.StopRaisingEvents();
				}
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x060037E1 RID: 14305 RVA: 0x000EC158 File Offset: 0x000EB158
		// (set) Token: 0x060037E2 RID: 14306 RVA: 0x000EC160 File Offset: 0x000EB160
		[IODescription("FSW_Filter")]
		[RecommendedAsConfigurable(true)]
		[DefaultValue("*.*")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				if (value == null || value == string.Empty)
				{
					value = "*.*";
				}
				if (string.Compare(this.filter, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.filter = value;
				}
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x060037E3 RID: 14307 RVA: 0x000EC18F File Offset: 0x000EB18F
		// (set) Token: 0x060037E4 RID: 14308 RVA: 0x000EC197 File Offset: 0x000EB197
		[DefaultValue(false)]
		[IODescription("FSW_IncludeSubdirectories")]
		public bool IncludeSubdirectories
		{
			get
			{
				return this.includeSubdirectories;
			}
			set
			{
				if (this.includeSubdirectories != value)
				{
					this.includeSubdirectories = value;
					this.Restart();
				}
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x060037E5 RID: 14309 RVA: 0x000EC1AF File Offset: 0x000EB1AF
		// (set) Token: 0x060037E6 RID: 14310 RVA: 0x000EC1B7 File Offset: 0x000EB1B7
		[Browsable(false)]
		[DefaultValue(8192)]
		public int InternalBufferSize
		{
			get
			{
				return this.internalBufferSize;
			}
			set
			{
				if (this.internalBufferSize != value)
				{
					if (value < 4096)
					{
						value = 4096;
					}
					this.internalBufferSize = value;
					this.Restart();
				}
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x060037E7 RID: 14311 RVA: 0x000EC1DE File Offset: 0x000EB1DE
		private bool IsHandleInvalid
		{
			get
			{
				return this.directoryHandle == null || this.directoryHandle.IsInvalid;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x060037E8 RID: 14312 RVA: 0x000EC1F5 File Offset: 0x000EB1F5
		// (set) Token: 0x060037E9 RID: 14313 RVA: 0x000EC200 File Offset: 0x000EB200
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RecommendedAsConfigurable(true)]
		[Editor("System.Diagnostics.Design.FSWPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[IODescription("FSW_Path")]
		public string Path
		{
			get
			{
				return this.directory;
			}
			set
			{
				value = ((value == null) ? string.Empty : value);
				if (string.Compare(this.directory, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					if (base.DesignMode)
					{
						if (value.IndexOfAny(FileSystemWatcher.wildcards) != -1 || value.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
						{
							throw new ArgumentException(SR.GetString("InvalidDirName", new object[]
							{
								value
							}));
						}
					}
					else if (!Directory.Exists(value))
					{
						throw new ArgumentException(SR.GetString("InvalidDirName", new object[]
						{
							value
						}));
					}
					this.directory = value;
					this.readGranted = false;
					this.Restart();
				}
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x060037EA RID: 14314 RVA: 0x000EC2A1 File Offset: 0x000EB2A1
		// (set) Token: 0x060037EB RID: 14315 RVA: 0x000EC2A9 File Offset: 0x000EB2A9
		[Browsable(false)]
		public override ISite Site
		{
			get
			{
				return base.Site;
			}
			set
			{
				base.Site = value;
				if (this.Site != null && this.Site.DesignMode)
				{
					this.EnableRaisingEvents = true;
				}
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x060037EC RID: 14316 RVA: 0x000EC2D0 File Offset: 0x000EB2D0
		// (set) Token: 0x060037ED RID: 14317 RVA: 0x000EC32A File Offset: 0x000EB32A
		[Browsable(false)]
		[DefaultValue(null)]
		[IODescription("FSW_SynchronizingObject")]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if (this.synchronizingObject == null && base.DesignMode)
				{
					IDesignerHost designerHost = (IDesignerHost)this.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						object rootComponent = designerHost.RootComponent;
						if (rootComponent != null && rootComponent is ISynchronizeInvoke)
						{
							this.synchronizingObject = (ISynchronizeInvoke)rootComponent;
						}
					}
				}
				return this.synchronizingObject;
			}
			set
			{
				this.synchronizingObject = value;
			}
		}

		// Token: 0x14000053 RID: 83
		// (add) Token: 0x060037EE RID: 14318 RVA: 0x000EC333 File Offset: 0x000EB333
		// (remove) Token: 0x060037EF RID: 14319 RVA: 0x000EC34C File Offset: 0x000EB34C
		[IODescription("FSW_Changed")]
		public event FileSystemEventHandler Changed
		{
			add
			{
				this.onChangedHandler = (FileSystemEventHandler)Delegate.Combine(this.onChangedHandler, value);
			}
			remove
			{
				this.onChangedHandler = (FileSystemEventHandler)Delegate.Remove(this.onChangedHandler, value);
			}
		}

		// Token: 0x14000054 RID: 84
		// (add) Token: 0x060037F0 RID: 14320 RVA: 0x000EC365 File Offset: 0x000EB365
		// (remove) Token: 0x060037F1 RID: 14321 RVA: 0x000EC37E File Offset: 0x000EB37E
		[IODescription("FSW_Created")]
		public event FileSystemEventHandler Created
		{
			add
			{
				this.onCreatedHandler = (FileSystemEventHandler)Delegate.Combine(this.onCreatedHandler, value);
			}
			remove
			{
				this.onCreatedHandler = (FileSystemEventHandler)Delegate.Remove(this.onCreatedHandler, value);
			}
		}

		// Token: 0x14000055 RID: 85
		// (add) Token: 0x060037F2 RID: 14322 RVA: 0x000EC397 File Offset: 0x000EB397
		// (remove) Token: 0x060037F3 RID: 14323 RVA: 0x000EC3B0 File Offset: 0x000EB3B0
		[IODescription("FSW_Deleted")]
		public event FileSystemEventHandler Deleted
		{
			add
			{
				this.onDeletedHandler = (FileSystemEventHandler)Delegate.Combine(this.onDeletedHandler, value);
			}
			remove
			{
				this.onDeletedHandler = (FileSystemEventHandler)Delegate.Remove(this.onDeletedHandler, value);
			}
		}

		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060037F4 RID: 14324 RVA: 0x000EC3C9 File Offset: 0x000EB3C9
		// (remove) Token: 0x060037F5 RID: 14325 RVA: 0x000EC3E2 File Offset: 0x000EB3E2
		[Browsable(false)]
		public event ErrorEventHandler Error
		{
			add
			{
				this.onErrorHandler = (ErrorEventHandler)Delegate.Combine(this.onErrorHandler, value);
			}
			remove
			{
				this.onErrorHandler = (ErrorEventHandler)Delegate.Remove(this.onErrorHandler, value);
			}
		}

		// Token: 0x14000057 RID: 87
		// (add) Token: 0x060037F6 RID: 14326 RVA: 0x000EC3FB File Offset: 0x000EB3FB
		// (remove) Token: 0x060037F7 RID: 14327 RVA: 0x000EC414 File Offset: 0x000EB414
		[IODescription("FSW_Renamed")]
		public event RenamedEventHandler Renamed
		{
			add
			{
				this.onRenamedHandler = (RenamedEventHandler)Delegate.Combine(this.onRenamedHandler, value);
			}
			remove
			{
				this.onRenamedHandler = (RenamedEventHandler)Delegate.Remove(this.onRenamedHandler, value);
			}
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x000EC430 File Offset: 0x000EB430
		public void BeginInit()
		{
			bool flag = this.enabled;
			this.StopRaisingEvents();
			this.enabled = flag;
			this.initializing = true;
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x000EC458 File Offset: 0x000EB458
		private unsafe void CompletionStatusChanged(uint errorCode, uint numBytes, NativeOverlapped* overlappedPointer)
		{
			Overlapped overlapped = Overlapped.Unpack(overlappedPointer);
			ulong num = (ulong)((ulong)((long)overlapped.OffsetHigh) << 32);
			num |= (ulong)overlapped.OffsetLow;
			IntPtr intPtr = (IntPtr)((long)num);
			FileSystemWatcher.FSWAsyncResult fswasyncResult = (FileSystemWatcher.FSWAsyncResult)overlapped.AsyncResult;
			try
			{
				if (!this.stopListening)
				{
					lock (this)
					{
						if (errorCode != 0U)
						{
							if (errorCode != 995U)
							{
								this.OnError(new ErrorEventArgs(new Win32Exception((int)errorCode)));
								this.EnableRaisingEvents = false;
							}
						}
						else if (fswasyncResult.session == this.currentSession)
						{
							if (numBytes == 0U)
							{
								this.NotifyInternalBufferOverflowEvent();
							}
							else
							{
								int num2 = 0;
								string text = null;
								int num3;
								do
								{
									num3 = Marshal.ReadInt32((IntPtr)((long)intPtr + (long)num2));
									int num4 = Marshal.ReadInt32((IntPtr)((long)intPtr + (long)num2 + 4L));
									int num5 = Marshal.ReadInt32((IntPtr)((long)intPtr + (long)num2 + 8L));
									string text2 = Marshal.PtrToStringUni((IntPtr)((long)intPtr + (long)num2 + 12L), num5 / 2);
									if (num4 == 4)
									{
										text = text2;
									}
									else if (num4 == 5)
									{
										if (text != null)
										{
											this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, text2, text);
											text = null;
										}
										else
										{
											this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, text2, text);
											text = null;
										}
									}
									else
									{
										if (text != null)
										{
											this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, null, text);
											text = null;
										}
										this.NotifyFileSystemEventArgs(num4, text2);
									}
									num2 += num3;
								}
								while (num3 != 0);
								if (text != null)
								{
									this.NotifyRenameEventArgs(WatcherChangeTypes.Renamed, null, text);
								}
							}
						}
					}
				}
			}
			finally
			{
				Overlapped.Free(overlappedPointer);
				if (this.stopListening || this.runOnce)
				{
					if (intPtr != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr);
					}
				}
				else
				{
					this.Monitor(intPtr);
				}
			}
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x000EC648 File Offset: 0x000EB648
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopRaisingEvents();
				this.onChangedHandler = null;
				this.onCreatedHandler = null;
				this.onDeletedHandler = null;
				this.onRenamedHandler = null;
				this.onErrorHandler = null;
				this.readGranted = false;
			}
			else
			{
				this.stopListening = true;
				if (!this.IsHandleInvalid)
				{
					this.directoryHandle.Close();
				}
			}
			this.disposed = true;
			base.Dispose(disposing);
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x000EC6B2 File Offset: 0x000EB6B2
		public void EndInit()
		{
			this.initializing = false;
			if (this.directory.Length != 0 && this.enabled)
			{
				this.StartRaisingEvents();
			}
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x000EC6D6 File Offset: 0x000EB6D6
		private bool IsSuspended()
		{
			return this.initializing || base.DesignMode;
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x000EC6E8 File Offset: 0x000EB6E8
		private bool MatchPattern(string relativePath)
		{
			string fileName = System.IO.Path.GetFileName(relativePath);
			return fileName != null && PatternMatcher.StrictMatchPattern(this.filter.ToUpper(CultureInfo.InvariantCulture), fileName.ToUpper(CultureInfo.InvariantCulture));
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x000EC724 File Offset: 0x000EB724
		private unsafe void Monitor(IntPtr bufferPtr)
		{
			if (!this.enabled || this.IsHandleInvalid)
			{
				return;
			}
			Overlapped overlapped = new Overlapped();
			if (bufferPtr == (IntPtr)0)
			{
				try
				{
					bufferPtr = Marshal.AllocHGlobal(this.internalBufferSize);
				}
				catch (OutOfMemoryException)
				{
					throw new OutOfMemoryException(SR.GetString("BufferSizeTooLarge", new object[]
					{
						this.internalBufferSize.ToString(CultureInfo.CurrentCulture)
					}));
				}
			}
			ulong num = (ulong)((long)bufferPtr);
			overlapped.OffsetHigh = (int)(num >> 32);
			overlapped.OffsetLow = (int)num;
			overlapped.AsyncResult = new FileSystemWatcher.FSWAsyncResult
			{
				session = this.currentSession
			};
			NativeOverlapped* ptr = overlapped.Pack(new IOCompletionCallback(this.CompletionStatusChanged), this.currentSession);
			bool flag = false;
			try
			{
				if (!this.IsHandleInvalid)
				{
					int num2;
					flag = UnsafeNativeMethods.ReadDirectoryChangesW(this.directoryHandle, new HandleRef(this, bufferPtr), this.internalBufferSize, this.includeSubdirectories ? 1 : 0, (int)this.notifyFilters, out num2, ptr, NativeMethods.NullHandleRef);
				}
			}
			catch (ObjectDisposedException)
			{
			}
			catch (ArgumentNullException)
			{
			}
			finally
			{
				if (!flag)
				{
					Overlapped.Free(ptr);
					Marshal.FreeHGlobal(bufferPtr);
					if (!this.IsHandleInvalid)
					{
						this.OnError(new ErrorEventArgs(new Win32Exception()));
					}
				}
			}
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x000EC88C File Offset: 0x000EB88C
		private void NotifyFileSystemEventArgs(int action, string name)
		{
			if (!this.MatchPattern(name))
			{
				return;
			}
			switch (action)
			{
			case 1:
				this.OnCreated(new FileSystemEventArgs(WatcherChangeTypes.Created, this.directory, name));
				return;
			case 2:
				this.OnDeleted(new FileSystemEventArgs(WatcherChangeTypes.Deleted, this.directory, name));
				return;
			case 3:
				this.OnChanged(new FileSystemEventArgs(WatcherChangeTypes.Changed, this.directory, name));
				return;
			default:
				return;
			}
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x000EC8F8 File Offset: 0x000EB8F8
		private void NotifyInternalBufferOverflowEvent()
		{
			InternalBufferOverflowException exception = new InternalBufferOverflowException(SR.GetString("FSW_BufferOverflow", new object[]
			{
				this.directory
			}));
			ErrorEventArgs e = new ErrorEventArgs(exception);
			this.OnError(e);
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000EC934 File Offset: 0x000EB934
		private void NotifyRenameEventArgs(WatcherChangeTypes action, string name, string oldName)
		{
			if (!this.MatchPattern(name) && !this.MatchPattern(oldName))
			{
				return;
			}
			RenamedEventArgs e = new RenamedEventArgs(action, this.directory, name, oldName);
			this.OnRenamed(e);
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000EC96C File Offset: 0x000EB96C
		protected void OnChanged(FileSystemEventArgs e)
		{
			FileSystemEventHandler fileSystemEventHandler = this.onChangedHandler;
			if (fileSystemEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(fileSystemEventHandler, new object[]
					{
						this,
						e
					});
					return;
				}
				fileSystemEventHandler(this, e);
			}
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000EC9C0 File Offset: 0x000EB9C0
		protected void OnCreated(FileSystemEventArgs e)
		{
			FileSystemEventHandler fileSystemEventHandler = this.onCreatedHandler;
			if (fileSystemEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(fileSystemEventHandler, new object[]
					{
						this,
						e
					});
					return;
				}
				fileSystemEventHandler(this, e);
			}
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000ECA14 File Offset: 0x000EBA14
		protected void OnDeleted(FileSystemEventArgs e)
		{
			FileSystemEventHandler fileSystemEventHandler = this.onDeletedHandler;
			if (fileSystemEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(fileSystemEventHandler, new object[]
					{
						this,
						e
					});
					return;
				}
				fileSystemEventHandler(this, e);
			}
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000ECA68 File Offset: 0x000EBA68
		protected void OnError(ErrorEventArgs e)
		{
			ErrorEventHandler errorEventHandler = this.onErrorHandler;
			if (errorEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(errorEventHandler, new object[]
					{
						this,
						e
					});
					return;
				}
				errorEventHandler(this, e);
			}
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x000ECABC File Offset: 0x000EBABC
		private void OnInternalFileSystemEventArgs(object sender, FileSystemEventArgs e)
		{
			lock (this)
			{
				if (!this.isChanged)
				{
					this.changedResult = new WaitForChangedResult(e.ChangeType, e.Name, false);
					this.isChanged = true;
					System.Threading.Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x06003807 RID: 14343 RVA: 0x000ECB18 File Offset: 0x000EBB18
		private void OnInternalRenameEventArgs(object sender, RenamedEventArgs e)
		{
			lock (this)
			{
				if (!this.isChanged)
				{
					this.changedResult = new WaitForChangedResult(e.ChangeType, e.Name, e.OldName, false);
					this.isChanged = true;
					System.Threading.Monitor.Pulse(this);
				}
			}
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x000ECB7C File Offset: 0x000EBB7C
		protected void OnRenamed(RenamedEventArgs e)
		{
			RenamedEventHandler renamedEventHandler = this.onRenamedHandler;
			if (renamedEventHandler != null)
			{
				if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
				{
					this.SynchronizingObject.BeginInvoke(renamedEventHandler, new object[]
					{
						this,
						e
					});
					return;
				}
				renamedEventHandler(this, e);
			}
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x000ECBCE File Offset: 0x000EBBCE
		private void Restart()
		{
			if (!this.IsSuspended() && this.enabled)
			{
				this.StopRaisingEvents();
				this.StartRaisingEvents();
			}
		}

		// Token: 0x0600380A RID: 14346 RVA: 0x000ECBEC File Offset: 0x000EBBEC
		private void StartRaisingEvents()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				new EnvironmentPermission(PermissionState.Unrestricted).Assert();
				if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				{
					throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			if (this.IsSuspended())
			{
				this.enabled = true;
				return;
			}
			if (!this.readGranted)
			{
				string fullPath = System.IO.Path.GetFullPath(this.directory);
				FileIOPermission fileIOPermission = new FileIOPermission(FileIOPermissionAccess.Read, fullPath);
				fileIOPermission.Demand();
				this.readGranted = true;
			}
			if (!this.IsHandleInvalid)
			{
				return;
			}
			this.directoryHandle = NativeMethods.CreateFile(this.directory, 1, 7, null, 3, 1107296256, new SafeFileHandle(IntPtr.Zero, false));
			if (this.IsHandleInvalid)
			{
				throw new FileNotFoundException(SR.GetString("FSW_IOError", new object[]
				{
					this.directory
				}));
			}
			this.stopListening = false;
			Interlocked.Increment(ref this.currentSession);
			SecurityPermission securityPermission = new SecurityPermission(PermissionState.Unrestricted);
			securityPermission.Assert();
			try
			{
				ThreadPool.BindHandle(this.directoryHandle);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.enabled = true;
			this.Monitor((IntPtr)0);
		}

		// Token: 0x0600380B RID: 14347 RVA: 0x000ECD38 File Offset: 0x000EBD38
		private void StopRaisingEvents()
		{
			if (this.IsSuspended())
			{
				this.enabled = false;
				return;
			}
			if (this.IsHandleInvalid)
			{
				return;
			}
			this.stopListening = true;
			this.directoryHandle.Close();
			this.directoryHandle = null;
			Interlocked.Increment(ref this.currentSession);
			this.enabled = false;
		}

		// Token: 0x0600380C RID: 14348 RVA: 0x000ECD8A File Offset: 0x000EBD8A
		public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType)
		{
			return this.WaitForChanged(changeType, -1);
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x000ECD94 File Offset: 0x000EBD94
		public WaitForChangedResult WaitForChanged(WatcherChangeTypes changeType, int timeout)
		{
			FileSystemEventHandler value = new FileSystemEventHandler(this.OnInternalFileSystemEventArgs);
			RenamedEventHandler value2 = new RenamedEventHandler(this.OnInternalRenameEventArgs);
			this.isChanged = false;
			this.changedResult = WaitForChangedResult.TimedOutResult;
			if ((changeType & WatcherChangeTypes.Created) != (WatcherChangeTypes)0)
			{
				this.Created += value;
			}
			if ((changeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes)0)
			{
				this.Deleted += value;
			}
			if ((changeType & WatcherChangeTypes.Changed) != (WatcherChangeTypes)0)
			{
				this.Changed += value;
			}
			if ((changeType & WatcherChangeTypes.Renamed) != (WatcherChangeTypes)0)
			{
				this.Renamed += value2;
			}
			bool enableRaisingEvents = this.EnableRaisingEvents;
			if (!enableRaisingEvents)
			{
				this.runOnce = true;
				this.EnableRaisingEvents = true;
			}
			WaitForChangedResult timedOutResult = WaitForChangedResult.TimedOutResult;
			lock (this)
			{
				if (timeout == -1)
				{
					while (!this.isChanged)
					{
						System.Threading.Monitor.Wait(this);
					}
				}
				else
				{
					System.Threading.Monitor.Wait(this, timeout, true);
				}
				timedOutResult = this.changedResult;
			}
			this.EnableRaisingEvents = enableRaisingEvents;
			this.runOnce = false;
			if ((changeType & WatcherChangeTypes.Created) != (WatcherChangeTypes)0)
			{
				this.Created -= value;
			}
			if ((changeType & WatcherChangeTypes.Deleted) != (WatcherChangeTypes)0)
			{
				this.Deleted -= value;
			}
			if ((changeType & WatcherChangeTypes.Changed) != (WatcherChangeTypes)0)
			{
				this.Changed -= value;
			}
			if ((changeType & WatcherChangeTypes.Renamed) != (WatcherChangeTypes)0)
			{
				this.Renamed -= value2;
			}
			return timedOutResult;
		}

		// Token: 0x040031EB RID: 12779
		private const NotifyFilters defaultNotifyFilters = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

		// Token: 0x040031EC RID: 12780
		private string directory;

		// Token: 0x040031ED RID: 12781
		private string filter;

		// Token: 0x040031EE RID: 12782
		private SafeFileHandle directoryHandle;

		// Token: 0x040031EF RID: 12783
		private NotifyFilters notifyFilters = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;

		// Token: 0x040031F0 RID: 12784
		private bool includeSubdirectories;

		// Token: 0x040031F1 RID: 12785
		private bool enabled;

		// Token: 0x040031F2 RID: 12786
		private bool initializing;

		// Token: 0x040031F3 RID: 12787
		private int internalBufferSize = 8192;

		// Token: 0x040031F4 RID: 12788
		private WaitForChangedResult changedResult;

		// Token: 0x040031F5 RID: 12789
		private bool isChanged;

		// Token: 0x040031F6 RID: 12790
		private ISynchronizeInvoke synchronizingObject;

		// Token: 0x040031F7 RID: 12791
		private bool readGranted;

		// Token: 0x040031F8 RID: 12792
		private bool disposed;

		// Token: 0x040031F9 RID: 12793
		private int currentSession;

		// Token: 0x040031FA RID: 12794
		private FileSystemEventHandler onChangedHandler;

		// Token: 0x040031FB RID: 12795
		private FileSystemEventHandler onCreatedHandler;

		// Token: 0x040031FC RID: 12796
		private FileSystemEventHandler onDeletedHandler;

		// Token: 0x040031FD RID: 12797
		private RenamedEventHandler onRenamedHandler;

		// Token: 0x040031FE RID: 12798
		private ErrorEventHandler onErrorHandler;

		// Token: 0x040031FF RID: 12799
		private bool stopListening;

		// Token: 0x04003200 RID: 12800
		private bool runOnce;

		// Token: 0x04003201 RID: 12801
		private static readonly char[] wildcards = new char[]
		{
			'?',
			'*'
		};

		// Token: 0x04003202 RID: 12802
		private static int notifyFiltersValidMask = 0;

		// Token: 0x02000729 RID: 1833
		private sealed class FSWAsyncResult : IAsyncResult
		{
			// Token: 0x17000CFE RID: 3326
			// (get) Token: 0x0600380E RID: 14350 RVA: 0x000ECEA4 File Offset: 0x000EBEA4
			public bool IsCompleted
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000CFF RID: 3327
			// (get) Token: 0x0600380F RID: 14351 RVA: 0x000ECEAB File Offset: 0x000EBEAB
			public WaitHandle AsyncWaitHandle
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000D00 RID: 3328
			// (get) Token: 0x06003810 RID: 14352 RVA: 0x000ECEB2 File Offset: 0x000EBEB2
			public object AsyncState
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x17000D01 RID: 3329
			// (get) Token: 0x06003811 RID: 14353 RVA: 0x000ECEB9 File Offset: 0x000EBEB9
			public bool CompletedSynchronously
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x04003203 RID: 12803
			internal int session;
		}
	}
}
