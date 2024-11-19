using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeSlashEffect : MonoBehaviour
{
    Material material;

    public float phase = 2;
    public float minPhase = 0;
    public float maxPhase = 3;

    public float brightness = 0.5f;
    public float startBrightness = 0.5f;
    public float maxBrightness = 3f;



    public float ringWidth1 = 16f;
    public float ringWidth2 = 6f;

    public float startRingWidth1 = 3;
    public float endRingWidth1 = 16f;

    public float startRingWidth2 = 6f;
    public float endRingWidth2 = 6f;


    public float sharpness = 0;
    public float maxSharpness = 0.25f;

    private void Awake()
    {
        material = transform.GetChild(0).GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        material.SetFloat("_Phase", phase);
        material.SetFloat("_Brightness", brightness);
        material.SetFloat("_Sharpness", sharpness);
        material.SetFloat("_RingWidth1", ringWidth1);
        material.SetFloat("_RingWidth2", ringWidth2);
    }


}
