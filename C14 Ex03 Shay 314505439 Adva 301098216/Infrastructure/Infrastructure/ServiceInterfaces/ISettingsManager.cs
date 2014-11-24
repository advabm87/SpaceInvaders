namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISettingsManager
    {
        List<IPlayerSettings> PlayersSettings { get; }
        
        int GameLevel { get; }
        
        int GameLevelDisplayedToPlayer { get; }
        
        int NumOfPlayers { get; set; }
        
        bool AllowWindowResizing { get; set; }
        
        bool FullScreenMode { get; set; }
        
        bool MouseVisibility { get; set; }
        
        int InitialeLivesPerPlayer { get; }
    }
}
