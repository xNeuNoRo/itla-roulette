using NAudio.Wave;

namespace SoundUtils
{
    public class Sound
    {
        private static IWavePlayer? AudioPlayer;
        private static AudioFileReader? AudioFile;
        private static bool LoopEnabled = false;

        public static IWavePlayer? LoadAudio(string AudioFilePath, int? startAtMs = null)
        {
            StopAudio();

            if (!File.Exists(AudioFilePath))
                return AudioPlayer;

            AudioFile = new AudioFileReader(AudioFilePath);

            if (
                startAtMs != null
                && startAtMs >= 0
                && startAtMs <= AudioFile.TotalTime.TotalMilliseconds
            )
                AudioFile.CurrentTime = TimeSpan.FromMilliseconds(startAtMs.Value);

            AudioPlayer = new WaveOutEvent();
            AudioPlayer.Init(AudioFile);

            return AudioPlayer;
        }

        public static bool PlayAudio()
        {
            try
            {
                if (AudioPlayer == null || IsPlaying())
                    return false;

                AudioPlayer?.Play();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void PlayAudioLoop(string AudioFilePath, double secondsBeforeEnd = 0.5)
        {
            if (LoadAudio(AudioFilePath) == null)
                return;

            LoopEnabled = true;

            Task.Run(() =>
            {
                while (LoopEnabled)
                {
                    if (AudioFile == null || AudioPlayer == null)
                        break;

                    AudioFile.Position = 0;
                    AudioPlayer.Play();

                    while (AudioPlayer.PlaybackState == PlaybackState.Playing)
                    {
                        if (
                            AudioFile.TotalTime.TotalMilliseconds
                                - AudioFile.CurrentTime.TotalMilliseconds
                            <= secondsBeforeEnd * 1000
                        )
                        {
                            break;
                        }

                        Thread.Sleep(50);
                    }
                }
            });
        }

        public static int GetAudioCurrentTime()
        {
            if (!HasAudioLoaded() || AudioFile == null || AudioPlayer == null)
                return 0;

            return (int)AudioFile.CurrentTime.TotalMilliseconds;
        }

        public static bool StopAudio()
        {
            try
            {
                if (AudioFile == null || !IsPlaying())
                    return false;

                AudioPlayer?.Stop();
                AudioPlayer?.Dispose();
                AudioPlayer = null;

                if (HasAudioLoaded())
                {
                    AudioFile?.Dispose();
                    AudioFile = null;
                }

                LoopEnabled = false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RestartAudio()
        {
            try
            {
                if (!HasAudioLoaded())
                    return false;

                if (AudioFile != null)
                {
                    AudioFile.Position = 0;
                }

                AudioPlayer?.Stop();
                AudioPlayer?.Play();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool PauseAudio()
        {
            if (AudioPlayer != null && AudioPlayer.PlaybackState == PlaybackState.Playing)
            {
                AudioPlayer.Pause();
                return true;
            }
            return false;
        }

        public static bool ResumeAudio()
        {
            if (AudioPlayer != null && AudioPlayer.PlaybackState == PlaybackState.Paused)
            {
                AudioPlayer.Play();
                return true;
            }
            return false;
        }

        public static bool isPaused()
        {
            return AudioPlayer != null && AudioPlayer.PlaybackState == PlaybackState.Paused;
        }

        public static bool isLoopEnabled()
        {
            return LoopEnabled;
        }

        public static bool SetVolume(float volume)
        {
            try
            {
                if (!HasAudioLoaded())
                    return false;

                if (AudioFile != null)
                {
                    AudioFile.Volume = volume;
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsPlaying()
        {
            return AudioPlayer != null && AudioPlayer.PlaybackState == PlaybackState.Playing;
        }

        public static bool HasAudioLoaded()
        {
            return AudioFile != null && AudioFile.Length > 0;
        }
    }
}
