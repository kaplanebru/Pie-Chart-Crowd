using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGenerator : MonoBehaviour
{
    public Person personPb;

    private void Start()
    {
        CrowdManager.Instance.SetValues();
        CreateCrowd();
    }
    
    Person CreateChar(Vector3 pos)
    {
        Person person = Instantiate(personPb, pos, Quaternion.identity, transform);
        return person;
    }
    
    void CreateCrowd()                   
    {
        
        CrowdManager.Instance.GeneratePos(CrowdManager.Instance.crowdQuota);
        for (int j = 0; j < CrowdManager.Instance.crowdQuota; j++) 
        {
            CrowdManager.Instance.people.Add(CreateChar(Vector3.zero));
        }

        CrowdManager.Instance.MaintainCircle();                                  
        CrowdManager.Instance.RefreshCrowd();

    }
}
