using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivePickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Deactive",Random.Range(3f,6f));
    }

    void Deactive()
    {
        gameObject.SetActive(false);
    }

   }
