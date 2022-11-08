using System;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	// Token: 0x0200009E RID: 158
	[ComVisible(true)]
	[Designer("System.Windows.Forms.Design.ComponentDocumentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IRootDesigner))]
	[RootDesignerSerializer("System.ComponentModel.Design.Serialization.RootCodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", true)]
	[Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(IDesigner))]
	[TypeConverter(typeof(ComponentConverter))]
	public interface IComponent : IDisposable
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600059E RID: 1438
		// (set) Token: 0x0600059F RID: 1439
		ISite Site { get; set; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060005A0 RID: 1440
		// (remove) Token: 0x060005A1 RID: 1441
		event EventHandler Disposed;
	}
}
