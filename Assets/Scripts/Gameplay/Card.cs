using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public PokerManager pokerManager;
    string card;

    public void SetTexture(string cardName)
    {
        spriteRenderer.sprite = Resources.Load<Sprite>(cardName);
        card = cardName;
    }

    // called when player "collects" this
    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject;
        if (player.name != "Player 1" && player.name != "Player 2") return;
        Debug.Log(player.name + " touched " + card);
        bool collected = pokerManager.GivePlayer(player.name, card);
        // Destroy this card only if the player collected it.
        if (collected) Destroy(gameObject);
    }
}
