using System;
using System.IO;
using System.Windows.Media;

namespace MemoryGame
{
    public static class SoundManager
    {
        #region Fields

        private static MediaPlayer mediaPlayer = new MediaPlayer();
        private static MediaPlayer effectPlayer = new MediaPlayer();

        #endregion Fields

        #region Functions

        /// <summary>
        /// Play background music
        /// </summary>
        public static void PlayBackgroundMusic()
        {
            mediaPlayer.Open(new Uri(Path.Combine(Environment.CurrentDirectory, "Assets/background_music.mp3")));
            mediaPlayer.Play();
        }

        /// <summary>
        /// Play card flip
        /// </summary>
        public static void PlayCardFlip() => PlayEffect("card_flip.mp3");

        /// <summary>
        /// Play correct
        /// </summary>
        public static void PlayCorrect() => PlayEffect("correct_match.mp3");

        /// <summary>
        /// Play incorrect
        /// </summary>
        public static void PlayIncorrect() => PlayEffect("incorrect_match.mp3");

        /// <summary>
        /// Play effect
        /// </summary>
        /// <param name="fileName">File name</param>
        private static void PlayEffect(string fileName)
        {
            effectPlayer.Open(new Uri(Path.Combine(Environment.CurrentDirectory, "Assets/SoundEffects/" + fileName)));
            effectPlayer.Play();
        }

        #endregion Functions
    }
}
