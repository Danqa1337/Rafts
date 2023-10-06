using System.Threading.Tasks;
using UnityEngine;
[RequireComponent(typeof(PoolableItem))]
public class PutBackToPoolWithDelay : MonoBehaviour
{
    public int lifeTimeMS;
    private async void OnEnable()
    {
        await Task.Delay(lifeTimeMS);
        ForcePut();
    }
    public void ForcePut()
    {
        Pooler.PutObjectBackToPool(gameObject);
    }
}
