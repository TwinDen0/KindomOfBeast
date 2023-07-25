using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibration : MonoBehaviour
{

    public static Vibration instance;

    private bool isVibration = false;

    void Update()
    {
        if (isVibration == true) Handheld.Vibrate();
    }

    public void PlayVibration(float time)
    {
        StartCoroutine(VibrationTime(time));
    }

    public void PlayVibration(bool isPressed)
    {
        isVibration = isPressed;
    }

    public void PlayVibration()
    {
        Handheld.Vibrate();
    }

    private IEnumerator VibrationTime(float time)
    {
        isVibration = true;
        yield return new WaitForSecondsRealtime(time);
        isVibration = false;
    }
}
