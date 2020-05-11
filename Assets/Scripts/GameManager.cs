using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ParticleSystem ps;
    public GameObject WellDone;
    // Start is called before the first frame update
    void Start()
    {
        StartNewLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEnd()
    {
        var main = ps.main;
        main.duration = 1.0f;
        ps.enableEmission = true;
        WellDone.gameObject.SetActive(true);
        //ps.MainModule.Duration = 2.0f;
        ps.Play();
    }

    public void StartNewLevel()
    {
        var main = ps.main;
        ps.enableEmission = false;
        main.playOnAwake = false;
        WellDone.gameObject.SetActive(false);
        ps.Stop();
    }
}
