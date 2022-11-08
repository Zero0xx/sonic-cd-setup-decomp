using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000CE RID: 206
	[MiscellaneousBits(1)]
	public struct VertexElement
	{
		// Token: 0x0600034B RID: 843 RVA: 0x00060668 File Offset: 0x0005FA68
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$98$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$99$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00060794 File Offset: 0x0005FB94
		// (set) Token: 0x0600034D RID: 845 RVA: 0x000607B0 File Offset: 0x0005FBB0
		public short Stream
		{
			get
			{
				return (short)this.m_Internal_Stream;
			}
			set
			{
				this.m_Internal_Stream = (ushort)value;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600034E RID: 846 RVA: 0x000607CC File Offset: 0x0005FBCC
		// (set) Token: 0x0600034F RID: 847 RVA: 0x000607E8 File Offset: 0x0005FBE8
		public short Offset
		{
			get
			{
				return (short)this.m_Internal_Offset;
			}
			set
			{
				this.m_Internal_Offset = (ushort)value;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00060804 File Offset: 0x0005FC04
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0006081C File Offset: 0x0005FC1C
		public DeclarationType DeclarationType
		{
			get
			{
				return (DeclarationType)this.m_Internal_Type;
			}
			set
			{
				this.m_Internal_Type = (byte)value;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00060838 File Offset: 0x0005FC38
		// (set) Token: 0x06000353 RID: 851 RVA: 0x00060850 File Offset: 0x0005FC50
		public DeclarationMethod DeclarationMethod
		{
			get
			{
				return (DeclarationMethod)this.m_Internal_Method;
			}
			set
			{
				this.m_Internal_Method = (byte)value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0006086C File Offset: 0x0005FC6C
		// (set) Token: 0x06000355 RID: 853 RVA: 0x00060884 File Offset: 0x0005FC84
		public DeclarationUsage DeclarationUsage
		{
			get
			{
				return (DeclarationUsage)this.m_Internal_Usage;
			}
			set
			{
				this.m_Internal_Usage = (byte)value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000356 RID: 854 RVA: 0x000608A0 File Offset: 0x0005FCA0
		// (set) Token: 0x06000357 RID: 855 RVA: 0x000608B8 File Offset: 0x0005FCB8
		public byte UsageIndex
		{
			get
			{
				return this.m_Internal_UsageIndex;
			}
			set
			{
				this.m_Internal_UsageIndex = value;
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000608D4 File Offset: 0x0005FCD4
		public VertexElement(short stream, short offset, DeclarationType declType, DeclarationMethod declMethod, DeclarationUsage declUsage, byte usageIndex)
		{
			this.Stream = stream;
			this.Offset = offset;
			this.DeclarationType = declType;
			this.DeclarationMethod = declMethod;
			this.DeclarationUsage = declUsage;
			this.UsageIndex = usageIndex;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00060914 File Offset: 0x0005FD14
		static VertexElement()
		{
			VertexElement vertexDeclarationEnd = default(VertexElement);
			vertexDeclarationEnd = new VertexElement(255, 0, DeclarationType.Unused, DeclarationMethod.Default, DeclarationUsage.Position, 0);
			VertexElement.VertexDeclarationEnd = vertexDeclarationEnd;
		}

		// Token: 0x04000F49 RID: 3913
		internal ushort m_Internal_Stream;

		// Token: 0x04000F4A RID: 3914
		internal ushort m_Internal_Offset;

		// Token: 0x04000F4B RID: 3915
		internal byte m_Internal_Type;

		// Token: 0x04000F4C RID: 3916
		internal byte m_Internal_Method;

		// Token: 0x04000F4D RID: 3917
		internal byte m_Internal_Usage;

		// Token: 0x04000F4E RID: 3918
		internal byte m_Internal_UsageIndex;

		// Token: 0x04000F4F RID: 3919
		public static readonly VertexElement VertexDeclarationEnd;
	}
}
