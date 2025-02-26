using UnityEngine;
using UnityEngine.SceneManagement;

namespace KlaskMP
{
    /// <summary>
    /// Handles playback of background music, 2D and 3D one-shot clips during the game.
    /// Makes use of the PoolManager for activating 3D AudioSources at desired world positions.
    /// </summary>
	public class AudioManager : MonoBehaviour
	{
        //reference to this script instance
		private static AudioManager instance;

        /// <summary>
        /// AudioSource for playing back lengthy music clips.
        /// </summary>
		public AudioSource musicSource;

        /// <summary>
        /// AudioSource for playing back one-shot 2D clips.
        /// </summary>
		public AudioSource audioSource;

        /// <summary>
        /// Prefab instantiated for playing back one-shot 3D clips.
        /// </summary>
        public GameObject oneShotPrefab;

        /// <summary>
        /// Array for storing background music clips, so they can be
        /// referenced in PlayMusic() by passing in their index value.
        /// </summary>
        public AudioClip[] musicClips;
        

        // Sets the instance reference, if not set already,
        // and keeps listening to scene changes.
		void Awake()
		{
            if (instance != null)
                return;

            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
		}


        /// <summary>
        /// Returns a reference to this script instance.
        /// </summary>
		public static AudioManager GetInstance()
		{
			return instance;
		}


        // Stop playing music after switching scenes. To keep playing
        // music in the new scene, this requires calling PlayMusic() again.
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            musicSource.Stop();
        }


        /// <summary>
        /// Play sound clip in 2D on the background audio source.
        /// There can only be one music clip playing at the same time.
        /// Only plays music if the player enabled it in the settings.
        /// </summary>
        public static void PlayMusic(int index)
        {
            instance.musicSource.clip = instance.musicClips[index];

            //user settings could have disabled the audio source
            if (instance.musicSource.enabled)
                instance.musicSource.Play();
        }


        /// <summary>
        /// Play sound clip passed in in 2D space.
        /// </summary>
        public static void Play2D(AudioClip clip)
        {
            instance.audioSource.PlayOneShot(clip);
        }
    }
}

