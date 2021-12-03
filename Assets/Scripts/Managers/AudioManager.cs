using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// Audio Manager
public class AudioManager : SceneSingleton<AudioManager> {

    // References
    AudioSource player;

    // Map string to audio
    Dictionary<string, AudioClip> songs = new Dictionary<string, AudioClip>();

    // Background music
    public AudioClip tutorial;
    public AudioClip shrine;
    public AudioClip lava;
    public AudioClip skeletonBoss;

    // Sound Effects
    public AudioClip fireball;
    public AudioClip gun;
    public AudioClip thunder;
    public AudioClip sword;

    // Level
    public string levelSong;

    // Setup
    void Start() {
        player = gameObject.GetComponent<AudioSource>();
        songs.Add("tutorial", tutorial);
        songs.Add("shrine", shrine);
        songs.Add("lava", lava);
        songs.Add("skeletonBoss", skeletonBoss);
        songs.Add("fireball", fireball);
        songs.Add("sword", sword);
        songs.Add("gun", gun);
        songs.Add("thunder", thunder);
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
}
