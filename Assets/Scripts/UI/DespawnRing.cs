using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DespawnRing : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image image;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        image.fillAmount = time / 10f;
    }

    public void SetPosition(Vector3 pos)
    {
        rectTransform.anchoredPosition = pos;
    }
}
