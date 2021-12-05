using System;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Slide categories
    /// </summary>
    public enum SlideCategories
    {
        Animals,
        Cars,
        Foods
    }

    /// <summary>
    /// Game ViewModel
    /// </summary>
    public class GameViewModel : ObservableObject
    {
        #region Properties

        /// <summary>
        /// Collection of slides we are playing with
        /// </summary>
        public SlideCollectionViewModel Slides { get; private set; }

        /// <summary>
        /// Game information scores, attempts etc
        /// </summary>
        public GameInfoViewModel GameInfo { get; private set; }

        /// <summary>
        /// Game timer for elapsed time
        /// </summary>
        public TimerViewModel Timer { get; private set; }

        /// <summary>
        /// Category we are playing in
        /// </summary>
        public SlideCategories Category { get; private set; }

        #endregion Properties

        #region Constructors

        public GameViewModel(SlideCategories category)
        {
            Category = category;
            SetupGame(category);
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Initialize game essentials
        /// </summary>
        /// <param name="category">Slide category</param>
        private void SetupGame(SlideCategories category)
        {
            Slides = new SlideCollectionViewModel();
            Timer = new TimerViewModel(new TimeSpan(0, 0, 1));
            GameInfo = new GameInfoViewModel();

            //Set attempts to the maximum allowed
            GameInfo.ClearInfo();

            //Create slides from image folder then display to be memorized
            Slides.CreateSlides("Assets/" + category.ToString());
            Slides.Memorize();

            //Game has started, begin count.
            Timer.Start();

            //Slides have been updated
            OnPropertyChanged("Slides");
            OnPropertyChanged("Timer");
            OnPropertyChanged("GameInfo");
        }

        /// <summary>
        /// Slide has been clicked
        /// </summary>
        /// <param name="slide">Slide</param>
        public void ClickedSlide(object slide)
        {
            if (Slides.CanSelect)
            {
                var selected = slide as PictureViewModel;
                Slides.SelectSlide(selected);
            }

            if (!Slides.AreSlidesActive)
            {
                if (Slides.CheckIfMatched())
                    GameInfo.Award(); //Correct match
                else
                    GameInfo.Penalize(); //Incorrect match
            }

            GameStatus();
        }

        /// <summary>
        /// Status of the current game
        /// </summary>
        private void GameStatus()
        {
            if (GameInfo.MatchAttempts < 0)
            {
                GameInfo.GameStatus(false);
                Slides.RevealUnmatched();
                Timer.Stop();
            }

            if (Slides.AllSlidesMatched)
            {
                GameInfo.GameStatus(true);
                Timer.Stop();
            }
        }

        /// <summary>
        /// Restart game
        /// </summary>
        public void Restart()
        {
            SoundManager.PlayIncorrect();
            SetupGame(Category);
        }

        #endregion Functions
    }
}
