using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZAxis : MonoBehaviour
{
    [Range(0,3)]
    [SerializeField] float rotationMultiplier = 0.3f;
    [SerializeField] float timeToChangeRotatationSpeed = 5f;

    public float zRotation;
    float timer = 0;

    private void Start()
    {
        zRotation = Random.Range(0f, 3f);
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 newRotation = new Vector3(0f, 0f, zRotation) * rotationMultiplier;
        transform.Rotate(newRotation);
        UpdateTimers();

        if (timer >= timeToChangeRotatationSpeed)
        {
            zRotation = Random.Range(0f, 3f);
            timer = 0;
        }
    }

    private void UpdateTimers()
    {
        timer += Time.deltaTime;
    }
}
