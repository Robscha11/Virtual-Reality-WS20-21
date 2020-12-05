using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SpaceNavigatorDriver;
using UnityEngine.UI;

// Group A:
// Immatriculation Number 122264
// Immatriculation Number 120617

public class Mover : MonoBehaviour
{
    private int mode = 0;    
    public string modeString = string.Empty;

    private Vector3 posLF; // cursor position last frame
    public float cursorVel; // measured cursor velocity

    private GameObject birdie;
    private GameObject mainCamera;

    private bool spacemouse_flag = false;  // because working with gamepad

    private float isotonicStartRateValueX = Input.GetAxis("Mouse X");
    private float isotonicStartRateValueY = Input.GetAxis("Mouse Y");
    private float isotonicStartAccValueX = Input.GetAxis("Mouse X");
    private float isotonicStartAccValueY = Input.GetAxis("Mouse Y");
    private float m = 1.0f;
    private float n = 0.1f;
    private bool xGoForw = false;
    private bool xGoBackw = false;
    private bool YGoUp = false;
    private bool YGoDown = false;
    private float k = 0.1f;
    private float l = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        posLF = transform.position;

        birdie = GameObject.Find("Birdie");
        mainCamera = GameObject.Find("Main Camera");

        // initial states
        SetMode(1); // isotonic position control
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMode();
        InputMapping();

        // check for birdie visibility
        bool flag = IsRenderedFrom(birdie.GetComponent<Renderer>(), mainCamera.GetComponent<Camera>());
        if (flag == false)
        {
            transform.position = posLF;
        }

        // calc actual velocity if birdie (displayed in HUD)
        float dist = Vector3.Distance(posLF, transform.position); // moved distance from last to actual frame
        posLF = transform.position;
        cursorVel = dist / Time.deltaTime;
        cursorVel = (float)System.Math.Round(cursorVel, 2);
        //Debug.Log(cursorVel);

    }


    void InputMapping()
    {
        // isotonic input
        float isotonicX = Input.GetAxis("Mouse X");
        float isotonicY = Input.GetAxis("Mouse Y");
        //Debug.Log(isotonicX + " " + isotonicY);

        // elastic input
        float elasticX = 0.0f;
        float elasticY = 0.0f;

        // select elastic input device
        if (spacemouse_flag == true) // elastic input from spacemouse
        {
            // elasticX = SpaceNavigator.Translation.x;     // in comments because  SpaceNavigator import also as comment
            // elasticY = SpaceNavigator.Translation.y;
        }

        if (spacemouse_flag == false)  
        {
            elasticX = Input.GetAxis("Horizontal") * 0.15f;
            elasticY = Input.GetAxis("Vertical") * -0.15f;
        }

        //Debug.Log(elasticX + " " + elasticY);

        switch (mode)
        {
            case 1:
                IsotonicPosition(isotonicX, isotonicY);
                break;
            case 2:
                IsotonicRate(isotonicX, isotonicY);
                break;
            case 3:
                IsotonicAcceleration(isotonicX, isotonicY);
                break;
            case 4:
                ElasticPosition(elasticX, elasticY);
                break;
            case 5:
                ElasticRate(elasticX, elasticY);
                break;
            case 6:
                ElasticAcceleration(elasticX, elasticY);
                break;
            default:
                break;
        }
    }

    void IsotonicPosition(float X, float Y)
    {
        Debug.Log("X: " + X);
        Debug.Log("Y: " + Y);
        float factor = 0.05f;
        transform.Translate(X * factor, Y * factor, 0.0f);
    }

    void IsotonicRate(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("X: " + X);
        Debug.Log("Y: " + Y);
        Debug.Log("isotonicStartRateValueX: " + isotonicStartRateValueX);
        Debug.Log("isotonicStartRateValueY: " + isotonicStartRateValueY);

        isotonicStartRateValueX += X;
        isotonicStartRateValueY += Y;

        // Note: the zone between -1.0 and 1.0 in x and also in y
        //       are assumed as "center-point", for better control

        if (isotonicStartRateValueX > 1.0f) {               
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        else if (isotonicStartRateValueX < -1.0f){
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }

        if (isotonicStartRateValueY > 1.0f)
        {
            transform.Translate(0, 1 * Time.deltaTime, 0);
        }
        else if (isotonicStartRateValueY < -1.0f)
        {
            transform.Translate(0, -1 * Time.deltaTime, 0);
        }

        // YOUR CODE - END    
    }

    void IsotonicAcceleration(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("X: " + X);
        Debug.Log("Y: " + Y);
        Debug.Log("isotonicStartAccValueX: " + isotonicStartAccValueX);
        Debug.Log("isotonicStartAccValueY: " + isotonicStartAccValueY);

        isotonicStartAccValueX += X;
        isotonicStartAccValueY += Y;
        
        // Note: the zone between -1.0 and 1.0 in x and also in y
        //       are assumed as "center-point", for better control

        if (isotonicStartAccValueX > 1.0f)
        {
            m += X;
            transform.Translate(m * Time.deltaTime , 0, 0);
        }
        else if (isotonicStartAccValueX < -1.0f)
        {
            m -= X;
            transform.Translate(-m * Time.deltaTime , 0, 0);
        }

        if (isotonicStartAccValueY > 1.0f)
        {
            n += Y;
            transform.Translate(0, n * Time.deltaTime, 0);
        }
        else if (isotonicStartAccValueY < -1.0f)
        {
            n -= Y;
            transform.Translate(0, -n * Time.deltaTime, 0);
        }

        // YOUR CODE - END
    }

    void ElasticPosition(float X, float Y)
    {
        Debug.Log("X: " + X);
        Debug.Log("Y: " + Y);
        float factor1 = 16.0f;
        float factor2 = 8.0f;
        transform.position = (new Vector3(X*factor1, Y*factor2, 0.0f)); 
    }

    void ElasticRate(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("X: " + X);
        Debug.Log("Y: " + Y);


        if (X > 0.0f) {
            xGoForw = true;
            xGoBackw = false;
        } else if (X < 0.0f)
        {
            xGoForw = false;
            xGoBackw = true;
        } else
        {
            xGoForw = false;
            xGoBackw = false;
        }
        
        if (xGoForw) { 
            transform.Translate(1 * Time.deltaTime, 0, 0);
        }
        if (xGoBackw){
            transform.Translate(-1 * Time.deltaTime, 0, 0);
        }



        if (Y > 0.0f)
        {
            YGoUp = true;
            YGoDown = false;
        }
        else if (Y < 0.0f)
        {
            YGoUp = false;
            YGoDown = true;
        }
        else
        {
            YGoUp = false;
            YGoDown = false;
        }
        if (YGoUp)
        {
            transform.Translate(0, 1 * Time.deltaTime, 0);
        }
        else if (YGoDown)
        {
            transform.Translate(0, -1 * Time.deltaTime, 0);
        }

        // YOUR CODE - END    
    }
    void ElasticAcceleration(float X, float Y)
    {
        // YOUR CODE - BEGIN   // still need some changes for accel.
        Debug.Log("X: " + X);
        Debug.Log("Y: " + Y);


        if (X > 0.0f)
        {
            xGoForw = true;
            xGoBackw = false;
        }
        else if (X < 0.0f)
        {
            xGoForw = false;
            xGoBackw = true;
        }
        else
        {
            xGoForw = false;
            xGoBackw = false;
        }

        if (xGoForw)
        {
            k += X*0.03f;
            transform.Translate(k * Time.deltaTime, 0, 0);
        }
        if (xGoBackw)
        {
            k += X*0.03f;
            transform.Translate( k * Time.deltaTime, 0, 0);
        }
        if (!xGoForw && !xGoBackw)
        {
            transform.Translate( k * Time.deltaTime, 0, 0);
        }



        if (Y > 0.0f)
        {
            YGoUp = true;
            YGoDown = false;
        }
        else if (Y < 0.0f)
        {
            YGoUp = false;
            YGoDown = true;
        }
        else
        {
            YGoUp = false;
            YGoDown = false;
        }

        if (YGoUp)
        {
            l += Y * 0.1f;
            transform.Translate(0, l * Time.deltaTime, 0);
        }
        if (YGoDown)
        {
            l += Y * 0.1f;
            transform.Translate(0, l * Time.deltaTime, 0);
        }
        if (!YGoUp && !YGoDown)
        {
            transform.Translate(0, l * Time.deltaTime, 0);
        }

        // YOUR CODE - END    
    }

    void Recenter()
    {
        transform.localPosition = Vector3.zero;
    }

    void SetMode(int MODE)
    {
        mode = MODE;

        switch (mode)
        {
            case 1:
                modeString = "isotonic position";                
                break;
            case 2:
                modeString = "isotonic rate";                
                break;
            case 3:
                modeString = "isotonic acceleration";                
                break;
            case 4:
                modeString = "elastic position";                
                break;
            case 5:
                modeString = "elastic rate";                
                break;
            case 6:
                modeString = "elastic acceleration";                
                break;
        }

        // print to console
        Debug.Log(string.Format("New Mode: {0}", modeString));

    }

    void UpdateMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetMode(1); // isotonic position
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetMode(2); // isotonic rate
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetMode(3); // isotonic acceleration
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetMode(4); // elastic position
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetMode(5); // elastic rate
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetMode(6); // elastic acceleration
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Recenter();
        }
    }

    bool IsRenderedFrom(Renderer renderer, Camera camera)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }


    // Excercise 3.7:
    // Which combinations were suitable for this task, which combinations were less suitable? Why? 
    //Come up with potential use cases for the six different combinations.Think in the context of object manipulation and viewpoint navigation.

    /* YOUR EXPLANATION - BEGIN
     
        

        ex3.1 - isotonic-position control: 
                    suitable - easy to control, exact (dis-)placement is possible, typical mouse application
                    direct (linear or non linear) behavior between input and output

                    use cases: typical mouse application, e.g. under Windows or Linux,

        ex3.2 - isotonic-rate control:
                    suitable - but needs a little practice to find the "center" by hand, and appropriate input factorization 
                            that's why we implemented our center with a little tolerance range

                    use cases: CAD-appl.,

        ex3.3 - isotonic-accel. control:
                    suitable - but needs a little practice to find the "center" by hand, and appropriate input factorization and velocity accumulation
                            that's why we implemented our center with a little tolerance range

                    use cases: CAD-appl.,
                            

        ex3.4- elastic-position control:
                    not suitable - hard to control, bad translation (small input range --> big output range), not precise

                    use cases: Gaming,

        ex3.5 - elastic-rate control:
                    suitable - easy and precise to control (automatic centering) 
                    needs appropriate input factorization

                    use cases: Gaming,

        ex3.6 - elastic -accel. control:
                    suitable - easy and precise to control (automatic centering) 
                    needs appropriate input factorization and velocity accumulation

                    use cases: Gaming, Flight-simul., 





        based on observation of all tasks and slights of lec.5 ( especially image on page 23)

     YOUR EXPLANATION - END  */

}
