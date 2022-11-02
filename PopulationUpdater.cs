using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityExtensions;

public class PopulationUpdater : MonoBehaviour
{
    public static Action<Person> OnIncreaseCrowd;

    private void OnEnable()
    {
        CrowdManager.OnPopulationUpdate += UpdatePopulation;
    }

    private void OnDisable()
    {
        CrowdManager.OnPopulationUpdate -= UpdatePopulation;    
    }

    void UpdatePopulation(int amount,Person person, List<Person> people)
    {
        CrowdManager.Instance.UpdateCrowdQuota(amount);
        
        if (amount > 0)
            IncreaseCrowd(person, people);
        else
            DecreaseCrowd(people);
        
        CrowdManager.Instance.RefreshCrowd();
    }

    
    void IncreaseCrowd(Person person, List<Person> people)
    {
        people.Add(person);
        OnIncreaseCrowd?.Invoke(person);
    }
    
    void DecreaseCrowd(List<Person> people)
    {
        Person temp = people.First();
        people.Remove(temp);
        Destroy(temp.gameObject);
    }

}
