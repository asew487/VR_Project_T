using System;
using UnityEngine;

public class Anvil : MonoBehaviour
{
    [SerializeField] GameObject pressObject;

    public Action Pressed;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Pressed?.Invoke();
        pressObject.transform.localPosition = new Vector3(0, -0.02f, 0);
    }

    private void OnTriggerExit(Collider other)
    {
        pressObject.transform.localPosition = Vector3.zero;
    }
}
