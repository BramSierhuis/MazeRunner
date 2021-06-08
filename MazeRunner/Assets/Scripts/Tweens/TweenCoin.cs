using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that can be assigned to coins to move them up and down
public class TweenCoin : MonoBehaviour
{
    void Start()
    {
        LeanTween.moveY(gameObject, transform.position.y - 0.3f, 0.3f).setLoopPingPong().setEaseInOutSine();

    }
}
