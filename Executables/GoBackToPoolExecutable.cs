using System.Threading;
using UnityEngine;
[RequireComponent(typeof(PoolableItem))]
public class GoBackToPoolExecutable : Executable
{
    public int delayMS;
    public CancellationTokenSource cancellationTokenSource;
    public async override void Execute()
    {
        cancellationTokenSource = new CancellationTokenSource();
        DelayTask(cancellationTokenSource.Token);
    }
    public async void DelayTask(CancellationToken token)
    {
        await System.Threading.Tasks.Task.Delay(delayMS);
        if (!token.IsCancellationRequested)
        {
            Pooler.PutObjectBackToPool(gameObject);
            base.Execute();
        }
    }
    private void OnDisable()
    {
        cancellationTokenSource?.Cancel();
    }
}
