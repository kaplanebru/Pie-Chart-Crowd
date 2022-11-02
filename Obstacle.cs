using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
   private Collider collider;
   public float floorLevel = -1.4f;

   private void Start()
   {
      collider = GetComponent<Collider>();
   }

   private void OnTriggerEnter(Collider other)
   {
      GameManager.Instance.crowdManager.UpdatePopulation( null, -1);
      collider.enabled = false;
      Neutralize();
   }

   void Neutralize()
   {
      transform.DOMoveY(floorLevel, 0.5f);
   }
}
