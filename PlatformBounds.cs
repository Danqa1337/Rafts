using UnityEngine;

public class PlatformBounds : MonoBehaviour
{
    public Bounds GetBounds()
    {
        return new Bounds(GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().bounds.size);
    }
}
