using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private Transform cachedTransform;
    private Vector3 offset;
    // Use this for initialization
    public void Start()
    {
        cachedTransform = GetComponent<Transform>();
        offset = cachedTransform.position - player.transform.position;
    }
    
    public void LateUpdate()
    {
        cachedTransform.position = player.transform.position + offset;
    }
}
