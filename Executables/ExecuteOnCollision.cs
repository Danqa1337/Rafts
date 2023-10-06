using UnityEngine;

public class ExecuteOnCollision : ExecutableWithTarget
{
    public bool doNotCollideWithSlibings;
    public int collisionCooldownMS;
    public Transform excludedRoot;
    private bool ready = true;

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (ready && !doNotCollideWithSlibings || collision.transform.root != transform.root && collision.transform.root != excludedRoot)
        {
            ready = false;
            Execute(collision.gameObject);
            await System.Threading.Tasks.Task.Delay(collisionCooldownMS);
            ready = true;
        }
    }
    private async void OnTriggerEnter2D(Collider2D collision)
    {
        if (ready && !doNotCollideWithSlibings || collision.transform.root != transform.root && collision.transform.root != excludedRoot)
        {
            ready = false;
            Execute(collision.gameObject);
            await System.Threading.Tasks.Task.Delay(collisionCooldownMS);
            ready = true;
        }
    }

}
