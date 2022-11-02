using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityExtensions;




public class CrowdManager : MonoBehaviour
{
    
    public static CrowdManager Instance;
    public static Action<List<Person>> OnRefreshCrowd;
    public static Action<int, Person, List<Person>> OnPopulationUpdate;


    private void Awake()
    {
        Instance = this;
    }
    
    public List<Person> people;
    public List<Vector3> posList = new();

    public int crowdQuota = 18, ringPopulationLimit = 20;
    public float angle = 20, radius = 0.5f, radiusIncrement = 1.5f, angleReduction = 1.5f;
    [ReadOnly] public float startAngle, startRadius, constantAngle;
    [ReadOnly]public int rest = 0, limitAngle;
    
    
    //// ------------  GENERATE POSITIONS INSIDE THE CIRCLE  ----------------------------------------------------------------

    public void SetValues()
    {
        SetCrowdQuota();
        startRadius = radius;
        startAngle = angle;
        constantAngle = angle;
        limitAngle=Mathf.RoundToInt(constantAngle/2)+1; //!!!
    }

    void SetCrowdQuota()                          //Crowd stars as  divisible by 3
    {
        int crowdRest = crowdQuota % 3;
        crowdQuota -= crowdRest;
    }

    Vector3 Pos(int i)
    {
        Vector3 pos = new Vector3(Mathf.Cos((angle * i + 90) *  Mathf.Deg2Rad) * radius, 0, Mathf.Sin((angle * i + 90) * Mathf.Deg2Rad) * radius);
        return pos;
    }
    
    public void GeneratePos(int amount)         //Generates positions inside the circle for new characters
    {
        for (int i = 0; i < amount; i++)
        {
            if(RingCompleted(i)) break;         //checks whether current ring is completed-360
            posList.Add(Pos(i + 1));      
        }
    }

    bool RingCompleted(int index)
    {
        if(angle * (index+1) > 360)  
        {
            //Next ring's positions will be generated with recursion
            
            radius+=radiusIncrement;            //expands new ring
            angle /= angleReduction;            //tightens the interval between characters
            rest += index;                      //returns the position index of the first character of the new ring
            int newIndex = crowdQuota - rest; 
            GeneratePos(newIndex);              //function calls itself for the upcoming rings
            return true;
        }
        return false;
    }
    
    
    public void RefreshCrowd()                         //Matches generated positions with characters
    {
        for (int j = 0; j < people.Count; j++)  
        {                                       
            people[j].transform.DOLocalMove(posList[posList.Count - 1 - j], 0.5f);      
        }

        OnRefreshCrowd?.Invoke(people);
        startAngle = constantAngle;
    }


    // ------------  UPDATE CROWD POPULATION ----------------------------------------------------------------
    

    void ResetSettings()                         //Resets increased radius and angles
    {
        posList.Clear();
        rest = 0;
        radius = startRadius;
        angle = startAngle;
    }

    public void UpdateCrowdQuota(int amount)
    {
        crowdQuota = people.Count + amount;
    }
    public void UpdatePopulation(Person person, int amount) //Action<Person> increaseOrDecrease 
    {
        ResetSettings();
        GeneratePos(crowdQuota+=amount); 
        MaintainCircle();
        OnPopulationUpdate?.Invoke(amount, person, people);
        //increaseOrDecrease(person);
    }
    
   
    
    // ------------  MAINTAIN CIRCLE SHAPE REGARDLESS OF PEOPLE AMOUNT ----------------------------------------------------------------
    
    
    bool IsCircle()    //!!!                                    //Returns the required people amount to form a circle according to the start angle
    {
        int requiredAmount = Mathf.RoundToInt(360 / angle);    //Latest angle value (of the latest ring)
        int index = posList.Count - rest;                       // Latest rest that passes 360 - helps to find the latest ring population
        if (index != requiredAmount)
            return false;
        
        return true;
       
    }

    public void MaintainCircle()                           
        //Helps to maintain the circle shape even with each new character added to the crowd - prevent the crowd to become spiral with each added char.
        //While increasing the population, calculates character amount in each ring in order to prevent narrowing of the circle   
    {
        while (!IsCircle())//(RequiredCharAmount() > 0)                
        {
            startAngle -= 0.1f;                         //Finds the proper start angle for the current population to create a circle shape
            ResetSettings();
            GeneratePos(crowdQuota); 
            
            if (startAngle < limitAngle)
            {
                crowdQuota++;                           //Prevents narrowing while maintaining the circle shape
                startAngle = constantAngle;
            }
        }
        
        CheckCircleExpansion();
        
    }

    void CheckCircleExpansion()
    {
        if (crowdQuota <= ringPopulationLimit || !(limitAngle < constantAngle)) return;
        limitAngle += 5;
        ringPopulationLimit += 5;
    }
    
    
    
    

    
   

}
