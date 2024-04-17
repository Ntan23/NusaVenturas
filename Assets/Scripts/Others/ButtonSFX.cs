using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    private AudioManager am;

    void Start() => am = AudioManager.instance;
    
    public void PlayClickSFX() => am.Play("Click");

    public void PlayHoverSFX() => am.Play("Hover");
}
