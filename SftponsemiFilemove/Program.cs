using System;
using System.IO;
using Renci.SshNet;

namespace SFTPconnect
{
	internal static class Program
	{
		static void Main()
		{
			string localDirectory = @"C:\xml\onsemi_de_xml\"; // Source directory where files are located
			string backupDirectory = @"C:\xml\backup_relocate\"; // relocate directory files
			string sftpDirectory = "/toON/Data Exchange/Test/"; // Remote SFTP directory path

			try
			{
				// Initialize SFTP client
				using (var client = new SftpClient("sftp.onsemi.com", "ATEC_sftp", "wstHSKXBSU7"))
				{
					client.Connect();

					string[] files = Directory.GetFiles(localDirectory);

					foreach (var filePath in files)
					{
						string fileName = Path.GetFileName(filePath);
						using (var fileStream = new FileStream(filePath, FileMode.Open))
						{
							client.UploadFile(fileStream, sftpDirectory + fileName);
						}

						string destinationFile = Path.Combine(backupDirectory, fileName);
						File.Move(filePath, destinationFile);
					}

					client.Disconnect();
				}

				Console.WriteLine("Files uploaded and moved to backup directory successfully!");
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}
			Environment.Exit(0);
		}
	}
}
