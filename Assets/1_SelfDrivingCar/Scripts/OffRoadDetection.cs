using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine;


public class OffRoadDetection : MonoBehaviour {        
    public CarRemoteControl CarRemoteControl;
	private CarController _carController;
    
    // Use this for initialization
    void Start () {
        _carController = CarRemoteControl.GetComponent<CarController>();
    }

    // Update is called once per frame
    
    void Update () {
        
    }    

    void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall")
            print(gameObject.name + " and " + collisionInfo.collider.name + " are still colliding");
            //Application.LoadLevel(Application.loadedLevel); //Restart scene

    }

    void OnCollisionExit(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == "Wall")
            print(gameObject.name + " and " + collisionInfo.collider.name + " are no longer colliding");
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == "Wall") {
            //print("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
            print("Detected collision between " + gameObject.name + " and " + GetGameObjectPath(collisionInfo.gameObject));
            print("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
            print("Their relative velocity is " + collisionInfo.relativeVelocity);
            _carController.IsOffRoad = true;
            Debug.Log("COLLISION!");
        }
    }
    
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}
