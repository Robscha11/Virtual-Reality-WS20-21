using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Group:
// Immatriculation Number 122264
// Immatriculation Number 120617

public class SolarExerciseScript : MonoBehaviour
{
    GameObject sun;
    GameObject earth;
    GameObject moon;

    GameObject earthAxis;
    GameObject moonAxis;
    GameObject earthGeometry;

    public float speedchange12 = 0;
    public float speedchange3 = 0;


    // Start is called before the first frame update
    void Start()
    {
        // YOUR CODE - BEGIN
        earth = GameObject.Find("Earth");
        moon = GameObject.Find("Moon");
        sun = GameObject.Find("Sun");

        //earth = GameObject.Find("Earth");
        //earth.transform.localRotation = Quaternion.Euler(0, 0, 23.5f);  // init. Earth axis/orbit tilt (only 1 times) 
        
        // YOUR CODE - END
    }

    // Update is called once per frame
    void Update()
    {
        // Exercise 1.9
        // Check if unity world matrix is the same as your own GetWorldTransform.
        if (!CompareMatrix(moon))
        {
            Debug.Log("not the same - solve exercise 1.9");
        }
        else
        {
            Debug.Log("the same - solved exercise 1.9");
        }

        // Control Speed with Arrow Buttons 
        // Exercise 1.7
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            // YOUR CODE - BEGIN

            Debug.Log("UpArrowButton : ");
            speedchange12 += 10.0f;
            speedchange3 += 1.0f;

            // YOUR CODE - END
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            // YOUR CODE - BEGIN

            Debug.Log("DownArrowButton : ");
            speedchange12 -= 10.0f;
            speedchange3 -= 1.0f;

            // YOUR CODE - END
        }


        // NOT in comments for exercise 1.8  from here 
        // Note : start rotation with arrow button !
        /*
        RotateAroundParent(earth, speedchange12);
        RotateAroundParent(moon, speedchange12);
        sun.transform.RotateAround(Vector3.zero, Vector3.up, speedchange3 * Time.deltaTime);
        */
        // to here
    }

    // Exercise 1.8
    void RotateAroundParent(GameObject go, float rotationVelocity)
    {
        // YOUR CODE - BEGIN
        go.transform.RotateAround(go.transform.parent.position, Vector3.up, rotationVelocity * Time.deltaTime);  //set rotation speed with no hardware dependencies
        // YOUR CODE - END
    }

    // Exercise 1.9
    Matrix4x4 GetWorldTransform(GameObject go)
    {
        Matrix4x4 mat = new Matrix4x4();

        //Matrix4x4 localMat = Matrix4x4.TRS(go.transform.localPosition, go.transform.localRotation, go.transform.localScale);
        //int counter = go.transform.hierarchyCount;
        //for (int i = 0; i < counter; i++) { ... localMat = mat; ... }  // more advanced version with loop when when tree depth unknown

        // Simplified version - sufficient for this task

        mat = Matrix4x4.TRS(go.transform.parent.parent.localPosition, go.transform.parent.parent.localRotation, go.transform.parent.parent.localScale)
              * Matrix4x4.TRS(go.transform.parent.localPosition, go.transform.parent.localRotation, go.transform.parent.localScale)
              * Matrix4x4.TRS(go.transform.localPosition, go.transform.localRotation, go.transform.localScale);

        return mat;
    }

    bool CompareMatrix(GameObject go)
    {
        Matrix4x4 Id = Matrix4x4.identity;
        Debug.Log("identity_matrix: ");
        Debug.Log(Id.ToString());               // prints the Identity matrix as reference

        Matrix4x4 unityWorldMat = Matrix4x4.TRS(go.transform.position, go.transform.rotation, go.transform.lossyScale);
        Debug.Log("unityWorldMat: ");
        Debug.Log(unityWorldMat.ToString());    // prints the unityWorld matrix as reference

        Matrix4x4 ownWorldMat = GetWorldTransform(go);
        Debug.Log("ownWorldMat: ");
        Debug.Log(ownWorldMat.ToString());      // prints the own calculated world matrix as reference

        if (unityWorldMat == ownWorldMat)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
