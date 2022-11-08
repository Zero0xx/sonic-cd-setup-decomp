using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x02000771 RID: 1905
	internal class Com2TypeInfoProcessor
	{
		// Token: 0x06006444 RID: 25668 RVA: 0x0016E178 File Offset: 0x0016D178
		private Com2TypeInfoProcessor()
		{
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x06006445 RID: 25669 RVA: 0x0016E180 File Offset: 0x0016D180
		private static ModuleBuilder ModuleBuilder
		{
			get
			{
				if (Com2TypeInfoProcessor.moduleBuilder == null)
				{
					AppDomain domain = Thread.GetDomain();
					AssemblyBuilder assemblyBuilder = domain.DefineDynamicAssembly(new AssemblyName
					{
						Name = "COM2InteropEmit"
					}, AssemblyBuilderAccess.Run);
					Com2TypeInfoProcessor.moduleBuilder = assemblyBuilder.DefineDynamicModule("COM2Interop.Emit");
				}
				return Com2TypeInfoProcessor.moduleBuilder;
			}
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x0016E1CC File Offset: 0x0016D1CC
		public static UnsafeNativeMethods.ITypeInfo FindTypeInfo(object obj, bool wantCoClass)
		{
			UnsafeNativeMethods.ITypeInfo typeInfo = null;
			int num = 0;
			while (typeInfo == null && num < 2)
			{
				if (wantCoClass != (num == 0))
				{
					goto IL_28;
				}
				if (obj is NativeMethods.IProvideClassInfo)
				{
					NativeMethods.IProvideClassInfo provideClassInfo = (NativeMethods.IProvideClassInfo)obj;
					try
					{
						typeInfo = provideClassInfo.GetClassInfo();
						goto IL_49;
					}
					catch
					{
						goto IL_49;
					}
					goto IL_28;
				}
				IL_49:
				num++;
				continue;
				IL_28:
				if (obj is UnsafeNativeMethods.IDispatch)
				{
					UnsafeNativeMethods.IDispatch dispatch = (UnsafeNativeMethods.IDispatch)obj;
					try
					{
						typeInfo = dispatch.GetTypeInfo(0, SafeNativeMethods.GetThreadLCID());
					}
					catch
					{
					}
					goto IL_49;
				}
				goto IL_49;
			}
			return typeInfo;
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x0016E24C File Offset: 0x0016D24C
		public static UnsafeNativeMethods.ITypeInfo[] FindTypeInfos(object obj, bool wantCoClass)
		{
			UnsafeNativeMethods.ITypeInfo[] array = null;
			int num = 0;
			UnsafeNativeMethods.ITypeInfo typeInfo = null;
			if (obj is NativeMethods.IProvideMultipleClassInfo)
			{
				NativeMethods.IProvideMultipleClassInfo provideMultipleClassInfo = (NativeMethods.IProvideMultipleClassInfo)obj;
				if (!NativeMethods.Succeeded(provideMultipleClassInfo.GetMultiTypeInfoCount(ref num)) || num == 0)
				{
					num = 0;
				}
				if (num > 0)
				{
					array = new UnsafeNativeMethods.ITypeInfo[num];
					for (int i = 0; i < num; i++)
					{
						if (!NativeMethods.Failed(provideMultipleClassInfo.GetInfoOfIndex(i, 1, ref typeInfo, 0, 0, IntPtr.Zero, IntPtr.Zero)))
						{
							array[i] = typeInfo;
						}
					}
				}
			}
			if (array == null || array.Length == 0)
			{
				typeInfo = Com2TypeInfoProcessor.FindTypeInfo(obj, wantCoClass);
				if (typeInfo != null)
				{
					array = new UnsafeNativeMethods.ITypeInfo[]
					{
						typeInfo
					};
				}
			}
			return array;
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x0016E2E4 File Offset: 0x0016D2E4
		public static int GetNameDispId(UnsafeNativeMethods.IDispatch obj)
		{
			int result = -1;
			string[] array = null;
			ComNativeDescriptor instance = ComNativeDescriptor.Instance;
			bool flag = false;
			instance.GetPropertyValue(obj, "__id", ref flag);
			if (flag)
			{
				array = new string[]
				{
					"__id"
				};
			}
			else
			{
				instance.GetPropertyValue(obj, -800, ref flag);
				if (flag)
				{
					result = -800;
				}
				else
				{
					instance.GetPropertyValue(obj, "Name", ref flag);
					if (flag)
					{
						array = new string[]
						{
							"Name"
						};
					}
				}
			}
			if (array != null)
			{
				int[] array2 = new int[]
				{
					-1
				};
				Guid empty = Guid.Empty;
				int idsOfNames = obj.GetIDsOfNames(ref empty, array, 1, SafeNativeMethods.GetThreadLCID(), array2);
				if (NativeMethods.Succeeded(idsOfNames))
				{
					result = array2[0];
				}
			}
			return result;
		}

		// Token: 0x06006449 RID: 25673 RVA: 0x0016E3A4 File Offset: 0x0016D3A4
		public static Com2Properties GetProperties(object obj)
		{
			if (obj == null || !Marshal.IsComObject(obj))
			{
				return null;
			}
			UnsafeNativeMethods.ITypeInfo[] array = Com2TypeInfoProcessor.FindTypeInfos(obj, false);
			if (array == null || array.Length == 0)
			{
				return null;
			}
			int num = -1;
			int num2 = -1;
			ArrayList arrayList = new ArrayList();
			int num3 = array.Length;
			for (int i = 0; i < array.Length; i++)
			{
				UnsafeNativeMethods.ITypeInfo typeInfo = array[i];
				if (typeInfo != null)
				{
					int[] array2 = new int[2];
					Guid guidForTypeInfo = Com2TypeInfoProcessor.GetGuidForTypeInfo(typeInfo, null, array2);
					PropertyDescriptor[] array3 = null;
					bool flag = guidForTypeInfo != Guid.Empty && Com2TypeInfoProcessor.processedLibraries != null && Com2TypeInfoProcessor.processedLibraries.Contains(guidForTypeInfo);
					if (flag)
					{
						Com2TypeInfoProcessor.CachedProperties cachedProperties = (Com2TypeInfoProcessor.CachedProperties)Com2TypeInfoProcessor.processedLibraries[guidForTypeInfo];
						if (array2[0] == cachedProperties.MajorVersion && array2[1] == cachedProperties.MinorVersion)
						{
							array3 = cachedProperties.Properties;
							if (i == 0 && cachedProperties.DefaultIndex != -1)
							{
								num = cachedProperties.DefaultIndex;
							}
						}
						else
						{
							flag = false;
						}
					}
					if (!flag)
					{
						array3 = Com2TypeInfoProcessor.InternalGetProperties(obj, typeInfo, -1, ref num2);
						if (i == 0 && num2 != -1)
						{
							num = num2;
						}
						if (Com2TypeInfoProcessor.processedLibraries == null)
						{
							Com2TypeInfoProcessor.processedLibraries = new Hashtable();
						}
						if (guidForTypeInfo != Guid.Empty)
						{
							Com2TypeInfoProcessor.processedLibraries[guidForTypeInfo] = new Com2TypeInfoProcessor.CachedProperties(array3, (i == 0) ? num : -1, array2[0], array2[1]);
						}
					}
					if (array3 != null)
					{
						arrayList.AddRange(array3);
					}
				}
			}
			Com2PropertyDescriptor[] array4 = new Com2PropertyDescriptor[arrayList.Count];
			arrayList.CopyTo(array4, 0);
			return new Com2Properties(obj, array4, num);
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x0016E52C File Offset: 0x0016D52C
		private static Guid GetGuidForTypeInfo(UnsafeNativeMethods.ITypeInfo typeInfo, Com2TypeInfoProcessor.StructCache structCache, int[] versions)
		{
			IntPtr zero = IntPtr.Zero;
			int typeAttr = typeInfo.GetTypeAttr(ref zero);
			if (!NativeMethods.Succeeded(typeAttr))
			{
				throw new ExternalException(SR.GetString("TYPEINFOPROCESSORGetTypeAttrFailed", new object[]
				{
					typeAttr
				}), typeAttr);
			}
			Guid result = Guid.Empty;
			NativeMethods.tagTYPEATTR tagTYPEATTR = null;
			try
			{
				if (structCache == null)
				{
					tagTYPEATTR = new NativeMethods.tagTYPEATTR();
				}
				else
				{
					tagTYPEATTR = (NativeMethods.tagTYPEATTR)structCache.GetStruct(typeof(NativeMethods.tagTYPEATTR));
				}
				UnsafeNativeMethods.PtrToStructure(zero, tagTYPEATTR);
				result = tagTYPEATTR.guid;
				if (versions != null)
				{
					versions[0] = (int)tagTYPEATTR.wMajorVerNum;
					versions[1] = (int)tagTYPEATTR.wMinorVerNum;
				}
			}
			finally
			{
				typeInfo.ReleaseTypeAttr(zero);
				if (structCache != null && tagTYPEATTR != null)
				{
					structCache.ReleaseStruct(tagTYPEATTR);
				}
			}
			return result;
		}

		// Token: 0x0600644B RID: 25675 RVA: 0x0016E5EC File Offset: 0x0016D5EC
		private static Type GetValueTypeFromTypeDesc(NativeMethods.tagTYPEDESC typeDesc, UnsafeNativeMethods.ITypeInfo typeInfo, object[] typeData, Com2TypeInfoProcessor.StructCache structCache)
		{
			NativeMethods.tagVT vt = (NativeMethods.tagVT)typeDesc.vt;
			if (vt > NativeMethods.tagVT.VT_UNKNOWN)
			{
				IntPtr unionMember;
				if (vt != NativeMethods.tagVT.VT_PTR)
				{
					if (vt != NativeMethods.tagVT.VT_USERDEFINED)
					{
						goto IL_2A;
					}
					unionMember = typeDesc.unionMember;
				}
				else
				{
					NativeMethods.tagTYPEDESC tagTYPEDESC = (NativeMethods.tagTYPEDESC)structCache.GetStruct(typeof(NativeMethods.tagTYPEDESC));
					try
					{
						try
						{
							UnsafeNativeMethods.PtrToStructure(typeDesc.unionMember, tagTYPEDESC);
						}
						catch
						{
							tagTYPEDESC = new NativeMethods.tagTYPEDESC();
							tagTYPEDESC.unionMember = (IntPtr)Marshal.ReadInt32(typeDesc.unionMember);
							tagTYPEDESC.vt = Marshal.ReadInt16(typeDesc.unionMember, 4);
						}
						if (tagTYPEDESC.vt == 12)
						{
							return Com2TypeInfoProcessor.VTToType((NativeMethods.tagVT)tagTYPEDESC.vt);
						}
						unionMember = tagTYPEDESC.unionMember;
					}
					finally
					{
						structCache.ReleaseStruct(tagTYPEDESC);
					}
				}
				UnsafeNativeMethods.ITypeInfo typeInfo2 = null;
				int num = typeInfo.GetRefTypeInfo(unionMember, ref typeInfo2);
				if (!NativeMethods.Succeeded(num))
				{
					throw new ExternalException(SR.GetString("TYPEINFOPROCESSORGetRefTypeInfoFailed", new object[]
					{
						num
					}), num);
				}
				try
				{
					if (typeInfo2 != null)
					{
						IntPtr zero = IntPtr.Zero;
						num = typeInfo2.GetTypeAttr(ref zero);
						if (!NativeMethods.Succeeded(num))
						{
							throw new ExternalException(SR.GetString("TYPEINFOPROCESSORGetTypeAttrFailed", new object[]
							{
								num
							}), num);
						}
						NativeMethods.tagTYPEATTR tagTYPEATTR = (NativeMethods.tagTYPEATTR)structCache.GetStruct(typeof(NativeMethods.tagTYPEATTR));
						UnsafeNativeMethods.PtrToStructure(zero, tagTYPEATTR);
						try
						{
							Guid guid = tagTYPEATTR.guid;
							if (!Guid.Empty.Equals(guid))
							{
								typeData[0] = guid;
							}
							switch (tagTYPEATTR.typekind)
							{
							case 0:
								return Com2TypeInfoProcessor.ProcessTypeInfoEnum(typeInfo2, structCache);
							case 3:
							case 5:
								return Com2TypeInfoProcessor.VTToType(NativeMethods.tagVT.VT_UNKNOWN);
							case 4:
								return Com2TypeInfoProcessor.VTToType(NativeMethods.tagVT.VT_DISPATCH);
							case 6:
								return Com2TypeInfoProcessor.GetValueTypeFromTypeDesc(tagTYPEATTR.Get_tdescAlias(), typeInfo2, typeData, structCache);
							}
							return null;
						}
						finally
						{
							typeInfo2.ReleaseTypeAttr(zero);
							structCache.ReleaseStruct(tagTYPEATTR);
						}
					}
				}
				finally
				{
					typeInfo2 = null;
				}
				return null;
			}
			if (vt == NativeMethods.tagVT.VT_DISPATCH || vt == NativeMethods.tagVT.VT_UNKNOWN)
			{
				typeData[0] = Com2TypeInfoProcessor.GetGuidForTypeInfo(typeInfo, structCache, null);
				return Com2TypeInfoProcessor.VTToType((NativeMethods.tagVT)typeDesc.vt);
			}
			IL_2A:
			return Com2TypeInfoProcessor.VTToType((NativeMethods.tagVT)typeDesc.vt);
		}

		// Token: 0x0600644C RID: 25676 RVA: 0x0016E880 File Offset: 0x0016D880
		private static PropertyDescriptor[] InternalGetProperties(object obj, UnsafeNativeMethods.ITypeInfo typeInfo, int dispidToGet, ref int defaultIndex)
		{
			if (typeInfo == null)
			{
				return null;
			}
			Hashtable hashtable = new Hashtable();
			int nameDispId = Com2TypeInfoProcessor.GetNameDispId((UnsafeNativeMethods.IDispatch)obj);
			bool flag = false;
			Com2TypeInfoProcessor.StructCache structCache = new Com2TypeInfoProcessor.StructCache();
			try
			{
				Com2TypeInfoProcessor.ProcessFunctions(typeInfo, hashtable, dispidToGet, nameDispId, ref flag, structCache);
			}
			catch (ExternalException)
			{
			}
			try
			{
				Com2TypeInfoProcessor.ProcessVariables(typeInfo, hashtable, dispidToGet, nameDispId, structCache);
			}
			catch (ExternalException)
			{
			}
			typeInfo = null;
			int num = hashtable.Count;
			if (flag)
			{
				num++;
			}
			PropertyDescriptor[] array = new PropertyDescriptor[num];
			int hr = 0;
			object[] retval = new object[1];
			ComNativeDescriptor instance = ComNativeDescriptor.Instance;
			foreach (object obj2 in hashtable.Values)
			{
				Com2TypeInfoProcessor.PropInfo propInfo = (Com2TypeInfoProcessor.PropInfo)obj2;
				if (!propInfo.NonBrowsable)
				{
					try
					{
						hr = instance.GetPropertyValue(obj, propInfo.DispId, retval);
					}
					catch (ExternalException ex)
					{
						hr = ex.ErrorCode;
					}
					if (!NativeMethods.Succeeded(hr))
					{
						propInfo.Attributes.Add(new BrowsableAttribute(false));
						propInfo.NonBrowsable = true;
					}
				}
				else
				{
					hr = 0;
				}
				Attribute[] array2 = new Attribute[propInfo.Attributes.Count];
				propInfo.Attributes.CopyTo(array2, 0);
				array[propInfo.Index] = new Com2PropertyDescriptor(propInfo.DispId, propInfo.Name, array2, propInfo.ReadOnly != 2, propInfo.ValueType, propInfo.TypeData, !NativeMethods.Succeeded(hr));
				if (propInfo.IsDefault)
				{
					int index = propInfo.Index;
				}
			}
			if (flag)
			{
				array[array.Length - 1] = new Com2AboutBoxPropertyDescriptor();
			}
			return array;
		}

		// Token: 0x0600644D RID: 25677 RVA: 0x0016EA50 File Offset: 0x0016DA50
		private static Com2TypeInfoProcessor.PropInfo ProcessDataCore(UnsafeNativeMethods.ITypeInfo typeInfo, IDictionary propInfoList, int dispid, int nameDispID, NativeMethods.tagTYPEDESC typeDesc, int flags, Com2TypeInfoProcessor.StructCache structCache)
		{
			string text = null;
			string text2 = null;
			int documentation = typeInfo.GetDocumentation(dispid, ref text, ref text2, null, null);
			ComNativeDescriptor instance = ComNativeDescriptor.Instance;
			if (!NativeMethods.Succeeded(documentation))
			{
				throw new COMException(SR.GetString("TYPEINFOPROCESSORGetDocumentationFailed", new object[]
				{
					dispid,
					documentation,
					instance.GetClassName(typeInfo)
				}), documentation);
			}
			if (text == null)
			{
				return null;
			}
			Com2TypeInfoProcessor.PropInfo propInfo = (Com2TypeInfoProcessor.PropInfo)propInfoList[text];
			if (propInfo == null)
			{
				propInfo = new Com2TypeInfoProcessor.PropInfo();
				propInfo.Index = propInfoList.Count;
				propInfoList[text] = propInfo;
				propInfo.Name = text;
				propInfo.DispId = dispid;
				propInfo.Attributes.Add(new DispIdAttribute(propInfo.DispId));
			}
			if (text2 != null)
			{
				propInfo.Attributes.Add(new DescriptionAttribute(text2));
			}
			if (propInfo.ValueType == null)
			{
				object[] array = new object[1];
				try
				{
					propInfo.ValueType = Com2TypeInfoProcessor.GetValueTypeFromTypeDesc(typeDesc, typeInfo, array, structCache);
				}
				catch (Exception)
				{
				}
				if (propInfo.ValueType == null)
				{
					propInfo.NonBrowsable = true;
				}
				if (propInfo.NonBrowsable)
				{
					flags |= 1024;
				}
				if (array[0] != null)
				{
					propInfo.TypeData = array[0];
				}
			}
			if ((flags & 1) != 0)
			{
				propInfo.ReadOnly = 1;
			}
			if ((flags & 64) != 0 || (flags & 1024) != 0 || propInfo.Name[0] == '_' || dispid == -515)
			{
				propInfo.Attributes.Add(new BrowsableAttribute(false));
				propInfo.NonBrowsable = true;
			}
			if ((flags & 512) != 0)
			{
				propInfo.IsDefault = true;
			}
			if ((flags & 4) != 0 && (flags & 16) != 0)
			{
				propInfo.Attributes.Add(new BindableAttribute(true));
			}
			if (dispid == nameDispID)
			{
				propInfo.Attributes.Add(new ParenthesizePropertyNameAttribute(true));
				propInfo.Attributes.Add(new MergablePropertyAttribute(false));
			}
			return propInfo;
		}

		// Token: 0x0600644E RID: 25678 RVA: 0x0016EC4C File Offset: 0x0016DC4C
		private static void ProcessFunctions(UnsafeNativeMethods.ITypeInfo typeInfo, IDictionary propInfoList, int dispidToGet, int nameDispID, ref bool addAboutBox, Com2TypeInfoProcessor.StructCache structCache)
		{
			IntPtr zero = IntPtr.Zero;
			int num = typeInfo.GetTypeAttr(ref zero);
			if (!NativeMethods.Succeeded(num) || zero == IntPtr.Zero)
			{
				throw new ExternalException(SR.GetString("TYPEINFOPROCESSORGetTypeAttrFailed", new object[]
				{
					num
				}), num);
			}
			NativeMethods.tagTYPEATTR tagTYPEATTR = (NativeMethods.tagTYPEATTR)structCache.GetStruct(typeof(NativeMethods.tagTYPEATTR));
			UnsafeNativeMethods.PtrToStructure(zero, tagTYPEATTR);
			try
			{
				if (tagTYPEATTR != null)
				{
					NativeMethods.tagFUNCDESC tagFUNCDESC = (NativeMethods.tagFUNCDESC)structCache.GetStruct(typeof(NativeMethods.tagFUNCDESC));
					NativeMethods.tagELEMDESC tagELEMDESC = (NativeMethods.tagELEMDESC)structCache.GetStruct(typeof(NativeMethods.tagELEMDESC));
					for (int i = 0; i < (int)tagTYPEATTR.cFuncs; i++)
					{
						IntPtr zero2 = IntPtr.Zero;
						num = typeInfo.GetFuncDesc(i, ref zero2);
						if (NativeMethods.Succeeded(num) && !(zero2 == IntPtr.Zero))
						{
							UnsafeNativeMethods.PtrToStructure(zero2, tagFUNCDESC);
							try
							{
								if (tagFUNCDESC.invkind == 1 || (dispidToGet != -1 && tagFUNCDESC.memid != dispidToGet))
								{
									if (tagFUNCDESC.memid == -552)
									{
										addAboutBox = true;
									}
								}
								else
								{
									bool flag = tagFUNCDESC.invkind == 2;
									NativeMethods.tagTYPEDESC tdesc;
									if (flag)
									{
										if (tagFUNCDESC.cParams != 0)
										{
											goto IL_198;
										}
										tdesc = tagFUNCDESC.elemdescFunc.tdesc;
									}
									else
									{
										if (tagFUNCDESC.lprgelemdescParam == IntPtr.Zero || tagFUNCDESC.cParams != 1)
										{
											goto IL_198;
										}
										Marshal.PtrToStructure(tagFUNCDESC.lprgelemdescParam, tagELEMDESC);
										tdesc = tagELEMDESC.tdesc;
									}
									Com2TypeInfoProcessor.PropInfo propInfo = Com2TypeInfoProcessor.ProcessDataCore(typeInfo, propInfoList, tagFUNCDESC.memid, nameDispID, tdesc, (int)tagFUNCDESC.wFuncFlags, structCache);
									if (propInfo != null && !flag)
									{
										propInfo.ReadOnly = 2;
									}
								}
							}
							finally
							{
								typeInfo.ReleaseFuncDesc(zero2);
							}
						}
						IL_198:;
					}
					structCache.ReleaseStruct(tagFUNCDESC);
					structCache.ReleaseStruct(tagELEMDESC);
				}
			}
			finally
			{
				typeInfo.ReleaseTypeAttr(zero);
				structCache.ReleaseStruct(tagTYPEATTR);
			}
		}

		// Token: 0x0600644F RID: 25679 RVA: 0x0016EE5C File Offset: 0x0016DE5C
		private static Type ProcessTypeInfoEnum(UnsafeNativeMethods.ITypeInfo enumTypeInfo, Com2TypeInfoProcessor.StructCache structCache)
		{
			if (enumTypeInfo == null)
			{
				return null;
			}
			try
			{
				IntPtr zero = IntPtr.Zero;
				int num = enumTypeInfo.GetTypeAttr(ref zero);
				if (!NativeMethods.Succeeded(num) || zero == IntPtr.Zero)
				{
					throw new ExternalException(SR.GetString("TYPEINFOPROCESSORGetTypeAttrFailed", new object[]
					{
						num
					}), num);
				}
				NativeMethods.tagTYPEATTR tagTYPEATTR = (NativeMethods.tagTYPEATTR)structCache.GetStruct(typeof(NativeMethods.tagTYPEATTR));
				UnsafeNativeMethods.PtrToStructure(zero, tagTYPEATTR);
				if (zero == IntPtr.Zero)
				{
					return null;
				}
				try
				{
					int cVars = (int)tagTYPEATTR.cVars;
					ArrayList arrayList = new ArrayList();
					ArrayList arrayList2 = new ArrayList();
					NativeMethods.tagVARDESC tagVARDESC = (NativeMethods.tagVARDESC)structCache.GetStruct(typeof(NativeMethods.tagVARDESC));
					object value = null;
					string text = null;
					string text2 = null;
					string text3 = null;
					enumTypeInfo.GetDocumentation(-1, ref text, ref text3, null, null);
					for (int i = 0; i < cVars; i++)
					{
						IntPtr zero2 = IntPtr.Zero;
						num = enumTypeInfo.GetVarDesc(i, ref zero2);
						if (NativeMethods.Succeeded(num) && !(zero2 == IntPtr.Zero))
						{
							try
							{
								UnsafeNativeMethods.PtrToStructure(zero2, tagVARDESC);
								if (tagVARDESC != null && tagVARDESC.varkind == 2 && !(tagVARDESC.unionMember == IntPtr.Zero))
								{
									text3 = (text2 = null);
									value = null;
									num = enumTypeInfo.GetDocumentation(tagVARDESC.memid, ref text2, ref text3, null, null);
									if (NativeMethods.Succeeded(num))
									{
										try
										{
											value = Marshal.GetObjectForNativeVariant(tagVARDESC.unionMember);
										}
										catch (Exception)
										{
										}
										arrayList2.Add(value);
										string value2;
										if (text3 != null)
										{
											value2 = text3;
										}
										else
										{
											value2 = text2;
										}
										arrayList.Add(value2);
									}
								}
							}
							finally
							{
								if (zero2 != IntPtr.Zero)
								{
									enumTypeInfo.ReleaseVarDesc(zero2);
								}
							}
						}
					}
					structCache.ReleaseStruct(tagVARDESC);
					if (arrayList.Count > 0)
					{
						IntPtr iunknownForObject = Marshal.GetIUnknownForObject(enumTypeInfo);
						try
						{
							text = iunknownForObject.ToString() + "_" + text;
							if (Com2TypeInfoProcessor.builtEnums == null)
							{
								Com2TypeInfoProcessor.builtEnums = new Hashtable();
							}
							else if (Com2TypeInfoProcessor.builtEnums.ContainsKey(text))
							{
								return (Type)Com2TypeInfoProcessor.builtEnums[text];
							}
							Type underlyingType = typeof(int);
							if (arrayList2.Count > 0 && arrayList2[0] != null)
							{
								underlyingType = arrayList2[0].GetType();
							}
							EnumBuilder enumBuilder = Com2TypeInfoProcessor.ModuleBuilder.DefineEnum(text, TypeAttributes.Public, underlyingType);
							for (int j = 0; j < arrayList.Count; j++)
							{
								enumBuilder.DefineLiteral((string)arrayList[j], arrayList2[j]);
							}
							Type type = enumBuilder.CreateType();
							Com2TypeInfoProcessor.builtEnums[text] = type;
							return type;
						}
						finally
						{
							if (iunknownForObject != IntPtr.Zero)
							{
								Marshal.Release(iunknownForObject);
							}
						}
					}
				}
				finally
				{
					enumTypeInfo.ReleaseTypeAttr(zero);
					structCache.ReleaseStruct(tagTYPEATTR);
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x06006450 RID: 25680 RVA: 0x0016F1C4 File Offset: 0x0016E1C4
		private static void ProcessVariables(UnsafeNativeMethods.ITypeInfo typeInfo, IDictionary propInfoList, int dispidToGet, int nameDispID, Com2TypeInfoProcessor.StructCache structCache)
		{
			IntPtr zero = IntPtr.Zero;
			int num = typeInfo.GetTypeAttr(ref zero);
			if (!NativeMethods.Succeeded(num) || zero == IntPtr.Zero)
			{
				throw new ExternalException(SR.GetString("TYPEINFOPROCESSORGetTypeAttrFailed", new object[]
				{
					num
				}), num);
			}
			NativeMethods.tagTYPEATTR tagTYPEATTR = (NativeMethods.tagTYPEATTR)structCache.GetStruct(typeof(NativeMethods.tagTYPEATTR));
			UnsafeNativeMethods.PtrToStructure(zero, tagTYPEATTR);
			try
			{
				if (tagTYPEATTR != null)
				{
					NativeMethods.tagVARDESC tagVARDESC = (NativeMethods.tagVARDESC)structCache.GetStruct(typeof(NativeMethods.tagVARDESC));
					for (int i = 0; i < (int)tagTYPEATTR.cVars; i++)
					{
						IntPtr zero2 = IntPtr.Zero;
						num = typeInfo.GetVarDesc(i, ref zero2);
						if (NativeMethods.Succeeded(num) && !(zero2 == IntPtr.Zero))
						{
							UnsafeNativeMethods.PtrToStructure(zero2, tagVARDESC);
							try
							{
								if (tagVARDESC.varkind != 2 && (dispidToGet == -1 || tagVARDESC.memid == dispidToGet))
								{
									Com2TypeInfoProcessor.PropInfo propInfo = Com2TypeInfoProcessor.ProcessDataCore(typeInfo, propInfoList, tagVARDESC.memid, nameDispID, tagVARDESC.elemdescVar.tdesc, (int)tagVARDESC.wVarFlags, structCache);
									if (propInfo.ReadOnly != 1)
									{
										propInfo.ReadOnly = 2;
									}
								}
							}
							finally
							{
								if (zero2 != IntPtr.Zero)
								{
									typeInfo.ReleaseVarDesc(zero2);
								}
							}
						}
					}
					structCache.ReleaseStruct(tagVARDESC);
				}
			}
			finally
			{
				typeInfo.ReleaseTypeAttr(zero);
				structCache.ReleaseStruct(tagTYPEATTR);
			}
		}

		// Token: 0x06006451 RID: 25681 RVA: 0x0016F340 File Offset: 0x0016E340
		private static Type VTToType(NativeMethods.tagVT vt)
		{
			if (vt <= NativeMethods.tagVT.VT_VECTOR)
			{
				switch (vt)
				{
				case NativeMethods.tagVT.VT_EMPTY:
				case NativeMethods.tagVT.VT_NULL:
					return null;
				case NativeMethods.tagVT.VT_I2:
					return typeof(short);
				case NativeMethods.tagVT.VT_I4:
				case NativeMethods.tagVT.VT_INT:
					return typeof(int);
				case NativeMethods.tagVT.VT_R4:
					return typeof(float);
				case NativeMethods.tagVT.VT_R8:
					return typeof(double);
				case NativeMethods.tagVT.VT_CY:
					return typeof(decimal);
				case NativeMethods.tagVT.VT_DATE:
					return typeof(DateTime);
				case NativeMethods.tagVT.VT_BSTR:
				case NativeMethods.tagVT.VT_LPSTR:
				case NativeMethods.tagVT.VT_LPWSTR:
					return typeof(string);
				case NativeMethods.tagVT.VT_DISPATCH:
					return typeof(UnsafeNativeMethods.IDispatch);
				case NativeMethods.tagVT.VT_ERROR:
				case NativeMethods.tagVT.VT_HRESULT:
					return typeof(int);
				case NativeMethods.tagVT.VT_BOOL:
					return typeof(bool);
				case NativeMethods.tagVT.VT_VARIANT:
					return typeof(Com2Variant);
				case NativeMethods.tagVT.VT_UNKNOWN:
					return typeof(object);
				case NativeMethods.tagVT.VT_DECIMAL:
				case (NativeMethods.tagVT)15:
				case NativeMethods.tagVT.VT_VOID:
				case NativeMethods.tagVT.VT_PTR:
				case NativeMethods.tagVT.VT_SAFEARRAY:
				case NativeMethods.tagVT.VT_CARRAY:
				case (NativeMethods.tagVT)32:
				case (NativeMethods.tagVT)33:
				case (NativeMethods.tagVT)34:
				case (NativeMethods.tagVT)35:
				case NativeMethods.tagVT.VT_RECORD:
				case (NativeMethods.tagVT)37:
				case (NativeMethods.tagVT)38:
				case (NativeMethods.tagVT)39:
				case (NativeMethods.tagVT)40:
				case (NativeMethods.tagVT)41:
				case (NativeMethods.tagVT)42:
				case (NativeMethods.tagVT)43:
				case (NativeMethods.tagVT)44:
				case (NativeMethods.tagVT)45:
				case (NativeMethods.tagVT)46:
				case (NativeMethods.tagVT)47:
				case (NativeMethods.tagVT)48:
				case (NativeMethods.tagVT)49:
				case (NativeMethods.tagVT)50:
				case (NativeMethods.tagVT)51:
				case (NativeMethods.tagVT)52:
				case (NativeMethods.tagVT)53:
				case (NativeMethods.tagVT)54:
				case (NativeMethods.tagVT)55:
				case (NativeMethods.tagVT)56:
				case (NativeMethods.tagVT)57:
				case (NativeMethods.tagVT)58:
				case (NativeMethods.tagVT)59:
				case (NativeMethods.tagVT)60:
				case (NativeMethods.tagVT)61:
				case (NativeMethods.tagVT)62:
				case (NativeMethods.tagVT)63:
				case NativeMethods.tagVT.VT_BLOB:
				case NativeMethods.tagVT.VT_STREAM:
				case NativeMethods.tagVT.VT_STORAGE:
				case NativeMethods.tagVT.VT_STREAMED_OBJECT:
				case NativeMethods.tagVT.VT_STORED_OBJECT:
				case NativeMethods.tagVT.VT_BLOB_OBJECT:
				case NativeMethods.tagVT.VT_CF:
					break;
				case NativeMethods.tagVT.VT_I1:
					return typeof(sbyte);
				case NativeMethods.tagVT.VT_UI1:
					return typeof(byte);
				case NativeMethods.tagVT.VT_UI2:
					return typeof(ushort);
				case NativeMethods.tagVT.VT_UI4:
				case NativeMethods.tagVT.VT_UINT:
					return typeof(uint);
				case NativeMethods.tagVT.VT_I8:
					return typeof(long);
				case NativeMethods.tagVT.VT_UI8:
					return typeof(ulong);
				case NativeMethods.tagVT.VT_USERDEFINED:
					throw new ArgumentException(SR.GetString("COM2UnhandledVT", new object[]
					{
						"VT_USERDEFINED"
					}));
				case NativeMethods.tagVT.VT_FILETIME:
					return typeof(NativeMethods.FILETIME);
				case NativeMethods.tagVT.VT_CLSID:
					return typeof(Guid);
				default:
					switch (vt)
					{
					}
					break;
				}
			}
			else if (vt != NativeMethods.tagVT.VT_ARRAY && vt != NativeMethods.tagVT.VT_BYREF && vt != NativeMethods.tagVT.VT_RESERVED)
			{
			}
			string name = "COM2UnhandledVT";
			object[] array = new object[1];
			object[] array2 = array;
			int num = 0;
			int num2 = (int)vt;
			array2[num] = num2.ToString(CultureInfo.InvariantCulture);
			throw new ArgumentException(SR.GetString(name, array));
		}

		// Token: 0x04003BB9 RID: 15289
		private static TraceSwitch DbgTypeInfoProcessorSwitch;

		// Token: 0x04003BBA RID: 15290
		private static ModuleBuilder moduleBuilder;

		// Token: 0x04003BBB RID: 15291
		private static Hashtable builtEnums;

		// Token: 0x04003BBC RID: 15292
		private static Hashtable processedLibraries;

		// Token: 0x02000772 RID: 1906
		internal class CachedProperties
		{
			// Token: 0x06006452 RID: 25682 RVA: 0x0016F5E9 File Offset: 0x0016E5E9
			internal CachedProperties(PropertyDescriptor[] props, int defIndex, int majVersion, int minVersion)
			{
				this.props = this.ClonePropertyDescriptors(props);
				this.MajorVersion = majVersion;
				this.MinorVersion = minVersion;
				this.defaultIndex = defIndex;
			}

			// Token: 0x17001522 RID: 5410
			// (get) Token: 0x06006453 RID: 25683 RVA: 0x0016F614 File Offset: 0x0016E614
			public PropertyDescriptor[] Properties
			{
				get
				{
					return this.ClonePropertyDescriptors(this.props);
				}
			}

			// Token: 0x17001523 RID: 5411
			// (get) Token: 0x06006454 RID: 25684 RVA: 0x0016F622 File Offset: 0x0016E622
			public int DefaultIndex
			{
				get
				{
					return this.defaultIndex;
				}
			}

			// Token: 0x06006455 RID: 25685 RVA: 0x0016F62C File Offset: 0x0016E62C
			private PropertyDescriptor[] ClonePropertyDescriptors(PropertyDescriptor[] props)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					if (props[i] is ICloneable)
					{
						array[i] = (PropertyDescriptor)((ICloneable)props[i]).Clone();
					}
					else
					{
						array[i] = props[i];
					}
				}
				return array;
			}

			// Token: 0x04003BBD RID: 15293
			private PropertyDescriptor[] props;

			// Token: 0x04003BBE RID: 15294
			public readonly int MajorVersion;

			// Token: 0x04003BBF RID: 15295
			public readonly int MinorVersion;

			// Token: 0x04003BC0 RID: 15296
			private int defaultIndex;
		}

		// Token: 0x02000773 RID: 1907
		public class StructCache
		{
			// Token: 0x06006456 RID: 25686 RVA: 0x0016F678 File Offset: 0x0016E678
			private Queue GetQueue(Type t, bool create)
			{
				object obj = this.queuedTypes[t];
				if (obj == null && create)
				{
					obj = new Queue();
					this.queuedTypes[t] = obj;
				}
				return (Queue)obj;
			}

			// Token: 0x06006457 RID: 25687 RVA: 0x0016F6B4 File Offset: 0x0016E6B4
			public object GetStruct(Type t)
			{
				Queue queue = this.GetQueue(t, true);
				object result;
				if (queue.Count == 0)
				{
					result = Activator.CreateInstance(t);
				}
				else
				{
					result = queue.Dequeue();
				}
				return result;
			}

			// Token: 0x06006458 RID: 25688 RVA: 0x0016F6E8 File Offset: 0x0016E6E8
			public void ReleaseStruct(object str)
			{
				Type type = str.GetType();
				Queue queue = this.GetQueue(type, false);
				if (queue != null)
				{
					queue.Enqueue(str);
				}
			}

			// Token: 0x04003BC1 RID: 15297
			private Hashtable queuedTypes = new Hashtable();
		}

		// Token: 0x02000774 RID: 1908
		private class PropInfo
		{
			// Token: 0x17001524 RID: 5412
			// (get) Token: 0x0600645A RID: 25690 RVA: 0x0016F722 File Offset: 0x0016E722
			// (set) Token: 0x0600645B RID: 25691 RVA: 0x0016F72A File Offset: 0x0016E72A
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			// Token: 0x17001525 RID: 5413
			// (get) Token: 0x0600645C RID: 25692 RVA: 0x0016F733 File Offset: 0x0016E733
			// (set) Token: 0x0600645D RID: 25693 RVA: 0x0016F73B File Offset: 0x0016E73B
			public int DispId
			{
				get
				{
					return this.dispid;
				}
				set
				{
					this.dispid = value;
				}
			}

			// Token: 0x17001526 RID: 5414
			// (get) Token: 0x0600645E RID: 25694 RVA: 0x0016F744 File Offset: 0x0016E744
			// (set) Token: 0x0600645F RID: 25695 RVA: 0x0016F74C File Offset: 0x0016E74C
			public Type ValueType
			{
				get
				{
					return this.valueType;
				}
				set
				{
					this.valueType = value;
				}
			}

			// Token: 0x17001527 RID: 5415
			// (get) Token: 0x06006460 RID: 25696 RVA: 0x0016F755 File Offset: 0x0016E755
			public ArrayList Attributes
			{
				get
				{
					return this.attributes;
				}
			}

			// Token: 0x17001528 RID: 5416
			// (get) Token: 0x06006461 RID: 25697 RVA: 0x0016F75D File Offset: 0x0016E75D
			// (set) Token: 0x06006462 RID: 25698 RVA: 0x0016F765 File Offset: 0x0016E765
			public int ReadOnly
			{
				get
				{
					return this.readOnly;
				}
				set
				{
					this.readOnly = value;
				}
			}

			// Token: 0x17001529 RID: 5417
			// (get) Token: 0x06006463 RID: 25699 RVA: 0x0016F76E File Offset: 0x0016E76E
			// (set) Token: 0x06006464 RID: 25700 RVA: 0x0016F776 File Offset: 0x0016E776
			public bool IsDefault
			{
				get
				{
					return this.isDefault;
				}
				set
				{
					this.isDefault = value;
				}
			}

			// Token: 0x1700152A RID: 5418
			// (get) Token: 0x06006465 RID: 25701 RVA: 0x0016F77F File Offset: 0x0016E77F
			// (set) Token: 0x06006466 RID: 25702 RVA: 0x0016F787 File Offset: 0x0016E787
			public object TypeData
			{
				get
				{
					return this.typeData;
				}
				set
				{
					this.typeData = value;
				}
			}

			// Token: 0x1700152B RID: 5419
			// (get) Token: 0x06006467 RID: 25703 RVA: 0x0016F790 File Offset: 0x0016E790
			// (set) Token: 0x06006468 RID: 25704 RVA: 0x0016F798 File Offset: 0x0016E798
			public bool NonBrowsable
			{
				get
				{
					return this.nonbrowsable;
				}
				set
				{
					this.nonbrowsable = value;
				}
			}

			// Token: 0x1700152C RID: 5420
			// (get) Token: 0x06006469 RID: 25705 RVA: 0x0016F7A1 File Offset: 0x0016E7A1
			// (set) Token: 0x0600646A RID: 25706 RVA: 0x0016F7A9 File Offset: 0x0016E7A9
			public int Index
			{
				get
				{
					return this.index;
				}
				set
				{
					this.index = value;
				}
			}

			// Token: 0x0600646B RID: 25707 RVA: 0x0016F7B2 File Offset: 0x0016E7B2
			public override int GetHashCode()
			{
				if (this.name != null)
				{
					return this.name.GetHashCode();
				}
				return base.GetHashCode();
			}

			// Token: 0x04003BC2 RID: 15298
			public const int ReadOnlyUnknown = 0;

			// Token: 0x04003BC3 RID: 15299
			public const int ReadOnlyTrue = 1;

			// Token: 0x04003BC4 RID: 15300
			public const int ReadOnlyFalse = 2;

			// Token: 0x04003BC5 RID: 15301
			private string name;

			// Token: 0x04003BC6 RID: 15302
			private int dispid = -1;

			// Token: 0x04003BC7 RID: 15303
			private Type valueType;

			// Token: 0x04003BC8 RID: 15304
			private readonly ArrayList attributes = new ArrayList();

			// Token: 0x04003BC9 RID: 15305
			private int readOnly;

			// Token: 0x04003BCA RID: 15306
			private bool isDefault;

			// Token: 0x04003BCB RID: 15307
			private object typeData;

			// Token: 0x04003BCC RID: 15308
			private bool nonbrowsable;

			// Token: 0x04003BCD RID: 15309
			private int index;
		}
	}
}
