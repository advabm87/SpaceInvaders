namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    public class SoundManager : GameService, ISoundManager
    {
        private AudioEngine m_AudioEngine;
        private WaveBank m_WaveBank;
        private SoundBank m_SoundBank;
        private string m_AudioEnginePath;
        private string m_WaveBankPath;
        private string m_SoundBankPath;
        private bool m_Mute = false;
        private float m_BackGroundMusicVolume = 1;
        private float m_SoundEffectsMusicVolume = 1;

        public SoundManager(Game i_Game, string i_AudioEnginePath, string i_WaveBankPath, string i_SoundBankPath)
            : base(i_Game)
        {
            m_AudioEnginePath = i_AudioEnginePath;
            m_WaveBankPath = i_WaveBankPath;
            m_SoundBankPath = i_SoundBankPath;
        }

        public override void Initialize()
        {
            m_AudioEngine = new AudioEngine(m_AudioEnginePath);
            m_WaveBank = new WaveBank(m_AudioEngine, m_WaveBankPath);
            m_SoundBank = new SoundBank(m_AudioEngine, m_SoundBankPath);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            m_AudioEngine.Update();
            base.Update(gameTime);
        }

        public void UnloadContent()
        {
            m_AudioEngine.Dispose();
            m_WaveBank.Dispose();
            m_SoundBank.Dispose();
        }

        public void ToggleMute()
        {
            m_Mute = !m_Mute;
            m_AudioEngine.GetCategory("BackGround").SetVolume(m_Mute ? 0 : m_BackGroundMusicVolume);
            m_AudioEngine.GetCategory("SoundsEffects").SetVolume(m_Mute ? 0 : m_SoundEffectsMusicVolume);
        }

        public void IncreaseBackGroundVolume()
        {
            m_BackGroundMusicVolume += 0.1f;
            m_BackGroundMusicVolume = MathHelper.Clamp(m_BackGroundMusicVolume, 0, 1);
            m_AudioEngine.GetCategory("BackGround").SetVolume(m_BackGroundMusicVolume);
        }

        public void DecreaseBackGroundVolume()
        {
            m_BackGroundMusicVolume -= 0.1f;
            m_BackGroundMusicVolume = MathHelper.Clamp(m_BackGroundMusicVolume, 0, 1);
            m_AudioEngine.GetCategory("BackGround").SetVolume(m_BackGroundMusicVolume);
        }

        public void IncreaseSoundsEffectsVolume()
        {
            m_SoundEffectsMusicVolume += 0.1f;
            m_SoundEffectsMusicVolume = MathHelper.Clamp(m_SoundEffectsMusicVolume, 0, 1);
            m_AudioEngine.GetCategory("SoundsEffects").SetVolume(m_SoundEffectsMusicVolume);
        }

        public void DecreaseSoundsEffectsVolume()
        {
            m_SoundEffectsMusicVolume -= 0.1f;
            m_SoundEffectsMusicVolume = MathHelper.Clamp(m_SoundEffectsMusicVolume, 0, 1);
            m_AudioEngine.GetCategory("SoundsEffects").SetVolume(m_SoundEffectsMusicVolume);
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISoundManager), this);
        }

        public SoundBank SoundBank
        {
            get { return m_SoundBank; }
        }

        public int BackGroundVolumeLevel
        {
            get
            {
                return (int)(Math.Round(m_BackGroundMusicVolume, 1) * 100);
            }
            
            set
            {
                if (value >= 0 && value <= 1)
                {
                    m_BackGroundMusicVolume = value;
                    m_AudioEngine.GetCategory("BackGround").SetVolume(m_BackGroundMusicVolume);
                }
            }
        }

        public int SoundsEffectsVolumeLevel
        {
            get
            {
                return (int)(Math.Round(m_SoundEffectsMusicVolume, 1) * 100);
            }
            
            set
            {
                if (value >= 0 && value <= 1)
                {
                    m_SoundEffectsMusicVolume = value;
                    m_AudioEngine.GetCategory("SoundsEffects").SetVolume(m_SoundEffectsMusicVolume);
                }
            }
        }

        public bool IsMute
        {
            get { return m_Mute; }
        }
    }
}
