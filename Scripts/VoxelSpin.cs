using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelSpin : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed = 2f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.Self);
    }
}