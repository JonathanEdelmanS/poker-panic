using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardObject;
    // public float xMin;
    // public float xMax;
    // public float yMin;
    // public float yMax;
    List<int> cardPositions = new List<int>();

    // Spawn a random card at a random location within bounds (and remove card from deck)
    public void SpawnCard(string cardName)
    {
        //Vector3 position = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -1.5f);
        float[] xpos = new float[19] {-4.2f,-1.0f,3.4f,7.8f,11.0f,.8f, 6.0f,-3.0f,-1.7f,3.4f,8.5f,9.9f, 0.6f,6.2f,-4.4f,11.2f,-2.0f,3.4f,8.8f};
        float[] ypos = new float[19] { 6.0f, 6.0f,6.8f,6.0f,6.0f, 4.8f,4.8f, 3.6f, 3.6f,3.6f,3.6f,3.6f, 2.0f,2.0f, 1.6f,1.6f, 0.8f,0.8f,0.8f};
        int index = Random.Range(0,19);
        while (cardPositions.Contains(index)) index = Random.Range(0, 19);
        cardPositions.Add(index);
        Vector3 position = new Vector3(xpos[index], ypos[index], -1.5f);
        GameObject card = Instantiate(cardObject, position, Quaternion.identity);
        Card cardProps = card.GetComponent<Card>();
        cardProps.SetTexture(cardName);
        cardProps.index = index;
    }

    public void RemoveCard(int index) {
        cardPositions.Remove(index);
    }
}
