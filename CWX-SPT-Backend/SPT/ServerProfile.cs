﻿using System.Text.Json.Serialization;

namespace CWX_SPT_Launcher_Backend.SPT;

public class ServerProfile
{
    [JsonPropertyName("username")] public string Username { get; set; } = "";
    [JsonPropertyName("hasPassword")] public bool HasPassword { get; set; }
    [JsonPropertyName("side")] public string Side { get; set; } = "";
    [JsonPropertyName("currlvl")] public int CurrentLevel { get; set; }
    [JsonPropertyName("currexp")] public long CurrentExp { get; set; }
    [JsonPropertyName("prevexp")] public long PreviousExp { get; set; }
    [JsonPropertyName("nextlvl")] public long NextLevel { get; set; }
    [JsonPropertyName("maxlvl")] public int MaxLevel { get; set; }
    [JsonPropertyName("profileId")] public string ProfileID { get; set; } = "";
    [JsonPropertyName("edition")] public string Edition { get; set; } = "";
    [JsonPropertyName("sptData")] public SPTData SptData { get; set; } = new SPTData();
}