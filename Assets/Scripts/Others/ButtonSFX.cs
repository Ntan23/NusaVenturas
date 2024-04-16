using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    private AudioManager am;

    void Start() => am = AudioManager.instance;
    
    public void PlayClickSFX() => am.Play("Click");

    
}
