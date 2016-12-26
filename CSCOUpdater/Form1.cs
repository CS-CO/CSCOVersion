using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;

using System.Reflection;

using CG.Web.MegaApiClient;

namespace CSCOUpdater
{
	public partial class Form1 : Form
	{
		string v_url = "https://raw.githubusercontent.com/tatjam/CSCOVersion/master/VERSION";

		int latest_version = 0;
		int installed_version = 0;

		bool no_v_file = false;

		bool done = false;

		DialogResult r;

		Task task;

		string latest_url = "";

		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			IProgress<double> progress = new Progress<double>(b => progressBar1.Value = (int)b);
			if (!done)
			{
				if(!button1.Enabled)
				{
					if (!File.Exists(Directory.GetCurrentDirectory() + "/csco/version.txt"))
					{
						File.WriteAllText(Directory.GetCurrentDirectory() + "/csco/version.txt", "" + latest_version);
					}
				}
				else
				{
						if (r == DialogResult.Yes)
						{
							if (no_v_file == true || !File.Exists(Directory.GetCurrentDirectory() + "/csco/version.txt"))
							{
								File.WriteAllText(Directory.GetCurrentDirectory() + "/csco/version.txt", "" + latest_version);
							}
							done = true;
							button1.Enabled = false;
							button1.Text = "Version File Created";
						}
						else
						{
							button1.Text = "Downloading...";

							delete_dir(Directory.GetCurrentDirectory() + "/TempDownloadCSCO");

							Directory.CreateDirectory(Directory.GetCurrentDirectory() + "/TempDownloadCSCO");

							MegaApiClient mega_client = new MegaApiClient();
							mega_client.LoginAnonymous();

							try
							{
								task = mega_client.DownloadFileAsync(new Uri(latest_url), 
									Directory.GetCurrentDirectory() + "/TempDownloadCSCO/temp.zip", progress);
							}
							catch (Exception ee)
							{
								MessageBox.Show("Couldn't Download: " + ee.Message);
								done = true;
							}
						}
				}
			}
			else
			{
				button1.Text = "Already ran updater!";
				button1.Enabled = false;
			}
		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void Form1_Load(object sender, EventArgs e)
		{

			string v_file = "NULL";
			string local_v_file = "NULL";

			if (Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).Name.ToLower() == "sourcemods")
			{
			
				button1.Text = "Getting version file";
				button1.Enabled = false;



				try
				{
					//Get the version file from github
					using (System.Net.WebClient client = new System.Net.WebClient())
					{
						v_file = client.DownloadString(v_url);
					}
				}
				catch (Exception)
				{
					MessageBox.Show("Error! Unable to get version file from github.\nCheck your internet connection!");
				}

				if(v_file != "NULL")
				{
					//If we get the file then continue downloading...

					//Process v_file
					latest_version = 0;

					string buff = "";

					//Find number
					foreach(char c in v_file)
					{
						if (c == ' ')
						{
							break;
						}
						else
						{
							buff += c;
						}
					}

					latest_version = Convert.ToInt32(buff);

					buff = "";

					bool reading = false;

					//Find URL
					foreach (char c in v_file)
					{
						if (!reading)
						{
							if (c == ' ')
							{
								reading = true;
							}
						}
						else
						{
							buff += c;
						}
					}

					latest_url = buff;
				}

				if (!Directory.Exists("csco"))
				{
					MessageBox.Show("No CS:CO installation found. Updating will download last version!");
				}
				else
				{

					try
					{
						// Get actual installed version
						local_v_file = File.ReadAllText(Directory.GetCurrentDirectory() + "/csco/version.txt");
						installed_version = Convert.ToInt32(local_v_file);
					}
					catch (Exception)
					{

						r = MessageBox.Show("Couldn't get version.txt. Do you have the latest version?", 
							"Error", MessageBoxButtons.YesNo);
						
						no_v_file = true;
					}
				}
			}
			else
			{
				MessageBox.Show("You are running the autoupdater from the wrong location. Can't update!");
			}

			//Show labels
			installed_v.Text = "Installed version: " + installed_version;
			latest_v.Text = "Latest version: " + latest_version;

			//Check if the download URL is available
			if (latest_url != "")
			{
				//Check if we need to update
				if(installed_version < latest_version)
				{
					button1.Text = "Update";
					button1.Enabled = true;
				}
				else
				{
					button1.Text = "No Update Required";
					button1.Enabled = false;
				}

			}
			else
			{
				button1.Enabled = false;
				button1.Text = "Can't Update!";
			}
		
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			progressBar1.Maximum = 100;

			//Directory.SetCurrentDirectory(System.Reflection.Assembly.GetExecutingAssembly().Location);

			try
			{
				if (task != null)
				{
					if (task.IsCompleted)
					{
						if (!done)
						{
							// The button was clicked and download successful
							done = true;
							progressBar1.Value = 100;

							button1.Text = "Deleting Old Install...";


							//Delete readme files
							if (File.Exists(Directory.GetCurrentDirectory() + "/HostMe.txt"))
							{
								File.Delete(Directory.GetCurrentDirectory() + "/HostMe.txt");
							}
							if (File.Exists(Directory.GetCurrentDirectory() + "/ReadMe.txt"))
							{
								File.Delete(Directory.GetCurrentDirectory() + "/ReadMe.txt");
							}

							delete_dir(Directory.GetCurrentDirectory() + "/csco");
							button1.Text = "Installing...";

							//Extract the downloaded .zip
							try
							{
								// Extract .zip
								ZipFile.ExtractToDirectory(Directory.GetCurrentDirectory() + "/TempDownloadCSCO/temp.zip",
									Directory.GetCurrentDirectory() + "/");
							}
							catch (Exception ee)
							{
								MessageBox.Show("Couldn't Unzip: " + ee.Message);
								done = true;
							}

							//If version.txt file is not included (ZooL will include it soon) create one
							if (no_v_file == true || !File.Exists("csco/version.txt"))
							{
								File.WriteAllText(Directory.GetCurrentDirectory() + "/csco/version.txt", "" + latest_version);
							}

							// Delete temp files
							delete_dir("TempDownloadCSCO");


							button1.Enabled = false;
							button1.Text = "Install Complete";
						}
					}
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show(ee.Message);
			}
		}

		public void delete_dir(string path)
		{
			if (Directory.Exists(path))
			{

				System.IO.DirectoryInfo di = new DirectoryInfo(path);

				foreach (FileInfo file in di.GetFiles())
				{
					file.Delete();
				}
				foreach (DirectoryInfo dir in di.GetDirectories())
				{
					dir.Delete(true);
				}

				Directory.Delete(path);
			}
		}

		private void label1_Click_1(object sender, EventArgs e)
		{

		}
	}

}
