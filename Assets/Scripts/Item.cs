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
}
