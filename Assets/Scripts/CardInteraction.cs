using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        isHovered = true;
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        isHovered = false;
        // Reset the position to original when not hovered
        if(playerMove == true) {
            transform.position = originalPosition;
        }   
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        if(playerMove) {
            if(cardNumber == "picker" && (cardColor == cardSelector.currentColor || cardNumber == cardSelector.lastCardNumber)){
                ChooseCard();
                cardSelector.SelectRandomCards(2);
                cardSelector.InstantiateCards(cardSelector.aiHand, false);
            }
            else if(cardSelector.isFirstMove == true || cardColor == cardSelector.currentColor || cardNumber == cardSelector.lastCardNumber) {
                ChooseCard();
            }
        }
    }

    private void ChooseCard()
    {
        transform.position = tablePosition + new Vector3(2, 3, -1);
        cardSelector.PlaceCard(gameObject.name, cardSelector.playerHand);
        cardSelector.isPlayerTurn = false;
        foreach (Transform child in table.transform)
        {
            Destroy(child.gameObject);
        }
        gameObject.transform.SetParent(table.transform);
    }
}
