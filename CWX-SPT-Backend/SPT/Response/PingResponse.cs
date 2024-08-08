﻿using System.Text.Json.Serialization;

namespace CWX_SPT_Launcher_Backend.SPT.Response;

public class PingResponse : ISptResponse<string>
{
    [JsonPropertyName("response")]
    public string Response { get; set; } = "";
}