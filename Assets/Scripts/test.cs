using UnityEngine;
using System;
using System.Collections.Generic;
using Unity;


public class test : MonoBehaviour
{
    [SerializeField] private int dimensions = 10;
    [SerializeField] private GameObject gObject;
    private void Start()
    {
        for (int i = 0; i < dimensions; i++)
        {
            for (int j = 0; j < dimensions; j++)
            {
                Instantiate(gObject, new Vector2(i, j) * 0.5f, Quaternion.identity, gameObject.transform);
            }
        }
    }
}
