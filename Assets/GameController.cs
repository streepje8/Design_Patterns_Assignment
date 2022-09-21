using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(AudioSource))]
public class GameController : Singleton<GameController>
{
    //Settings
    public TextAsset level;
    public bool isRecording = false;
    public float notespeed = 2f;
    
    //Pool and active notes
    public GameObject notePrefab;
    public UnityObjectPool<GameObject> notes;
    public List<RuntimeNote> hitNotes = new List<RuntimeNote>();
    
    //Score stuff
    public int score = 0;
    public TMP_Text scoretext;
    
    private string recordingString = "";
    private AudioSource source;
    private float delay = 1f; //Give slow computers time to load
    private bool started = false;
    private List<LoadedNote> loadedNotes = new List<LoadedNote>();
    private void Awake()
    {
        Instance = this;
        loadedNotes = LevelLoader.LoadLevel(level.text, notespeed); //Load the level
        notes = new UnityObjectPool<GameObject>(notePrefab);
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (delay > 0) delay -= Time.deltaTime;
        if (delay <= 0 && !started)
        {
            source.Play();
            started = true;
        }

        if (loadedNotes.Count > 0 && source.time > loadedNotes[0].time)
        {
            notes.GetInstance().GetComponent<RuntimeNote>().Activate(loadedNotes[0].color);
            loadedNotes.RemoveAt(0);
        }

        scoretext.text = score.ToString();
    }


    public void RedKey()
    {
        hitNotes.ForEach(x => x.KeyPress(false));
        if(isRecording) recordingString += source.time + "/RED\n";
    }

    public void BlueKey()
    {
        hitNotes.ForEach(x => x.KeyPress(true));
        if(isRecording) recordingString += source.time + "/BLUE\n";
    }

    public void PrintRecordedData() => Debug.Log(recordingString);
}


