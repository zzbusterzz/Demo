using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxBackground : MonoBehaviour
{
    public MoveCamera parallaxCamera;
    public List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    void Start()
    {
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;
    }

    void Move(float delta)
    {
        
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}