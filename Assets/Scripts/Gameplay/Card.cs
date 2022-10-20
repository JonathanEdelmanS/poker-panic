using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PokerManager pokerManager;
    public AudioClip tokenCollectAudio;
    public float lifetime;
    float expiration = Mathf.Infinity;
    public string card;

    public void SetTexture(string cardName)
    {
        spriteRenderer.sprite = Resources.Load<Sprite>(cardName);
        card = cardName;
        expiration = Time.time + lifetime;
    }

    // called when player "collects" this
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (player.name != "Player 1" && player.name != "Player 2") return;
        bool collected = pokerManager.TryGivePlayer(player.name, card);
        if (collected) Collect();
        else pokerManager.SetTouching(player.name, this);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var player = other.gameObject;
        if (player.name != "Player 1" && player.name != "Player 2") return;
        pokerManager.StopTouching(player.name, this);
    }

    public void Collect()
    {
        AudioSource.PlayClipAtPoint(tokenCollectAudio, transform.position);
        pokerManager.UnloadCard();
        Destroy(gameObject);
    }

    void Update()
    {
        if (spriteRenderer == null) return;
        if (Time.time > expiration)
        {
            pokerManager.UnloadCard();
            pokerManager.SpawnCard();
            Destroy(gameObject);
        }
    }
}
