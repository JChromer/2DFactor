using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManger : MonoBehaviour
{
    [SerializeField] Block blockPrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void transformBlock()
    {
        if (blockPrefab != null)
        {
            

            blockPrefab.transform.SetParent(transform, false);
        }
    }
}
