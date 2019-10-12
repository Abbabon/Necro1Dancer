using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFlash : MonoBehaviour
{
    private float alphaMax = 0.4f;
    private float alphaDecrement = 0.02f;
    private float alphaIncrement = 0.1f;
    CanvasGroup flashLayer = GameEngine.Instance.redFlashOfDoom;

    public IEnumerator Flash()
    {
        while (flashLayer.alpha < alphaMax)
        {
            flashLayer.alpha += alphaIncrement;
            yield return new WaitForSeconds(0.01f);
        }
        while (flashLayer.alpha > 0)
        {
            flashLayer.alpha += alphaIncrement;
            yield return new WaitForSeconds(0.01f);
        }
    yield return new WaitForSeconds(1);
    }
}
