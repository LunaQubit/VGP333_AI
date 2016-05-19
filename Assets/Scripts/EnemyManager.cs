using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager s_Instance;

    public static EnemyManager Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = GameObject.FindObjectOfType<EnemyManager>();
            }
            return s_Instance;
        }
    }

    public GameObject Player;
}

