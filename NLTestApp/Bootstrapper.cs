using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLTestApp.Models;

namespace NLTestApp
{
	public class Bootstrapper
	{
		public static void Run()
		{
			var connectionString = "";
			using (var db = new DatabaseContext(connectionString))
			{
				
			}
		}
	}
}