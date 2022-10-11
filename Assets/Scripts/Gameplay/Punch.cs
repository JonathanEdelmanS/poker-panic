using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public GameObject enemy;
    public PokerManager pokerManager;
    public Collider2D thisCollider;
    public float expiration;
    float lifetime;

    public void Activate()
    {
        thisCollider.enabled = true;
        lifetime = expiration;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            thisCollider.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject player = other.gameObject;
        if (player == enemy) pokerManager.Punch(player);
    }
}
