using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour
{
    private DataManager dm;

    void Start() => dm = DataManager.instance;

    public void DeleteGameData() => dm.ResetData();
}
