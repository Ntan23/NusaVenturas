using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute]
public class IngredientSO : ScriptableObject
{
    public string ingredientName;
    public int ingredientID;
    public GameObject ingredientSprite;
    public GameObject spawnedIngredient;
}
