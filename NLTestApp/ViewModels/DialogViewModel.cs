using NLTestApp.ViewModels;
using System.Windows;

namespace NLTestApp.ViewModels
{
	class DialogViewModel: BaseViewModel
	{
		Window window = new Window();
		public bool? ShowDialog(object dialogViewModel, int? height = null, int? width = null)
		{
			window.Content = dialogViewModel;
			window.MaxHeight = height.Value;
			window.MaxWidth = width.Value;
			return window.ShowDialog();
		}

		public void CloseDialog(object dialogViewModel){
			window.Content = dialogViewModel;
			window.Close();
		}
	}
}