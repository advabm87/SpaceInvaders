namespace SpaceInvaders
{
    using System.Collections.Generic;
    using Infrastructure;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class SpaceInvaders : Game
    {
        private GraphicsDeviceManager m_GraphicsMgr;
        private ScreensMananger m_ScreensMananger;
        private SoundManager m_SoundManager;
        private FontManager m_FontManager;
        private SettingsManager m_SettingsManager;
        private InputManager m_InputManager;

        public SpaceInvaders()
        {
            m_GraphicsMgr = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            m_InputManager = new InputManager(this);
            m_SoundManager = new SoundManager(this, @"Content\Sounds\GameAudio.xgs", @"Content\Sounds\WaveBank.xwb", @"Content\Sounds\SoundBank.xsb");
            m_FontManager = new FontManager(this, @"Fonts\Calibri");
            m_SettingsManager = new SettingsManager(this, 2, 3);
            m_ScreensMananger = new ScreensMananger(this);
            m_ScreensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.Title = "Space Invaders :)";
            this.IsMouseVisible = true;
            m_SoundManager.SoundBank.PlayCue("BackGroundMusic");
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            m_SoundManager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (m_InputManager.KeyPressed(Keys.M))
            {
                m_SoundManager.ToggleMute();
            }
        }
    }
}
