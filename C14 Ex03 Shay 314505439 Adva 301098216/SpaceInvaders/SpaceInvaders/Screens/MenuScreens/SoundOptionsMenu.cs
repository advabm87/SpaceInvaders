using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Microsoft.Xna.Framework;

namespace SpaceInvaders
{
    public class SoundOptionsMenu : MenuScreen
    {
        private SoundEffectsItem m_SoundEffectsItem;
        private ToggleSoundItem m_ToggleSoundItem;
        private BackgrounMusicVolumItem m_BackgrounMusicVolumItem;
        private DoneItem m_DoneItem;

        public SoundOptionsMenu(Game i_Game)
            : base(i_Game, "Sound Options")
        {
            SoundManager m_SoundManager = (SoundManager)this.Game.Services.GetService(typeof(ISoundManager));

            m_SoundEffectsItem = new SoundEffectsItem("Sound-Effects", this);
            m_SoundEffectsItem.TitleValue = m_SoundManager.SoundsEffectsVolumeLevel.ToString();

            m_ToggleSoundItem = new ToggleSoundItem("Toggle-Sound", this);
            m_ToggleSoundItem.TitleValue = m_SoundManager.IsMute.ToString();

            m_BackgrounMusicVolumItem = new BackgrounMusicVolumItem("Backgroun-Music-Volum", this);
            m_BackgrounMusicVolumItem.TitleValue = m_SoundManager.BackGroundVolumeLevel.ToString();

            m_DoneItem = new DoneItem("Done", this);

            AddMenuItem(m_SoundEffectsItem);
            AddMenuItem(m_ToggleSoundItem);
            AddMenuItem(m_BackgrounMusicVolumItem);
            AddMenuItem(m_DoneItem);
        }
    }
}