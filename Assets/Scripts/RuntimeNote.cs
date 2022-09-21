using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RuntimeNote : MonoBehaviour
{
    public bool isActive = false;
    public float hitDistance = 0.5f;
    public bool isHittable { get; private set; }
    private bool color = false;
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
        this.color = color;
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
            Vector3 position = transform.position;
            position += Vector3.left * (GameController.Instance.notespeed * Time.deltaTime);
            isHittable = position.x < -7 + hitDistance && position.x > -7 - hitDistance; //Bad hardcode!
            transform.position = position;
            
            //Check if the note is off screen
            if (transform.position.x < -11) //Another bad hardcode
            {
                isActive = false;
                rend.enabled = false;
                GameController.Instance.notes.FreeInstance(gameObject);
            }
        }
    }

    public void KeyPress(bool color) //Would normally change it up to program it the other way, so the input checks the note but for now I dont have to fix it.
    {
        if (isActive && isHittable && this.color == color)
        {
            isActive = false;
            transform.position = Vector3.one * 1000f;
            rend.enabled = false;
            GameController.Instance.score += 100;
            GameController.Instance.notes.FreeInstance(gameObject);
        }
    }
}
