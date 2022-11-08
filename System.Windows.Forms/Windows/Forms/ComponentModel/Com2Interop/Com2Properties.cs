using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000761 RID: 1889
	internal class Com2Properties
	{
		// Token: 0x140003F4 RID: 1012
		// (add) Token: 0x06006403 RID: 25603 RVA: 0x0016D7FD File Offset: 0x0016C7FD
		// (remove) Token: 0x06006404 RID: 25604 RVA: 0x0016D816 File Offset: 0x0016C816
		public event EventHandler Disposed;

		// Token: 0x06006405 RID: 25605 RVA: 0x0016D830 File Offset: 0x0016C830
		public Com2Properties(object obj, Com2PropertyDescriptor[] props, int defaultIndex)
		{
			this.SetProps(props);
			this.weakObjRef = new WeakReference(obj);
			this.defaultIndex = defaultIndex;
			this.typeInfoVersions = this.GetTypeInfoVersions(obj);
			this.touchedTime = DateTime.Now.Ticks;
		}

		// Token: 0x17001514 RID: 5396
		// (get) Token: 0x06006406 RID: 25606 RVA: 0x0016D884 File Offset: 0x0016C884
		// (set) Token: 0x06006407 RID: 25607 RVA: 0x0016D88F File Offset: 0x0016C88F
		internal bool AlwaysValid
		{
			get
			{
				return this.alwaysValid > 0;
			}
			set
			{
				if (!value)
				{
					if (this.alwaysValid > 0)
					{
						this.alwaysValid--;
					}
					return;
				}
				if (this.alwaysValid == 0 && !this.CheckValid())
				{
					return;
				}
				this.alwaysValid++;
			}
		}

		// Token: 0x17001515 RID: 5397
		// (get) Token: 0x06006408 RID: 25608 RVA: 0x0016D8CB File Offset: 0x0016C8CB
		public Com2PropertyDescriptor DefaultProperty
		{
			get
			{
				if (!this.CheckValid(true))
				{
					return null;
				}
				if (this.defaultIndex != -1)
				{
					return this.props[this.defaultIndex];
				}
				if (this.props.Length > 0)
				{
					return this.props[0];
				}
				return null;
			}
		}

		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x06006409 RID: 25609 RVA: 0x0016D904 File Offset: 0x0016C904
		public object TargetObject
		{
			get
			{
				if (!this.CheckValid(false) || this.touchedTime == 0L)
				{
					return null;
				}
				return this.weakObjRef.Target;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x0600640A RID: 25610 RVA: 0x0016D928 File Offset: 0x0016C928
		public long TicksSinceTouched
		{
			get
			{
				if (this.touchedTime == 0L)
				{
					return 0L;
				}
				return DateTime.Now.Ticks - this.touchedTime;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x0600640B RID: 25611 RVA: 0x0016D958 File Offset: 0x0016C958
		public Com2PropertyDescriptor[] Properties
		{
			get
			{
				this.CheckValid(true);
				if (this.touchedTime == 0L || this.props == null)
				{
					return null;
				}
				this.touchedTime = DateTime.Now.Ticks;
				for (int i = 0; i < this.props.Length; i++)
				{
					this.props[i].SetNeedsRefresh(255, true);
				}
				return this.props;
			}
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x0600640C RID: 25612 RVA: 0x0016D9C0 File Offset: 0x0016C9C0
		public bool TooOld
		{
			get
			{
				this.CheckValid(false, false);
				return this.touchedTime != 0L && this.TicksSinceTouched > Com2Properties.AGE_THRESHHOLD;
			}
		}

		// Token: 0x0600640D RID: 25613 RVA: 0x0016D9E4 File Offset: 0x0016C9E4
		public void AddExtendedBrowsingHandlers(Hashtable handlers)
		{
			object targetObject = this.TargetObject;
			if (targetObject == null)
			{
				return;
			}
			for (int i = 0; i < Com2Properties.extendedInterfaces.Length; i++)
			{
				Type type = Com2Properties.extendedInterfaces[i];
				if (type.IsInstanceOfType(targetObject))
				{
					Com2ExtendedBrowsingHandler com2ExtendedBrowsingHandler = (Com2ExtendedBrowsingHandler)handlers[type];
					if (com2ExtendedBrowsingHandler == null)
					{
						com2ExtendedBrowsingHandler = (Com2ExtendedBrowsingHandler)Activator.CreateInstance(Com2Properties.extendedInterfaceHandlerTypes[i]);
						handlers[type] = com2ExtendedBrowsingHandler;
					}
					if (!type.IsAssignableFrom(com2ExtendedBrowsingHandler.Interface))
					{
						throw new ArgumentException(SR.GetString("COM2BadHandlerType", new object[]
						{
							type.Name,
							com2ExtendedBrowsingHandler.Interface.Name
						}));
					}
					com2ExtendedBrowsingHandler.SetupPropertyHandlers(this.props);
				}
			}
		}

		// Token: 0x0600640E RID: 25614 RVA: 0x0016DA9E File Offset: 0x0016CA9E
		public void Dispose()
		{
			if (this.props != null)
			{
				if (this.Disposed != null)
				{
					this.Disposed(this, EventArgs.Empty);
				}
				this.weakObjRef = null;
				this.props = null;
				this.touchedTime = 0L;
			}
		}

		// Token: 0x0600640F RID: 25615 RVA: 0x0016DAD7 File Offset: 0x0016CAD7
		public bool CheckValid()
		{
			return this.CheckValid(false);
		}

		// Token: 0x06006410 RID: 25616 RVA: 0x0016DAE0 File Offset: 0x0016CAE0
		public bool CheckValid(bool checkVersions)
		{
			return this.CheckValid(checkVersions, true);
		}

		// Token: 0x06006411 RID: 25617 RVA: 0x0016DAEC File Offset: 0x0016CAEC
		internal bool CheckValid(bool checkVersions, bool callDispose)
		{
			if (this.AlwaysValid)
			{
				return true;
			}
			bool flag = this.weakObjRef != null && this.weakObjRef.IsAlive;
			if (flag && checkVersions)
			{
				long[] array = this.GetTypeInfoVersions(this.weakObjRef.Target);
				if (array.Length != this.typeInfoVersions.Length)
				{
					flag = false;
				}
				else
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != this.typeInfoVersions[i])
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					this.typeInfoVersions = array;
				}
			}
			if (!flag && callDispose)
			{
				this.Dispose();
			}
			return flag;
		}

		// Token: 0x06006412 RID: 25618 RVA: 0x0016DB7C File Offset: 0x0016CB7C
		private long[] GetTypeInfoVersions(object comObject)
		{
			UnsafeNativeMethods.ITypeInfo[] array = Com2TypeInfoProcessor.FindTypeInfos(comObject, false);
			long[] array2 = new long[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = this.GetTypeInfoVersion(array[i]);
			}
			return array2;
		}

		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x06006413 RID: 25619 RVA: 0x0016DBB5 File Offset: 0x0016CBB5
		private static int CountMemberOffset
		{
			get
			{
				if (Com2Properties.countOffset == -1)
				{
					Com2Properties.countOffset = Marshal.SizeOf(typeof(Guid)) + IntPtr.Size + 24;
				}
				return Com2Properties.countOffset;
			}
		}

		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x06006414 RID: 25620 RVA: 0x0016DBE1 File Offset: 0x0016CBE1
		private static int VersionOffset
		{
			get
			{
				if (Com2Properties.versionOffset == -1)
				{
					Com2Properties.versionOffset = Com2Properties.CountMemberOffset + 12;
				}
				return Com2Properties.versionOffset;
			}
		}

		// Token: 0x06006415 RID: 25621 RVA: 0x0016DC00 File Offset: 0x0016CC00
		private unsafe long GetTypeInfoVersion(UnsafeNativeMethods.ITypeInfo pTypeInfo)
		{
			IntPtr zero = IntPtr.Zero;
			int typeAttr = pTypeInfo.GetTypeAttr(ref zero);
			if (!NativeMethods.Succeeded(typeAttr))
			{
				return 0L;
			}
			long result;
			try
			{
				System.Runtime.InteropServices.ComTypes.TYPEATTR typeattr;
				try
				{
					typeattr = *(System.Runtime.InteropServices.ComTypes.TYPEATTR*)((void*)zero);
				}
				catch
				{
					return 0L;
				}
				long num = 0L;
				int* ptr = (int*)(&num);
				byte* ptr2 = (byte*)(&typeattr);
				*ptr = *(int*)(ptr2 + Com2Properties.CountMemberOffset);
				ptr++;
				*ptr = *(int*)(ptr2 + Com2Properties.VersionOffset);
				result = num;
			}
			finally
			{
				pTypeInfo.ReleaseTypeAttr(zero);
			}
			return result;
		}

		// Token: 0x06006416 RID: 25622 RVA: 0x0016DC94 File Offset: 0x0016CC94
		internal void SetProps(Com2PropertyDescriptor[] props)
		{
			this.props = props;
			if (props != null)
			{
				for (int i = 0; i < props.Length; i++)
				{
					props[i].PropertyManager = this;
				}
			}
		}

		// Token: 0x04003B92 RID: 15250
		private static TraceSwitch DbgCom2PropertiesSwitch = new TraceSwitch("DbgCom2Properties", "Com2Properties: debug Com2 properties manager");

		// Token: 0x04003B93 RID: 15251
		private static long AGE_THRESHHOLD = (long)((ulong)-1294967296);

		// Token: 0x04003B94 RID: 15252
		internal WeakReference weakObjRef;

		// Token: 0x04003B95 RID: 15253
		private Com2PropertyDescriptor[] props;

		// Token: 0x04003B96 RID: 15254
		private int defaultIndex = -1;

		// Token: 0x04003B97 RID: 15255
		private long touchedTime;

		// Token: 0x04003B98 RID: 15256
		private long[] typeInfoVersions;

		// Token: 0x04003B99 RID: 15257
		private int alwaysValid;

		// Token: 0x04003B9A RID: 15258
		private static Type[] extendedInterfaces = new Type[]
		{
			typeof(NativeMethods.ICategorizeProperties),
			typeof(NativeMethods.IProvidePropertyBuilder),
			typeof(NativeMethods.IPerPropertyBrowsing),
			typeof(NativeMethods.IVsPerPropertyBrowsing),
			typeof(NativeMethods.IManagedPerPropertyBrowsing)
		};

		// Token: 0x04003B9B RID: 15259
		private static Type[] extendedInterfaceHandlerTypes = new Type[]
		{
			typeof(Com2ICategorizePropertiesHandler),
			typeof(Com2IProvidePropertyBuilderHandler),
			typeof(Com2IPerPropertyBrowsingHandler),
			typeof(Com2IVsPerPropertyBrowsingHandler),
			typeof(Com2IManagedPerPropertyBrowsingHandler)
		};

		// Token: 0x04003B9D RID: 15261
		private static int countOffset = -1;

		// Token: 0x04003B9E RID: 15262
		private static int versionOffset = -1;
	}
}
