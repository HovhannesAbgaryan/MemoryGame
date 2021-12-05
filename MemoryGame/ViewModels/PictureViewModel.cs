using MemoryGame.Models;
using System.Windows.Media;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Picture ViewModel
    /// </summary>
    public class PictureViewModel : ObservableObject
    {
        #region Fields

        private readonly PictureModel model;

        private bool isViewed;
        private bool isMatched;
        private bool isFailed;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Id of this slide
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Is being viewed by user?
        /// </summary>
        public bool IsViewed
        {
            get => isViewed;
            private set
            {
                isViewed = value;
                OnPropertyChanged("SlideImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// Has been matched?
        /// </summary>
        public bool IsMatched
        {
            get => isMatched;
            private set
            {
                isMatched = value;
                OnPropertyChanged("SlideImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// Has failed to be matched?
        /// </summary>
        public bool IsFailed
        {
            get => isFailed;
            private set
            {
                isFailed = value;
                OnPropertyChanged("SlideImage");
                OnPropertyChanged("BorderBrush");
            }
        }

        /// <summary>
        /// Can user select this slide?
        /// </summary>
        public bool IsSelectable => !IsMatched && !IsViewed;

        /// <summary>
        /// Image to be displayed
        /// </summary>
        public string SlideImage => IsMatched ? model.Source : IsViewed ? model.Source : "/MemoryGame;component/Assets/mystery_image.jpg";

        /// <summary>
        /// Brush color of border based on status
        /// </summary>
        public Brush BorderBrush => IsFailed ? Brushes.Red : IsMatched ? Brushes.Green : IsViewed ? Brushes.Yellow : Brushes.Black;

        #endregion Properties

        #region Constructors

        public PictureViewModel(PictureModel model)
        {
            this.model = model;
            Id = model.Id;
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Has been matched
        /// </summary>
        public void MarkMatched()
        {
            isMatched = true;
        }

        /// <summary>
        /// Has failed to match
        /// </summary>
        public void MarkFailed()
        {
            isFailed = true;
        }

        /// <summary>
        /// No longer being viewed
        /// </summary>
        public void ClosePeek()
        {
            isViewed = false;
            isFailed = false;
            OnPropertyChanged("isSelectable");
            OnPropertyChanged("SlideImage");
        }

        /// <summary>
        /// Let user view
        /// </summary>
        public void PeekAtImage()
        {
            isViewed = true;
            OnPropertyChanged("SlideImage");
        }

        #endregion Functions
    }
}
