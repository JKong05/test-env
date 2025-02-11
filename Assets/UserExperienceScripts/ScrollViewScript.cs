using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class ScrollViewScript : MonoBehaviour
{

    public GameObject participantNumPrefab; //Text UI prefab
    public GameObject storyPrefab; // UI prefab
    public Transform participantContentPanel; //Scrollview Content
    public Transform storyContentPanel; //Scrollview Content


    public void PopulateParticipantScrollView()
    {
        for (int i = 1; i <= 4; i++)
        {
            //Create new text UI element and place it in scroll view's content
            GameObject newTextObject = Instantiate(participantNumPrefab, participantContentPanel);

            //Assign text to text UI element
            if (newTextObject.GetComponentInChildren<TextMeshProUGUI>())
                newTextObject.GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
            else
                Debug.LogError("Prefab does not have a TextMeshProUGUI component attached");
        }
        for (int i = 1; i <= 100; i++)
        {
            //Create new text UI element and place it in scroll view's content
            GameObject newTextObject = Instantiate(participantNumPrefab, participantContentPanel);

            //Assign text to text UI element
            if (newTextObject.GetComponentInChildren<TextMeshProUGUI>())
                newTextObject.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            else
                Debug.LogError("Prefab does not have a TextMeshProUGUI component attached");
        }
        for (int i = 1; i <= 5; i++)
        {
            //Create new text UI element and place it in scroll view's content
            GameObject newTextObject = Instantiate(participantNumPrefab, participantContentPanel);

            //Assign text to text UI element
            if (newTextObject.GetComponentInChildren<TextMeshProUGUI>())
                newTextObject.GetComponentInChildren<TextMeshProUGUI>().alpha = 0;
            else
                Debug.LogError("Prefab does not have a TextMeshProUGUI component attached");
        }
    }

}