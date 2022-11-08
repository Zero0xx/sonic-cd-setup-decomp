using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using Microsoft.DirectX.PrivateImplementationDetails;
using Microsoft.VisualC;

namespace Microsoft.DirectX.Direct3D
{
	// Token: 0x020000AC RID: 172
	[MiscellaneousBits(1)]
	public struct Caps
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0005CCE8 File Offset: 0x0005C0E8
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
						string[] array = new string[<Module>.$ConstGCArrayBound$0x092d518d$68$];
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
					string[] array2 = new string[<Module>.$ConstGCArrayBound$0x092d518d$69$];
					array2[0] = fields[num2].Name;
					array2[1] = value.ToString();
					stringBuilder.Append(string.Format(CultureInfo.CurrentUICulture, new string((sbyte*)(&<Module>.??_C@_09GAFEAPMM@?$HL0?$HN?3?5?$HL1?$HN?6?$AA@)), array2));
					num2++;
				}
				while (num2 < fields.Length);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0005CE14 File Offset: 0x0005C214
		public unsafe float MaxNPatchTessellationLevel
		{
			get
			{
				return *(ref this.m_Internal + 216);
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x0600023A RID: 570 RVA: 0x0005CE34 File Offset: 0x0005C234
		public DeviceType DeviceType
		{
			get
			{
				return this.m_Internal;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0005CE50 File Offset: 0x0005C250
		public unsafe int AdapterOrdinal
		{
			get
			{
				return *(ref this.m_Internal + 4);
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0005CE6C File Offset: 0x0005C26C
		public unsafe DriverCaps DriverCaps
		{
			get
			{
				DriverCaps result = default(DriverCaps);
				result = new DriverCaps(*(ref this.m_Internal + 8), *(ref this.m_Internal + 12), *(ref this.m_Internal + 16));
				return result;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0005CEAC File Offset: 0x0005C2AC
		public unsafe PresentInterval PresentationIntervals
		{
			get
			{
				return (PresentInterval)(*(ref this.m_Internal + 20));
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0005CEC8 File Offset: 0x0005C2C8
		public unsafe CursorCaps CursorCaps
		{
			get
			{
				CursorCaps result = default(CursorCaps);
				result = new CursorCaps(*(ref this.m_Internal + 24));
				return result;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x0600023F RID: 575 RVA: 0x0005CEF4 File Offset: 0x0005C2F4
		public unsafe DeviceCaps DeviceCaps
		{
			get
			{
				DeviceCaps result = default(DeviceCaps);
				result = new DeviceCaps(*(ref this.m_Internal + 28), *(ref this.m_Internal + 212));
				return result;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0005CF30 File Offset: 0x0005C330
		public unsafe MiscCaps PrimitiveMiscCaps
		{
			get
			{
				MiscCaps result = default(MiscCaps);
				result = new MiscCaps(*(ref this.m_Internal + 32));
				return result;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0005CF5C File Offset: 0x0005C35C
		public unsafe RasterCaps RasterCaps
		{
			get
			{
				RasterCaps result = default(RasterCaps);
				result = new RasterCaps(*(ref this.m_Internal + 36));
				return result;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0005CF88 File Offset: 0x0005C388
		public unsafe ComparisonCaps ZCompareCaps
		{
			get
			{
				ComparisonCaps result = default(ComparisonCaps);
				result = new ComparisonCaps(*(ref this.m_Internal + 40));
				return result;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0005CFB4 File Offset: 0x0005C3B4
		public unsafe ComparisonCaps AlphaCompareCaps
		{
			get
			{
				ComparisonCaps result = default(ComparisonCaps);
				result = new ComparisonCaps(*(ref this.m_Internal + 52));
				return result;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0005CFE0 File Offset: 0x0005C3E0
		public unsafe BlendCaps SourceBlendCaps
		{
			get
			{
				BlendCaps result = default(BlendCaps);
				result = new BlendCaps(*(ref this.m_Internal + 44));
				return result;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0005D00C File Offset: 0x0005C40C
		public unsafe BlendCaps DestinationBlendCaps
		{
			get
			{
				BlendCaps result = default(BlendCaps);
				result = new BlendCaps(*(ref this.m_Internal + 48));
				return result;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0005D038 File Offset: 0x0005C438
		public unsafe int MaxTextureWidth
		{
			get
			{
				return *(ref this.m_Internal + 88);
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0005D054 File Offset: 0x0005C454
		public unsafe int MaxTextureHeight
		{
			get
			{
				return *(ref this.m_Internal + 92);
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0005D070 File Offset: 0x0005C470
		public unsafe int MaxVolumeExtent
		{
			get
			{
				return *(ref this.m_Internal + 96);
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0005D08C File Offset: 0x0005C48C
		public unsafe int MaxTextureRepeat
		{
			get
			{
				return *(ref this.m_Internal + 100);
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0005D0A8 File Offset: 0x0005C4A8
		public unsafe int MaxTextureAspectRatio
		{
			get
			{
				return *(ref this.m_Internal + 104);
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0005D0C4 File Offset: 0x0005C4C4
		public unsafe int MaxAnisotropy
		{
			get
			{
				return *(ref this.m_Internal + 108);
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0005D0E0 File Offset: 0x0005C4E0
		public unsafe float MaxVertexW
		{
			get
			{
				return *(ref this.m_Internal + 112);
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0005D0FC File Offset: 0x0005C4FC
		public unsafe float GuardBandLeft
		{
			get
			{
				return *(ref this.m_Internal + 116);
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0005D118 File Offset: 0x0005C518
		public unsafe float GuardBandTop
		{
			get
			{
				return *(ref this.m_Internal + 120);
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0005D134 File Offset: 0x0005C534
		public unsafe float GuardBandRight
		{
			get
			{
				return *(ref this.m_Internal + 124);
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0005D150 File Offset: 0x0005C550
		public unsafe float GuardBandBottom
		{
			get
			{
				return *(ref this.m_Internal + 128);
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0005D170 File Offset: 0x0005C570
		public unsafe float ExtentsAdjust
		{
			get
			{
				return *(ref this.m_Internal + 132);
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0005D190 File Offset: 0x0005C590
		public unsafe int MaxActiveLights
		{
			get
			{
				return *(ref this.m_Internal + 160);
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0005D1B0 File Offset: 0x0005C5B0
		public unsafe int MaxUserClipPlanes
		{
			get
			{
				return *(ref this.m_Internal + 164);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0005D1D0 File Offset: 0x0005C5D0
		public unsafe int MaxVertexBlendMatrices
		{
			get
			{
				return *(ref this.m_Internal + 168);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000255 RID: 597 RVA: 0x0005D1F0 File Offset: 0x0005C5F0
		public unsafe int MaxVertexBlendMatrixIndex
		{
			get
			{
				return *(ref this.m_Internal + 172);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0005D210 File Offset: 0x0005C610
		public unsafe float MaxPointSize
		{
			get
			{
				return *(ref this.m_Internal + 176);
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0005D230 File Offset: 0x0005C630
		public unsafe int MaxPrimitiveCount
		{
			get
			{
				return *(ref this.m_Internal + 180);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0005D250 File Offset: 0x0005C650
		public unsafe int MaxVertexIndex
		{
			get
			{
				return *(ref this.m_Internal + 184);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0005D270 File Offset: 0x0005C670
		public unsafe int MaxStreams
		{
			get
			{
				return *(ref this.m_Internal + 188);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0005D290 File Offset: 0x0005C690
		public unsafe int MaxStreamStride
		{
			get
			{
				return *(ref this.m_Internal + 192);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0005D2B0 File Offset: 0x0005C6B0
		public unsafe float PixelShader1xMaxValue
		{
			get
			{
				return *(ref this.m_Internal + 208);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0005D2D0 File Offset: 0x0005C6D0
		public unsafe int NumberSimultaneousRts
		{
			get
			{
				return *(ref this.m_Internal + 240);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0005D2F0 File Offset: 0x0005C6F0
		public unsafe LineCaps LineCaps
		{
			get
			{
				LineCaps result = default(LineCaps);
				result = new LineCaps(*(ref this.m_Internal + 84));
				return result;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0005D31C File Offset: 0x0005C71C
		public unsafe ShadeCaps ShadeCaps
		{
			get
			{
				ShadeCaps result = default(ShadeCaps);
				result = new ShadeCaps(*(ref this.m_Internal + 56));
				return result;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0005D348 File Offset: 0x0005C748
		public unsafe int MaxTextureBlendStages
		{
			get
			{
				return *(ref this.m_Internal + 148);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0005D368 File Offset: 0x0005C768
		public unsafe int MaxSimultaneousTextures
		{
			get
			{
				return *(ref this.m_Internal + 152);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0005D388 File Offset: 0x0005C788
		public unsafe int MaxVertexShaderConst
		{
			get
			{
				return *(ref this.m_Internal + 200);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0005D3A8 File Offset: 0x0005C7A8
		public unsafe int MasterAdapterOrdinal
		{
			get
			{
				return *(ref this.m_Internal + 224);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000263 RID: 611 RVA: 0x0005D3C8 File Offset: 0x0005C7C8
		public unsafe int AdapterOrdinalInGroup
		{
			get
			{
				return *(ref this.m_Internal + 228);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0005D3E8 File Offset: 0x0005C7E8
		public unsafe int NumberOfAdaptersInGroup
		{
			get
			{
				return *(ref this.m_Internal + 232);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000265 RID: 613 RVA: 0x0005D408 File Offset: 0x0005C808
		public unsafe TextureCaps TextureCaps
		{
			get
			{
				TextureCaps result = default(TextureCaps);
				result = new TextureCaps(*(ref this.m_Internal + 60));
				return result;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0005D434 File Offset: 0x0005C834
		public unsafe FilterCaps StretchRectangleFilterCaps
		{
			get
			{
				FilterCaps result = default(FilterCaps);
				result = new FilterCaps(*(ref this.m_Internal + 244));
				return result;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000267 RID: 615 RVA: 0x0005D464 File Offset: 0x0005C864
		public unsafe FilterCaps VertexTextureFilterCaps
		{
			get
			{
				FilterCaps result = default(FilterCaps);
				result = new FilterCaps(*(ref this.m_Internal + 284));
				return result;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0005D494 File Offset: 0x0005C894
		public unsafe FilterCaps TextureFilterCaps
		{
			get
			{
				FilterCaps result = default(FilterCaps);
				result = new FilterCaps(*(ref this.m_Internal + 64));
				return result;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0005D4C0 File Offset: 0x0005C8C0
		public unsafe FilterCaps CubeTextureFilterCaps
		{
			get
			{
				FilterCaps result = default(FilterCaps);
				result = new FilterCaps(*(ref this.m_Internal + 68));
				return result;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0005D4EC File Offset: 0x0005C8EC
		public unsafe FilterCaps VolumeTextureFilterCaps
		{
			get
			{
				FilterCaps result = default(FilterCaps);
				result = new FilterCaps(*(ref this.m_Internal + 72));
				return result;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0005D518 File Offset: 0x0005C918
		public unsafe AddressCaps TextureAddressCaps
		{
			get
			{
				AddressCaps result = default(AddressCaps);
				result = new AddressCaps(*(ref this.m_Internal + 76));
				return result;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0005D544 File Offset: 0x0005C944
		public unsafe AddressCaps VolumeTextureAddressCaps
		{
			get
			{
				AddressCaps result = default(AddressCaps);
				result = new AddressCaps(*(ref this.m_Internal + 80));
				return result;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0005D570 File Offset: 0x0005C970
		public unsafe StencilCaps StencilCaps
		{
			get
			{
				StencilCaps result = default(StencilCaps);
				result = new StencilCaps(*(ref this.m_Internal + 136));
				return result;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0005D5A0 File Offset: 0x0005C9A0
		public unsafe TextureOperationCaps TextureOperationCaps
		{
			get
			{
				TextureOperationCaps result = default(TextureOperationCaps);
				result = new TextureOperationCaps(*(ref this.m_Internal + 144));
				return result;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0005D5D0 File Offset: 0x0005C9D0
		public unsafe VertexFormatCaps VertexFormatCaps
		{
			get
			{
				VertexFormatCaps result = default(VertexFormatCaps);
				result = new VertexFormatCaps(*(ref this.m_Internal + 140));
				return result;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0005D600 File Offset: 0x0005CA00
		public unsafe VertexProcessingCaps VertexProcessingCaps
		{
			get
			{
				VertexProcessingCaps result = default(VertexProcessingCaps);
				result = new VertexProcessingCaps(*(ref this.m_Internal + 156));
				return result;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0005D630 File Offset: 0x0005CA30
		public unsafe DeclarationTypeCaps DeclTypes
		{
			get
			{
				DeclarationTypeCaps result = default(DeclarationTypeCaps);
				result = new DeclarationTypeCaps(*(ref this.m_Internal + 236));
				return result;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0005D660 File Offset: 0x0005CA60
		public unsafe Version VertexShaderVersion
		{
			get
			{
				return new Version((int)(*(ref this.m_Internal + 197)), *(ref this.m_Internal + 196) & 255);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0005D698 File Offset: 0x0005CA98
		public unsafe Version PixelShaderVersion
		{
			get
			{
				return new Version((int)(*(ref this.m_Internal + 205)), *(ref this.m_Internal + 204) & 255);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0005D6D0 File Offset: 0x0005CAD0
		public unsafe int MaxVertexShader30InstructionSlots
		{
			get
			{
				return *(ref this.m_Internal + 296);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000275 RID: 629 RVA: 0x0005D6F0 File Offset: 0x0005CAF0
		public unsafe int MaxPixelShader30InstructionSlots
		{
			get
			{
				return *(ref this.m_Internal + 300);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0005D710 File Offset: 0x0005CB10
		public unsafe VertexShaderCaps VertexShaderCaps
		{
			get
			{
				VertexShaderCaps result = default(VertexShaderCaps);
				result = new VertexShaderCaps();
				_D3DVSHADERCAPS2_0* handle = ref this.m_Internal + 248;
				result.SetHandle(handle);
				return result;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0005D748 File Offset: 0x0005CB48
		public unsafe PixelShaderCaps PixelShaderCaps
		{
			get
			{
				PixelShaderCaps result = default(PixelShaderCaps);
				result = new PixelShaderCaps();
				_D3DPSHADERCAPS2_0* handle = ref this.m_Internal + 264;
				result.SetHandle(handle);
				return result;
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0005D780 File Offset: 0x0005CB80
		public Caps()
		{
			initblk(ref this.m_Internal, 0, 304);
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0005D7A4 File Offset: 0x0005CBA4
		internal unsafe _D3DCAPS9* Handle
		{
			get
			{
				return &this.m_Internal;
			}
		}

		// Token: 0x04000EF0 RID: 3824
		internal _D3DCAPS9 m_Internal;
	}
}
