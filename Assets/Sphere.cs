 using UnityEngine;
 using System.Collections;
 
 public class Sphere: MonoBehaviour {
         
     public GameObject cubePrefab;
     public float sizingFactor = 0.02f;
     private Vector3 startSize;
     private float startX;
     private float startY;
         
     void Update () {
         if (Input.GetMouseButtonDown (0)) {
             print("getMouseButtonDown");
             float positionZ = 10.0f;
             Vector3 position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y,positionZ);
             startX = position.x;
             startY = position.y;
             startSize = cubePrefab.transform.localScale;
             print("startX:"+startX);
             
         }
 
         if (Input.GetMouseButton (0)) {
             Vector3 size = cubePrefab.transform.localScale;
             print(size);
             size.x = startSize.x + (Input.mousePosition.x - startX + Input.mousePosition.y - startY) * sizingFactor;
             size.y = startSize.y + (Input.mousePosition.x - startX+ Input.mousePosition.y - startY) * sizingFactor;
             size.z = startSize.z + (Input.mousePosition.x - startX+ Input.mousePosition.y - startY) * sizingFactor;
             cubePrefab.transform.localScale = size;
             print("RESIZE");
         }
     }
 }