using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CardSelector : MonoBehaviour
{
    public Texture2D[] allCards;
    public List<Texture2D> selectedCards = new List<Texture2D>();
    public List<Texture2D> playerHand = new List<Texture2D>();
    public List<Texture2D> aiHand = new List<Texture2D>();
    public GameObject cardPrefab;
    public GameObject cardHolder;
    private GameObject instance;

    private Vector3 ogPosition;

    public float xPos, yPos, spacing;

    public bool isPlayerTurn;
    public bool isAiTurn;

    public string lastPlacedCard;


    void Start()
    {
        //Select Player Hand
        SelectRandomCards(7);

        InstantiateCards(playerHand);
        //Select AI Hand
        SelectRandomCards(7);

        InstantiateCards(aiHand);


        ogPosition = cardHolder.transform.position;

        isPlayerTurn = true;
        isAiTurn = false;
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

    void InstantiateCard(Texture2D sprite, string spriteName)//TODO : Fix this function
    {
        if (cardHolder != null) {
            instance = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity, cardHolder.transform);
            instance.name = spriteName;
        }
        
            
        SpriteRenderer renderer = instance.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            renderer.sprite = Sprite.Create(sprite, new Rect(0, 0, sprite.width, sprite.height), new Vector2(1f, 1f));
        }
        
        instance.transform.position = new Vector3(xPos, yPos, 0);
        xPos += 4f;
    }

    void InstantiateCards(List<Texture2D> cardList)

    {
        foreach (Texture2D sprite in selectedCards)
        {
            InstantiateCard(sprite, sprite.name);
            cardList.Add(sprite);
        }
    }

    public void DrawCardFromDeck() {
        SelectRandomCards(1);

        Texture2D sprite = selectedCards[0];

        InstantiateCard(sprite, sprite.name);

        playerHand.Add(sprite);

        //RepositionCards();
    }

    //Burn random 5 cards
    public void BurnCards () {
        for(int i = 0; i < 4;i++) {
            int steve = Random.Range(0, playerHand.Count);
            
            Transform cardToDelete = cardHolder.transform.Find(playerHand[steve].name);
            Destroy(cardToDelete.gameObject);

            playerHand.RemoveAt(steve);
        }

        RepositionCards();
    }

    public void RepositionCards()
    {
        for (int i = 0; i < playerHand.Count; i++)
        {
            Transform card = cardHolder.transform.Find(playerHand[i].name);
            Vector3 newPosition = new Vector3(i * spacing, card.position.y, card.position.z);
            card.GetComponent<CardInteraction>().originalPosition = newPosition;
            card.position = newPosition;
        }

        cardHolder.transform.position = ogPosition;
    }

    public void PlaceCard(string cardName) {
        lastPlacedCard = cardName;
        for(int i = 0;  i < playerHand.Count; i++) {
            if(playerHand[i].name.Equals(cardName)){
                playerHand.RemoveAt(i);
                break;
            }
        }
        isPlayerTurn = false;
    }
}