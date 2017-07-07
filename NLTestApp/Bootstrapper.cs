using System;
using System.IO;
using System.Xml.Serialization;
using NLTestApp.Models;

namespace NLTestApp
{
	static class Bootstrapper
	{
		public static DatabaseContext DataBaseContext { get; set; }
		public static Settings Settings { get; set; }

		public static void Run()
		{
			Settings = new Settings{ConnectionString = "DefaultConnection" };
			var xml = new XmlSerializer(typeof(Settings));
			try
			{
				using (Stream reader = new FileStream("app_settings.xml", FileMode.Open))
					Settings = (Settings) xml.Deserialize(reader);
				DataBaseContext = new DatabaseContext(Settings.ConnectionString);
			}
			catch (Exception)
			{
				DataBaseContext = new DatabaseContext("DefaultConnection");
			}
		}
	}
}