using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public Texture2D CurrentCursor;
    public Texture2D CursorClicked;


    private void Awake()
    {
        ChangeCursor(CurrentCursor);
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ChangeCursor(Texture2D newCursor)
    {
        Cursor.SetCursor(newCursor, Vector2.zero,CursorMode.Auto);
    }
}
