using System;
using System.Windows.Threading;

namespace MemoryGame.ViewModels
{
    /// <summary>
    /// Timer ViewModel
    /// </summary>
    public class TimerViewModel : ObservableObject
    {
        #region Fields

        private DispatcherTimer playedTimer;
        private TimeSpan timePlayed;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Time
        /// </summary>
        public TimeSpan Time
        {
            get => timePlayed;
            set
            {
                timePlayed = value;
                OnPropertyChanged("Time");
            }
        }

        #endregion Properties

        #region Constructors

        public TimerViewModel(TimeSpan time)
        {
            playedTimer = new DispatcherTimer
            {
                Interval = time
            };
            playedTimer.Tick += PlayedTimerTick;
            timePlayed = new TimeSpan();
        }

        #endregion Constructors

        #region Functions

        /// <summary>
        /// Start timer
        /// </summary>
        public void Start()
        {
            playedTimer.Start();
        }

        /// <summary>
        /// Stop timer
        /// </summary>
        public void Stop()
        {
            playedTimer.Stop();
        }

        /// <summary>
        /// Played timer tick
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void PlayedTimerTick(object sender, EventArgs e)
        {
            Time = timePlayed.Add(new TimeSpan(0, 0, 1));
        }

        #endregion Functions
    }
}
