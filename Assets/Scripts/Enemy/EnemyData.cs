using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float _health = 10.0f;
    //[SerializeField] public EnemyData _enemyData;
    //var a = _enemyData._health;

    public float _startWaitingTime = 4;
    public float _timeToRotate = 2;
    public float _walkSpeed = 2.6f;
    public float _runSpedd = 20f;

    public float _viewRadius = 15;
    public float _viewAngle = 90;
    public LayerMask _playerMakk;
    public LayerMask _obstacleMask;
    public float _meshResolution = 1f;
    public int _edgeIteration = 4;
    public float _edgeDistance = 0.5f;

    public Transform[] _wayPoints;
    public int _currentWayPointIndex;


}   
