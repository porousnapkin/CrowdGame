﻿using UnityEngine;
using System.Collections;

public class CrowdSpawner : MonoBehaviour {
    public CrowdCreator creator;
	
	void Start () {
        StartCoroutine(SpawnCoroutine());
	}

    IEnumerator SpawnCoroutine() {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            creator.SpawnUnit(transform.position);
        }
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
