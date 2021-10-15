using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{
    [SerializeField]
    Renderer renderer;
    [SerializeField]
    Renderer glowRenderer;
    [SerializeField]
    GameObject glowObject;

    [Space(10)]

    [SerializeField]
    int scoreValue = 50;
    [SerializeField]
    bool addedToScore;
    [SerializeField]
    bool currentlyHeld;

    ItemReceiver itemReceiver;

    private void Start()
    {
        //Disables glow on start
        ItemGlow(false);
    }

    protected internal int Score
    {
        //The get is setup so components outside of this scripts can access the score but not set its value
        get
        {
            return scoreValue;
        }
    }

    protected internal void SetMaterial(Material mainMat, Material glowMat)
    {
        renderer.material = mainMat;
        glowRenderer.material = glowMat;
    }

    public void OnTriggerStay(Collider other)
    {
        //If the trigger has the ItemReceiver component on the object, a local variable is made that can be used within the if statement. 
        //If statement only run if this item has yet to be added to score, not currently held and the the receiver variable can be passed through
        if(other.TryGetComponent(out ItemReceiver receiver))
        {
            //Sets item receiver variable
            itemReceiver = receiver;

            //If the item has been dropped on the wagon and not scored
            if(!addedToScore && !currentlyHeld)
            {
                addedToScore = true;
                receiver.AddPoints(scoreValue);
                receiver.WagonGlow(false);
            }

            //If the item is being held over the wagon
            else
            {
                receiver.WagonGlow(true);
            }
        } 
    }

    public void OnTriggerExit(Collider other)
    {
        //Disables wagon glow if the held item leaves the wagon - Doesnt check if held by player
        if(itemReceiver != null)
        {
            itemReceiver.WagonGlow(false);
            itemReceiver = null;
        }
    }

    internal bool HaveAddedToScore
    {
        set
        {
            //Sets addedToScore bool, but if value is true, the object is destroyed
            addedToScore = value;
            if(value == true)
            {
                Destroy(gameObject);
            }
        }
    }

    internal bool CurrentlyHeld
    {
        set
        {
            currentlyHeld = value;
        }
    }

    public void ItemGlow(bool glowState)
    {
        glowObject.SetActive(glowState);
    }


    /// <summary>
    /// Following three functions are unity functions. They're called when the mouse
    /// completes one of the three states (enter,exit or pressed down). In this instance,
    /// the functions enable/disable the glow object behind the item. Functio
    /// </summary>
    public void OnMouseEnter()
    {
        if(!currentlyHeld && !GameController.instance.IsGameComplete)
        {
            ItemGlow(true);
        }
    }

    public void OnMouseExit()
    {
        if(!GameController.instance.IsGameComplete)
        {
            ItemGlow(false);
        }
    }

    public void OnMouseDown()
    {
        if(!GameController.instance.IsGameComplete)
        {
            ItemGlow(false);
        }
    }

    public void OnMouseUp()
    {
        if(!GameController.instance.IsGameComplete)
        {
            ItemGlow(true);
        }
    }
}
