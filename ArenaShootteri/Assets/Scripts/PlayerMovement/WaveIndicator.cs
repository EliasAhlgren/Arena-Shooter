using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveIndicator : MonoBehaviour
{
    public Image waveImage;
    public Sprite[] spriteArray;

    public void UpdateWaveIndicator(int wave)
    {
        if (wave > 99)
        {
            waveImage.sprite = spriteArray[99];
        }
        else
        {
            waveImage.sprite = spriteArray[wave - 1];
        }

        waveImage.color = Color.white;
    }

    public void WaveEnd()
    {
        waveImage.color = Color.gray;
    }
}
