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

    public bool isPlayerTurn, isAiTurn;

    public string lastPlacedCard;

    public string currentColor;

    public bool isFirstMove;
    public string lastCardNumber;

    private Vector3 deckStartPosition;
    //Basically GameManager script
    void Start()
    {
        //Select Player Hand
        SelectRandomCards(7);

        InstantiateCards(playerHand, true);
        //Select AI Hand
        SelectRandomCards(7);

        InstantiateCards(aiHand, false);


        ogPosition = cardHolder.transform.position;

        isPlayerTurn = true;
        isAiTurn = false;
        currentColor = string.Empty;
        isFirstMove = true;

        string firstCardName = playerHand[0].name;
        Transform firstCard = cardHolder.transform.Find(firstCardName);
        deckStartPosition = firstCard.position;
    }

    public void SelectRandomCards(int count)
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

    public GameObject InstantiateCard(Texture2D sprite, string spriteName)//TODO : Fix this function
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

        return instance;
    }

    public void InstantiateCards(List<Texture2D> cardList, bool showCardsOnScreen)

    {
        foreach (Texture2D sprite in selectedCards)
        {
            if(showCardsOnScreen) {
                InstantiateCard(sprite, sprite.name);
            }
            
            cardList.Add(sprite);
        }
    }

    public void DrawCardFromDeck(List<Texture2D> hand, bool showCardOnScreen) {
        SelectRandomCards(1);

        Texture2D sprite = selectedCards[0];

        if(showCardOnScreen) {
            InstantiateCard(sprite, sprite.name);
        }
        
        hand.Add(sprite);

        //RepositionCards();
    }

    //Burn random 5 cards
    public void BurnCards (List<Texture2D> hand, bool isPlayer) {
        for(int i = 0; i < 4;i++) {
            int steve = Random.Range(0, hand.Count);
            Transform cardToDelete = cardHolder.transform.Find(hand[steve].name);
            Destroy(cardToDelete.gameObject);

            hand.RemoveAt(steve);
        }
        if(isPlayer) {
            RepositionCards();
        }
        
    }

    public void RepositionCards()
    {
        Debug.Log(playerHand);
        //Place first card on fixed position
        string firstCardName = playerHand[0].name;
        Transform firstCard = cardHolder.transform.Find(firstCardName);
        firstCard.position = deckStartPosition;
        firstCard.GetComponent<CardInteraction>().originalPosition = deckStartPosition;

        for (int i = 1; i < playerHand.Count; i++)
        {
            Transform card = cardHolder.transform.Find(playerHand[i].name);
            Vector3 newPosition = new Vector3(firstCard.position.x + i * spacing, card.position.y, card.position.z);
            card.GetComponent<CardInteraction>().originalPosition = newPosition;
            card.position = newPosition;
        }

        cardHolder.transform.position = ogPosition;
        Transform lastCard = cardHolder.transform.GetChild(cardHolder.transform.childCount - 1);
        Debug.Log(ogPosition);
        xPos = lastCard.position.x;
    }

    public void PlaceCard(string cardName, List<Texture2D> cardList) {
        lastPlacedCard = cardName;
        for(int i = 0;  i < cardList.Count; i++) {
            if(cardList[i].name.Equals(cardName)){
                cardList.RemoveAt(i);
                break;
            }
        }
        string[] extractCard = cardName.Split('_');
        string cardColor = extractCard[0];
        currentColor = cardColor;
        lastCardNumber = extractCard[1];
        isFirstMove = false;
    }
}