using System.Windows;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Game info ViewModel
    /// </summary>
    public class GameInfoViewModel : ObservableObject
    {
        #region Consts

        private const int maxAttempts = 4;
        private const int pointAward = 75;
        private const int pointDeduction = 15;

        #endregion Consts

        #region Fields

        private int matchAttempts;
        private int score;
        private bool isGameLost;
        private bool isGameWon;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Match attempts
        /// </summary>
        public int MatchAttempts
        {
            get => matchAttempts;
            private set
            {
                matchAttempts = value;
                OnPropertyChanged("MatchAttempts");
            }
        }

        /// <summary>
        /// Score
        /// </summary>
        public int Score
        {
            get => score;
            private set
            {
                score = value;
                OnPropertyChanged("Score");
            }
        }

        /// <summary>
        /// Lost message
        /// </summary>
        public Visibility LostMessage => isGameLost ? Visibility.Visible : Visibility.Hidden;

        /// <summary>
        /// Win message
        /// </summary>
        public Visibility WinMessage => isGameWon ? Visibility.Visible : Visibility.Hidden;

        #endregion Properties

        #region Functions

        /// <summary>
        /// Game status
        /// </summary>
        /// <param name="isWin">Is win?</param>
        public void GameStatus(bool isWin)
        {
            if (isWin)
            {
                isGameWon = true;
                OnPropertyChanged("WinMessage");
            }
            else
            {
                isGameLost = true;
                OnPropertyChanged("LostMessage");
            }
        }

        /// <summary>
        /// Clear info
        /// </summary>
        public void ClearInfo()
        {
            Score = 0;
            MatchAttempts = maxAttempts;

            isGameLost = false;
            isGameWon = false;

            OnPropertyChanged("LostMessage");
            OnPropertyChanged("WinMessage");
        }

        /// <summary>
        /// Award
        /// </summary>
        public void Award()
        {
            Score += pointAward;
            SoundManager.PlayCorrect();
        }

        /// <summary>
        /// Penalize
        /// </summary>
        public void Penalize()
        {
            Score -= pointDeduction;
            MatchAttempts--;
            SoundManager.PlayIncorrect();
        }

        #endregion Functions
    }
}
