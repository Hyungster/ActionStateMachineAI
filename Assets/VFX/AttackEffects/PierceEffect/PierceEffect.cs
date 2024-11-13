using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceEffect : MonoBehaviour
{
    Material material;

    public float phase = 1;
    [HideInInspector] public float minPhase = 1f;
    [HideInInspector] public float maxPhase = -0.2f;


    private void Awake()
    {
        material = transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        material.SetFloat("_Phase", phase);
    }
}
