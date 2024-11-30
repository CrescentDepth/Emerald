using Emerald.Helpers.Settings.JSON;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Emerald.Helpers.Settings;

public static class SettingsSystem
{
    public static JSON.Settings Settings { get; private set; } = JSON.Settings.CreateNew();
    public static Account[] Accounts { get; set; }

    public static event EventHandler<string>? APINoMatch;
//    public static T GetSerializedFromSettings<T>(string key, T def)
//    {
//        string json;
//        try
//        {
//            json = ApplicationData.Current.RoamingSettings.Values[key] as string;
//            return JsonSerializer.Deserialize<T>(json);
//        }
//        catch
//        {
//            json = JsonSerializer.Serialize(def);
//            ApplicationData.Current.RoamingSettings.Values[key] = json;
//            return def;
//        }
//    }
//    public static void LoadData()
//    {
//        Settings = GetSerializedFromSettings("Settings", JSON.Settings.CreateNew());
//        Accounts = GetSerializedFromSettings("Accounts", Array.Empty<Account>());

//        if (Settings.APIVersion != DirectResoucres.SettingsAPIVersion)
//        {
//            APINoMatch?.Invoke(null, ApplicationData.Current.RoamingSettings.Values["Settings"] as string);
//            ApplicationData.Current.RoamingSettings.Values["Settings"] = JSON.Settings.CreateNew().Serialize();
//            Settings = JsonSerializer.Deserialize<JSON.Settings>(ApplicationData.Current.RoamingSettings.Values["Settings"] as string);
//        }
//    }

//    public static async Task CreateBackup(string system)
//    {
//        string json = await FileIO.ReadTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists));
//        var l = json.IsNullEmptyOrWhiteSpace() ? new Backups() : JsonSerializer.Deserialize<Backups>(json);

//        var bl = l.AllBackups == null ? new List<SettingsBackup>() : l.AllBackups.ToList();
//        bl.Add(new SettingsBackup() { Time = DateTime.Now, Backup = system });
//        l.AllBackups = bl.ToArray();
//        json = l.Serialize();

//        await FileIO.WriteTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists), json);
//    }

//    public static async Task DeleteBackup(int Index)
//    {
//        string json = await FileIO.ReadTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists));
//        var l = json.IsNullEmptyOrWhiteSpace() ? new Backups() : JsonSerializer.Deserialize<Backups>(json);

//        var bl = l.AllBackups == null ? new List<SettingsBackup>() : l.AllBackups.ToList();
//        bl.RemoveAt(Index);
//        l.AllBackups = bl.ToArray();
//        json = l.Serialize();

//        await FileIO.WriteTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists), json);
//    }

//    public static async Task DeleteBackup(DateTime time)
//    {
//        string json = await FileIO.ReadTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists));
//        var l = json.IsNullEmptyOrWhiteSpace() ? new Backups() : JsonSerializer.Deserialize<Backups>(json);

//        var bl = l.AllBackups == null ? new List<SettingsBackup>() : l.AllBackups.ToList();
//        bl.Remove(x => x.Time == time);
//        l.AllBackups = bl.ToArray();
//        json = l.Serialize();

//        await FileIO.WriteTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists), json);
//    }
//    public static async Task RenameBackup(DateTime time, string name)
//    {
//        string json = await FileIO.ReadTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists));
//        var l = json.IsNullEmptyOrWhiteSpace() ? new Backups() : JsonSerializer.Deserialize<Backups>(json);

//        var bl = l.AllBackups == null ? new List<SettingsBackup>() : l.AllBackups.ToList();
//        bl.FirstOrDefault(x => x.Time == time).Name = name;
//        l.AllBackups = bl.ToArray();
//        json = l.Serialize();

//        await FileIO.WriteTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists), json);
//    }

//    public static async Task<List<SettingsBackup>> GetBackups()
//    {
//        string json = await FileIO.ReadTextAsync(await ApplicationData.Current.LocalFolder.CreateFileAsync("backups.json", CreationCollisionOption.OpenIfExists));
//        var l = json.IsNullEmptyOrWhiteSpace() ? new Backups() : JsonSerializer.Deserialize<Backups>(json);

//        return l.AllBackups == null ? new List<SettingsBackup>() : l.AllBackups.ToList();
//    }

//    public static void SaveData()
//    {
//        Settings.LastSaved = DateTime.Now;
//        ApplicationData.Current.RoamingSettings.Values["Settings"] = Settings.Serialize();
//        ApplicationData.Current.RoamingSettings.Values["Accounts"] = JsonSerializer.Serialize(Accounts);
//    }
//}
}
