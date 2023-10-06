using System.Collections.Generic;
using UnityEngine;
public class SailorProperties : MonoBehaviour
{
    [SerializeField] public SailorAIComponent sailorAIComponent;
    [SerializeField] public HealthComponent healthComponent;
    public List<Ability> abilities;

}
