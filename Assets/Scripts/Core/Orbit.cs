using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Cinemachine;

namespace RPG.Core
{
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
        [SerializeField] float height = 0f;
        [SerializeField] float distance = 1f;

        //[SerializeField] CinemachineVirtualCamera followCamera;
        //[SerializeField] CinemachineVirtualCamera freeLookCamera;

        void Start()
        {
            offset = new Vector3(0f, height, distance);
            //offset = new Vector3(player.position.x + 2f, player.position.y , player.position.z +2f );

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
            //followCamera.Priority = 99;
            //freeLookCamera.Priority = 100;


            //Debug.Log("MouseButton" + mouseButton + " START Drag");
        }

        public virtual void OnDragging(int mouseButton)
        {
            if (mouseButton == 1)
            {
                offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
                //offset.y = 0f;
                
                transform.position = player.position + offset;
                transform.LookAt(player.position);
            }
            //Debug.Log("MouseButton" + mouseButton + "DRAGGING");
        }

        public virtual void OnDraggingEnd(int mouseButton)
        {
            //followCamera.Priority = 100; freeLookCamera.Priority = 99;

            //Debug.Log("MouseButton" + mouseButton + " END Drag");
        }
    }
}