namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class SettingsManager : GameService, ISettingsManager
    {
        private class PlayerSettings : IPlayerSettings
        {
            private int m_Score;
            private int m_Lives;
            private bool m_Enabled;

            public int Score
            {
                get { return m_Score; }
                set { m_Score = value; }
            }

            public int Lives
            {
                get { return m_Lives; }
                set { m_Lives = value; }
            }

            public bool Enabled
            {
                get { return m_Enabled; }
                set { m_Enabled = value; }
            }
        }
        
        private int m_NumOfPlayers;
        private List<IPlayerSettings> m_PlayersSettings;
        private int m_Level = 1;
        private int m_LevelDisplayedToPlayer = 1;
        private bool m_AllowWindowResizing = false;
        private bool m_FullScreenMode = false;
        private bool m_MouseVisibility = true;
        private int m_InitialeLivesPerPlayer;

        public SettingsManager(Game i_Game, int i_NumOfPlayers, int i_LivesPerPlayer)
            : base(i_Game)
        {
            m_NumOfPlayers = i_NumOfPlayers;
            m_PlayersSettings = new List<IPlayerSettings>();
            m_InitialeLivesPerPlayer = i_LivesPerPlayer;

            for (int i = 0; i < NumOfPlayers; i++)
            {
                PlayerSettings playerSettings = new PlayerSettings();

                playerSettings.Lives = i_LivesPerPlayer;
                playerSettings.Score = 0;
                playerSettings.Enabled = true;
                m_PlayersSettings.Add(playerSettings);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            GameLevelDisplayedToPlayer = 1;

            foreach (IPlayerSettings playerSettings in PlayersSettings)
            {
                playerSettings.Score = 0;
                playerSettings.Lives = InitialeLivesPerPlayer;
            }
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(ISettingsManager), this);
        }

        public List<IPlayerSettings> PlayersSettings
        {
            get { return m_PlayersSettings; }
        }

        public int GameLevel
        {
            get { return m_Level; }
        }

        public int GameLevelDisplayedToPlayer
        {
            get
            {
                return m_LevelDisplayedToPlayer;
            }

            set
            {
                m_LevelDisplayedToPlayer = value;
                m_Level = m_LevelDisplayedToPlayer % 6 == 0 ? 1 : m_LevelDisplayedToPlayer % 6;
            }
        }

        public int NumOfPlayers
        {
            get
            {
                return m_NumOfPlayers;
            }

            set
            {
                if (m_NumOfPlayers > value)
                {
                    m_NumOfPlayers = value;
                    m_PlayersSettings.RemoveRange(m_NumOfPlayers, m_PlayersSettings.Count - m_NumOfPlayers);
                }
                else if (m_NumOfPlayers < value)
                {
                    for (int i = 0; i < value - m_NumOfPlayers; i++)
                    {
                        PlayerSettings playerSettings = new PlayerSettings();

                        playerSettings.Lives = m_PlayersSettings[0].Lives;
                        playerSettings.Score = 0;
                        playerSettings.Enabled = true;
                        m_PlayersSettings.Add(playerSettings);
                    }

                    m_NumOfPlayers = value;
                }
            }
        }

        public bool AllowWindowResizing
        {
            get { return m_AllowWindowResizing; }
            set 
            { 
                m_AllowWindowResizing = value;
                Game.Window.AllowUserResizing = m_AllowWindowResizing;
            }
        }

        public bool FullScreenMode
        {
            get { return m_FullScreenMode; }
            set 
            {
                GraphicsDeviceManager graphicsDeviceManager = Game.Services.GetService(typeof(IGraphicsDeviceManager)) as GraphicsDeviceManager;
                graphicsDeviceManager.ToggleFullScreen();
                m_FullScreenMode = value;
            }
        }

        public bool MouseVisibility
        {
            get { return m_MouseVisibility; }
            set 
            { 
                m_MouseVisibility = value;
                Game.IsMouseVisible = m_MouseVisibility;
            }
        }

        public int InitialeLivesPerPlayer
        {
            get { return m_InitialeLivesPerPlayer; }
        }
    }
}
