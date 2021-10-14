using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReceiver : MonoBehaviour
{
    public void AddPoints(int points)
    {
        GameController.instance.AddedScore = points;
        GameController.instance.currentItemObject.HaveAddedToScore = true;
    }
    //Destroy item object when stays on trigger
}
