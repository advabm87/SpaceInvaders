using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class PlayItem : MenuItem
    {
        public PlayItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override void EnterScreen(GameScreen i_GameScreen)
        {
            i_GameScreen.ScreensManager.SetCurrentScreen(new LevelTransitionScreen(i_GameScreen.Game));
        }
    }

    public class SoundOptionsItem : MenuItem
    {
        public SoundOptionsItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override void EnterScreen(GameScreen i_GameScreen)
        {
            i_GameScreen.ScreensManager.SetCurrentScreen(new SoundOptionsMenu(i_GameScreen.Game));
        }
    }

    public class ChoosePlayersItem : MenuItem
    {
        public ChoosePlayersItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            SettingsManager m_SettingsManager = (SettingsManager)this.GameScreen.Game.Services.GetService(typeof(ISettingsManager));

            int players = m_SettingsManager.NumOfPlayers;
            players = players == 2 ? 1 : 2;
            m_SettingsManager.NumOfPlayers = players;
            return string.Format("{1}", this.Title, players.ToString());
        }
    }

    public class ScreenOptionsItem : MenuItem
    {
        public ScreenOptionsItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override void EnterScreen(GameScreen i_GameScreen)
        {
            i_GameScreen.ScreensManager.SetCurrentScreen(new ScreenOptionsMenu(i_GameScreen.Game));
        }
    }

    public class QuitGameIten : MenuItem
    {
        public QuitGameIten(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override void EnterScreen(GameScreen i_GameScreen)
        {
            i_GameScreen.Game.Exit();
        }
    }
}