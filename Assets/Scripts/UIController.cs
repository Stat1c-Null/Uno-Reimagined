using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject GameManager;
    public int maxCardsPerTurn;
    public bool skipMove;
    private CardSelector cardSelector;
    public GameObject colorButtonContainer;
    Image RedButton;
    Image YellowButton;
    Image GreenButton;
    Image BlueButton;
    // Start is called before the first frame update
    void Start()
    {
        cardSelector = GameManager.GetComponent<CardSelector>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshHand()
    {
        //Delete current hand for player and AI
        cardSelector.playerHand.Clear();
        cardSelector.aiHand.Clear();
        foreach (Transform child in cardSelector.cardHolder.transform)
        {
            Destroy(child.gameObject);
        }
        cardSelector.xPos = 0f;
        //Repopulate hand with new cards
        cardSelector.SelectRandomCards(7);
        cardSelector.InstantiateCards(cardSelector.playerHand, true);
        cardSelector.SelectRandomCards(7);
        cardSelector.InstantiateCards(cardSelector.aiHand, false);

        
    }

    public void DrawCard()
    {
        if (cardSelector.isPlayerTurn && maxCardsPerTurn > 0)
        {
            cardSelector.DrawCardFromDeck(cardSelector.playerHand, true);
            maxCardsPerTurn -= 1;
        }
    }

    public void EndTurn() {
        skipMove = cardSelector.isPlayerTurn;
        if (skipMove == true && maxCardsPerTurn == 0) {
            cardSelector.BurnCards(cardSelector.playerHand, true);
            cardSelector.isPlayerTurn = false;
            cardSelector.isAiTurn = true;
        }else if (skipMove == false) { 
            cardSelector.isAiTurn = true;
            cardSelector.isPlayerTurn = false;
        }
        maxCardsPerTurn = 3;
    }
}
