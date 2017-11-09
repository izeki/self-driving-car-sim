using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine;

public class TrackDetection : MonoBehaviour {
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
        if (collisionInfo.gameObject.tag == "Player")
            print(gameObject.name + " and " + collisionInfo.collider.name + " are still colliding");
            //Application.LoadLevel(Application.loadedLevel); //Restart scene

    }

    void OnCollisionExit(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == "Player")
            print(gameObject.name + " and " + collisionInfo.collider.name + " are no longer colliding");
    }

    void OnCollisionEnter(Collision collisionInfo) {
        if (collisionInfo.gameObject.tag == "Player") {
            print("Detected collision between " + gameObject.name + " and " + collisionInfo.collider.name);
            print("There are " + collisionInfo.contacts.Length + " point(s) of contacts");
            print("Their relative velocity is " + collisionInfo.relativeVelocity);
            _carController.IsOffRoad = true;
            Debug.Log("COLLISION!");
        }
    }
}
