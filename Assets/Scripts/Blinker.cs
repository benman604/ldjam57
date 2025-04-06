using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    
    public IEnumerator blinkFor(float blinkFor, float timeVisible = 0.1f, float timeInvisible = 0.1f) {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.enabled = true;
       
        var whenAreWeDone = Time.time + blinkFor;

        while (Time.time < whenAreWeDone)
        {
            Debug.Log("Blinking...");
            renderer.enabled = false;
            yield return new WaitForSeconds(timeInvisible);
            renderer.enabled = true;
            yield return new WaitForSeconds(timeVisible);
        }

        renderer.enabled = true;
    }
}
