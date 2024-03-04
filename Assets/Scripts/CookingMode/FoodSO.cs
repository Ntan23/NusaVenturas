using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public int foodID;
    public Sprite foodSprite;
    [TextArea(3,10)]
    public string foodIngredients;
}
