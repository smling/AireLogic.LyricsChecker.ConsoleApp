# AireLogic.LyricsChecker.ConsoleApp

Console application which given the name of an artist, will produce the average(mean) number of words in their songs.

# Compile 
Execute command below to compile application
```powershell
dotnet build 
```

# Usage
Execute with paraemter below.
```Powershell
AireLogic.LyricsChecker.ConsoleApp.exe --artist=[artist name]

# Example
# AireLogic.LyricsChecker.ConsoleApp.exe --artist=coldpay
```

# Libraries employed
|Name|Description|URL|
---|---|---
|MetaBrainz.MusicBrainz| Wrapper class for getting data from MusicBrainz v2 API |https://github.com/Zastai/MetaBrainz.MusicBrainz |
|NUnit|Running Unit test||