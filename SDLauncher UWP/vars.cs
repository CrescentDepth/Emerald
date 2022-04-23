﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CmlLib.Core;
using CmlLib.Core.Auth;
using Windows.UI.Xaml;

namespace SDLauncher_UWP
{
    class vars
    {
        //App
        public static bool closing = false;
        public static bool showXMLOnClose = false;
        public static ElementTheme? theme = ElementTheme.Default;
        public static bool ShowLaunchTips = false;
        public static bool ShowTips = true;
        public static bool UI = true;
        public static bool autoLog = false;
        public static int LoadedRam = 1024;
        public static CMLauncher LauncherSynced;
        public static ObservableCollection<Account> Accounts;
        public static int AccountsCount;
        
        public static int? CurrentAccountCount;

        //CMLLIB
        public static MSession session;
        public static string UserName;
        public static int MinRam;
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
        //
        public static string ProgressStatus;
    }
}
