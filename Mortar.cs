using Unity.Entities;
using UnityEngine;
public class Mortar : MonoBehaviour
{
    public Transform barrelsEnd;
    public PoolableItem projectilePrefab;

    public async void Shoot(Vector2 to, Entity sender)
    {
        var direction = (to.ToFloat2() - transform.position.ToFloat2()).Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x * transform.localScale.x) * Mathf.Rad2Deg - 90;
        transform.GetChild(0).localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Clamp(angle, -10, 10)));

        for (int i = 0; i < 5; i++)
        {
            var projectile = Pooler.Take(projectilePrefab, barrelsEnd.position).GetComponent<Projectile>();
            var explosion = Pooler.Take("BigExplosion", barrelsEnd.position);
            explosion.transform.rotation = barrelsEnd.rotation;
            projectile.Launch(sender, barrelsEnd.position, to + UnityEngine.Random.insideUnitCircle * 4, transform.parent);
            if (transform.parent.GetComponent<EntityLink>() != null) transform.parent.GetComponent<EntityLink>().entity.AddBufferElement(new PushElement(UnityEngine.Random.insideUnitCircle));
            await System.Threading.Tasks.Task.Delay(300);

        }

    }
}

