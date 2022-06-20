﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core.VersionLoader;
using CmlLib.Core.Auth;
using SDLauncher_UWP.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace SDLauncher_UWP
{
    public static class vars
    {//some of these are not used (outdated)
        public static event EventHandler ThemeUpdated = delegate { };
        public static event EventHandler BackgroundUpdatd = delegate { };
        //App
        public static bool closing = false;
        public static bool showXMLOnClose = false;
        private static ElementTheme? theme = ElementTheme.Default;
        public static ElementTheme? Theme { get { return theme; } set { theme = value; ThemeUpdated(theme, new EventArgs()); } }
        public static BitmapImage BackgroundImage;
        public static string BackgroundImagePath = "";
        public static bool ShowLaunchTips = false;
        public static bool CustomBackground = false;
        public static bool ShowTips = true;
        public static bool UseOldVerSeletor = false;
        public static bool AdminLaunch = true;
        public static bool GameLogs = false;
        public static bool autoLog = false;
        public static bool AutoClose = false;
        public static int LoadedRam = 1024;
        public static SDLauncher Launcher;
        public static RPCHelper SDRPC;
        public static ObservableCollection<Account> Accounts;
        public static int AccountsCount;
        
        public static int? CurrentAccountCount;

        //CMLLIB
        public static MSession session;
        public static string UserName = "";
        public static int MinRam;
        public static int JVMScreenWidth = 0;
        public static int JVMScreenHeight = 0;
        public static bool FullScreen = false;
        public static List<string> JVMArgs = new List<string>();
        public static int CurrentRam = 0;
        //
        public static long SliderRamMax;
        public static long SliderRamMin;
        public static long SliderRamValue;
        //
        public static bool AssestsCheck = true;
        public static bool HashCheck = true;
        //
        public static bool IsFixedDiscord = false;
    }
}
