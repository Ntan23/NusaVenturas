using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public string foodOrigin;
    public int foodID;
    public int foodScore;
    public Sprite foodSprite;
    [TextArea(3,10)]
    public string foodIngredients;
}
