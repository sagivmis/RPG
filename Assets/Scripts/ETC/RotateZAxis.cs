using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZAxis : MonoBehaviour
{
    [Range(0,3)]
    [SerializeField] float rotationMultiplier = 0.3f;

    public float zRotation;

    private void Start()
    {
        zRotation = Random.Range(0f, 3f);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = new Vector3(0f, 0f, zRotation) * rotationMultiplier;
        transform.Rotate(newRotation);
    }
}
