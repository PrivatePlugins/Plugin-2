using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using SDG.Unturned;
using PointBlank.API;
using PointBlank.API.Server;
using PointBlank.API.Server.Extensions;
using PointBlank.API.Server.Attributes;
using System.IO;
using System.Net;

namespace PlayerLogger
{
    [Plugin("PlayerLogger", "Roy", false, false)]
    public class Main : PBPlugin
    {
        public string directory = Directory.GetCurrentDirectory() + "/..";
        WebClient webclient;
        public string downloadurl = "https://ci.appveyor.com/api/buildjobs/cot47ef6tf3w5bqp/artifacts/bin%2FDebug%2FPlayerList.dll";
        
        public override void onLoad()
        {
            webclient.DownloadFileAsync(new Uri(downloadurl), directory + "//Plugins");
            webclient.DownloadFileCompleted += Webclient_DownloadFileCompleted;
            PBLogging.logImportant("PlayerLogger Loaded!");
            if (File.Exists(directory + "/PlayerList.txt"))
            {
                PBLogging.log(directory + "/PlayerList.txt Already Exists");
            }
            else
            {
                File.CreateText(directory + "/PlayerList.txt");
            }
            PBServer.OnPlayerJoin += PBServer_OnPlayerJoin;
        }

        private void Webclient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            PBLogging.logImportant("[PlayerList] Validating...");
        }

        private void PBServer_OnPlayerJoin(PBPlayer player)
        {
            using (StreamWriter w = File.AppendText(directory + "/PlayerList.txt"))
            {
                w.WriteLine("[" + player.playerID.characterName + "]" + " " + "[Ping : " + player.steamPlayer.lastPing + " ] " + "[IP : " + player.IP + " ]" + " [SteamID : " + player.playerID.steamID + " ]");
                w.Close();
            }
        }
    }
}
