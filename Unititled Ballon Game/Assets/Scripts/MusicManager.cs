using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance; // Singleton instance

    public AudioClip startSceneSong; // Song for the main menu
    public AudioClip winSceneSong;
    public AudioClip loseSceneSong;

    public AudioClip[] levelSongs; // Array of songs for levels
    private int currentSongIndex = 0; // Tracks which song is currently playing in the level playlist

    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern to ensure only one instance of the MusicManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
            return; // Ensure only the original persists
        }

        // Get or add AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
    }

    // This method is called whenever a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // Handle music for specific scenes
        if (sceneName == "MainMenu")
        {
            PlaySceneSpecificSong(startSceneSong);
        }
        else if (sceneName == "LevelCompleteScene")
        {
            PlaySceneSpecificSong(winSceneSong);
        }
        else if (sceneName == "LevelLostScene")
        {
            PlaySceneSpecificSong(loseSceneSong);
        }
        else
        {
            // If it's a level scene, continue or start the playlist
            PlayLevelMusic();
        }
    }

    // Plays a specific song for certain scenes
    private void PlaySceneSpecificSong(AudioClip song)
    {
        if (audioSource.clip != song) // Only change if it's a different song
        {
            audioSource.Stop(); // Stop the current song
            audioSource.clip = song;
            audioSource.loop = true; // Loop scene-specific songs
            audioSource.Play();
        }
    }

    // Plays through the level music playlist
    private void PlayLevelMusic()
    {
        if (levelSongs.Length == 0) return; // If no level songs are set, return

        // If the audio source is playing and the clip is already a level song, don't interrupt
        if (audioSource.isPlaying && System.Array.Exists(levelSongs, song => song == audioSource.clip))
        {
            return; // Do not interrupt if it's already playing a level song
        }

        // Play the current song in the playlist
        audioSource.Stop(); // Stop the current song
        audioSource.clip = levelSongs[currentSongIndex];
        audioSource.loop = false; // Do not loop a single song
        audioSource.Play();

        // Subscribe to the AudioSource's 'clip ended' event
        StartCoroutine(WaitForSongToEnd());
    }

    // Coroutine to wait for the song to finish and move to the next one
    private IEnumerator WaitForSongToEnd()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying); // Wait until the current song finishes

        // Move to the next song in the playlist
        currentSongIndex++;

        // If we've gone through all songs, reset to the first song
        if (currentSongIndex >= levelSongs.Length)
        {
            currentSongIndex = 0;
        }

        // Start playing the next song in the playlist
        PlayLevelMusic();
    }
}
