using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeAnimation : MonoBehaviour
{
    
    void Start() => LeanTween.moveLocalY(this.gameObject, this.gameObject.transform.position.y + 0.1f, 0.6f).setEaseLinear().setLoopPingPong();
    
}
