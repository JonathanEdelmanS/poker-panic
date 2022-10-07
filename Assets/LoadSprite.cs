using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTexture(string card)
    {
        spriteRenderer.sprite = Resources.Load<Sprite>(card);
    }
}
