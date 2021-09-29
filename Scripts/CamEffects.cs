using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class CamEffects : MonoBehaviour
{

    public float intensity;

    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Hidden/NightVisionShader"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && intensity != 0)// Turn OFF
        {
            intensity = 0;
        }else if (Input.GetKeyDown(KeyCode.V) && intensity == 0)// Turn ON
        {
            intensity = 1;
        }
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (intensity == 0)
        {
            Graphics.Blit(source, destination);
            return;
        }

        material.SetFloat("_bwBlend", intensity);
        Graphics.Blit(source, destination, material);
    }
}
