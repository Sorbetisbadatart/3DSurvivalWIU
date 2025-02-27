using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyState
{
    public void OnEnter();
  
    public void OnUpdate();

    public void OnExit();
}
