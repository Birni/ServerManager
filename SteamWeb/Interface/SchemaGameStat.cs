﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamWeb.Models
{
    public class SchemaGameStat
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("defaultvalue")]
        public uint DefaultValue { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }

    public class SchemaGameAchievement
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("defaultvalue")]
        public uint DefaultValue { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("hidden")]
        public uint Hidden { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("icongray")]
        public string Icongray { get; set; }
    }

    public class AvailableGameStats
    {
        [JsonProperty("stats")]
        public IList<SchemaGameStat> Stats { get; set; }

        [JsonProperty("achievements")]
        public IList<SchemaGameAchievement> Achievements { get; set; }
    }

    public class SchemaForGameResult
    {
        [JsonProperty("gameName")]
        public string GameName { get; set; }

        [JsonProperty("gameVersion")]
        public string GameVersion { get; set; }

        [JsonProperty("availableGameStats")]
        public AvailableGameStats AvailableGameStats { get; set; }
    }

    public class SchemaForGameResultContainer
    {
        [JsonProperty("game")]
        public SchemaForGameResult Result { get; set; }
    }
}