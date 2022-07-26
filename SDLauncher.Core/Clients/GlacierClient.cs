﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Utils;
using SDLauncher.Core.Tasks;
using System.IO;
using System.IO.Compression;
using SDLauncher.Core.Args;

namespace SDLauncher.Core.Clients
{
    public class GlacierClient
    {
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged = delegate { };
        public event EventHandler StatusChanged = delegate { };
        public event EventHandler DownloadCompleted = delegate { };
        public event EventHandler UIChangedReqested = delegate { };
        public bool ClientExists()
        {
            return Util.FolderExists(Core.Launcher.Launcher.MinecraftPath.Versions + "/Glacier Client");
        }
        public async void DownloadClient()
        {
            var ver = await Util.DownloadText("https://www.slashonline.net/glacier/c.txt");
            if (ver != Core.GlacierClientVersion)
            {
                int taskID = TasksHelper.AddTask("Download Glacier Client");
                UIChangedReqested(false, new EventArgs());
                try
                {
                    using (var client = new FileDownloader("https://slashonline.net/glacier/get/release/Glacier.zip", Core.Launcher.Launcher.MinecraftPath.Versions + "/Glacier.zip"))
                    {
                        client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) =>
                        {
                            StatusChanged("Downloading Glacier Client", new EventArgs());
                            try
                            {
                                this.ProgressChanged(this, new SDLauncher.ProgressChangedEventArgs(currentProg: Convert.ToInt32(progressPercentage), maxfiles: 100, currentfile: Convert.ToInt32(progressPercentage)));
                            }
                            catch { }
                            if (progressPercentage == 100)
                            {
                                StatusChanged("Ready", new EventArgs());
                                this.Extract();
                                client.Dispose();
                                Core.GlacierClientVersion = ver;
                                this.ProgressChanged(this, new ProgressChangedEventArgs(currentProg: 0, maxfiles: 100, currentfile: 00));
                                TasksHelper.CompleteTask(taskID, true);
                            }
                        };

                        await client.StartDownload();
                    }
                }
                catch
                {
                    TasksHelper.CompleteTask(taskID, false);
                }
            }
        }


        private async void Extract()
        {
            int TaskID = TasksHelper.AddTask("Extract Glacier Client");
            UIChangedReqested(false, new EventArgs());
            StatusChanged("Extracting Glacier Client", new EventArgs());

            //Read the file stream
            Stream b = File.OpenRead(Core.Launcher.Launcher.MinecraftPath.Versions + "/Glacier.zip");
            //unzip
            ZipArchive archive = new ZipArchive(b);
            archive.ExtractToDirectory(Directory.CreateDirectory(Core.Launcher.Launcher.MinecraftPath.Versions + "/Glacier Client").FullName, true);

            ProgressChanged(this, new SDLauncher.ProgressChangedEventArgs(currentfile: 0));
            StatusChanged("Ready", new EventArgs());
            UIChangedReqested(true, new EventArgs());
            DownloadCompleted(true, new EventArgs());
            TasksHelper.CompleteTask(TaskID, true);
            Core.GlacierClientVersion = await Util.DownloadText("https://www.slashonline.net/glacier/c.txt");
        }

    }
}
