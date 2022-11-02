using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorSeparator : MonoBehaviour
{
  
   // ------------  SEPARATE CHARS ACCORDING TO PIE CHART COLORS ----------------------------------------------------------------
   private void OnEnable()
   {
       CrowdManager.OnRefreshCrowd += RefreshColors;
       PopulationUpdater.OnIncreaseCrowd += IncreaseColors;
   }

   private void OnDisable()
   {
       CrowdManager.OnRefreshCrowd -= RefreshColors;
       PopulationUpdater.OnIncreaseCrowd -= IncreaseColors;

   }

   public enum Colors
   {
       Red,
       Green,
       Blue
   };

   private void Start()
   {
       int pieStartAmount = CrowdManager.Instance.crowdQuota / 3;
      _colorPopulation = new () {{Colors.Red, pieStartAmount}, {Colors.Green, pieStartAmount}, {Colors.Blue, pieStartAmount}};
   }

   private Dictionary<Colors, int> _colorPopulation = new();
        
    
    public float[] colorAngles = new float[3];

    void RefreshColors(List<Person> people)
    {
        UpdateColorAngles();
        SetColorPos(people);
    }

    void UpdateColorAngles()
    {
        for (int i = 0; i < _colorPopulation.Count; i++)
        {
            colorAngles[i] = 360 * _colorPopulation.Values.ElementAt(i)/CrowdManager.Instance.crowdQuota;
        }

        for (int i = 0; i < colorAngles.Length-1; i++)
        {
            colorAngles[i + 1] += colorAngles[i];
        }
    }
    
    void SetColorPos(List<Person> people)                                       //Coloring chars according to their angle
    {
        for (int i = 0; i < people.Count; i++)                            
        {
            if (PosAngle(i) >= 0 && PosAngle(i) <  colorAngles[0] )
                people[i].PaintChar(Colors.Red);
            
            else if (PosAngle(i) >=  colorAngles[0] && PosAngle(i) <  colorAngles[1])
                people[i].PaintChar( Colors.Green);
            
            else if (PosAngle(i) >=  colorAngles[1] && PosAngle(i) < 360)
                people[i].PaintChar(Colors.Blue);
            
            
            //print("angle: " + PosAngle(i) + " " +  "pos: " + posList[i]);
        }
    }
    
    float PosAngle(int i)                                    //Find each char's angle according to their position in the circle
    {
        var posList = CrowdManager.Instance.posList;
        float posAngle = Mathf.Atan2(posList[posList.Count - 1 - i].z, posList[posList.Count - 1 - i].x) * Mathf.Rad2Deg;
        if (posAngle < 0) posAngle = (360 + posAngle) % 360;     
        return posAngle;                                                
    }

   
    

    void IncreaseColors(Person person)                //Increase pie according to newcomer char's color
    {
        _colorPopulation[person.color]++;
    }
    
    /*
   *  [Serializable]
     public struct ColorPopulation {
     public string colorName;
     public int amount;
    }
    public ColorPopulation[] colorPopulation;
   */
    
}
