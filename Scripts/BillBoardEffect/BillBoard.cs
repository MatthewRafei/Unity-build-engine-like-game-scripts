using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private SpriteRenderer ammoSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        ammoSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Other methods lead to sprite rotating around camera weirdly
        // So this appears to be the best way to do this
        transform.LookAt(Billboarding.instance.transform);
        transform.Rotate(0, 180, 0);
    }
}
