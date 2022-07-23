using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Vector3 testPoint;
    public GameObject testBomb;
    // Start is called before the first frame update
    void Start()
    {
        testPoint = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {                        
            Instantiate(testBomb, transform.position, testBomb.transform.rotation);            
        }
    }
}
