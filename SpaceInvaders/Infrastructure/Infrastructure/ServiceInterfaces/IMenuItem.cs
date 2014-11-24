using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure
{
    public interface IMenuItem
    {
        string Title { get; }

        string TitleValue { get; }

        void EnterScreen(GameScreen i_GameScreen);

        string ItemSelected(GameScreen i_GameScreen, Keys i_Key);
    }
}
