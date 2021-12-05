using MemoryGame.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Slide collection ViewModel
    /// </summary>
    public class SlideCollectionViewModel : ObservableObject
    {
        #region Consts

        //Interval for how long a user peeks at selections
        private const int peekSeconds = 3;

        //Interval for how long a user has to memorize slides
        private const int openSeconds = 5;

        #endregion Consts

        #region Fields

        //Selected slides for matching
        private PictureViewModel firstSelectedSlide;
        private PictureViewModel secondSelectedSlide;

        //Timers for peeking at slides and initial display for memorizing
        private DispatcherTimer peekTimer;
        private DispatcherTimer openingTimer;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Collection of picture slides
        /// </summary>
        public ObservableCollection<PictureViewModel> MemorySlides { get; private set; }

        /// <summary>
        /// Are selected slides still being displayed
        /// </summary>
        public bool AreSlidesActive => firstSelectedSlide == null || secondSelectedSlide == null;

        /// <summary>
        /// Have all slides been matched
        /// </summary>
        public bool AllSlidesMatched
        {
            get
            {
                foreach (var slide in MemorySlides)
                    if (!slide.IsMatched)
                        return false;

                return true;
            }
        }

        //Can user select a slide
        public bool CanSelect { get; private set; }

        #endregion Properties

        #region Constructors

        public SlideCollectionViewModel()
        {
            peekTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, peekSeconds)
            };
            peekTimer.Tick += PeekTimerTick;

            openingTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, openSeconds)
            };
            openingTimer.Tick += OpeningTimer_Tick;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Create slides from images in file directory
        /// </summary>
        /// <param name="imagesPath">Images path</param>
        public void CreateSlides(string imagesPath)
        {
            //New list of slides
            MemorySlides = new ObservableCollection<PictureViewModel>();
            var models = GetModelsFrom(imagesPath);

            //Create slides with matching pairs from models
            for (int i = 0; i < 6; i++)
            {
                //Create 2 matching slides
                var newSlide = new PictureViewModel(models[i]);
                var newSlideMatch = new PictureViewModel(models[i]);

                //Add new slides to collection
                MemorySlides.Add(newSlide);
                MemorySlides.Add(newSlideMatch);

                //Initially display images for user
                newSlide.PeekAtImage();
                newSlideMatch.PeekAtImage();
            }

            ShuffleSlides();
            OnPropertyChanged("MemorySlides");
        }

        /// <summary>
        /// Select a slide to be matched
        /// </summary>
        /// <param name="slide">Slide</param>
        public void SelectSlide(PictureViewModel slide)
        {
            slide.PeekAtImage();

            if (firstSelectedSlide == null)
            {
                firstSelectedSlide = slide;
            }
            else if (secondSelectedSlide == null)
            {
                secondSelectedSlide = slide;
                HideUnmatched();
            }

            SoundManager.PlayCardFlip();
            OnPropertyChanged("areSlidesActive");
        }

        /// <summary>
        /// Are the selected slides a match
        /// </summary>
        public bool CheckIfMatched()
        {
            if (firstSelectedSlide.Id == secondSelectedSlide.Id)
            {
                MatchCorrect();
                return true;
            }
            else
            {
                MatchFailed();
                return false;
            }
        }

        /// <summary>
        /// Selected slides did not match
        /// </summary>
        private void MatchFailed()
        {
            firstSelectedSlide.MarkFailed();
            secondSelectedSlide.MarkFailed();
            ClearSelected();
        }

        /// <summary>
        /// Selected slides matched
        /// </summary>
        private void MatchCorrect()
        {
            firstSelectedSlide.MarkMatched();
            secondSelectedSlide.MarkMatched();
            ClearSelected();
        }

        /// <summary>
        /// Clear selected slides
        /// </summary>
        private void ClearSelected()
        {
            firstSelectedSlide = null;
            secondSelectedSlide = null;
            CanSelect = false;
        }

        /// <summary>
        /// Reveal all unmatched slides
        /// </summary>
        public void RevealUnmatched()
        {
            foreach (var slide in MemorySlides)
            {
                if (!slide.IsMatched)
                {
                    peekTimer.Stop();
                    slide.MarkFailed();
                    slide.PeekAtImage();
                }
            }
        }

        /// <summary>
        /// Hide all slides that are unmatched
        /// </summary>
        public void HideUnmatched()
        {
            peekTimer.Start();
        }

        /// <summary>
        /// Display slides for memorizing
        /// </summary>
        public void Memorize()
        {
            openingTimer.Start();
        }

        /// <summary>
        /// Get slide picture models for creating picture views
        /// </summary>
        /// <param name="relativePath">Relative path</param>
        /// <returns>List of PictureModels</returns>
        private List<PictureModel> GetModelsFrom(string relativePath)
        {
            //List of models for picture slides
            var models = new List<PictureModel>();

            //Get all image URIs in folder
            var images = Directory.GetFiles(@relativePath, "*.jpg", SearchOption.AllDirectories);

            //Slide id begin at 0
            var id = 0;

            foreach (string image in images)
            {
                models.Add(new PictureModel() { Id = id, Source = "/MemoryGame;component/" + image });
                id++;
            }

            return models;
        }

        /// <summary>
        /// Randomize the location of the slides in collection
        /// </summary>
        private void ShuffleSlides()
        {
            //Randomizing slide indexes
            var random = new Random();

            //Shuffle memory slides
            for (int i = 0; i < 64; i++)
            {
                MemorySlides.Reverse();
                MemorySlides.Move(random.Next(0, MemorySlides.Count), random.Next(0, MemorySlides.Count));
            }
        }

        /// <summary>
        /// Close slides being memorized
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void OpeningTimer_Tick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                slide.ClosePeek();
                CanSelect = true;
            }
            OnPropertyChanged("areSlidesActive");
            openingTimer.Stop();
        }

        /// <summary>
        /// Display selected card
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void PeekTimerTick(object sender, EventArgs e)
        {
            foreach (var slide in MemorySlides)
            {
                if (!slide.IsMatched)
                {
                    slide.ClosePeek();
                    CanSelect = true;
                }
            }
            OnPropertyChanged("areSlidesActive");
            peekTimer.Stop();
        }

        #endregion Functions
    }
}
