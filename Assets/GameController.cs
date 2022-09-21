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
    public int score = 0;

    public List<RuntimeNote> hitNotes = new List<RuntimeNote>();
    public GameObject notePrefab;

    public TextAsset level;
    public bool isRecording = false;
    private string recordingString = "";
    public TMP_Text scoretext;

    public float notespeed = 2f;

    public UnityObjectPool<GameObject> notes;
    private AudioSource source;
    private float delay = 10f;
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
            RuntimeNote n = notes.GetInstance().GetComponent<RuntimeNote>();
            n.Activate(loadedNotes[0].color);
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


