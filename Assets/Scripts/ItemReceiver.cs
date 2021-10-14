using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    public void AddPoints(int points)
    {
        GameController.instance.currentItemObject.HaveAddedToScore = true;
        GameController.instance.AddedScore = points;
    }
    //Destroy item object when stays on trigger
}
