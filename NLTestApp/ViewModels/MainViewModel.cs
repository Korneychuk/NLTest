using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using NLTestApp.Models;

namespace NLTestApp.ViewModels
{
	class MainViewModel : BaseViewModel
	{
		public MainViewModel()
		{
			Bootstrapper.Run();
			Martians = new ObservableCollection<Martian>();
			ImportDataCommand = new RelayCommand(async () => { await Task.Run(OnImportDataAsync); });
			ExportDataCommand = new RelayCommand(async () => { await Task.Run(OnExportDataAsync); });
			SetConnectionStingCommand = new RelayCommand(OnSetConnectionSting);
		}

		public ObservableCollection<Martian> Martians { get; set; }

		public string ConnectionString { get; set; }

		public RelayCommand ImportDataCommand { get; set; }
		//Если выбран фильтр - csv, предполагается, что будет загружен файл с разделителями - запятыми
		//В остальных случаях, предполагается, что будет загружен файл с разделителями - табами (tsv)
		async Task OnImportDataAsync()
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "CSV files (*.csv)|*.csv|TSV files (*.tsv)|*.tsv", /*| All files(*.*) | *.**/
				RestoreDirectory = true
			};

			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					using (var parser = new TextFieldParser(openFileDialog.OpenFile()))
					{
						parser.TextFieldType = FieldType.Delimited;
						parser.SetDelimiters(openFileDialog.FilterIndex == 1 ? "," : "\t");
						while (!parser.EndOfData)
							Martians.Add(new Martian(parser.ReadFields()));
					}
					//using (var db = Bootstrapper.DataBaseContext)
					//{
						Bootstrapper.DataBaseContext.Martians.AddRange(Martians.ToList());
						await Bootstrapper.DataBaseContext.SaveChangesAsync();
					//}
					MessageBox.Show("Импорт данных завершен");
				}
				catch (Exception e)
				{
					MessageBox.Show(e.Message);
				}
			}
		}

		public RelayCommand ExportDataCommand { get; set; }
		async Task OnExportDataAsync()
		{
			var martians = new List<Martian>();
			//using (var db = Bootstrapper.DataBaseContext)
				await Bootstrapper.DataBaseContext.Martians.ForEachAsync(x => martians.Add(x));
			using (var writer = new StreamWriter("martians.tsv"))
				martians.ForEach(x => writer.WriteLine($"{x.FullName}\t{x.BirthDate}\t{x.Email}\t{x.Phone}"));
			MessageBox.Show("Экспорт данных завершен");
		}

		public RelayCommand SetConnectionStingCommand { get; set; }
		void OnSetConnectionSting()
		{
			var settings = new Settings {ConnectionString = ConnectionString};
			var xml = new XmlSerializer(typeof(Settings));
			using (var writer = new StreamWriter("app_settings.xml"))
				xml.Serialize(writer, settings);
			if (MessageBox.Show("Перезапустить приложение для вступлений изменений в силу?") == MessageBoxResult.OK)
			{
				System.Windows.Forms.Application.Restart();
				Application.Current.Shutdown();
			}
		}
	}
}