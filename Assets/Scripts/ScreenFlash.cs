using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlash : MonoBehaviour
{
    public int numFlashes = 2;
    public float timeBetweenFlash = 0.1f;


    // public IEnumerator FlashInput()
    //{
    //    // save the InputField.textComponent color
    //    [SerializeField] private Canvas canvas;

    //    Color defaultColor = input.textComponent.color;

    //    for (int i = 0; i < numFlashes; i++)
    //    {
    //        // if the current color is the default color - change it to the flash color
    //        if (input.textComponent.color == defaultColor)
    //        {
    //            input.textComponent.color = flashColor;
    //        }
    //        else // otherwise change it back to the default color
    //        {
    //            input.textComponent.color = defaultColor;
    //        }
    //        yield return new WaitForSeconds(timeBetweenFlash);
    //    }
    //    Destroy(input.gameObject, 1); // magic door closes - remove object
    //    yield return new WaitForSeconds(1);
    //}
}
