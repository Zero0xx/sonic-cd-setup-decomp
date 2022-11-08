using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000F1 RID: 241
	public sealed class Transforms
	{
		// Token: 0x060004D2 RID: 1234 RVA: 0x00061F64 File Offset: 0x00061364
		public unsafe override string ToString()
		{
			Type type = base.GetType();
			StringBuilder stringBuilder = new StringBuilder();
			PropertyInfo[] properties = type.GetProperties();
			int num = 0;
			if (0 < properties.Length)
			{
				do
				{
					MethodInfo getMethod = properties[num].GetGetMethod();
					if (getMethod != null)
					{
						object obj = getMethod.Invoke(this, null);
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$125$];
						array[0] = properties[num].Name;
						string text;
						if (obj != null)
						{
							text = obj.ToString();
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
			return stringBuilder.ToString();
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00073010 File Offset: 0x00072410
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00073030 File Offset: 0x00072430
		public Matrix View
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.View);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.View, value);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00073054 File Offset: 0x00072454
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00073074 File Offset: 0x00072474
		public Matrix Projection
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Projection);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Projection, value);
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00073098 File Offset: 0x00072498
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x000730B8 File Offset: 0x000724B8
		public Matrix Texture0
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture0);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture0, value);
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x000730DC File Offset: 0x000724DC
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x000730FC File Offset: 0x000724FC
		public Matrix Texture1
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture1);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture1, value);
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00073120 File Offset: 0x00072520
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00073140 File Offset: 0x00072540
		public Matrix Texture2
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture2);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture2, value);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00073164 File Offset: 0x00072564
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x00073184 File Offset: 0x00072584
		public Matrix Texture3
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture3);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture3, value);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000731A8 File Offset: 0x000725A8
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x000731C8 File Offset: 0x000725C8
		public Matrix Texture4
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture4);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture4, value);
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x000731EC File Offset: 0x000725EC
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x0007320C File Offset: 0x0007260C
		public Matrix Texture5
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture5);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture5, value);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060004E3 RID: 1251 RVA: 0x00073230 File Offset: 0x00072630
		// (set) Token: 0x060004E4 RID: 1252 RVA: 0x00073250 File Offset: 0x00072650
		public Matrix Texture6
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture6);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture6, value);
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00073274 File Offset: 0x00072674
		// (set) Token: 0x060004E6 RID: 1254 RVA: 0x00073294 File Offset: 0x00072694
		public Matrix Texture7
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.Texture7);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.Texture7, value);
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x000732B8 File Offset: 0x000726B8
		// (set) Token: 0x060004E8 RID: 1256 RVA: 0x000732DC File Offset: 0x000726DC
		public Matrix World
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.World);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.World, value);
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00073304 File Offset: 0x00072704
		// (set) Token: 0x060004EA RID: 1258 RVA: 0x00073328 File Offset: 0x00072728
		public Matrix World1
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.World1);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.World1, value);
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00073350 File Offset: 0x00072750
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x00073374 File Offset: 0x00072774
		public Matrix World2
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.World2);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.World2, value);
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0007339C File Offset: 0x0007279C
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x000733C0 File Offset: 0x000727C0
		public Matrix World3
		{
			get
			{
				return this.pDevice.GetTransform(TransformType.World3);
			}
			set
			{
				this.pDevice.SetTransform(TransformType.World3, value);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x000733E8 File Offset: 0x000727E8
		public Matrix GetWorldMatrixByIndex(int index)
		{
			return this.pDevice.GetTransform(index + TransformType.World);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0007340C File Offset: 0x0007280C
		public void SetWorldMatrixByIndex(int index, Matrix value)
		{
			this.pDevice.SetTransform(index + TransformType.World, value);
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00072FF0 File Offset: 0x000723F0
		internal Transforms(Device dev)
		{
			this.pDevice = dev;
		}

		// Token: 0x0400101B RID: 4123
		internal Device pDevice;
	}
}
