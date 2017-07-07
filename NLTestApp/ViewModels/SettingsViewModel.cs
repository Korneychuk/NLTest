using System.IO;
using System.Windows;
using System.Xml.Serialization;
using NLTestApp.Models;

namespace NLTestApp.ViewModels
{
	class SettingsViewModel : BaseViewModel
	{
		public SettingsViewModel(Settings settings)
		{
			ConnectionString = settings.ConnectionString;
			SetConnectionStingCommand = new RelayCommand(OnSetConnectionSting);
		}

		public RelayCommand SetConnectionStingCommand { get; set; }
		void OnSetConnectionSting()
		{
			var settings = new Settings { ConnectionString = ConnectionString };
			var xml = new XmlSerializer(typeof(Settings));
			using (var writer = new StreamWriter("app_settings.xml"))
				xml.Serialize(writer, settings);
			if (MessageBox.Show("Перезапустить приложение для вступлений изменений в силу?") == MessageBoxResult.OK)
			{
				System.Windows.Forms.Application.Restart();
				Application.Current.Shutdown();
			}
		}

		string _connectionString;public string ConnectionString
		{
			get => _connectionString;
			set
			{
				_connectionString = value;
				OnPropertyChanged(() => ConnectionString);
			}
		}
	}
}