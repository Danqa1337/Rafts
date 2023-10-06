using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
#endif
using UnityEngine.AI;

[ExecuteInEditMode]

public class RockRandomizer : MonoBehaviour
{

    public List<GameObject> sailors;
    public List<GameObject> miscObjects;
    public List<Sprite> sprites;
    public float sailorsOccurence;
    public float objectsOccurence;
    public bool adjust;
    private Vector2 lastSize;
    private bool changedLastFrame => lastSize != GetComponent<SpriteRenderer>().size || transform.hasChanged;
    public float timer;
#if UNITY_EDITOR
    public async void Update()
    {
        if (Application.isPlaying || PrefabStageUtility.GetCurrentPrefabStage() != null) return;

        timer--;

        if (changedLastFrame)
        {
            timer = 10;
            lastSize = GetComponent<SpriteRenderer>().size;
            transform.hasChanged = false;
            Clear();
            if (adjust) Adjust();

        }
        if (timer == 1)
        {
            if (adjust) Adjust();
            Randomize();
        }

    }
#endif

    void Adjust()
    {
        var top = gameObject;
        var topRenderer = top.GetComponent<SpriteRenderer>();
        var navMeshObstacle = GetComponentInChildren<NavMeshObstacle>();
        var collider = GetComponent<BoxCollider2D>();
        var sideRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        var bounds = transform.GetChild(1).GetComponent<BoxCollider2D>();

        top.GetComponent<SpriteRenderer>().sprite = sprites.RandomItem();

        sideRenderer.size = new Vector2(topRenderer.size.x, sideRenderer.size.y);

        navMeshObstacle.size = topRenderer.size + new Vector2(0, sideRenderer.size.y - 1);
        navMeshObstacle.center = new Vector2(0, topRenderer.size.y * 0.5f - sideRenderer.size.y * 0.5f + 0.5f);

        collider.size = navMeshObstacle.size;
        collider.offset = navMeshObstacle.center;

        bounds.size = topRenderer.size;
        bounds.offset = new Vector2(0, topRenderer.size.y * 0.5f);

    }
    void Clear()
    {
        var children = transform.GetChildren();
        for (int i = 2; i < children.Length; i++)
        {
            DestroyImmediate(children[i]);
        }
    }
    public async void Randomize()
    {
        timer = 0;
        Clear();
        var top = gameObject;
        var topRenderer = top.GetComponent<SpriteRenderer>();
        var sailorsCount = Random.Range(0, sailorsOccurence) * ((topRenderer.size.x * topRenderer.size.y) / 32f);
        var objectsCount = Random.Range(0, objectsOccurence) * ((topRenderer.size.x * topRenderer.size.y) / 32f);
        if (sailors.Count > 0)
        {
            for (int i = 0; i < sailorsCount; i++)
            {
                var obj = Instantiate(sailors.RandomItem(), top.transform);
                obj.transform.localPosition = RandomPosOnTop();
            }
        }

        if (miscObjects.Count > 0)
        {
            for (int i = 0; i < objectsCount; i++)
            {
                var obj = Instantiate(miscObjects.RandomItem(), top.transform);
                obj.transform.localPosition = RandomPosOnTop();
                obj.GetComponent<SpriteRenderer>().sortingOrder = -(int)(transform.position.y * 100);

            }
        }

        Vector2 RandomPosOnTop()
        {
            var sizeX = topRenderer.size.x / 2 - 0.5f;
            var sizeY = topRenderer.size.y / 2 - 0.5f;
            return new Vector3(UnityEngine.Random.Range(-sizeX, sizeX), topRenderer.size.y / 2 + UnityEngine.Random.Range(-sizeY, sizeY));
        }
    }

}
