using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent (typeof (Camera))]
public class ArcballScript : MonoBehaviour
{
    // Is true when the user wants to rotate the camera
    bool ballEnabled = false;
    bool rotated = false;
 
    float rotationSpeed = 1f;
    float radius = 20f;

    Vector3 worldPosition;
    private GameObject cubePrefab = null;
    public float sizingFactor = 0.02f;
    private Vector3 startSize;
    private float startX;
    private float startY;
 
 
    // The mouse cursor's position during the last frame
    Vector3 last = new Vector3();
 
    // The target that the camera looks at
    Vector3 target = new Vector3 (0, 0, 0);

    Vector3 pos = new Vector3(0,0,0);

    // The spherical coordinates
    Vector3 sc = new Vector3 ();
 
    void Start ()
    {
        this.transform.position = new Vector3 (0.0f, 0.0f, radius);
        this.transform.LookAt (pos);
        sc = getSphericalCoordinates (this.transform.position);
    }
 
    Vector3 getSphericalCoordinates(Vector3 cartesian)
    {
        float r = Mathf.Sqrt(
            Mathf.Pow(cartesian.x, 2) + 
            Mathf.Pow(cartesian.y, 2) + 
            Mathf.Pow(cartesian.z, 2)
        );
 
        float phi = Mathf.Atan2(cartesian.z / cartesian.x, cartesian.x);
        float theta = Mathf.Acos(cartesian.y / r);
 
       if (cartesian.x < 0)
            phi += Mathf.PI;
 
        return new Vector3 (r, phi, theta);
    }
 
    Vector3 getCartesianCoordinates(Vector3 spherical)
    {
        Vector3 ret = new Vector3 ();
 
        ret.x = spherical.x * Mathf.Cos (spherical.z) * Mathf.Cos (spherical.y);
        ret.y = spherical.x * Mathf.Sin (spherical.z);
        ret.z = spherical.x * Mathf.Cos (spherical.z) * Mathf.Sin (spherical.y);
 
        return ret;
    }
     
    // Update is called once per frame
    void Update ()
    {
        if (Input.GetMouseButtonDown(0)) {
            cubePrefab = null;
            bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
              if (isShiftKeyDown) {
                     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitData;
                        if(Physics.Raycast(ray, out hitData, 1000))
                            {
                                cubePrefab = hitData.transform.gameObject;
                                worldPosition = hitData.point;
                                                float positionZ = 10.0f;
                                                Vector3 position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y,positionZ);
                                                startX = position.x;
                                                startY = position.y;
                                                startSize = cubePrefab.transform.localScale;
                            }
              }
        }

      if (Input.GetMouseButtonUp(0)) {
        cubePrefab = null;

    if (rotated == false) {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hitData;
                        if(Physics.Raycast(ray, out hitData, 1000))
                            {
                                if (hitData.transform.tag=="Untagged") {
                                    print("Adding Hull");
                                    Material yourMaterial = Resources.Load("HullMaterial", typeof(Material)) as Material;
                                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    sphere.transform.position = new Vector3(hitData.transform.position.x, hitData.transform.position.y, hitData.transform.position.z);
                                    sphere.GetComponent<Renderer>().material = yourMaterial;
                                    sphere.transform.localScale = new Vector3(sphere.transform.localScale.x*1.5f, sphere.transform.localScale.y*1.5f,sphere.transform.localScale.z*1.5f);
                                    sphere.transform.tag = "Hull";
                                } else {
                                    print("Adding sphere");
                                worldPosition = hitData.point;

                                bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                                if (!isShiftKeyDown) {
                                    
                                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                    sphere.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
                                    sphere.transform.parent = hitData.transform;
                                     Vector3 size = sphere.transform.localScale;
                                     // Make sure the new sphere is not larger than the parent sphere
                                     Vector3 parentScale = sphere.transform.parent.transform.localScale;
                                     if (size.x > parentScale.x) {
                                         sphere.transform.localScale = new Vector3(parentScale.x, parentScale.y, parentScale.z);
                                     }

                                }

                                }

                            } else {
                                Debug.Log("No raycast");
                               
                            }
            }

      }

      if ( (Input.GetMouseButton (0)) && (cubePrefab!=null) ) {
          print("cubePrefab:"+cubePrefab);
                                        Vector3 size = cubePrefab.transform.localScale;
                     
                                        size.x = startSize.x + (Input.mousePosition.x - startX + Input.mousePosition.y - startY) * sizingFactor;
                                        size.y = startSize.y + (Input.mousePosition.x - startX+ Input.mousePosition.y - startY) * sizingFactor;
                                        size.z = startSize.z + (Input.mousePosition.x - startX+ Input.mousePosition.y - startY) * sizingFactor;
                                        
                                        if (cubePrefab.transform.parent) {
                                                if (size.x>1) {
                                                size.x = 1;
                                                size.y = 1;
                                                size.z = 1;
                                            } else
                                               if (size.x<-1) {
                                                size.x = -1;
                                                size.y = -1;
                                                size.z = -1;
                                            }
                                        }

                                        cubePrefab.transform.localScale = size;


      }


        // Whenever the left mouse button is pressed, the
        // mouse cursor's position is stored for the arc-
        // ball camera as a reference.
        if (Input.GetMouseButtonDown(0))
        {
            // last is a global vec3 variable
            bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            if (isShiftKeyDown==false) {
                ballEnabled = true;
                last = Input.mousePosition;
                rotated = false;
            }
            
        }
 
        // When the user releases the left mouse button,
        // all we have to do is to reset the flag.
        if (Input.GetMouseButtonUp (0)) {
            ballEnabled = false;
            cubePrefab = null;
        }
 
        if (ballEnabled)
        {
            // Get the deltas that describe how much the mouse cursor got moved between frames
            float dx = (last.x - Input.mousePosition.x) * rotationSpeed;
            float dy = (last.y - Input.mousePosition.y) * rotationSpeed;
 
            // Only update the camera's position if the mouse got moved in either direction
            if (dx != 0f || dy != 0f)
            {
                // Rotate the camera left and right
                sc.y += dx * Time.deltaTime;
 
                // Rotate the camera up and down
                // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
                sc.z = Mathf.Clamp (sc.z + dy * Time.deltaTime, -1.5f, 1.5f);
 
                // Calculate the cartesian coordinates for unity
                transform.position = getCartesianCoordinates (sc) ;

                // Make the camera look at the target
                transform.LookAt (pos);

                rotated = true;
               
            }
 
            // Update the last mouse position
            last = Input.mousePosition;
        }

      

        float scroll = Input.GetAxis ("Mouse ScrollWheel");
        if (scroll!=0) {
            transform.Translate(0, 0, scroll * 5, Space.Self);
              transform.LookAt(pos);
              float distance = Vector3.Distance(pos, transform.position);
              sc = new Vector3(distance, sc.y, sc.z);


  
          
        }
            
    }
}