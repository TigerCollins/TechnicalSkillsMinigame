using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    Renderer renderer;

    [Space(10)]

    [SerializeField]
    int scoreValue = 50;
    [SerializeField]
    bool addedToScore;
    [SerializeField]
    bool currentlyHeld;

    protected internal int Score
    {
        //The get is setup so components outside of this scripts can access the score but not set its value
        get
        {
            return scoreValue;
        }
    }

    protected internal void SetMaterial(Material mat)
    {
        renderer.material = mat;
    }

    public void OnTriggerStay(Collider other)
    {
        //If the trigger has the ItemReceiver component on the object, a local variable is made that can be used within the if statement. 
        //If statement only run if this item has yet to be added to score, not currently held and the the receiver variable can be passed through
        if(other.TryGetComponent(out ItemReceiver receiver) && !addedToScore && !currentlyHeld)
        {
            addedToScore = true;
            receiver.AddPoints(scoreValue);
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
}
