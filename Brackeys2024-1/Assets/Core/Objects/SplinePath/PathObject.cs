using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathObject : MonoBehaviour
{
    [Header("Position")] 
    public Vector2 yOffsetRange;
    [Header("Rotation")]
    public Vector2 rotationSpeedRange;
    private Vector3 rotation;
    [Header("Scale")]
    public Vector2 scaleRange;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 newPos = transform.localPosition;
        newPos.y += Random.Range(yOffsetRange.x, yOffsetRange.y);
        transform.localPosition = newPos;
        
        rotation = new Vector3(Random.Range(rotationSpeedRange.x, rotationSpeedRange.y), Random.Range(rotationSpeedRange.x, rotationSpeedRange.y), Random.Range(rotationSpeedRange.x, rotationSpeedRange.y));
        
        Vector3 scale = transform.localScale + Vector3.one * Random.Range(scaleRange.x, scaleRange.y);
        transform.localScale = scale;
    }

    void FixedUpdate()
    {
        transform.Rotate(rotation * Time.fixedDeltaTime);
    }
}
