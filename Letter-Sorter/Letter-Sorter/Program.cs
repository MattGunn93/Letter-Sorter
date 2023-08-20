namespace Letter_Sorter
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var combinedLettersPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\CombinedLetters";
			string date = DateTime.Now.ToString("yyyyMMdd");
			var admissionDir = new DirectoryInfo(combinedLettersPath + @"\Input\Admission\" + date);
			var scholarshipDir = new DirectoryInfo(combinedLettersPath + @"\Input\scholarship\" + date);
			string archivePath = combinedLettersPath + @"\Archive\" + date;

			//if (!Directory.Exists(archivePath))
			//{
			//	Directory.CreateDirectory(archivePath);
			//}
			//else
			//{
			//	throw new Exception();
			//}
			//var archiveDir = new DirectoryInfo(archivePath);

			//LetterService.ArchiveLetters(admissionDir, archiveDir);
			//LetterService.ArchiveLetters(scholarshipDir, archiveDir);

			var admissionLetters = new Dictionary<string, string>();
			var scholarshipLetters = new Dictionary<string, string>();

			foreach(FileInfo file in admissionDir.GetFiles())
			{
				string key = Path.GetFileNameWithoutExtension(file.FullName).Split('-')[1];
				admissionLetters.Add(key, file.Name);
			}
			foreach (FileInfo file in scholarshipDir.GetFiles())
			{
				string key = Path.GetFileNameWithoutExtension(file.FullName).Split('-')[1];
				scholarshipLetters.Add(key, file.Name);
			}
		}
	}
	public interface ILetterService
	{
		/// <summary>	
		/// Combine two letter files into one file.
		/// </summary>
		/// <param name="inputFile1">File path for the first letter.</param>
		/// <param name="inputFile2">File path for the second letter.></param>
		/// <param name="resultFile">File path for the combined letter></param>
		void CombineTwoLetters(string inputFile1,string inputFile2, string resultFile);
	}

	public class LetterService : ILetterService
	{
		static Dictionary<string, string>? GetLetters()
		{
			return null;
		}
		internal static void ArchiveLetters(DirectoryInfo source, DirectoryInfo target)
		{
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name));
		}
		static void SortLetters()
		{

		}
		public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile)
		{

		}
	}
}