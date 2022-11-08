using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Xml.XPath;
using System.Xml.Xsl.IlGen;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl
{
	// Token: 0x0200000C RID: 12
	internal class XmlILGenerator
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002678 File Offset: 0x00001678
		public XmlILCommand Generate(QilExpression query, TypeBuilder typeBldr)
		{
			this.qil = query;
			bool useLRE = !this.qil.IsDebug && typeBldr == null;
			bool isDebug = this.qil.IsDebug;
			XmlILTrace.WriteQil(this.qil, "qilbefore.xml");
			XmlILTrace.TraceOptimizations(this.qil, "qilopt.xml");
			if (XmlILTrace.IsEnabled)
			{
				useLRE = false;
			}
			this.optVisitor = new XmlILOptimizerVisitor(this.qil, !this.qil.IsDebug);
			this.qil = this.optVisitor.Optimize();
			XmlILTrace.WriteQil(this.qil, "qilafter.xml");
			XmlILCommand result;
			try
			{
				XmlILModule.CreateModulePermissionSet.Assert();
				if (typeBldr != null)
				{
					this.module = new XmlILModule(typeBldr);
				}
				else
				{
					this.module = new XmlILModule(useLRE, isDebug);
				}
				this.helper = new GenerateHelper(this.module, this.qil.IsDebug);
				this.CreateHelperFunctions();
				MethodInfo methExec = this.module.DefineMethod("Execute", typeof(void), new Type[0], new string[0], XmlILMethodAttributes.NonUser);
				XmlILMethodAttributes xmlAttrs = (this.qil.Root.SourceLine == null) ? XmlILMethodAttributes.NonUser : XmlILMethodAttributes.None;
				MethodInfo methRoot = this.module.DefineMethod("Root", typeof(void), new Type[0], new string[0], xmlAttrs);
				foreach (EarlyBoundInfo earlyBoundInfo in this.qil.EarlyBoundTypes)
				{
					this.helper.StaticData.DeclareEarlyBound(earlyBoundInfo.NamespaceUri, earlyBoundInfo.EarlyBoundType);
				}
				this.CreateFunctionMetadata(this.qil.FunctionList);
				this.CreateGlobalValueMetadata(this.qil.GlobalVariableList);
				this.CreateGlobalValueMetadata(this.qil.GlobalParameterList);
				this.GenerateExecuteFunction(methExec, methRoot);
				this.xmlIlVisitor = new XmlILVisitor();
				this.xmlIlVisitor.Visit(this.qil, this.helper, methRoot);
				XmlQueryStaticData staticData = new XmlQueryStaticData(this.qil.DefaultWriterSettings, this.qil.WhitespaceRules, this.helper.StaticData);
				if (typeBldr != null)
				{
					this.CreateTypeInitializer(staticData);
				}
				this.module.BakeMethods();
				ExecuteDelegate delExec = (ExecuteDelegate)this.module.CreateDelegate("Execute", typeof(ExecuteDelegate));
				result = new XmlILCommand(delExec, staticData);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return result;
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002924 File Offset: 0x00001924
		private void CreateFunctionMetadata(IList<QilNode> funcList)
		{
			foreach (QilNode qilNode in funcList)
			{
				QilFunction qilFunction = (QilFunction)qilNode;
				Type[] array = new Type[qilFunction.Arguments.Count];
				string[] array2 = new string[qilFunction.Arguments.Count];
				for (int i = 0; i < qilFunction.Arguments.Count; i++)
				{
					QilParameter qilParameter = (QilParameter)qilFunction.Arguments[i];
					array[i] = XmlILTypeHelper.GetStorageType(qilParameter.XmlType);
					if (qilParameter.DebugName != null)
					{
						array2[i] = qilParameter.DebugName;
					}
				}
				Type returnType;
				if (XmlILConstructInfo.Read(qilFunction).PushToWriterLast)
				{
					returnType = typeof(void);
				}
				else
				{
					returnType = XmlILTypeHelper.GetStorageType(qilFunction.XmlType);
				}
				XmlILMethodAttributes xmlAttrs = (qilFunction.SourceLine == null) ? XmlILMethodAttributes.NonUser : XmlILMethodAttributes.None;
				MethodInfo functionBinding = this.module.DefineMethod(qilFunction.DebugName, returnType, array, array2, xmlAttrs);
				for (int j = 0; j < qilFunction.Arguments.Count; j++)
				{
					XmlILAnnotation.Write(qilFunction.Arguments[j]).ArgumentPosition = j;
				}
				XmlILAnnotation.Write(qilFunction).FunctionBinding = functionBinding;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002A8C File Offset: 0x00001A8C
		private void CreateGlobalValueMetadata(IList<QilNode> globalList)
		{
			foreach (QilNode qilNode in globalList)
			{
				QilReference qilReference = (QilReference)qilNode;
				Type storageType = XmlILTypeHelper.GetStorageType(qilReference.XmlType);
				XmlILMethodAttributes xmlAttrs = (qilReference.SourceLine == null) ? XmlILMethodAttributes.NonUser : XmlILMethodAttributes.None;
				MethodInfo functionBinding = this.module.DefineMethod(qilReference.DebugName.ToString(), storageType, new Type[0], new string[0], xmlAttrs);
				XmlILAnnotation.Write(qilReference).FunctionBinding = functionBinding;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B24 File Offset: 0x00001B24
		private MethodInfo GenerateExecuteFunction(MethodInfo methExec, MethodInfo methRoot)
		{
			this.helper.MethodBegin(methExec, null, false);
			this.EvaluateGlobalValues(this.qil.GlobalVariableList);
			this.EvaluateGlobalValues(this.qil.GlobalParameterList);
			this.helper.LoadQueryRuntime();
			this.helper.Call(methRoot);
			this.helper.MethodEnd();
			return methExec;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B84 File Offset: 0x00001B84
		private void CreateHelperFunctions()
		{
			XmlILModule xmlILModule = this.module;
			string name = "SyncToNavigator";
			Type typeFromHandle = typeof(XPathNavigator);
			Type[] paramTypes = new Type[]
			{
				typeof(XPathNavigator),
				typeof(XPathNavigator)
			};
			string[] paramNames = new string[2];
			MethodInfo methInfo = xmlILModule.DefineMethod(name, typeFromHandle, paramTypes, paramNames, (XmlILMethodAttributes)3);
			this.helper.MethodBegin(methInfo, null, false);
			Label label = this.helper.DefineLabel();
			this.helper.Emit(OpCodes.Ldarg_0);
			this.helper.Emit(OpCodes.Brfalse, label);
			this.helper.Emit(OpCodes.Ldarg_0);
			this.helper.Emit(OpCodes.Ldarg_1);
			this.helper.Call(XmlILMethods.NavMoveTo);
			this.helper.Emit(OpCodes.Brfalse, label);
			this.helper.Emit(OpCodes.Ldarg_0);
			this.helper.Emit(OpCodes.Ret);
			this.helper.MarkLabel(label);
			this.helper.Emit(OpCodes.Ldarg_1);
			this.helper.Call(XmlILMethods.NavClone);
			this.helper.MethodEnd();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002CAC File Offset: 0x00001CAC
		private void EvaluateGlobalValues(IList<QilNode> iterList)
		{
			foreach (QilNode qilNode in iterList)
			{
				QilIterator nd = (QilIterator)qilNode;
				if (this.qil.IsDebug || OptimizerPatterns.Read(nd).MatchesPattern(OptimizerPatternName.MaybeSideEffects))
				{
					MethodInfo functionBinding = XmlILAnnotation.Write(nd).FunctionBinding;
					this.helper.LoadQueryRuntime();
					this.helper.Call(functionBinding);
					this.helper.Emit(OpCodes.Pop);
				}
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002D44 File Offset: 0x00001D44
		public void CreateTypeInitializer(XmlQueryStaticData staticData)
		{
			byte[] array;
			Type[] array2;
			staticData.GetObjectData(out array, out array2);
			FieldInfo fldInfo = this.module.DefineInitializedData("__staticData", array);
			FieldInfo fldInfo2 = this.module.DefineField("staticData", typeof(object));
			FieldInfo fldInfo3 = this.module.DefineField("ebTypes", typeof(Type[]));
			ConstructorInfo methInfo = this.module.DefineTypeInitializer();
			this.helper.MethodBegin(methInfo, null, false);
			this.helper.LoadInteger(array.Length);
			this.helper.Emit(OpCodes.Newarr, typeof(byte));
			this.helper.Emit(OpCodes.Dup);
			this.helper.Emit(OpCodes.Ldtoken, fldInfo);
			this.helper.Call(XmlILMethods.InitializeArray);
			this.helper.Emit(OpCodes.Stsfld, fldInfo2);
			if (array2 != null)
			{
				LocalBuilder locBldr = this.helper.DeclareLocal("$$$types", typeof(Type[]));
				this.helper.LoadInteger(array2.Length);
				this.helper.Emit(OpCodes.Newarr, typeof(Type));
				this.helper.Emit(OpCodes.Stloc, locBldr);
				for (int i = 0; i < array2.Length; i++)
				{
					this.helper.Emit(OpCodes.Ldloc, locBldr);
					this.helper.LoadInteger(i);
					this.helper.LoadType(array2[i]);
					this.helper.Emit(OpCodes.Stelem_Ref);
				}
				this.helper.Emit(OpCodes.Ldloc, locBldr);
				this.helper.Emit(OpCodes.Stsfld, fldInfo3);
			}
			this.helper.MethodEnd();
		}

		// Token: 0x040000BE RID: 190
		private QilExpression qil;

		// Token: 0x040000BF RID: 191
		private GenerateHelper helper;

		// Token: 0x040000C0 RID: 192
		private XmlILOptimizerVisitor optVisitor;

		// Token: 0x040000C1 RID: 193
		private XmlILVisitor xmlIlVisitor;

		// Token: 0x040000C2 RID: 194
		private XmlILModule module;
	}
}
