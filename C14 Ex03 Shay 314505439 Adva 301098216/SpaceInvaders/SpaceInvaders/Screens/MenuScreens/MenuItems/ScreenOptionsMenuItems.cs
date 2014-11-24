using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class WindowResizingItem : MenuItem
    {
        public WindowResizingItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            SettingsManager settingsManager = (SettingsManager)i_GameScreen.Game.Services.GetService(typeof(ISettingsManager));
            bool allowResizing = !settingsManager.AllowWindowResizing;
            settingsManager.AllowWindowResizing = allowResizing;
            return allowResizing == true ? "On" : "Off";
        }
    }

    public class FullScreenModeItem : MenuItem
    {
        public FullScreenModeItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            SettingsManager settingsManager = (SettingsManager)i_GameScreen.Game.Services.GetService(typeof(ISettingsManager));
            bool fullScreen = !settingsManager.FullScreenMode;
            settingsManager.FullScreenMode = fullScreen;
            return fullScreen == true ? "On" : "Off";
        }
    }

    public class MouseVisabilityItem : MenuItem
    {
        public MouseVisabilityItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            SettingsManager settingsManager = (SettingsManager)i_GameScreen.Game.Services.GetService(typeof(ISettingsManager));
            bool mouseVisabile = !settingsManager.MouseVisibility;
            settingsManager.MouseVisibility = mouseVisabile;
            return mouseVisabile == true ? "Visible" : "Invisible";
        }
    }
}
