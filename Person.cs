using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityExtensions;

public class Person : MonoBehaviour
{
    private Collider collider;
    public ColorSeparator.Colors color;
    public SkinnedMeshRenderer mesh;

    private void Awake()
    {
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        
        collider = GetComponent<Collider>();
        PaintChar(color);
    }
    
    public void PaintChar(ColorSeparator.Colors _color)
    {
        switch (_color)
        {
            case ColorSeparator.Colors.Red:
                mesh.material.color = Color.red;
                break;
            case ColorSeparator.Colors.Green:
                mesh.material.color = Color.green;
                break;
            case ColorSeparator.Colors.Blue:
                mesh.material.color = Color.blue;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        color = _color;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        collider.enabled = false;
        JoinCrowd();

    }

    void JoinCrowd()
    {
        transform.SetParent(GameManager.Instance.crowdManager.transform);
        CrowdManager.Instance.UpdatePopulation( this, 1);
    }
    
    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }

    void Arrived()
    {
        if (transform.position.z <= GameManager.Instance.crowdManager.transform.position.z)
        {
            transform.DOKill();
            JoinCrowd();
        }
    }
}
