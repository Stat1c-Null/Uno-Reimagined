using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardSelector : MonoBehaviour
{
    public Texture2D[] allCards;
    private List<Texture2D> selectedCards = new List<Texture2D>();
    public List<Texture2D> playerHand = new List<Texture2D>();

    public GameObject cardPrefab;

    public float xPos, yPos;

    void Start()
    {
        SelectRandomCards(7);

        InstantiateCards();
    }

    void SelectRandomCards(int count)
    {

        selectedCards.Clear();

        if (allCards.Length < count)
        {
            Debug.LogWarning("Not enough cards to select the requested number. Using all available sprites.");
            selectedCards.AddRange(allCards);
            return;
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, allCards.Length);
            selectedCards.Add(allCards[randomIndex]);
            allCards[randomIndex] = allCards[allCards.Length - 1 - i]; // Swap with last element to avoid duplicates
        }
    }

    void InstantiateCard(Texture2D sprite)
    {
        GameObject instance = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
            
        SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sprite = Sprite.Create(sprite, new Rect(0, 0, sprite.width, sprite.height), new Vector2(1f, 1f));
        }
        
        instance.transform.position = new Vector3(xPos, yPos, 0);
        xPos += 4f;
    }

    void InstantiateCards()
    {
        foreach (Texture2D sprite in selectedCards)
        {
            InstantiateCard(sprite);
            playerHand.Add(sprite);
        }
    }

    public void DrawCardFromDeck() {
        SelectRandomCards(1);

        Texture2D sprite = selectedCards[0];

        InstantiateCard(sprite);

        playerHand.Add(sprite);
    }

    //Burn random 5 cards
    public void BurnCards () {
        for(int i = 0; i < playerHand.Count;i++) {
            int steve = Random.Range(0, playerHand.Count);
            playerHand.RemoveAt(steve);
        }
    }

}