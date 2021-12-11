using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Audio Manager
public class AudioManager : SceneSingleton<AudioManager> {

    // References
    AudioSource player;
    AudioSource loop;

    // Map string to audio
    Dictionary<string, AudioClip> songs = new Dictionary<string, AudioClip>();

    // Background music
    public AudioClip tutorial;
    public AudioClip shrine;
    public AudioClip lava;
    public AudioClip skeletonBoss;

    // Sound Effects

    // Weapons
    public AudioClip gun;
    public AudioClip sword;

    // Skills
    public AudioClip thunder;
    public AudioClip sprint;
    public AudioClip barrier;
    public AudioClip portal;
    public AudioClip teleport;
    public AudioClip explosion;

    // Enemies
    public AudioClip fireball;

    // Level
    public string levelSong;

    // Setup
    void Start() {
        // Get players
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        player = sources[0];
        loop = sources[1];
        player.loop = true;
        loop.loop = true;
        loop.volume = 0.5f;

        // Music
        songs.Add("tutorial", tutorial);
        songs.Add("shrine", shrine);
        songs.Add("lava", lava);
        songs.Add("skeletonBoss", skeletonBoss);

        // Weapons
        songs.Add("sword", sword);
        songs.Add("gun", gun);

        // Skills
        songs.Add("thunder", thunder);
        songs.Add("sprint", sprint);
        songs.Add("barrier", barrier);
        songs.Add("portal", portal);
        songs.Add("teleport", teleport);
        songs.Add("explosion", explosion);

        // Enemies
        songs.Add("fireball", fireball);

        // Play level music
        PlaySoundtrack(levelSong);
    }

    // Play background music
    public void PlaySoundtrack(string song) {
        player.clip = songs[song];
        player.Play();
    }

    // Play SFX
    public void PlaySound(string sound) {
        player.PlayOneShot(songs[sound]);
    }

    // Start Looping SFX
    public void StartLoop(string sound) {
        loop.clip = songs[sound];
        loop.Play();
    }

    // Stop Looping SFX
    public void StopLoop() {
        loop.Stop();
    }
}
