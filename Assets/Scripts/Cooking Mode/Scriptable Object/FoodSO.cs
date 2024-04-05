using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public string foodRecipeID;
    public string foodOrigin;
    public int foodID;
    public int foodPrice;
    public Sprite foodSpriteWithFrame;
    public Sprite foodSpriteWithoutFrame;
    [TextArea(3,10)]
    public string foodIngredients;
    public Sprite foodIngredientsSprite;
    public bool isUnlocked;
}
