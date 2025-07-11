using Unity.VisualScripting;
using UnityEngine;

public class SpriteBillBoard : MonoBehaviour
{
    private Transform target;
    void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
    }
    // Update is called once per frame
    private void Update()
    {
        
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
