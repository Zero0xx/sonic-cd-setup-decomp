using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x02000088 RID: 136
	public sealed class CustomVertex
	{
		// Token: 0x06000191 RID: 401 RVA: 0x0005A3B8 File Offset: 0x000597B8
		private CustomVertex()
		{
		}

		// Token: 0x02000089 RID: 137
		[MiscellaneousBits(1)]
		public struct Transformed
		{
			// Token: 0x06000192 RID: 402 RVA: 0x0005A3D0 File Offset: 0x000597D0
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$37$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$38$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000CD RID: 205
			// (get) Token: 0x06000193 RID: 403 RVA: 0x0005A4FC File Offset: 0x000598FC
			// (set) Token: 0x06000194 RID: 404 RVA: 0x0005A538 File Offset: 0x00059938
			public Vector4 Position
			{
				get
				{
					Vector4 result = default(Vector4);
					result = new Vector4(this.X, this.Y, this.Z, this.Rhw);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
					this.Rhw = value.W;
				}
			}

			// Token: 0x06000195 RID: 405 RVA: 0x0005A5B0 File Offset: 0x000599B0
			public Transformed(Vector4 value)
			{
				this.Position = value;
			}

			// Token: 0x06000196 RID: 406 RVA: 0x0005A580 File Offset: 0x00059980
			public Transformed(float xvalue, float yvalue, float zvalue, float rhwvalue)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Rhw = rhwvalue;
			}

			// Token: 0x170000CC RID: 204
			// (get) Token: 0x06000197 RID: 407 RVA: 0x0005A5CC File Offset: 0x000599CC
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.TransformedTextured);
				}
			}

			// Token: 0x04000E78 RID: 3704
			public float X;

			// Token: 0x04000E79 RID: 3705
			public float Y;

			// Token: 0x04000E7A RID: 3706
			public float Z;

			// Token: 0x04000E7B RID: 3707
			public float Rhw;

			// Token: 0x04000E7C RID: 3708
			public const VertexFormats Format = VertexFormats.Transformed;
		}

		// Token: 0x0200008A RID: 138
		[MiscellaneousBits(1)]
		public struct TransformedTextured
		{
			// Token: 0x06000198 RID: 408 RVA: 0x0005A5E4 File Offset: 0x000599E4
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$39$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$40$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000CF RID: 207
			// (get) Token: 0x06000199 RID: 409 RVA: 0x0005A710 File Offset: 0x00059B10
			// (set) Token: 0x0600019A RID: 410 RVA: 0x0005A74C File Offset: 0x00059B4C
			public Vector4 Position
			{
				get
				{
					Vector4 result = default(Vector4);
					result = new Vector4(this.X, this.Y, this.Z, this.Rhw);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
					this.Rhw = value.W;
				}
			}

			// Token: 0x0600019B RID: 411 RVA: 0x0005A7D4 File Offset: 0x00059BD4
			public TransformedTextured(Vector4 value, float u, float v)
			{
				this.Position = value;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x0600019C RID: 412 RVA: 0x0005A794 File Offset: 0x00059B94
			public TransformedTextured(float xvalue, float yvalue, float zvalue, float rhwvalue, float u, float v)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Rhw = rhwvalue;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x170000CE RID: 206
			// (get) Token: 0x0600019D RID: 413 RVA: 0x0005A800 File Offset: 0x00059C00
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.TransformedTextured);
				}
			}

			// Token: 0x04000E7D RID: 3709
			public float X;

			// Token: 0x04000E7E RID: 3710
			public float Y;

			// Token: 0x04000E7F RID: 3711
			public float Z;

			// Token: 0x04000E80 RID: 3712
			public float Rhw;

			// Token: 0x04000E81 RID: 3713
			public float Tu;

			// Token: 0x04000E82 RID: 3714
			public float Tv;

			// Token: 0x04000E83 RID: 3715
			public const VertexFormats Format = VertexFormats.Texture1 | VertexFormats.Transformed;
		}

		// Token: 0x0200008B RID: 139
		[MiscellaneousBits(1)]
		public struct TransformedColored
		{
			// Token: 0x0600019E RID: 414 RVA: 0x0005A818 File Offset: 0x00059C18
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$41$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$42$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x0600019F RID: 415 RVA: 0x0005A944 File Offset: 0x00059D44
			// (set) Token: 0x060001A0 RID: 416 RVA: 0x0005A980 File Offset: 0x00059D80
			public Vector4 Position
			{
				get
				{
					Vector4 result = default(Vector4);
					result = new Vector4(this.X, this.Y, this.Z, this.Rhw);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
					this.Rhw = value.W;
				}
			}

			// Token: 0x060001A1 RID: 417 RVA: 0x0005AA00 File Offset: 0x00059E00
			public TransformedColored(Vector4 value, int c)
			{
				this.Position = value;
				this.Color = c;
			}

			// Token: 0x060001A2 RID: 418 RVA: 0x0005A9C8 File Offset: 0x00059DC8
			public TransformedColored(float xvalue, float yvalue, float zvalue, float rhwvalue, int c)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Rhw = rhwvalue;
				this.Color = c;
			}

			// Token: 0x170000D0 RID: 208
			// (get) Token: 0x060001A3 RID: 419 RVA: 0x0005AA24 File Offset: 0x00059E24
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.TransformedColored);
				}
			}

			// Token: 0x04000E84 RID: 3716
			public float X;

			// Token: 0x04000E85 RID: 3717
			public float Y;

			// Token: 0x04000E86 RID: 3718
			public float Z;

			// Token: 0x04000E87 RID: 3719
			public float Rhw;

			// Token: 0x04000E88 RID: 3720
			public int Color;

			// Token: 0x04000E89 RID: 3721
			public const VertexFormats Format = VertexFormats.Diffuse | VertexFormats.Transformed;
		}

		// Token: 0x0200008C RID: 140
		[MiscellaneousBits(1)]
		public struct TransformedColoredTextured
		{
			// Token: 0x060001A4 RID: 420 RVA: 0x0005AA3C File Offset: 0x00059E3C
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$43$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$44$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x060001A5 RID: 421 RVA: 0x0005AB68 File Offset: 0x00059F68
			// (set) Token: 0x060001A6 RID: 422 RVA: 0x0005ABA4 File Offset: 0x00059FA4
			public Vector4 Position
			{
				get
				{
					Vector4 result = default(Vector4);
					result = new Vector4(this.X, this.Y, this.Z, this.Rhw);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
					this.Rhw = value.W;
				}
			}

			// Token: 0x060001A7 RID: 423 RVA: 0x0005AC34 File Offset: 0x0005A034
			public TransformedColoredTextured(Vector4 value, int c, float u, float v)
			{
				this.Position = value;
				this.Tu = u;
				this.Tv = v;
				this.Color = c;
			}

			// Token: 0x060001A8 RID: 424 RVA: 0x0005ABEC File Offset: 0x00059FEC
			public TransformedColoredTextured(float xvalue, float yvalue, float zvalue, float rhwvalue, int c, float u, float v)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Rhw = rhwvalue;
				this.Tu = u;
				this.Tv = v;
				this.Color = c;
			}

			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x060001A9 RID: 425 RVA: 0x0005AC68 File Offset: 0x0005A068
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.TransformedColoredTextured);
				}
			}

			// Token: 0x04000E8A RID: 3722
			public float X;

			// Token: 0x04000E8B RID: 3723
			public float Y;

			// Token: 0x04000E8C RID: 3724
			public float Z;

			// Token: 0x04000E8D RID: 3725
			public float Rhw;

			// Token: 0x04000E8E RID: 3726
			public int Color;

			// Token: 0x04000E8F RID: 3727
			public float Tu;

			// Token: 0x04000E90 RID: 3728
			public float Tv;

			// Token: 0x04000E91 RID: 3729
			public const VertexFormats Format = VertexFormats.Texture1 | VertexFormats.Diffuse | VertexFormats.Transformed;
		}

		// Token: 0x0200008D RID: 141
		[MiscellaneousBits(1)]
		public struct PositionOnly
		{
			// Token: 0x060001AA RID: 426 RVA: 0x0005AC80 File Offset: 0x0005A080
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$45$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$46$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x060001AB RID: 427 RVA: 0x0005ADAC File Offset: 0x0005A1AC
			// (set) Token: 0x060001AC RID: 428 RVA: 0x0005ADE0 File Offset: 0x0005A1E0
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x060001AD RID: 429 RVA: 0x0005AE44 File Offset: 0x0005A244
			public PositionOnly(Vector3 value)
			{
				this.Position = value;
			}

			// Token: 0x060001AE RID: 430 RVA: 0x0005AE1C File Offset: 0x0005A21C
			public PositionOnly(float xvalue, float yvalue, float zvalue)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
			}

			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x060001AF RID: 431 RVA: 0x0005AE60 File Offset: 0x0005A260
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionOnly);
				}
			}

			// Token: 0x04000E92 RID: 3730
			public float X;

			// Token: 0x04000E93 RID: 3731
			public float Y;

			// Token: 0x04000E94 RID: 3732
			public float Z;

			// Token: 0x04000E95 RID: 3733
			public const VertexFormats Format = VertexFormats.Position;
		}

		// Token: 0x0200008E RID: 142
		[MiscellaneousBits(1)]
		public struct PositionNormal
		{
			// Token: 0x060001B0 RID: 432 RVA: 0x0005AE78 File Offset: 0x0005A278
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$47$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$48$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000D8 RID: 216
			// (get) Token: 0x060001B1 RID: 433 RVA: 0x0005AFA4 File Offset: 0x0005A3A4
			// (set) Token: 0x060001B2 RID: 434 RVA: 0x0005AFD8 File Offset: 0x0005A3D8
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x170000D7 RID: 215
			// (get) Token: 0x060001B3 RID: 435 RVA: 0x0005B014 File Offset: 0x0005A414
			// (set) Token: 0x060001B4 RID: 436 RVA: 0x0005B048 File Offset: 0x0005A448
			public Vector3 Normal
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.Nx, this.Ny, this.Nz);
					return result;
				}
				set
				{
					this.Nx = value.X;
					this.Ny = value.Y;
					this.Nz = value.Z;
				}
			}

			// Token: 0x060001B5 RID: 437 RVA: 0x0005B0C4 File Offset: 0x0005A4C4
			public PositionNormal(Vector3 pos, Vector3 nor)
			{
				this.Position = pos;
				this.Normal = nor;
			}

			// Token: 0x060001B6 RID: 438 RVA: 0x0005B084 File Offset: 0x0005A484
			public PositionNormal(float xvalue, float yvalue, float zvalue, float nxvalue, float nyvalue, float nzvalue)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Nx = nxvalue;
				this.Ny = nyvalue;
				this.Nz = nzvalue;
			}

			// Token: 0x170000D6 RID: 214
			// (get) Token: 0x060001B7 RID: 439 RVA: 0x0005B0E8 File Offset: 0x0005A4E8
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionNormal);
				}
			}

			// Token: 0x04000E96 RID: 3734
			public float X;

			// Token: 0x04000E97 RID: 3735
			public float Y;

			// Token: 0x04000E98 RID: 3736
			public float Z;

			// Token: 0x04000E99 RID: 3737
			public float Nx;

			// Token: 0x04000E9A RID: 3738
			public float Ny;

			// Token: 0x04000E9B RID: 3739
			public float Nz;

			// Token: 0x04000E9C RID: 3740
			public const VertexFormats Format = VertexFormats.PositionNormal;
		}

		// Token: 0x0200008F RID: 143
		[MiscellaneousBits(1)]
		public struct PositionColored
		{
			// Token: 0x060001B8 RID: 440 RVA: 0x0005B100 File Offset: 0x0005A500
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$49$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$50$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x060001B9 RID: 441 RVA: 0x0005B22C File Offset: 0x0005A62C
			// (set) Token: 0x060001BA RID: 442 RVA: 0x0005B260 File Offset: 0x0005A660
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x060001BB RID: 443 RVA: 0x0005B2CC File Offset: 0x0005A6CC
			public PositionColored(Vector3 value, int c)
			{
				this.Position = value;
				this.Color = c;
			}

			// Token: 0x060001BC RID: 444 RVA: 0x0005B29C File Offset: 0x0005A69C
			public PositionColored(float xvalue, float yvalue, float zvalue, int c)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Color = c;
			}

			// Token: 0x170000D9 RID: 217
			// (get) Token: 0x060001BD RID: 445 RVA: 0x0005B2F0 File Offset: 0x0005A6F0
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionColored);
				}
			}

			// Token: 0x04000E9D RID: 3741
			public float X;

			// Token: 0x04000E9E RID: 3742
			public float Y;

			// Token: 0x04000E9F RID: 3743
			public float Z;

			// Token: 0x04000EA0 RID: 3744
			public int Color;

			// Token: 0x04000EA1 RID: 3745
			public const VertexFormats Format = VertexFormats.Diffuse | VertexFormats.Position;
		}

		// Token: 0x02000090 RID: 144
		[MiscellaneousBits(1)]
		public struct PositionNormalColored
		{
			// Token: 0x060001BE RID: 446 RVA: 0x0005B308 File Offset: 0x0005A708
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$51$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$52$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x060001BF RID: 447 RVA: 0x0005B434 File Offset: 0x0005A834
			// (set) Token: 0x060001C0 RID: 448 RVA: 0x0005B468 File Offset: 0x0005A868
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x060001C1 RID: 449 RVA: 0x0005B4A4 File Offset: 0x0005A8A4
			// (set) Token: 0x060001C2 RID: 450 RVA: 0x0005B4D8 File Offset: 0x0005A8D8
			public Vector3 Normal
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.Nx, this.Ny, this.Nz);
					return result;
				}
				set
				{
					this.Nx = value.X;
					this.Ny = value.Y;
					this.Nz = value.Z;
				}
			}

			// Token: 0x060001C3 RID: 451 RVA: 0x0005B55C File Offset: 0x0005A95C
			public PositionNormalColored(Vector3 pos, Vector3 nor, int c)
			{
				this.Position = pos;
				this.Normal = nor;
				this.Color = c;
			}

			// Token: 0x060001C4 RID: 452 RVA: 0x0005B514 File Offset: 0x0005A914
			public PositionNormalColored(float xvalue, float yvalue, float zvalue, float nxvalue, float nyvalue, float nzvalue, int c)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Nx = nxvalue;
				this.Ny = nyvalue;
				this.Nz = nzvalue;
				this.Color = c;
			}

			// Token: 0x170000DB RID: 219
			// (get) Token: 0x060001C5 RID: 453 RVA: 0x0005B588 File Offset: 0x0005A988
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionNormalColored);
				}
			}

			// Token: 0x04000EA2 RID: 3746
			public float X;

			// Token: 0x04000EA3 RID: 3747
			public float Y;

			// Token: 0x04000EA4 RID: 3748
			public float Z;

			// Token: 0x04000EA5 RID: 3749
			public float Nx;

			// Token: 0x04000EA6 RID: 3750
			public float Ny;

			// Token: 0x04000EA7 RID: 3751
			public float Nz;

			// Token: 0x04000EA8 RID: 3752
			public int Color;

			// Token: 0x04000EA9 RID: 3753
			public const VertexFormats Format = VertexFormats.PositionNormal | VertexFormats.Diffuse;
		}

		// Token: 0x02000091 RID: 145
		[MiscellaneousBits(1)]
		public struct PositionNormalTextured
		{
			// Token: 0x060001C6 RID: 454 RVA: 0x0005B5A0 File Offset: 0x0005A9A0
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$53$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$54$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000E0 RID: 224
			// (get) Token: 0x060001C7 RID: 455 RVA: 0x0005B6CC File Offset: 0x0005AACC
			// (set) Token: 0x060001C8 RID: 456 RVA: 0x0005B700 File Offset: 0x0005AB00
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x170000DF RID: 223
			// (get) Token: 0x060001C9 RID: 457 RVA: 0x0005B73C File Offset: 0x0005AB3C
			// (set) Token: 0x060001CA RID: 458 RVA: 0x0005B770 File Offset: 0x0005AB70
			public Vector3 Normal
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.Nx, this.Ny, this.Nz);
					return result;
				}
				set
				{
					this.Nx = value.X;
					this.Ny = value.Y;
					this.Nz = value.Z;
				}
			}

			// Token: 0x060001CB RID: 459 RVA: 0x0005B7FC File Offset: 0x0005ABFC
			public PositionNormalTextured(Vector3 pos, Vector3 nor, float u, float v)
			{
				this.Position = pos;
				this.Normal = nor;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x060001CC RID: 460 RVA: 0x0005B7AC File Offset: 0x0005ABAC
			public PositionNormalTextured(float xvalue, float yvalue, float zvalue, float nxvalue, float nyvalue, float nzvalue, float u, float v)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Nx = nxvalue;
				this.Ny = nyvalue;
				this.Nz = nzvalue;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x060001CD RID: 461 RVA: 0x0005B830 File Offset: 0x0005AC30
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionNormalTextured);
				}
			}

			// Token: 0x04000EAA RID: 3754
			public float X;

			// Token: 0x04000EAB RID: 3755
			public float Y;

			// Token: 0x04000EAC RID: 3756
			public float Z;

			// Token: 0x04000EAD RID: 3757
			public float Nx;

			// Token: 0x04000EAE RID: 3758
			public float Ny;

			// Token: 0x04000EAF RID: 3759
			public float Nz;

			// Token: 0x04000EB0 RID: 3760
			public float Tu;

			// Token: 0x04000EB1 RID: 3761
			public float Tv;

			// Token: 0x04000EB2 RID: 3762
			public const VertexFormats Format = VertexFormats.PositionNormal | VertexFormats.Texture1;
		}

		// Token: 0x02000092 RID: 146
		[MiscellaneousBits(1)]
		public struct PositionTextured
		{
			// Token: 0x060001CE RID: 462 RVA: 0x0005B848 File Offset: 0x0005AC48
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$55$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$56$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000E2 RID: 226
			// (get) Token: 0x060001CF RID: 463 RVA: 0x0005B974 File Offset: 0x0005AD74
			// (set) Token: 0x060001D0 RID: 464 RVA: 0x0005B9A8 File Offset: 0x0005ADA8
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x060001D1 RID: 465 RVA: 0x0005BA1C File Offset: 0x0005AE1C
			public PositionTextured(Vector3 pos, float u, float v)
			{
				this.Position = pos;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x060001D2 RID: 466 RVA: 0x0005B9E4 File Offset: 0x0005ADE4
			public PositionTextured(float xvalue, float yvalue, float zvalue, float u, float v)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x170000E1 RID: 225
			// (get) Token: 0x060001D3 RID: 467 RVA: 0x0005BA48 File Offset: 0x0005AE48
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionTextured);
				}
			}

			// Token: 0x04000EB3 RID: 3763
			public float X;

			// Token: 0x04000EB4 RID: 3764
			public float Y;

			// Token: 0x04000EB5 RID: 3765
			public float Z;

			// Token: 0x04000EB6 RID: 3766
			public float Tu;

			// Token: 0x04000EB7 RID: 3767
			public float Tv;

			// Token: 0x04000EB8 RID: 3768
			public const VertexFormats Format = VertexFormats.Texture1 | VertexFormats.Position;
		}

		// Token: 0x02000093 RID: 147
		[MiscellaneousBits(1)]
		public struct PositionColoredTextured
		{
			// Token: 0x060001D4 RID: 468 RVA: 0x0005BA60 File Offset: 0x0005AE60
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
							string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$57$];
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
						string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$58$];
						array2[0] = fields[num2].Name;
						array2[1] = value.ToString();
						stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
						num2++;
					}
					while (num2 < fields.Length);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x170000E4 RID: 228
			// (get) Token: 0x060001D5 RID: 469 RVA: 0x0005BB8C File Offset: 0x0005AF8C
			// (set) Token: 0x060001D6 RID: 470 RVA: 0x0005BBC0 File Offset: 0x0005AFC0
			public Vector3 Position
			{
				get
				{
					Vector3 result = default(Vector3);
					result = new Vector3(this.X, this.Y, this.Z);
					return result;
				}
				set
				{
					this.X = value.X;
					this.Y = value.Y;
					this.Z = value.Z;
				}
			}

			// Token: 0x060001D7 RID: 471 RVA: 0x0005BC3C File Offset: 0x0005B03C
			public PositionColoredTextured(Vector3 value, int c, float u, float v)
			{
				this.Position = value;
				this.Color = c;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x060001D8 RID: 472 RVA: 0x0005BBFC File Offset: 0x0005AFFC
			public PositionColoredTextured(float xvalue, float yvalue, float zvalue, int c, float u, float v)
			{
				this.X = xvalue;
				this.Y = yvalue;
				this.Z = zvalue;
				this.Color = c;
				this.Tu = u;
				this.Tv = v;
			}

			// Token: 0x170000E3 RID: 227
			// (get) Token: 0x060001D9 RID: 473 RVA: 0x0005BC70 File Offset: 0x0005B070
			public static int StrideSize
			{
				get
				{
					return sizeof(CustomVertex.PositionColoredTextured);
				}
			}

			// Token: 0x04000EB9 RID: 3769
			public float X;

			// Token: 0x04000EBA RID: 3770
			public float Y;

			// Token: 0x04000EBB RID: 3771
			public float Z;

			// Token: 0x04000EBC RID: 3772
			public int Color;

			// Token: 0x04000EBD RID: 3773
			public float Tu;

			// Token: 0x04000EBE RID: 3774
			public float Tv;

			// Token: 0x04000EBF RID: 3775
			public const VertexFormats Format = VertexFormats.Texture1 | VertexFormats.Diffuse | VertexFormats.Position;
		}
	}
}
