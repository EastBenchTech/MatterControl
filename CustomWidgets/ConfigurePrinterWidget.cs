﻿/*
Copyright (c) 2018, John Lewin
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System.Linq;
using MatterHackers.Agg;
using MatterHackers.Agg.UI;
using MatterHackers.MatterControl.CustomWidgets;
using MatterHackers.MatterControl.SlicerConfiguration;

namespace MatterHackers.MatterControl
{
	public class ConfigurePrinterWidget : FlowLayoutWidget
	{
		private SliceSettingsTabView sliceSettingsTabView;
		private SettingsContext settingsContext;
		private PrinterConfig printer;
		private ThemeConfig theme;

		public ConfigurePrinterWidget(SettingsContext settingsContext, PrinterConfig printer, ThemeConfig theme)
			: base(FlowDirection.TopToBottom)
		{
			this.settingsContext = settingsContext;
			this.printer = printer;
			this.theme = theme;

			this.RebuildTabView();

			ApplicationController.Instance.ShowHelpChanged += ShowHelp_Changed;
		}

		private void RebuildTabView()
		{
			this.CloseAllChildren();

			var inlineTitleEdit = new InlineTitleEdit(printer.Settings.GetValue(SettingsKey.printer_name), theme, boldFont: true);
			inlineTitleEdit.TitleChanged += (s, e) =>
			{
				printer.Settings.SetValue(SettingsKey.printer_name, inlineTitleEdit.Text);
			};
			this.AddChild(inlineTitleEdit);

			this.AddChild(
				sliceSettingsTabView = new SliceSettingsTabView(
					settingsContext,
					printer,
					"Printer",
					theme,
					isPrimarySettingsView: true,
					databaseMRUKey: UserSettingsKey.ConfigurePrinter_CurrentTab));
		}

		private void ShowHelp_Changed(object sender, System.EventArgs e)
		{
			this.RebuildTabView();
		}

		public override void OnClosed(ClosedEventArgs e)
		{
			ApplicationController.Instance.ShowHelpChanged -= ShowHelp_Changed;
			base.OnClosed(e);
		}
	}
}
