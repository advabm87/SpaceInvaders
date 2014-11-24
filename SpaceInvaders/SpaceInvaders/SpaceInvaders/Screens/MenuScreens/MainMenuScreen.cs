using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class MainMenuScreen : MenuScreen
    {
        private PlayItem m_PlayItem;
        private SoundOptionsItem m_SoundOptionsItem;
        private ChoosePlayersItem m_ChoosePlayersItem;
        private ScreenOptionsItem m_ScreenOptionsItem;
        private QuitGameIten m_QuitItem;

        public MainMenuScreen(Game i_Game)
            : base(i_Game, "Main Menu")
        {
            SettingsManager m_SettingsManager = (SettingsManager)this.Game.Services.GetService(typeof(ISettingsManager));
            m_PlayItem = new PlayItem("Play", this);
            m_SoundOptionsItem = new SoundOptionsItem("Sound Options", this);
            m_ChoosePlayersItem = new ChoosePlayersItem("Players", this);
            m_ChoosePlayersItem.TitleValue = m_SettingsManager.NumOfPlayers.ToString();
            m_ScreenOptionsItem = new ScreenOptionsItem("Screen Options", this);
            m_QuitItem = new QuitGameIten("Quit", this);

            AddMenuItem(m_PlayItem);
            AddMenuItem(m_SoundOptionsItem);
            AddMenuItem(m_ChoosePlayersItem);
            AddMenuItem(m_ScreenOptionsItem);
            AddMenuItem(m_QuitItem);
        }
    }
}