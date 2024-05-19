using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour
{
    private DataManager dm;
    private ShopManager sm;

    void Start() 
    {
        dm = DataManager.instance;
        sm = ShopManager.instance;
    }

    public void DeleteGameData() 
    {
        dm.ResetData();
        sm.ResetShop();
    }
}
