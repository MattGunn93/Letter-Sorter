using System.Text;

namespace Letter_Sorter
{
	internal class Program
	{
		static void Main(string[] args)
		{
			#region Fields
			var ls = new LetterService();
			var log = new StringBuilder(DateTime.Now.ToString("MM/dd/yyyy") + " Report\n--------------------------------\n\nNumber of Combined Letters: ");
			string date = DateTime.Now.ToString("yyyyMMdd");
			// Paths & Directories
			var combinedLettersPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\CombinedLetters";
			var admissionDir = new DirectoryInfo(combinedLettersPath + @"\Input\Admission\" + date);
			var scholarshipDir = new DirectoryInfo(combinedLettersPath + @"\Input\scholarship\" + date);
			string archivePath = combinedLettersPath + @"\Archive\" + date;
			string outputPath = combinedLettersPath + @"\Output\" + date;
			#endregion

			// Creating output directories
			try
			{
				Directory.CreateDirectory(archivePath + @"\Admission");
				Directory.CreateDirectory(archivePath + @"\Scholarship");
				Directory.CreateDirectory(outputPath);
			}
			catch { throw new Exception("One or both directories already exist!"); }

			// Archiving letters
			ls.ArchiveLetters(admissionDir, archivePath + @"\Admission");
			ls.ArchiveLetters(scholarshipDir, archivePath + @"\Scholarship");

			// Creates Dictionaries of admission and scholarship letters 
			var admissionLetters = LetterService.GetLetters(admissionDir);
			var scholarshipLetters = LetterService.GetLetters(scholarshipDir);
			if(admissionLetters != null)
			{
				List<string> combinedLetters = new List<string>();
				int combinedCount = 0;
				foreach (string key in admissionLetters.Keys)
				{
					FileInfo? admission;
					FileInfo? scholarship;

					admissionLetters.TryGetValue(key, out admission);
					scholarshipLetters.TryGetValue(key, out scholarship);

					if (admission != null && scholarship != null)
					{
						ls.CombineTwoLetters(admission.FullName, scholarship.FullName, outputPath);
						scholarship.Delete();
						combinedLetters.Add(key);
						combinedCount++;
					}
					else
						admission?.MoveTo(Path.Combine(outputPath, admission.Name));
				}
				log.Append(combinedCount.ToString()+"\n");
				foreach(string id in combinedLetters)
				{
					log.AppendLine("    " + id);
				}
			}
			admissionDir.Delete();
			scholarshipDir.Delete();
			File.AppendAllText(outputPath+@"\log.txt", log.ToString());
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
		void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile);

	}

	public class LetterService : ILetterService
	{
		internal static Dictionary<string, FileInfo> GetLetters(DirectoryInfo dir)
		{
			var value = new Dictionary<string, FileInfo>();
			foreach (FileInfo file in dir.GetFiles())
			{
				string key = Path.GetFileNameWithoutExtension(file.FullName).Split('-')[1];
				value.Add(key, file);
			}
			return value;
		}
		internal void ArchiveLetters(DirectoryInfo source, string target)
		{
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target, file.Name));
		}
		public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile)
		{
			using (StreamWriter sw = File.AppendText(inputFile1))
			{
				sw.WriteLine("\n");
				using (StreamReader sr = File.OpenText(inputFile2))
				{
					while (!sr.EndOfStream)
					{
						sw.WriteLine(sr.ReadLine());
					}
				}
			}
			var file = new FileInfo(inputFile1);
			file?.MoveTo(Path.Combine(resultFile, file.Name));
		}
	}
}