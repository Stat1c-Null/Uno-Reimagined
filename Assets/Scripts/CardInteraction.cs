using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInteraction : MonoBehaviour
{

    private bool isHovered = false, placed = false;
    public float floatSpeed; // Speed of floating
    public float floatHeight; // How high the sprite will float

    public Vector3 originalPosition;
    private Vector3 tablePosition;

    void Start()
    {
        originalPosition = transform.position;
        tablePosition = GameObject.FindWithTag("Table").transform.position;
    }

    void Update()
    {
        if (isHovered && !placed)
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
        if(placed == false) {
            transform.position = originalPosition;
        }   
    }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        placed = true;
        transform.position = tablePosition + new Vector3(5, 10, 0);
    }
}
