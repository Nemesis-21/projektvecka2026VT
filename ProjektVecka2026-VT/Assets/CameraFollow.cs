//Edgar, 2026-03-03
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform target;
    private float changeY;
    private float changeZ;

    // Update is called once per frame
    void Update()
    {
        if (target.position.y>4f)
        {
            changeY = target.position.y;
        }
        else
        {
            changeY = 4f;
        }


       
       changeZ = Mathf.Max(target.position.z-10f, -10f);

        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, changeY, changeZ), Time.deltaTime * 1f);
       
    }
}
