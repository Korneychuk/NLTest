namespace NLTestApp.Models
{
	public class Martian
	{
		public Martian()
		{
		}

		public Martian(string[] properties)
		{
			FullName = properties[0];
			BirthDate = properties[1].Replace("-", "/").Replace(".", "/").Replace("\\", "/").Replace("|", "/");
			Email = properties[2].ToLower();
			Phone = properties[3].Replace("-", "").Replace("(","").Replace(")", "").Replace("+", "").Replace(" ","");
		}

		public int Id { get; set; }

		public string FullName { get; set; }

		public string BirthDate { get; set; }

		public string Email { get; set; }

		public string Phone { get; set; }
	}
}