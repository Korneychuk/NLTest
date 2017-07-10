using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using NLTestApp.Models;
using System.Collections.ObjectModel;

namespace NLTestApp.ViewModels
{
	class MainViewModel : BaseViewModel
	{
		const int LINES = 25;

		public MainViewModel()
		{
			Bootstrapper.Run();
			Martians = new ObservableCollection<Martian>();
			getMartians();
			ImportDataCommand = new RelayCommand(async () => { await Task.Run(OnImportDataAsync); });
			ExportDataCommand = new RelayCommand(async () => { await Task.Run(OnExportDataAsync); });
			UpdateCommand = new RelayCommand(async () => { await Task.Run(OnUpdate); });
			RemoveMartianCommand = new RelayCommand<Martian>(OnRemoveMartian);
			OpenSettingsDialogCommand = new RelayCommand(OnOpenSettingsDialog);
			IncrementPageCommand = new RelayCommand(OnIncrementPage);
			DecrementPageCommand = new RelayCommand(OnDecrementPage);
		}

		public ObservableCollection<Martian> Martians { get; set; }

		int _martianCount;
		int _pageNo;
		public int MaxPage
		{
			get
			{
				var res = (double) _martianCount / LINES;
				return (int) Math.Ceiling(res);
			}
		}

		public int UIPageNo => _pageNo + 1;

		public RelayCommand ImportDataCommand { get; set; }
		async Task OnImportDataAsync()
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = "CSV files (*.csv)|*.csv|TSV files (*.tsv)|*.tsv",
				RestoreDirectory = true
			};

			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					var martians = new List<Martian>();
					using (var parser = new TextFieldParser(openFileDialog.OpenFile()))
					{
						parser.TextFieldType = FieldType.Delimited;
						parser.SetDelimiters(openFileDialog.FilterIndex == 1 ? "," : "\t");
						while (!parser.EndOfData)
						{
							var martian = new Martian(parser.ReadFields());
							if (!string.IsNullOrEmpty(martian.FullName) && !string.IsNullOrEmpty(martian.BirthDate) && (!string.IsNullOrEmpty(martian.Phone) || !string.IsNullOrEmpty(martian.Email)))
								martians.Add(martian);
						}
						AddMartians(martians);
					}
						Bootstrapper.DataBaseContext.Martians.AddRange(martians);
						await Bootstrapper.DataBaseContext.SaveChangesAsync();
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
			try
			{
				var martians = new List<Martian>();
				await Bootstrapper.DataBaseContext.Martians.ForEachAsync(x => martians.Add(x));
				using (var writer = new StreamWriter("martians.tsv"))
					martians.ForEach(x => writer.WriteLine($"{x.FullName}\t{x.BirthDate}\t{x.Email}\t{x.Phone}"));
				MessageBox.Show("Экспорт данных завершен");
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		public RelayCommand OpenSettingsDialogCommand { get; set; }
		void OnOpenSettingsDialog()
		{
			var settingsDialog = new SettingsViewModel();
			settingsDialog.ShowDialog(settingsDialog, 200, 400);
		}

		List<Martian> _removedMartians = new List<Martian>();
		public RelayCommand<Martian> RemoveMartianCommand { get; private set; }
		void OnRemoveMartian(Martian martian)
		{
			if (martian != null)
			{
				Martians.Remove(martian);
				_removedMartians.Add(martian);
			}
		}

		public RelayCommand IncrementPageCommand { get; private set; }
		void OnIncrementPage()
		{
			if (_pageNo < MaxPage - 1)
			{
				_pageNo++;
				getMartians();
			}
		}

		public RelayCommand DecrementPageCommand { get; private set; }
		void OnDecrementPage()
		{
			if (_pageNo != 0)
			{
				_pageNo--;
				getMartians();
			}
		}

		public RelayCommand UpdateCommand { get; private set; }
		async Task OnUpdate()
		{
			Bootstrapper.DataBaseContext.Martians.RemoveRange(_removedMartians);
			_removedMartians = new List<Martian>();
			foreach (var martian in Martians)
			{
				var martianInBase = await Bootstrapper.DataBaseContext.Martians.SingleOrDefaultAsync(x => x.Id == martian.Id);
				martianInBase.FullName = martian.FullName;
				martianInBase.BirthDate = martian.BirthDate;
				martianInBase.Phone = martian.Phone;
				martianInBase.Email = martian.Email;
			}
			await Bootstrapper.DataBaseContext.SaveChangesAsync();
			MessageBox.Show("Обновление базы данных завершено");
		}

		void AddMartians(List<Martian> martians)
		{
			App.Current.Dispatcher.Invoke(delegate
			{
				martians.ForEach(x => Martians.Add(x));
			});
		}

		async Task Initialize ()
		{
			var martians = new List<Martian>();
			_martianCount = Bootstrapper.DataBaseContext.Martians.Count();
			await Bootstrapper.DataBaseContext.Martians.ForEachAsync(x => martians.Add(x));
			AddMartians(martians);
		}

		void getMartians()
		{
			Martians.Clear();
			_martianCount = Bootstrapper.DataBaseContext.Martians.Count();
			AddMartians(Bootstrapper.DataBaseContext.Martians.OrderBy(x=>x.Id).Skip(_pageNo * LINES).Take(LINES).ToList());
			OnPropertyChanged(() => UIPageNo);
			OnPropertyChanged(() => MaxPage);
		}
	}
}