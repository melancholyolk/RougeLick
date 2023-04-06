using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetect : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Ray r = new Ray(Vector3.zero, Vector3.right);
            RaycastHit[] hit = Physics.RaycastAll(r, 10);
            if (hit.Length > 0)
            {
                foreach (var VARIABLE in hit)
                {
                    Debug.Log(VARIABLE.point);
                }
            }
        }  
    }
}
