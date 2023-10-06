using System.ComponentModel;
using Unity.Entities;
using UnityEngine.SceneManagement;
using UnityWeld.Binding;

[Binding]
public class ViewModel : Singleton<ViewModel>, INotifyPropertyChanged
{
    private int _sailorsCount;
    private (float, float) _avgCooldown;
    private int _currentXp;
    private int _currentLevel;
    private int _xpToNextLevel;
    private float _levelBonus;
    [Binding]
    public int SailorsCount
    {
        get => _sailorsCount;
        set
        {
            _sailorsCount = value;
            InvokePropertyChange("SailorsCount");
        }
    }
    [Binding]
    public (float, float) AvgCooldown
    {
        get => _avgCooldown;
        set
        {
            _avgCooldown = value;
            InvokePropertyChange("AvgCooldown");
        }
    }

    [Binding]
    public int CurrentXp
    {
        get => _currentXp;
        set
        {
            _currentXp = value;
            InvokePropertyChange("CurrentXp");
            InvokePropertyChange("Xp");

        }
    }

    [Binding]
    public int XpToNextLevel
    {
        get => _xpToNextLevel;
        set
        {
            _xpToNextLevel = value;
            InvokePropertyChange("XpToNextLevel");
            InvokePropertyChange("Xp");

        }
    }
    [Binding]
    public int CurrentLevel
    {
        get => _currentLevel;
        set
        {
            _currentLevel = value;
            InvokePropertyChange("CurrentLevel");

        }
    }
    [Binding]
    public float StatsBonus
    {
        get => _levelBonus;
        set
        {
            _levelBonus = value;
            InvokePropertyChange("StatsBonus");

        }
    }
    [Binding]
    public (int, int) Xp
    {
        get => (_currentXp, _xpToNextLevel);
        set
        {

        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void InvokePropertyChange(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName: propertyName));
    }
    [Binding]
    public void Restart()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        em.DestroyEntity(em.UniversalQuery);
        DefaultWorldInitialization.Initialize("Default World", false);

        SceneManager.LoadScene(1);
    }
    [Binding]
    public void Quit()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        em.DestroyEntity(em.UniversalQuery);

        SceneManager.LoadScene(0);
    }

}
