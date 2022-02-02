using System;
using System.Collections.Generic;
using System.Text;

namespace AireLogic.LyricsChecker.ConsoleApp
{
    internal class Constants
    {
        public static class Parameters {
            public const string APP_ID = "app-id";
            public const string APP_SECRET = "app-secret";
            public const string ARTICE = "artist";
            public const string PARAMETER_PREFIX = "--";
            public const string PARAMETER_VALUE_SEPERATOR = "=";
        }
        public static class Settings {
            public static class Key {
                public const string KEY_LYRICS_API_ROOT = "lyrics-ovh-api-root";
            }
            public static class LyricsOvh { 
                public const string DEFAULT_ROOR_URL = "https://api.lyrics.ovh/v1/";
            }
        }
    }
}
