using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script for the difficulty buttons, updates their appearance based on 
//which one is selected
public class TweenButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Sprite selected;
    [SerializeField]
    private Sprite unselected;

    //Get called when the button gets clicked
    public void OnClick()
    {
        //Try to shrink all other buttons
        foreach (TweenButton button in transform.parent.GetComponentsInChildren<TweenButton>())
        {
            if (button != this)
                button.Shrink();
        }

        //Change sprite
        GetComponent<Image>().sprite = selected;

        //Tween button
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        LeanTween.scaleX(gameObject, 1.3f, 0.3f).setEaseOutBounce();
    }

    //Gets called when button needs to be shrinked
    public void Shrink()
    {
        //Only shrink when te button is bigger
        if(transform.localScale.x > 1)
        {
            //Change sprite
            GetComponent<Image>().sprite = unselected;

            //Shrink
            LeanTween.cancel(gameObject);
            LeanTween.scaleX(gameObject, 1f, 0.2f).setEaseInSine();
        }
    }
}
