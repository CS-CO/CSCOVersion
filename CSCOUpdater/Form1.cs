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

				}
				else
				{
					button1.Text = "Downloading...";

					delete_dir("./TempDownloadCSCO");

					Directory.CreateDirectory("./TempDownloadCSCO");

					MegaApiClient mega_client = new MegaApiClient();
					mega_client.LoginAnonymous();

					try
					{
						task = mega_client.DownloadFileAsync(new Uri(latest_url), "./TempDownloadCSCO/temp.zip", progress);
					}
					catch(Exception ee)
					{
						MessageBox.Show("Couldn't Download: " + ee.Message);
						done = true;
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

			if (Directory.GetParent("./").Name == "sourcemods")
			{


				button1.Text = "Getting version file";
				button1.Enabled = false;



				try
				{
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
					//Process v_file
					latest_version = 0;

					string buff = "";

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

				if (!Directory.Exists("./csco"))
				{
					MessageBox.Show("No CS:CO installation found. Updating will download last version!");
				}
				else
				{

					try
					{
						// Get actual installed version
						local_v_file = File.ReadAllText("./csco/version.txt");
						installed_version = Convert.ToInt32(local_v_file);
					}
					catch (Exception)
					{

						MessageBox.Show("Couldn't get version.txt, assuming old version installed. Updating will download and create version.txt");
						no_v_file = true;
					}
				}
			}
			else
			{
				MessageBox.Show("You are running the autoupdater from the wrong location. Can't update!");
			}

			installed_v.Text = "Installed version: " + installed_version;
			latest_v.Text = "Latest version: " + latest_version;

			if (latest_url != "")
			{
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

			try
			{
				if (task != null)
				{
					if (task.IsCompleted)
					{
						if (!done)
						{
							done = true;
							progressBar1.Value = 100;

							button1.Text = "Deleting Old Install...";

							if (File.Exists("./HostMe.txt"))
							{
								File.Delete("./HostMe.txt");
							}
							if (File.Exists("./ReadMe.txt"))
							{
								File.Delete("./ReadMe.txt");
							}

							delete_dir("./csco");
							button1.Text = "Installing...";

							try
							{
								// Extract .zip
								ZipFile.ExtractToDirectory("./TempDownloadCSCO/temp.zip", "./");
							}
							catch (Exception ee)
							{
								MessageBox.Show("Couldn't Unzip: " + ee.Message);
								done = true;
							}

							if (no_v_file == true || !File.Exists("./csco/version.txt"))
							{
								File.WriteAllText("./csco/version.txt", "" + latest_version);
							}

							// Delete temp files
							delete_dir("./TempDownloadCSCO");


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
