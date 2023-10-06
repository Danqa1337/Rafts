using UnityEngine;

public class ChangeHealthOnTargetExecutable : ExecutableWithTarget
{
    public int change;

    public override void Execute(GameObject target)
    {
        var link = target.GetComponent<EntityLink>();
        if (link != null && link.entity.HasBuffer<HealthChangedElement>())
        {
            link.entity.AddBufferElement(new HealthChangedElement(change));
            link.entity.SetZeroSizedTagComponentData(new HealthChangedTag());
        }
        base.Execute(target);
    }
}
