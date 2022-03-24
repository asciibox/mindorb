using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MousePositionOnTerrain : MonoBehaviour
{
    Vector3 worldPosition;
    // Update is called once per frame

     private GameObject cubePrefab = null;
     public float sizingFactor = 0.02f;
     private Vector3 startSize;
     private float startX;
     private float startY;

    void Update()
    {
      if (Input.GetMouseButtonUp(0)) {
          cubePrefab = null;
      }
      if (Input.GetMouseButtonDown(0)) {
        Debug.Log("OnMouseDown");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitData;
                if(Physics.Raycast(ray, out hitData, 1000))
                    {
                        worldPosition = hitData.point;
                        Debug.Log("World Position");
                        Debug.Log(worldPosition);

                        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

                        if (!isShiftKeyDown) {

                            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            sphere.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
                            sphere.transform.parent = hitData.transform;

                        } else {

                            cubePrefab = hitData.transform.gameObject;

                                        print("getMouseButtonDown");
                                        float positionZ = 10.0f;
                                        Vector3 position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y,positionZ);
                                        startX = position.x;
                                        startY = position.y;
                                        startSize = cubePrefab.transform.localScale;
                                        print("startX:"+startX);
                                
                            
                                  

                        }

                    } else {
                        Debug.Log("No raycast");
                        cubePrefab = null;
                    }
      }

      if ( (Input.GetMouseButton (0)) && (cubePrefab!=null) ) {
                                        Vector3 size = cubePrefab.transform.localScale;
                                        print(size);
                                        size.x = startSize.x + (Input.mousePosition.x - startX + Input.mousePosition.y - startY) * sizingFactor;
                                        size.y = startSize.y + (Input.mousePosition.x - startX+ Input.mousePosition.y - startY) * sizingFactor;
                                        size.z = startSize.z + (Input.mousePosition.x - startX+ Input.mousePosition.y - startY) * sizingFactor;
                                        
                                        print("cubePrefab.transform.parent:"+cubePrefab.transform.parent);
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


                                        print("RESIZE");
      }

     

    }

}