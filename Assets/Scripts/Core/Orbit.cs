using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Orbit : MonoBehaviour
{
    //since i added the new input system the orbit zooms a bit- for now its good

    public float turnSpeed = 4.0f;
    [SerializeField] Transform player;

    // array for storing if the 3 mouse buttons are dragging
    bool[] isDragActive;

    // for remembering if a button was down in previous frame
    bool[] downInPreviousFrame;

    public Vector3 offset;

    void Start()
    {
        offset = new Vector3(player.position.x-transform.position.x, player.position.y+2f - transform.position.y, player.position.z +2f - transform.position.z);

        isDragActive = new bool[] { false, false, false };
        downInPreviousFrame = new bool[] { false, false, false };
    }

    void LateUpdate()
    {
        CheckDrag();

    }


    public void CheckDrag()
    {
        for (int i = 0; i < isDragActive.Length; i++)
        {
            if (Input.GetMouseButton(i))
            {
                if (downInPreviousFrame[i])
                {
                    if (isDragActive[i])
                    {
                        OnDragging(i);
                    }
                    else
                    {
                        isDragActive[i] = true;
                        OnDraggingStart(i);
                    }
                }
                downInPreviousFrame[i] = true;
            }
            else
            {
                if (isDragActive[i])
                {
                    isDragActive[i] = false;
                    OnDraggingEnd(i);
                }
                downInPreviousFrame[i] = false;
            }
        }
    }
    public virtual void OnDraggingStart(int mouseButton)
    {
        //Debug.Log("MouseButton" + mouseButton + " START Drag");
    }

    public virtual void OnDragging(int mouseButton)
    {
        if (mouseButton == 1)
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
            offset.y = 0f;

            transform.position = player.position + offset;
            transform.LookAt(player.position);
        }
        //Debug.Log("MouseButton" + mouseButton + "DRAGGING");
    }

    public virtual void OnDraggingEnd(int mouseButton)
    {
        //Debug.Log("MouseButton" + mouseButton + " END Drag");
    }
}