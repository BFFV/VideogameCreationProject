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
    AudioSource menu;
    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    public AudioMixerGroup loopMixer;
    public string currentMusic;

    // Map string to audio
    Dictionary<string, AudioClip> songs = new Dictionary<string, AudioClip>();

    // Map string to audiosource for loops
    Dictionary<string, AudioSource> loops = new Dictionary<string, AudioSource>();

    // Background music
    public AudioClip tutorial;
    public AudioClip shrine;
    public AudioClip lava;
    public AudioClip heaven1;
    public AudioClip heaven2;
    public AudioClip finalArena;
    public AudioClip skeletonBoss;
    public AudioClip angelBoss;
    public AudioClip finalBoss1;
    public AudioClip finalBoss2;
    public AudioClip finalBoss3;

    // Sound Effects

    // Events
    public AudioClip victory;
    public AudioClip item;
    public AudioClip save;
    public AudioClip laser;

    // Weapons
    public AudioClip gun;
    public AudioClip sword;
    public AudioClip wind;
    public AudioClip fire;
    public AudioClip burning;

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
        menu = sources[2];
        music.loop = true;
        music.ignoreListenerPause = true;
        menu.ignoreListenerPause = true;

        // Adjust initial volume
        musicMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol", 1)) * 20);
        sfxMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol", 1)) * 20);

        // Music
        currentMusic = levelSong;
        songs.Add("tutorial", tutorial);
        songs.Add("shrine", shrine);
        songs.Add("lava", lava);
        songs.Add("heaven1", heaven1);
        songs.Add("heaven2", heaven2);
        songs.Add("finalArena", finalArena);
        songs.Add("skeletonBoss", skeletonBoss);
        songs.Add("angelBoss", angelBoss);
        songs.Add("finalBoss1", finalBoss1);
        songs.Add("finalBoss2", finalBoss2);
        songs.Add("finalBoss3", finalBoss3);

        // Events
        songs.Add("victory", victory);
        songs.Add("item", item);
        songs.Add("save", save);
        songs.Add("laser", laser);

        // Weapons
        songs.Add("sword", sword);
        songs.Add("gun", gun);
        songs.Add("wind", wind);
        songs.Add("tornado", wind);
        songs.Add("fire", fire);
        songs.Add("burning", burning);

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
        currentMusic = song;
    }

    // Play SFX
    public void PlaySound(string sound, float volume = 1) {
        sfx.PlayOneShot(songs[sound], volume);
    }

    // Start Looping SFX
    public void StartLoop(string sound) {
        AudioSource looper;
        if (loops.ContainsKey(sound)) {
            looper = loops[sound];
        } else {
            looper = gameObject.AddComponent<AudioSource>();
            looper.outputAudioMixerGroup = loopMixer;
            looper.loop = true;
            looper.volume = 0.5f;
            looper.clip = songs[sound];
            loops.Add(sound, looper);
        }
        looper.Play();
    }

    // Stop Looping SFX
    public void StopLoop(string loop) {
        loops[loop].Stop();
    }

    // Play Menu SFX
    public void PlayMenu(string sound, float volume = 1) {
        menu.PlayOneShot(songs[sound], volume);
    }
}
