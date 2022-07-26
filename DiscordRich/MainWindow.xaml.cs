﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;

namespace DiscordRich
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Main.Init();
            
            InitializeComponent();

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void OpenURL(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://website.crazyne55.repl.co/");
        }

        private void SUBMIT_DATA_Click(object sender, RoutedEventArgs e)
        {
            //TextBox _ = (TextBox)this.FindName("");
            TextBox INPUT_Details = (TextBox)this.FindName("INPUT_Details");
            TextBox INPUT_State = (TextBox)this.FindName("INPUT_State");
            TextBox INPUT_LargeImage = (TextBox)this.FindName("INPUT_LargeImage");
            TextBox INPUT_LargeText = (TextBox)this.FindName("INPUT_LargeText");
            TextBox INPUT_SmallImage = (TextBox)this.FindName("INPUT_SmallImage");
            TextBox INPUT_SmallText = (TextBox)this.FindName("INPUT_SmallText");
            TextBox INPUT_PartyID = (TextBox)this.FindName("INPUT_PartyID");
            TextBox INPUT_PartyCurrent = (TextBox)this.FindName("INPUT_PartyCurrent");
            TextBox INPUT_PartyMax = (TextBox)this.FindName("INPUT_PartyMax");

            Discord.Activity activity = new Discord.Activity();
            
            activity.Details = INPUT_Details.Text;
            activity.State = INPUT_State.Text;
            activity.Assets.LargeImage = INPUT_LargeImage.Text;
            activity.Assets.LargeText = INPUT_LargeText.Text;
            activity.Assets.SmallImage = INPUT_SmallImage.Text;
            activity.Assets.SmallText = INPUT_SmallText.Text;
            activity.Party.Id = INPUT_PartyID.Text;
            if(int.TryParse(INPUT_PartyCurrent.Text, out int result1))
            { activity.Party.Size.CurrentSize = result1; }
            if (int.TryParse(INPUT_PartyMax.Text, out int result2))
            { activity.Party.Size.MaxSize = result2; }
            activity.Instance = true;
            
            Main.UpdateActivity(Helper.GetDiscord(), activity);
        }

        private void SUBMIT_RUN_Click(object sender, RoutedEventArgs e)
        {
            Discord.Discord discord = Helper.GetDiscord();
            GC.KeepAlive(discord);
            if (discord != null)
            {
                Main.updateThread.Abort();
                discord.Dispose();
            }


            TextBox INPUT_AppID = (TextBox)this.INPUT_AppID;
            if (Int64.TryParse(INPUT_AppID.Text, out _)| INPUT_AppID.Text.Length == 0)
            {
                if (INPUT_AppID.Text.Length == 0)
                {
                    discord = Helper.MakeDiscord(Int64.Parse("957715991138283571"), (UInt64)Discord.CreateFlags.Default);
                }
                else
                {
                    discord = Helper.MakeDiscord(Int64.Parse(INPUT_AppID.Text), (UInt64)Discord.CreateFlags.Default);
                }
                if (discord != null) { Main.Init(); INPUT_AppID.Background = new SolidColorBrush(Color.FromRgb(160, 255, 160)); }
                else INPUT_AppID.Background = new SolidColorBrush(Color.FromRgb(255, 160, 160));






                ((TextBox)this.FindName("INPUT_Details")).IsEnabled = true;
                ((TextBox)this.FindName("INPUT_State")).IsEnabled = true;
                ((TextBox)this.FindName("INPUT_LargeText")).IsEnabled = true;
                ((TextBox)this.FindName("INPUT_SmallText")).IsEnabled = true;
                ((TextBox)this.FindName("INPUT_PartyID")).IsEnabled = true;
                ((TextBox)this.FindName("INPUT_PartyCurrent")).IsEnabled = true;
                ((TextBox)this.FindName("INPUT_PartyMax")).IsEnabled = true;
                ((Button)this.FindName("SUBMIT_DATA")).IsEnabled = true;



                if (!(INPUT_AppID.Text.Length == 0 | INPUT_AppID.Text == "957715991138283571"))
                {
                    ((TextBox)this.FindName("INPUT_LargeImage")).IsEnabled = true;
                    ((TextBox)this.FindName("INPUT_SmallImage")).IsEnabled = true;
                }
                else 
                {
                    ((TextBox)this.FindName("INPUT_LargeImage")).IsEnabled = false;
                    ((TextBox)this.FindName("INPUT_SmallImage")).IsEnabled = false;
                }
            }
            else
            {
                INPUT_AppID.Background = new SolidColorBrush(Color.FromRgb(255, 160, 160));

                ((TextBox)this.FindName("INPUT_Details")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_State")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_LargeText")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_SmallText")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_PartyID")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_PartyCurrent")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_PartyMax")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_LargeImage")).IsEnabled = false;
                ((TextBox)this.FindName("INPUT_SmallImage")).IsEnabled = false;
                ((Button)this.FindName("SUBMIT_DATA")).IsEnabled = false;
            } 
        }
    }


    public class Main
    {
        public static Thread updateThread;
        public static void Init()
        {
            Discord.Discord discord = Helper.GetDiscord();
            Discord.Activity activity;

            activity = new Discord.Activity();
            activity.Details = "DiscordRich, The Rich Presense";
            activity.State = "App Made By crazyne55!";
            activity.Assets.LargeImage = "dr_logo";
            activity.Assets.LargeText = "DiscordRich Logo";
            activity.Assets.SmallImage = "dr_logo";
            activity.Assets.SmallText = "smol logo :)";
            activity.Party.Id = "LobbyID";
            activity.Party.Size.CurrentSize = 6;
            activity.Party.Size.MaxSize = 9;
            activity.Instance = true;
            UpdateActivity(discord, activity);

            updateThread = new Thread(() => {
                Discord.Discord _discord = Helper.GetDiscord();
                while (true)
                {
                    try
                    {
                        _discord.RunCallbacks();
                    } catch { }
                    Thread.Sleep(1000 / 60);
                }
            });
            updateThread.Start();
        }



        // Update user's activity for your game.
        // Party and secrets are vital.
        // Read https://discordapp.com/developers/docs/rich-presence/how-to for more details.
        public static void UpdateActivity(Discord.Discord discord, Discord.Activity activity)
        {
            var activityManager = discord.GetActivityManager();

            activityManager.UpdateActivity(activity, result => {});
        }
    }

    class Helper
    {
        public static Discord.Discord discord;
        public static Discord.Discord MakeDiscord(long ClientID, UInt64 DiscordFlag)
        {
            discord = new Discord.Discord(ClientID, DiscordFlag);
            GC.KeepAlive(discord);
            return discord;
        }
        public static Discord.Discord GetDiscord()
        {
            return discord;
        }
    }


}
