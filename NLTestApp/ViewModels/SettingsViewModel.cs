using NLTestApp.Models;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace NLTestApp.ViewModels
{
	class SettingsViewModel : DialogViewModel
	{
		public SettingsViewModel()
		{
			Settings = Bootstrapper.Settings;
			ApplySettingsCommand = new RelayCommand(OnApplySettings);
			CancelSettingsCommand = new RelayCommand(OnCancelSettings);
		}

		public Settings Settings { get; set; }

		public RelayCommand ApplySettingsCommand { get; set; }
		void OnApplySettings()
		{
			var xml = new XmlSerializer(typeof(Settings));
			using (var writer = new StreamWriter("app_settings.xml"))
				xml.Serialize(writer, Settings);
			CloseDialog(this);
			if (MessageBox.Show("Перезапустить приложение для вступлений изменений в силу?") == MessageBoxResult.OK)
			{
				System.Windows.Forms.Application.Restart();
				Application.Current.Shutdown();
			};
		}

		public RelayCommand CancelSettingsCommand { get; set; }
		void OnCancelSettings()
		{
			CloseDialog(this);
		}
	}
}