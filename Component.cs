using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour { 

    
    GameObject earthAxis;
    GameObject moonAxis;
    GameObject earthGeometry;

    GameObject earth;
    GameObject moon;
    GameObject sun;

    public float rotSpeed1 = 30.0f;     // values from here regarding Ex. 1.5 & 1.6
    public float rotSpeed2 = 30.0f;
    public float rotSpeed3 = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        earth = GameObject.Find("Earth");
        earth.transform.localRotation = Quaternion.Euler(0, 0, 23.5f);  // init. Earth axis/orbit tilt (only 1 times) 
    }

    // Update is called once per frame
    void Update()
    {

        earthAxis = GameObject.Find("EarthAxis");
        moonAxis = GameObject.Find("MoonAxis");
        earthGeometry = GameObject.Find("EarthGeometry");
       
        moon = GameObject.Find("Moon");
        sun = GameObject.Find("Sun");

        rotSpeed1 = rotSpeed2 = GetComponent<SolarExerciseScript>().speedchange12;  // regarding Ex. 1.7 - Accsess to other Script
        rotSpeed3 = GetComponent<SolarExerciseScript>().speedchange3;               // start init Rota. with upArrowButton !

        Debug.Log("rotSpeed1 : " + rotSpeed1);
        Debug.Log("rotSpeed2 : " + rotSpeed2);
        Debug.Log("rotSpeed3 : " + rotSpeed3);


        earth.transform.Rotate(new Vector3(0, 1, 0), rotSpeed1 * Time.deltaTime);  //set rotation speed with no hardware dependencies
        moon.transform.Rotate(Vector3.up, rotSpeed2 * Time.deltaTime);

        sun.transform.RotateAround(Vector3.zero, Vector3.up, rotSpeed3 * Time.deltaTime);
    }
}
