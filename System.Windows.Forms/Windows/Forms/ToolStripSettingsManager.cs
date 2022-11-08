using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
	// Token: 0x020006DF RID: 1759
	internal class ToolStripSettingsManager
	{
		// Token: 0x06005CD6 RID: 23766 RVA: 0x00150FBE File Offset: 0x0014FFBE
		internal ToolStripSettingsManager(Form owner, string formKey)
		{
			this.form = owner;
			this.formKey = formKey;
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x00150FD4 File Offset: 0x0014FFD4
		internal void Load()
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.FindToolStrips(true, this.form.Controls))
			{
				ToolStrip toolStrip = (ToolStrip)obj;
				if (toolStrip != null && !string.IsNullOrEmpty(toolStrip.Name))
				{
					ToolStripSettings toolStripSettings = new ToolStripSettings(this.GetSettingsKey(toolStrip));
					if (!toolStripSettings.IsDefault)
					{
						arrayList.Add(new ToolStripSettingsManager.SettingsStub(toolStripSettings));
					}
				}
			}
			this.ApplySettings(arrayList);
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x0015107C File Offset: 0x0015007C
		internal void Save()
		{
			foreach (object obj in this.FindToolStrips(true, this.form.Controls))
			{
				ToolStrip toolStrip = (ToolStrip)obj;
				if (toolStrip != null && !string.IsNullOrEmpty(toolStrip.Name))
				{
					ToolStripSettings toolStripSettings = new ToolStripSettings(this.GetSettingsKey(toolStrip));
					ToolStripSettingsManager.SettingsStub settingsStub = new ToolStripSettingsManager.SettingsStub(toolStrip);
					toolStripSettings.ItemOrder = settingsStub.ItemOrder;
					toolStripSettings.Name = settingsStub.Name;
					toolStripSettings.Location = settingsStub.Location;
					toolStripSettings.Size = settingsStub.Size;
					toolStripSettings.ToolStripPanelName = settingsStub.ToolStripPanelName;
					toolStripSettings.Visible = settingsStub.Visible;
					toolStripSettings.Save();
				}
			}
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x0015115C File Offset: 0x0015015C
		internal static string GetItemOrder(ToolStrip toolStrip)
		{
			StringBuilder stringBuilder = new StringBuilder(toolStrip.Items.Count);
			for (int i = 0; i < toolStrip.Items.Count; i++)
			{
				stringBuilder.Append((toolStrip.Items[i].Name == null) ? "null" : toolStrip.Items[i].Name);
				if (i != toolStrip.Items.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x001511E4 File Offset: 0x001501E4
		private void ApplySettings(ArrayList toolStripSettingsToApply)
		{
			if (toolStripSettingsToApply.Count == 0)
			{
				return;
			}
			this.SuspendAllLayout(this.form);
			Dictionary<string, ToolStrip> itemLocationHash = this.BuildItemOriginationHash();
			Dictionary<object, List<ToolStripSettingsManager.SettingsStub>> dictionary = new Dictionary<object, List<ToolStripSettingsManager.SettingsStub>>();
			foreach (object obj in toolStripSettingsToApply)
			{
				ToolStripSettingsManager.SettingsStub settingsStub = (ToolStripSettingsManager.SettingsStub)obj;
				object obj2 = (!string.IsNullOrEmpty(settingsStub.ToolStripPanelName)) ? settingsStub.ToolStripPanelName : null;
				if (obj2 == null)
				{
					if (!string.IsNullOrEmpty(settingsStub.Name))
					{
						ToolStrip toolStrip = ToolStripManager.FindToolStrip(this.form, settingsStub.Name);
						this.ApplyToolStripSettings(toolStrip, settingsStub, itemLocationHash);
					}
				}
				else
				{
					if (!dictionary.ContainsKey(obj2))
					{
						dictionary[obj2] = new List<ToolStripSettingsManager.SettingsStub>();
					}
					dictionary[obj2].Add(settingsStub);
				}
			}
			ArrayList arrayList = this.FindToolStripPanels(true, this.form.Controls);
			foreach (object obj3 in arrayList)
			{
				ToolStripPanel toolStripPanel = (ToolStripPanel)obj3;
				foreach (object obj4 in toolStripPanel.Controls)
				{
					Control control = (Control)obj4;
					control.Visible = false;
				}
				string text = toolStripPanel.Name;
				if (string.IsNullOrEmpty(text) && toolStripPanel.Parent is ToolStripContainer && !string.IsNullOrEmpty(toolStripPanel.Parent.Name))
				{
					text = toolStripPanel.Parent.Name + "." + toolStripPanel.Dock.ToString();
				}
				toolStripPanel.BeginInit();
				if (dictionary.ContainsKey(text))
				{
					List<ToolStripSettingsManager.SettingsStub> list = dictionary[text];
					if (list != null)
					{
						foreach (ToolStripSettingsManager.SettingsStub settings in list)
						{
							if (!string.IsNullOrEmpty(settings.Name))
							{
								ToolStrip toolStrip2 = ToolStripManager.FindToolStrip(this.form, settings.Name);
								this.ApplyToolStripSettings(toolStrip2, settings, itemLocationHash);
								toolStripPanel.Join(toolStrip2, settings.Location);
							}
						}
					}
				}
				toolStripPanel.EndInit();
			}
			this.ResumeAllLayout(this.form, true);
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x001514B0 File Offset: 0x001504B0
		private void ApplyToolStripSettings(ToolStrip toolStrip, ToolStripSettingsManager.SettingsStub settings, Dictionary<string, ToolStrip> itemLocationHash)
		{
			if (toolStrip != null)
			{
				toolStrip.Visible = settings.Visible;
				toolStrip.Size = settings.Size;
				string itemOrder = settings.ItemOrder;
				if (!string.IsNullOrEmpty(itemOrder))
				{
					string[] array = itemOrder.Split(new char[]
					{
						','
					});
					Regex regex = new Regex("(\\S+)");
					int num = 0;
					while (num < toolStrip.Items.Count && num < array.Length)
					{
						Match match = regex.Match(array[num]);
						if (match != null && match.Success)
						{
							string value = match.Value;
							if (!string.IsNullOrEmpty(value) && itemLocationHash.ContainsKey(value))
							{
								toolStrip.Items.Insert(num, itemLocationHash[value].Items[value]);
							}
						}
						num++;
					}
				}
			}
		}

		// Token: 0x06005CDC RID: 23772 RVA: 0x00151584 File Offset: 0x00150584
		private Dictionary<string, ToolStrip> BuildItemOriginationHash()
		{
			ArrayList arrayList = this.FindToolStrips(true, this.form.Controls);
			Dictionary<string, ToolStrip> dictionary = new Dictionary<string, ToolStrip>();
			if (arrayList != null)
			{
				foreach (object obj in arrayList)
				{
					ToolStrip toolStrip = (ToolStrip)obj;
					foreach (object obj2 in toolStrip.Items)
					{
						ToolStripItem toolStripItem = (ToolStripItem)obj2;
						if (!string.IsNullOrEmpty(toolStripItem.Name))
						{
							dictionary[toolStripItem.Name] = toolStrip;
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x0015165C File Offset: 0x0015065C
		private ArrayList FindControls(Type baseType, bool searchAllChildren, Control.ControlCollection controlsToLookIn, ArrayList foundControls)
		{
			if (controlsToLookIn == null || foundControls == null)
			{
				return null;
			}
			try
			{
				for (int i = 0; i < controlsToLookIn.Count; i++)
				{
					if (controlsToLookIn[i] != null && baseType.IsAssignableFrom(controlsToLookIn[i].GetType()))
					{
						foundControls.Add(controlsToLookIn[i]);
					}
				}
				if (searchAllChildren)
				{
					for (int j = 0; j < controlsToLookIn.Count; j++)
					{
						if (controlsToLookIn[j] != null && !(controlsToLookIn[j] is Form) && controlsToLookIn[j].Controls != null && controlsToLookIn[j].Controls.Count > 0)
						{
							foundControls = this.FindControls(baseType, searchAllChildren, controlsToLookIn[j].Controls, foundControls);
						}
					}
				}
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			return foundControls;
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x00151738 File Offset: 0x00150738
		private ArrayList FindToolStripPanels(bool searchAllChildren, Control.ControlCollection controlsToLookIn)
		{
			return this.FindControls(typeof(ToolStripPanel), true, this.form.Controls, new ArrayList());
		}

		// Token: 0x06005CDF RID: 23775 RVA: 0x0015175B File Offset: 0x0015075B
		private ArrayList FindToolStrips(bool searchAllChildren, Control.ControlCollection controlsToLookIn)
		{
			return this.FindControls(typeof(ToolStrip), true, this.form.Controls, new ArrayList());
		}

		// Token: 0x06005CE0 RID: 23776 RVA: 0x0015177E File Offset: 0x0015077E
		private string GetSettingsKey(ToolStrip toolStrip)
		{
			if (toolStrip != null)
			{
				return this.formKey + "." + toolStrip.Name;
			}
			return string.Empty;
		}

		// Token: 0x06005CE1 RID: 23777 RVA: 0x001517A0 File Offset: 0x001507A0
		private void ResumeAllLayout(Control start, bool performLayout)
		{
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.ResumeAllLayout(controls[i], performLayout);
			}
			start.ResumeLayout(performLayout);
		}

		// Token: 0x06005CE2 RID: 23778 RVA: 0x001517DC File Offset: 0x001507DC
		private void SuspendAllLayout(Control start)
		{
			start.SuspendLayout();
			Control.ControlCollection controls = start.Controls;
			for (int i = 0; i < controls.Count; i++)
			{
				this.SuspendAllLayout(controls[i]);
			}
		}

		// Token: 0x04003926 RID: 14630
		private Form form;

		// Token: 0x04003927 RID: 14631
		private string formKey;

		// Token: 0x020006E0 RID: 1760
		private struct SettingsStub
		{
			// Token: 0x06005CE3 RID: 23779 RVA: 0x00151814 File Offset: 0x00150814
			public SettingsStub(ToolStrip toolStrip)
			{
				this.ToolStripPanelName = string.Empty;
				ToolStripPanel toolStripPanel = toolStrip.Parent as ToolStripPanel;
				if (toolStripPanel != null)
				{
					if (!string.IsNullOrEmpty(toolStripPanel.Name))
					{
						this.ToolStripPanelName = toolStripPanel.Name;
					}
					else if (toolStripPanel.Parent is ToolStripContainer && !string.IsNullOrEmpty(toolStripPanel.Parent.Name))
					{
						this.ToolStripPanelName = toolStripPanel.Parent.Name + "." + toolStripPanel.Dock.ToString();
					}
				}
				this.Visible = toolStrip.Visible;
				this.Size = toolStrip.Size;
				this.Location = toolStrip.Location;
				this.Name = toolStrip.Name;
				this.ItemOrder = ToolStripSettingsManager.GetItemOrder(toolStrip);
			}

			// Token: 0x06005CE4 RID: 23780 RVA: 0x001518DC File Offset: 0x001508DC
			public SettingsStub(ToolStripSettings toolStripSettings)
			{
				this.ToolStripPanelName = toolStripSettings.ToolStripPanelName;
				this.Visible = toolStripSettings.Visible;
				this.Size = toolStripSettings.Size;
				this.Location = toolStripSettings.Location;
				this.Name = toolStripSettings.Name;
				this.ItemOrder = toolStripSettings.ItemOrder;
			}

			// Token: 0x04003928 RID: 14632
			public bool Visible;

			// Token: 0x04003929 RID: 14633
			public string ToolStripPanelName;

			// Token: 0x0400392A RID: 14634
			public Point Location;

			// Token: 0x0400392B RID: 14635
			public Size Size;

			// Token: 0x0400392C RID: 14636
			public string ItemOrder;

			// Token: 0x0400392D RID: 14637
			public string Name;
		}
	}
}
