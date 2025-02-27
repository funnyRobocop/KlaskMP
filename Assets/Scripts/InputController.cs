using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    private StrikerMover mover;
    private Vector3 lastMousePos;
    public bool CanMove { get; set; }

    private void Awake()
    {
        mover = GetComponent<StrikerMover>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            CanMove = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            CanMove = false;
        }
    }

    private void FixedUpdate()
    {
        if (!CanMove)
        {
            mover.Stop();
            return;
        }
        
        var movement = Vector3.ClampMagnitude(new Vector3(Input.mousePosition.x - lastMousePos.x, 0f, Input.mousePosition.y - lastMousePos.y), Consts.MaxStrikerSpeed);
            lastMousePos = Input.mousePosition;

        mover.Move(movement);
    }
}
