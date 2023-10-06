using System.Collections.Generic;
using System.ComponentModel;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityWeld.Binding;
[Binding]
public class MainMenu : MonoBehaviour, INotifyPropertyChanged
{
    [SerializeField] public List<RaftSellectionData> raftList;

    [SerializeField] public CircularList<RaftSellectionData> raftsCircularList;

    public event PropertyChangedEventHandler PropertyChanged;

    private string _description = "description";
    private string _name = "name";

    [Binding]
    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            InvokePropertyChange("Description");
        }
    }
    [Binding]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            InvokePropertyChange("Name");
        }
    }

    private void Awake()
    {
        raftsCircularList = new CircularList<RaftSellectionData>();

        foreach (var item in raftList)
        {
            raftsCircularList.Add(item);
        }
        UpdateRaftInfo(raftList[0]);

    }

    [Binding]
    public void Next()
    {
        UpdateRaftInfo(raftsCircularList.Next());
    }
    [Binding]
    public void Prev()
    {
        UpdateRaftInfo(raftsCircularList.Prev());
    }
    [Binding]
    public void Quit()
    {
        Application.Quit();
    }
    [Binding]
    public void Play()
    {
        PlayerPrefs.SetInt("SelectedRaft", raftsCircularList.Index);
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        em.DestroyEntity(em.UniversalQuery);
        DefaultWorldInitialization.Initialize("Default World", false);

        SceneManager.LoadScene(1);
    }

    public void UpdateRaftInfo(RaftSellectionData raftSellectionData)
    {
        Description = raftSellectionData.description;
        Name = raftSellectionData.name;
        var oldRaft = FindObjectOfType<Raft>();

        CameraFolow.instance.target = raftSellectionData.gameObject.transform;

    }
    private void InvokePropertyChange(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName: propertyName));
    }
}

[System.Serializable]
public class RaftSellectionData
{
    public GameObject gameObject;
    public string name;
    public string description;
}
