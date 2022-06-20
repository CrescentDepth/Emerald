﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using CmlLib.Core.Installer.FabricMC;
using CmlLib.Core.Downloader;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using SDLauncher_UWP.Views;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using SDLauncher_UWP.Helpers;
using CmlLib.Core;
using SDLauncher_UWP.Resources;
#pragma warning disable CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.
#pragma warning disable CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.
#pragma warning disable CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.
#pragma warning disable CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SDLauncher_UWP
{
    public sealed partial class MainPage : Page
    {
        public BaseLauncherPage launcher;
        public SettingsPage settingsPage;
        public SettingsDataManager settings = new SettingsDataManager();
        public MainPage()
        {
            this.InitializeComponent();
            vars.Launcher = SDLauncher.CreateLauncher(new MinecraftPath(ApplicationData.Current.LocalFolder.Path));
            launcher = new BaseLauncherPage();
            settingsPage = new SettingsPage();
            settingsPage.UpdateBGRequested += SettingsPage_UpdateBGRequested;
            settingsPage.BackRequested += SettingsPage_BackRequested;
            launcher.UIchanged += Launcher_UIchanged;
            launcher.InitializeLauncher();
            vars.Launcher.TasksHelper.TaskAddRequested += TasksHelper_TaskAddRequested;
            vars.Launcher.TasksHelper.TaskCompleteRequested += TasksHelper_TaskCompleteRequested; ;
            if(!string.IsNullOrEmpty(vars.BackgroundImagePath))
            {
                settingsPage.GetAndSetBG();
            }
        }

        private void TasksHelper_TaskCompleteRequested(object sender, int e)
        {
            tasks.CompleteTask(e);
        }

        private void TasksHelper_TaskAddRequested(object sender, UserControls.Task e)
        {
            tasks.AddTask(e.Name, e.ID);
        }

        private void SettingsPage_UpdateBGRequested(object sender, EventArgs e)
        {
            Page_ActualThemeChanged(null, null);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Page_ActualThemeChanged(null, null);
            MainFrame.Content = launcher;
            foreach (var account in vars.Accounts)
            {
                
            }
            if (vars.ShowTips)
            {
                tipacc.IsOpen = true;
            }
            if (vars.IsFixedDiscord)
            {
                btnPinDiscord_Click(null, null);
            }
            var computerMemory = Util.GetMemoryMb() * 1024;
            if (computerMemory != null)
            {
                int max = (int)computerMemory;
                int min = max / 10;
                double slidermin = (long)(max / 7);
                vars.MinRam = min;
                if (slidermin < 1024 && slidermin > 0)
                {
                    if (slidermin >= 512)
                        slidermin = 1024;
                    else
                        slidermin = 512;
                }
                else if (slidermin > 1024)
                {
                    slidermin = 1024;
                }
                vars.SliderRamMin = (long)slidermin;
                vars.SliderRamMax = max;
                vars.SliderRamValue = max / 2;
                settingsPage = new SettingsPage();
                if (vars.LoadedRam != 0)
                {
                    settingsPage.SetRam(vars.LoadedRam);
                    vars.CurrentRam = vars.LoadedRam;
                }
                else
                {
                    settingsPage.btnAutoRAM_Click(null, null);
                }
            }
            else
            {
               await MessageBox.Show(Localized.Error, Localized.RamFailed, MessageBoxButtons.Ok);
            }
        }

        private void SettingsPage_BackRequested(object sender, EventArgs e)
        {
            btnBack_Click(null, null);
        }

        private void Launcher_UIchanged(object sender, SDLauncher.UIChangeRequestedEventArgs e)
        {
            settingsPage.IsEnabled = e.UI;
            btnAccount.IsEnabled = e.UI;
        }

        public string localize(string key)
        {
            return Localizer.GetLocalizedString(key);
        }
        private void Page_Loading(FrameworkElement sender, object args)
        { 
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            UpdateTitleBarLayout(coreTitleBar);
            Window.Current.SetTitleBar(AppTitleBar);
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
            Window.Current.Activated += Current_Activated;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        BitmapImage bg;
        private async void Timer_Tick(object sender, object e)
        {
            if (!string.IsNullOrEmpty(vars.UserName))
            {
                txtUsername.Text = vars.UserName;
                txtLogin.Text = vars.UserName;
                prpFly.DisplayName = vars.UserName;
                prpLogin.DisplayName = vars.UserName;
                btnLogin.Tag = "Change";
            }
            else
            {
                txtUsername.Text = Localizer.GetLocalizedString("MainPage_Login");
                txtLogin.Text = Localizer.GetLocalizedString("MainPage_Login");
                prpFly.DisplayName = "";
                prpLogin.DisplayName = "";
                btnLogin.Tag = "Login";
            }
            if (vars.closing)
            {
                vars.closing = false;
                await settings.CreateSettingsFile(true);
            }
            if (vars.CustomBackground)
            {
                if (bg != vars.BackgroundImage)
                {
                    imgBack.ImageSource = vars.BackgroundImage;
                    bg = vars.BackgroundImage;
                }
            }
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            UpdateTitleBarLayout(sender);
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            AppTitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Current_Activated(object sender, WindowActivatedEventArgs e)
        {
            //if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            //{

            //    AppTitle.Foreground = disabletxt.Foreground;
            //}
            //else
            //{

            //    AppTitle.Foreground = enabletxt.Foreground;
            //}
        }

        private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar)
        {
            AppTitleBar.Height = coreTitleBar.Height;

            Thickness currMargin = AppTitleBar.Margin;
        }

        [Obsolete]
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            loginFly.Hide();
            btnBack.Visibility = Visibility.Visible;
            pnlTitle.Margin = new Thickness(50, pnlTitle.Margin.Top, pnlTitle.Margin.Right, pnlTitle.Margin.Bottom);
            MainFrame.Content = settingsPage;
            settingsPage.ScrollteToTop();
        }

        Login login = new Login();
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            loginFly.Hide();
            login = new Login();
            _ = login.ShowAsync();
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            loginFly.Hide();
            if (discordFixed.Visibility != Visibility.Visible)
                Canvas.SetZIndex(discordView, 9);
            discordView.IsPaneOpen = true;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            btnBack.Visibility = Visibility.Collapsed;
            pnlTitle.Margin = new Thickness(0, pnlTitle.Margin.Top, pnlTitle.Margin.Right, pnlTitle.Margin.Bottom);
            MainFrame.Content = launcher;        
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
            }
            else
            {
            }
        }

        private void tipacc_CloseButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            vars.ShowLaunchTips = true;
        }

        private void Page_ActualThemeChanged(FrameworkElement sender, object args)
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            if (this.ActualTheme == ElementTheme.Light)
            {
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonPressedBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.Black;
                titleBar.ButtonHoverBackgroundColor = ((SolidColorBrush)Application.Current.Resources["LayerFillColorDefaultBrush"]).Color;
                    imgDiscord.Source = new BitmapImage(new Uri("ms-appx:///Assets/Discord/discord.jpg"));
                if (!vars.CustomBackground)
                {
                    imgBack.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BackDrops/bg-light.png"));
                }
            }
            if (this.ActualTheme == ElementTheme.Dark)
            {
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonPressedBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = ((SolidColorBrush)Application.Current.Resources["LayerFillColorDefaultBrush"]).Color;
                imgDiscord.Source = new BitmapImage(new Uri("ms-appx:///Assets/Discord/discord-dark.png"));
                if (!vars.CustomBackground)
                {
                    imgBack.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BackDrops/bg.jpg"));
                }
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
        }

        private void btnPinDiscord_Click(object sender, RoutedEventArgs e)
        {
            // await ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
            Canvas.SetZIndex(discordView, 1);
            Canvas.SetZIndex(discordFixed, 9);
            vars.IsFixedDiscord = true;
            discordView.IsPaneOpen = false;
            btnUnPinDiscord.IsChecked = true;
            discordFixed.Visibility = Visibility.Visible;
            try
            {
                if (wv2Discord.Source.ToString() != wv2DiscordSticked.Source.ToString())
                {
                    wv2DiscordSticked.CoreWebView2.Navigate(wv2Discord.Source.ToString());
                }
            }
            catch
            {

            }
        }

        private void btnUnPinDiscord_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetZIndex(discordView, 9);
            if (wv2Discord.Source.ToString() != wv2DiscordSticked.Source.ToString())
            {
                wv2DiscordSticked.CoreWebView2.Navigate(wv2DiscordSticked.Source.ToString());
            }
            vars.IsFixedDiscord = false;
            discordView.IsPaneOpen = true;
            btnPinDiscord.IsChecked = false;
            discordFixed.Visibility = Visibility.Collapsed;
        }

        private void discordView_PaneClosed(SplitView sender, object args)
        {
            Canvas.SetZIndex(discordView, 1);
        }
    }
}
