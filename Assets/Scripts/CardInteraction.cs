using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class CardInteraction : MonoBehaviour
{

    private bool isHovered = false, playerMove= false;
    public float floatSpeed; // Speed of floating
    public float floatHeight; // How high the sprite will float

    public Vector3 originalPosition;
    private Vector3 tablePosition;
    private GameObject table;
    public GameObject GameManager;
    private string cardColor;
    private CardSelector cardSelector;
    private string cardNumber;
    private GameObject UICanvas;
    private int playerCardNumber;
    private int enemyCardNumber;

    void Start()
    {
        originalPosition = transform.position;
        table = GameObject.FindWithTag("Table");
        tablePosition = table.transform.position;
        GameManager = GameObject.FindWithTag("GameManager");
        string[] extractCard = gameObject.name.Split('_');
        cardColor = extractCard[0];
        cardNumber = extractCard[1];
        cardSelector = GameManager.GetComponent<CardSelector>();
        UICanvas = GameObject.FindGameObjectWithTag("UI");
    }

    void Update()
    {
        playerMove = cardSelector.isPlayerTurn;
        if (isHovered && playerMove)
        {
            // Make the sprite float up and down
            float newY = originalPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
            transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
        }
    }

    /// <summary>
    /// Called when the mouse enters the GUIElement or Collider.
    /// </summary>
    void OnMouseEnter()
    {
        if(transform.parent.name != "CardTable"){
            isHovered = true;
        }
        
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        isHovered = false;
        // Reset the position to original when not hovered
        if(playerMove == true && transform.parent.name != "CardTable") {
            transform.position = originalPosition;
        }   
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        //Try converting cards into integers so we can compare if player is using card with higher number than previously played card
        try{
            playerCardNumber = int.Parse(cardNumber);
            enemyCardNumber = int.Parse(cardSelector.lastCardNumber);
        } catch(FormatException ex) {
            Debug.Log(ex.ToString());
        }
        if(playerMove && transform.parent.name != "CardTable") {
            //check for picker card
            if((cardNumber == "picker" && (cardColor == cardSelector.currentColor || cardNumber == cardSelector.lastCardNumber)) || (cardNumber == "picker" && cardSelector.isFirstMove == true)){
                ChooseCard();
                cardSelector.SelectRandomCards(2);
                cardSelector.InstantiateCards(cardSelector.aiHand, false);
            }
            else if(cardColor == "wild") {
                UICanvas.GetComponent<UIController>().colorButtonContainer.SetActive(true);
                ChooseCard();
            }
            else if(cardSelector.isFirstMove == true || (cardColor == cardSelector.currentColor && playerCardNumber > enemyCardNumber) || cardNumber == cardSelector.lastCardNumber) {
                ChooseCard();
            }
        }
    }

    private void ChooseCard()
    {
        //transform.position = tablePosition + new Vector3(2, 3, -1);
        cardSelector.PlaceCard(gameObject.name, cardSelector.playerHand);
        cardSelector.isPlayerTurn = false;
        foreach (Transform child in table.transform)
        {
            Destroy(child.gameObject);
        }
        gameObject.transform.SetParent(table.transform);
        gameObject.transform.position = table.transform.position;
        cardSelector.RepositionCards();
    }
}
