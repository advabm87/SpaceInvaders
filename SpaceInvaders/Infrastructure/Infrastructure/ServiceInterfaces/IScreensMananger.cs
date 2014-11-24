//*** Guy Ronen © 2008-2011 ***//

namespace Infrastructure
{
    public interface IScreensMananger
    {
        GameScreen ActiveScreen { get; }
        
        void SetCurrentScreen(GameScreen i_NewScreen);
        
        void Push(GameScreen i_GameScreen);
        
        bool Remove(GameScreen i_Screen);
        
        void Add(GameScreen i_Screen);
    }
}
