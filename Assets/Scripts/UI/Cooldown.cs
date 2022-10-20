using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
    public float cooldown = 10f;
    public Text timer;
    float current;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        current = Mathf.Max(current - Time.deltaTime, 0f);
        timer.text = (current != 0f) ? Mathf.Ceil(current).ToString() : "!!!";
    }

    public bool IsReady()
    {
        return current == 0f;
    }

    public void Reset()
    {
        current = cooldown;
    }
}
