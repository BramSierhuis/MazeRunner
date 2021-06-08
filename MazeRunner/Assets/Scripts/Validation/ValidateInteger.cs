using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script to validate integer values from widht and height inputs
public class ValidateInteger : MonoBehaviour
{
    [Header("Values")]
    [SerializeField]
    private int minValue = 3;
    [SerializeField]
    private int maxValue = 30;

    [Header("References")]
    [SerializeField]
    private Text errorText;
    [SerializeField]
    private GameObject errorWindow;

    public void Validate()
    {
        //The text of the input field
        string text = gameObject.GetComponent<TMP_InputField>().text;
        if (text == "")
        {
            errorWindow.SetActive(true);
            errorText.text = "Enter a value please";

            //This disables the play button
            GameManager.Instance.SetInputError(true);
        }
        //No need to try if parse is possible, TMP's option to only allow ints is enabled
        else if(int.Parse(text) < minValue)
        {
            errorWindow.SetActive(true);
            errorText.text = "Enter a value bigger then 3";
            GameManager.Instance.SetInputError(true);
        }
        else if(int.Parse(text) > maxValue)
        {
            errorWindow.SetActive(true);
            errorText.text = "Enter a value no smaller then 30";
            GameManager.Instance.SetInputError(true);
        }
        else
        {
            //Hide error, enable play button
            errorWindow.SetActive(false);
            GameManager.Instance.SetInputError(false);
        }
    }
}
