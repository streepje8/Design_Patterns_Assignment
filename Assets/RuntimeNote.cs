using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class RuntimeNote : MonoBehaviour
{
    public bool isActive = false;
    public float hitDistance = 0.5f;
    public bool isHittable { get; private set; }
    private SpriteRenderer rend;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        GameController.Instance.hitNotes.Add(this);
    }

    private void OnDestroy()
    {
        GameController.Instance.hitNotes.Remove(this);
    }

    public void Activate(bool color)
    {
        isActive = true;
        if (color)
        {
            rend.color = Color.blue;
            transform.position = new Vector3(10, 2.5f, 0);
        }
        else
        {
            rend.color = Color.red;
            transform.position = new Vector3(10, -2.5f, 0);
        }
        rend.enabled = true;
    }
    
    void Update()
    {
        if (isActive)
        {
            var position = transform.position;
            position += Vector3.left * (GameController.Instance.notespeed * Time.deltaTime);
            isHittable = position.x < -7 + hitDistance && position.x > -7 - hitDistance; //Bad hardcode!
            transform.position = position;
            if (transform.position.x < -11)
            {
                isActive = false;
                transform.position = Vector3.one * 1000f;
                rend.enabled = false;
                GameController.Instance.notes.FreeInstance(this.gameObject);
            }
        }
    }

    public void KeyPress(bool color)
    {
        if (isActive && isHittable)
        {
            isActive = false;
            transform.position = Vector3.one * 1000f;
            rend.enabled = false;
            GameController.Instance.score += 100;
            GameController.Instance.notes.FreeInstance(this.gameObject);
        }
    }
}
