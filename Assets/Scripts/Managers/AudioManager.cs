using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Audio Manager
public class AudioManager : SceneSingleton<AudioManager> {

    // References
    AudioSource music;
    AudioSource sfx;
    AudioSource sfxLoop;
    AudioSource menu;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    // Map string to audio
    Dictionary<string, AudioClip> songs = new Dictionary<string, AudioClip>();

    // Background music
    public AudioClip tutorial;
    public AudioClip shrine;
    public AudioClip lava;
    public AudioClip skeletonBoss;

    // Sound Effects

    // Events
    public AudioClip victory;
    public AudioClip item;
    public AudioClip save;

    // Weapons
    public AudioClip gun;
    public AudioClip sword;
    public AudioClip wind;
    public AudioClip fire;

    // Skills
    public AudioClip thunder;
    public AudioClip storm;
    public AudioClip sprint;
    public AudioClip barrier;
    public AudioClip portal;
    public AudioClip teleport;
    public AudioClip explosion;
    public AudioClip blackHole;
    public AudioClip holyBeam;
    public AudioClip holyCharge;
    public AudioClip ice;

    // PLayer
    public AudioClip damage;
    public AudioClip switchWeapon;

    // Enemies
    public AudioClip fireball;

    // Inventory
    public AudioClip pause;
    public AudioClip unpause;
    public AudioClip switchWindow;
    public AudioClip unlock;

    // Level
    public string levelSong;

    // Setup
    void Start() {
        // Get players
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        music = sources[0];
        sfx = sources[1];
        sfxLoop = sources[2];
        menu = sources[3];
        music.loop = true;
        sfxLoop.loop = true;
        sfxLoop.volume = 0.5f;
        music.ignoreListenerPause = true;
        menu.ignoreListenerPause = true;

        // Adjust initial volume
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 1)) * 20);
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol", 1)) * 20);

        // Music
        songs.Add("tutorial", tutorial);
        songs.Add("shrine", shrine);
        songs.Add("lava", lava);
        songs.Add("skeletonBoss", skeletonBoss);

        // Events
        songs.Add("victory", victory);
        songs.Add("item", item);
        songs.Add("save", save);

        // Weapons
        songs.Add("sword", sword);
        songs.Add("gun", gun);
        songs.Add("wind", wind);
        songs.Add("fire", fire);

        // Skills
        songs.Add("thunder", thunder);
        songs.Add("storm", storm);
        songs.Add("sprint", sprint);
        songs.Add("barrier", barrier);
        songs.Add("portal", portal);
        songs.Add("teleport", teleport);
        songs.Add("explosion", explosion);
        songs.Add("blackHole", blackHole);
        songs.Add("holyCharge", holyCharge);
        songs.Add("holyBeam", holyBeam);
        songs.Add("ice", ice);

        // Player
        songs.Add("damage", damage);
        songs.Add("switchWeapon", switchWeapon);

        // Enemies
        songs.Add("fireball", fireball);

        // Inventory
        songs.Add("pause", pause);
        songs.Add("unpause", unpause);
        songs.Add("switchWindow", switchWindow);
        songs.Add("unlock", unlock);

        // Play level music
        PlaySoundtrack(levelSong);
    }

    // Play background music
    public void PlaySoundtrack(string song) {
        music.clip = songs[song];
        music.Play();
    }

    // Play SFX
    public void PlaySound(string sound, float volume = 1) {
        sfx.PlayOneShot(songs[sound], volume);
    }

    // Start Looping SFX
    public void StartLoop(string sound) {
        sfxLoop.clip = songs[sound];
        sfxLoop.Play();
    }

    // Stop Looping SFX
    public void StopLoop() {
        sfxLoop.Stop();
    }

    // Play Menu SFX
    public void PlayMenu(string sound, float volume = 1) {
        menu.PlayOneShot(songs[sound], volume);
    }
}
