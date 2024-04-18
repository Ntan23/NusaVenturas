using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float targetYLocation;
    private float intialYLocation;

    void Start() => intialYLocation = transform.localPosition.y;

    public void OpenDoor() => LeanTween.moveLocalY(gameObject, targetYLocation, 0.6f);

    public void CloseDoor() => LeanTween.moveLocalY(gameObject, intialYLocation, 0.6f);
}
