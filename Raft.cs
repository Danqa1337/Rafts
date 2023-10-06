using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
public class Raft : MonoBehaviour
{
    [HideInInspector] public new Rigidbody2D rb;

    public float pushMultipler;
    public float dropChanceMultipler;

    public int sailorsAlive;
    public List<Entity> sailors = new List<Entity>();

    public delegate void SailorEventHandler(Entity sailor);

    public ParticleSystem WaveEmitter;

    public float maxVelocity;
    public float minVelocity;
    public float Velocity
    {
        get => rb.velocity.magnitude;
    }

    private void OnDestroy()
    {

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

}
