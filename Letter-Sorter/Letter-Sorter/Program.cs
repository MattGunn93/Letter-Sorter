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
			string outputPath = combinedLettersPath + @"\Output\" + date;

			if (!Directory.Exists(archivePath) || !Directory.Exists(outputPath))
			{
				Directory.CreateDirectory(archivePath);
				Directory.CreateDirectory(outputPath);
			}
			else
			{
				throw new Exception("One or both directories already exist!");
			}
			var archiveDir = new DirectoryInfo(archivePath);
			var outputDir = new DirectoryInfo(outputPath);

			LetterService.ArchiveLetters(admissionDir, archiveDir);
			LetterService.ArchiveLetters(scholarshipDir, archiveDir);

			var admissionLetters = LetterService.GetLetters(admissionDir);
			var scholarshipLetters = LetterService.GetLetters(scholarshipDir);

			if(admissionLetters != null && scholarshipLetters != null)
			{
				foreach (string key in admissionLetters.Keys)
				{
					FileInfo? admission;
					FileInfo? scholarship;

					admissionLetters.TryGetValue(key, out admission);
					scholarshipLetters.TryGetValue(key, out scholarship);

					if (admission != null && scholarship != null)
					{
						using (StreamWriter sw = File.AppendText(admission.FullName))
						{
							sw.WriteLine("\n");
							using (StreamReader sr = File.OpenText(scholarship.FullName))
							{
								while(!sr.EndOfStream)
								{
									sw.WriteLine(sr.ReadLine());
								}
							}
						}
						scholarship.Delete();
					}
					admission?.MoveTo(Path.Combine(outputDir.FullName, admission.Name));
				}
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
		internal static void ArchiveLetters(DirectoryInfo source, DirectoryInfo target)
		{
			foreach (FileInfo file in source.GetFiles())
				file.CopyTo(Path.Combine(target.FullName, file.Name));
		}
		public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile)
		{

		}
	}
}