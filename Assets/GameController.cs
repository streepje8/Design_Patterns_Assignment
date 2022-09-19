using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public struct levelnote
{
    public float time;
    public bool color;

    public levelnote(float time, bool color)
    {
        this.time = time;
        this.color = color;
    }
}

[RequireComponent(typeof(AudioSource))]
public class GameController : Singleton<GameController>
{
    public int score = 0;

    public List<HitNote> hitNotes = new List<HitNote>();
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
    private List<levelnote> lnotes = new List<levelnote>();
    private void Awake()
    {
        Instance = this;
        notes = new UnityObjectPool<GameObject>(notePrefab);
        source = GetComponent<AudioSource>();
        foreach(string s in level.text.Split('\n'))
        {
            if (s.Length > 2)
            {
                string b = Regex.Replace(s, @"/s+", "");
                string[] a = b.Split('/');
                string c = Regex.Replace(a[1], "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
                float time = float.Parse(a[0]) - (17f/notespeed);
                if(time > 0.01f)
                    lnotes.Add(new levelnote(time,
                    c.Equals("BLUE", StringComparison.OrdinalIgnoreCase)));
            }
        }
    }

    private void Update()
    {
        if (delay > 0) delay -= Time.deltaTime;
        if (delay <= 0 && !started)
        {
            source.Play();
            started = true;
        }

        if (lnotes.Count > 0 && source.time > lnotes[0].time)
        {
            HitNote n = notes.GetInstance().GetComponent<HitNote>();
            n.Activate(lnotes[0].color);
            lnotes.RemoveAt(0);
        }

        scoretext.text = score.ToString();
    }


    public void RedKey()
    {
        foreach (var hitNote in hitNotes)
        {
            hitNote.KeyPress(false);
        }
        if(isRecording)recordingString += source.time + "/RED\n";
    }

    public void BlueKey()
    {
        foreach (var hitNote in hitNotes)
        {
            hitNote.KeyPress(true);
        }
        if(isRecording)recordingString += source.time + "/BLUE\n";
    }

    public void PrintKey()
    {
        Debug.Log(recordingString);
    }
}
