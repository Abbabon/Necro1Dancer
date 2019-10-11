using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    public IEnumerator shake()
    {
        EZCameraShake.CameraShakeInstance shakeInstance = EZCameraShake.CameraShaker.Instance.StartShake(2, 2, 0.1f);

        yield return new WaitForSeconds(0.15f);

        shakeInstance.StartFadeOut(0.1f);

        yield return null;
    }
}
