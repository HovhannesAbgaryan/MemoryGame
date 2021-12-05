namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Start menu ViewModel
    /// </summary>
    public class StartMenuViewModel
    {
        #region Fields

        private readonly MainWindow mainWindow;

        #endregion Fields

        #region Constructors

        public StartMenuViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            SoundManager.PlayBackgroundMusic();
        }

        #endregion Constructors

        #region Functions

        public void StartNewGame(int categoryIndex)
        {
            var category = (SlideCategories)categoryIndex;
            GameViewModel newGame = new GameViewModel(category);
            mainWindow.DataContext = newGame;
        }

        #endregion Functions
    }
}
