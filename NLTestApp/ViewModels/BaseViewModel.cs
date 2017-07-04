using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NLTestApp.RelayCommand;

namespace Infrastructure.Common.Windows.ViewModels
{
	public abstract class BaseViewModel : INotifyPropertyChanged
	{
		List<PropertyInfo> _relayCommandProperties;

		public event PropertyChangedEventHandler PropertyChanged;

		public BaseViewModel()
		{
			var relayCommandFullTypeName = typeof(RelayCommand).ToString();
			_relayCommandProperties = GetType().GetProperties().Where(x => x.ToString().Contains(relayCommandFullTypeName)).ToList();
		}
		public void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(ExtractPropertyName(propertyExpression)));
		}

		static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
				throw new ArgumentNullException("propertyExpression");
			var memberExpression = propertyExpression.Body as MemberExpression;
			if (memberExpression == null)
				throw new ArgumentException("The expression is not a member access expression.", "propertyExpression");
			var property = memberExpression.Member as PropertyInfo;
			if (property == null)
				throw new ArgumentException("The member access expression does not access a property.", "propertyExpression");
			return property.Name;
		}
	}
}
