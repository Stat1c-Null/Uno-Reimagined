using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorChanger : MonoBehaviour, IPointerClickHandler
{
    public string color;
    private GameObject GameManager;
    private CardSelector cardSelector;
    private GameObject UICanvas;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.FindWithTag("GameManager");
        cardSelector = GameManager.GetComponent<CardSelector>();
        UICanvas = GameObject.FindGameObjectWithTag("UI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("UI Image clicked: " + gameObject.name);
        switch(gameObject.name) {
            case "GreenButton":
                cardSelector.currentColor = "green";
                break;
            case "RedButton":
                cardSelector.currentColor = "red";
                break;
            case "YellowButton":
                cardSelector.currentColor = "yellow";
                break;
            case "BlueButton":
                cardSelector.currentColor = "blue";
                break;
        }
        UICanvas.GetComponent<UIController>().colorButtonContainer.SetActive(false);
    }
}
