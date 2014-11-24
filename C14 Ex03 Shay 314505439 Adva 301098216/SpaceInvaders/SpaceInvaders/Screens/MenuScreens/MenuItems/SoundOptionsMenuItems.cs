using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class SoundEffectsItem : MenuItem
    {
        public SoundEffectsItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            int currVolume = i_GameScreen.SoundManager.SoundsEffectsVolumeLevel;
            if (i_Key == Keys.PageUp)
            {
                if (currVolume < 100)
                {
                    i_GameScreen.SoundManager.IncreaseSoundsEffectsVolume();
                }
                else
                {
                    i_GameScreen.SoundManager.SoundsEffectsVolumeLevel = 0;
                }
            }

            if (i_Key == Keys.PageDown)
            {
                if (currVolume > 0)
                {
                    i_GameScreen.SoundManager.DecreaseSoundsEffectsVolume();
                }
                else
                {
                    i_GameScreen.SoundManager.SoundsEffectsVolumeLevel = 1;
                }
            }

            return i_GameScreen.SoundManager.SoundsEffectsVolumeLevel.ToString();
        }
    }

    public class ToggleSoundItem : MenuItem
    {
        public ToggleSoundItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            i_GameScreen.SoundManager.ToggleMute();

            return i_GameScreen.SoundManager.IsMute.ToString();
        }
    }

    public class BackgrounMusicVolumItem : MenuItem
    {
        public BackgrounMusicVolumItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            int currVolume = i_GameScreen.SoundManager.BackGroundVolumeLevel;
            if (i_Key == Keys.PageUp)
            {
                if (currVolume < 100)
                {
                    i_GameScreen.SoundManager.IncreaseBackGroundVolume();
                }
                else
                {
                    i_GameScreen.SoundManager.BackGroundVolumeLevel = 0;
                }
            }

            if (i_Key == Keys.PageDown)
            {
                if (currVolume > 0)
                {
                    i_GameScreen.SoundManager.DecreaseBackGroundVolume();
                }
                else
                {
                    i_GameScreen.SoundManager.BackGroundVolumeLevel = 1;
                }
            }

            return i_GameScreen.SoundManager.BackGroundVolumeLevel.ToString();
        }
    }

    public class DoneItem : MenuItem
    {
        public DoneItem(string i_Title, GameScreen i_GameScreen)
            : base(i_Title, i_GameScreen)
        {
        }

        public override void EnterScreen(GameScreen i_GameScreen)
        {
            i_GameScreen.ExitScreen();
        }
    }
}