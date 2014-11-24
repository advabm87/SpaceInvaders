using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure
{
    public class MenuItem : IMenuItem
    {
        private string m_Title;
        private string m_TitleValue = string.Empty;

        public MenuItem(string i_Title, GameScreen i_GameScreen)
            : base()
        {
            m_Title = i_Title;
            m_GameScreen = i_GameScreen;
        }

        public string Title
        {
            get { return m_Title; }
        }

        public string TitleValue
        {
            get { return m_TitleValue; }
            set { m_TitleValue = value; }
        }

        public virtual void EnterScreen(GameScreen i_GameScreen) 
        { 
        }

        public virtual string ItemSelected(GameScreen i_GameScreen, Keys i_Key)
        {
            return string.Empty;
        }

        private GameScreen m_GameScreen;

        public GameScreen GameScreen
        {
            get { return m_GameScreen; }
        }
    }
}