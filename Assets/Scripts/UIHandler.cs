using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHandler : MonoBehaviour
{
    internal static UIHandler instance;

    [SerializeField]
    internal EventSystem eventSystem;

    [Header("Text Visualiser Variables")]
    [SerializeField]
    List<ButtonTextVisualiser> buttonTextVisualisers;

    public void Awake()
    {
        //If statement destroys this component if the instance already exists, stops multiple static instances
        if(instance ==null)
        {
            instance = this;
        }

        else
        {
            Destroy(this);
        }
        
    }

    //This function runs through the list of the Button Visualisers and checks if they're selected. If selected they show the pointer.
    internal void ToggleOtherPointerIndicators(ButtonTextVisualiser visualiser)
    {
        foreach (ButtonTextVisualiser item in buttonTextVisualisers)
        {
            if(visualiser == item)
            {
                item.PointerCheck(true);
            }

            else
            {
                item.PointerCheck(false);
            }    
        }
    }
}
