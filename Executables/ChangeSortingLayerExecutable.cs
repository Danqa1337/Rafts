using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class ChangeSortingLayerExecutable : Executable
{
    public string layer;
    public int order;
    public async override void Execute()
    {
        if (layer != "") GetComponent<SpriteRenderer>().sortingLayerName = layer;
        GetComponent<SpriteRenderer>().sortingOrder = order;
        base.Execute();
    }
}
