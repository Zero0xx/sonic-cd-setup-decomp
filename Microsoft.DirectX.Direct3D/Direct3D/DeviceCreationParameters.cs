using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000B1 RID: 177
	[MiscellaneousBits(1)]
	public struct DeviceCreationParameters
	{
		// Token: 0x06000282 RID: 642 RVA: 0x0005D9C8 File Offset: 0x0005CDC8
		public unsafe override string ToString()
		{
			object obj = this;
			Type type = obj.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null && !getMethod.IsStatic)
					{
						object obj2 = getMethod.Invoke(obj, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$72$];
						array[0] = properties[num].Name;
						string text;
						if (obj2 != null)
						{
							text = obj2.ToString();
						}
						else
						{
							text = string.Empty;
						}
						array[1] = text;
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array));
					}
					num++;
				}
				while (num < properties.Length);
			}
			FieldInfo[] fields = type.GetFields();
			int num2 = 0;
			if (0 < fields.Length)
			{
				do
				{
					object value = fields[num2].GetValue(obj);
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$73$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0005DAF4 File Offset: 0x0005CEF4
		public int AdapterOrdinal
		{
			get
			{
				return this.m_AdapterOrdinal;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0005DB0C File Offset: 0x0005CF0C
		public DeviceType DeviceType
		{
			get
			{
				return this.m_DeviceType;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0005DB24 File Offset: 0x0005CF24
		public Control FocusWindow
		{
			get
			{
				return Control.FromHandle(this.m_hFocusWindow);
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0005DB44 File Offset: 0x0005CF44
		public IntPtr FocusWindowHandle
		{
			get
			{
				return this.m_hFocusWindow;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0005DB5C File Offset: 0x0005CF5C
		public BehaviorFlags Behavior
		{
			get
			{
				BehaviorFlags result = default(BehaviorFlags);
				result = new BehaviorFlags((CreateFlags)this.m_BehaviorFlags);
				return result;
			}
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0005DB84 File Offset: 0x0005CF84
		public DeviceCreationParameters()
		{
			ref DeviceCreationParameters deviceCreationParameters& = ref this;
			initblk(ref deviceCreationParameters&, 0, 16);
		}

		// Token: 0x04000EF5 RID: 3829
		private int m_AdapterOrdinal;

		// Token: 0x04000EF6 RID: 3830
		private DeviceType m_DeviceType;

		// Token: 0x04000EF7 RID: 3831
		private IntPtr m_hFocusWindow;

		// Token: 0x04000EF8 RID: 3832
		private int m_BehaviorFlags;
	}
}
