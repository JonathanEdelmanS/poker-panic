using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    public GameObject enemy;
    public PokerManager pokerManager;
    public Collider2D thisCollider;
    public float radius;
    public float expiration;
    float lifetime;
    public Cooldown cooldown;

    public void Activate()
    {
        Vector3 thisPos = transform.position;
        Vector3 enemyPos = enemy.transform.position;
        if ((thisPos - enemyPos).magnitude < radius && cooldown.IsReady())
        {
            pokerManager.Punch(enemy);
            cooldown.Reset();
        }
    }

    void Update()
    {
        if (PlayerPunchable()) {
            Debug.Log('Player is punchable');
        }
    }

    // detect if other player is within radius
    private void PlayerPunchable() {
        Vector3 thisPos = transform.position;
        Vector3 enemyPos = enemy.transform.position;
        return (thisPos - enemyPos).magnitude < radius && cooldown.IsReady()
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    GameObject player = other.gameObject;
    //    if (player.name != "Player 1" && player.name != "Player 2") return;
    //    Debug.Log(player.name + " is here");
    //    if (player == enemy) pokerManager.Punch(player);
    //}
}
