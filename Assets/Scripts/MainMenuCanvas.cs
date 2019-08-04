using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : MonoBehaviour
{
    public Canvas controlCanvas, creditCanvas;
   
    public void ShowControls()
    {
        controlCanvas.enabled = true;
    }

    public void HideControls()
    {
        controlCanvas.enabled = false;
    }

    public void ShowCredits()
    {
        creditCanvas.enabled = true;
    }

    public void HideCredits()
    {
        creditCanvas.enabled = false;
    }
}
