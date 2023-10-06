using UnityEngine;

public class CameraFolow : Singleton<CameraFolow>
{
    public int speed;
    public Transform target;

    private void Update()
    {
        if (target != null)
        {
            var displacement = new Vector3(0, -(transform.position.z * Mathf.Tan(transform.rotation.x)), 0);

            transform.position += (target.position + displacement - transform.position).normalized.ToFloat2().ToVector3() * speed * Time.deltaTime;
        }
    }
}
