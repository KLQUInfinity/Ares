using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxMotion : MonoBehaviour
{
    [Header("1 or 0 to toggle axis offset")]
    public float xOffSetSpeed = 1; 
    public float yOffSetSpeed = 1;
    public bool isRandomOffSet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xOffset = Time.time * xOffSetSpeed;
        float yOffset = Time.time * yOffSetSpeed;
        if (isRandomOffSet)
        {
            xOffset = Random.Range(1, 5);
            yOffset= Mathf.Abs(Random.Range(1, 5));
        }
        RenderSettings.skybox.SetFloat("_Rotation", xOffset);

    }
}
