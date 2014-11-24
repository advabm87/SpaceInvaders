using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class ScreenOptionsMenu : MenuScreen
    {
        private WindowResizingItem m_WindowResizingItem;
        private FullScreenModeItem m_FullScreenModeItem;
        private MouseVisabilityItem m_MouseVisabilityItem;
        private DoneItem m_DoneItem;
        private SettingsManager m_SettingsManager;

        public ScreenOptionsMenu(Game i_Game)
            : base(i_Game, "Options")
        {
            m_SettingsManager = (SettingsManager)this.Game.Services.GetService(typeof(ISettingsManager));

            m_WindowResizingItem = new WindowResizingItem("Window Resizing", this);
            m_WindowResizingItem.TitleValue = (m_SettingsManager.AllowWindowResizing == true) ? "On" : "Off";
            m_FullScreenModeItem = new FullScreenModeItem("Full Screen Mode", this);
            m_FullScreenModeItem.TitleValue = m_SettingsManager.FullScreenMode ? "On" : "Off";
            m_MouseVisabilityItem = new MouseVisabilityItem("Mouse Visability", this);
            m_MouseVisabilityItem.TitleValue = m_SettingsManager.MouseVisibility ? "Visible" : "Invisible";
            m_DoneItem = new DoneItem("Done", this);

            AddMenuItem(m_WindowResizingItem);
            AddMenuItem(m_FullScreenModeItem);
            AddMenuItem(m_MouseVisabilityItem);
            AddMenuItem(m_DoneItem);
        }
    }
}