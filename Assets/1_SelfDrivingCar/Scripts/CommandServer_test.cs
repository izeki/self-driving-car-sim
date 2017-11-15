using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SocketIO;
using UnityStandardAssets.Vehicles.Car;
using System;
using System.Security.AccessControl;

public class CommandServer : MonoBehaviour
{
	public CarRemoteControl CarRemoteControl;
	public Camera FrontFacingCamera;
	private SocketIOComponent _socket;
	private CarController _carController;
    
    private SocketIOComponent _socket2;

	// Use this for initialization
	void Start()
	{
		_socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent>();
		_socket.On("open", OnOpen);
		_socket.On("steer", OnSteer);
        _socket.On("offRoad", OnOffRoad);
		_socket.On("manual", onManual);
		_carController = CarRemoteControl.GetComponent<CarController>();
        
        _socket2 = GameObject.Find("SocketIO2").GetComponent<SocketIOComponent>();
        _socket2.On("open", OnOpenData);
        _socket2.On("data", OnSendData);
	}

	// Update is called once per frame
	void Update()
	{
	}

	void OnOpen(SocketIOEvent obj)
	{
		Debug.Log("Connection Open");
		EmitTelemetry(obj);
	}
    
    void OnOpenData(SocketIOEvent obj) 
    {
        Debug.Log("Connection Data Open");
		EmitData(obj);
    }
    
    void OnSendData(SocketIOEvent obj) 
    {
        Debug.Log("OnSendData");
		EmitData(obj);
    }

	// 
	void onManual(SocketIOEvent obj)
	{
		EmitTelemetry (obj);
	}

	void OnSteer(SocketIOEvent obj)
	{
		JSONObject jsonObject = obj.data;
		//    print(float.Parse(jsonObject.GetField("steering_angle").str));
		CarRemoteControl.SteeringAngle = float.Parse(jsonObject.GetField("steering_angle").str);
		CarRemoteControl.Acceleration = float.Parse(jsonObject.GetField("throttle").str);
		EmitTelemetry(obj);
	}
    
    void OnOffRoad(SocketIOEvent obj)
    {
        CarRemoteControl.IsOffRoad = false;
        CarRemoteControl.SteeringAngle = 0.0f;
		CarRemoteControl.Acceleration = 0.0f;
        EmitTelemetry(obj);
    }
    
    void EmitData(SocketIOEvent obj) 
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
            _socket2.Emit("data", new JSONObject());
        });
    }

	void EmitTelemetry(SocketIOEvent obj)
	{
		UnityMainThreadDispatcher.Instance().Enqueue(() =>
		{
			//print("Attempting to Send...");
			// send only if it's not being manually driven
			//if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
            if (! _carController.IsAuto) {
				_socket.Emit("telemetry", new JSONObject());
			}
            else if (_carController.IsOffRoad) {
                _socket.Emit("offRoad", new JSONObject());
            }
			else {
				// Collect Data from the Car
				Dictionary<string, string> data = new Dictionary<string, string>();
				data["steering_angle"] = _carController.CurrentSteerAngle.ToString("N4");
				data["throttle"] = _carController.AccelInput.ToString("N4");
				data["speed"] = _carController.CurrentSpeed.ToString("N4");
                data["pitch"] = _carController.CurrentSpeed.ToString("N4");
                data["yaw"] = _carController.CurrentSpeed.ToString("N4");
				data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera));
				_socket.Emit("telemetry", new JSONObject(data));
			}
		});

		//    UnityMainThreadDispatcher.Instance().Enqueue(() =>
		//    {
		//      	
		//      
		//
		//		// send only if it's not being manually driven
		//		if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S))) {
		//			_socket.Emit("telemetry", new JSONObject());
		//		}
		//		else {
		//			// Collect Data from the Car
		//			Dictionary<string, string> data = new Dictionary<string, string>();
		//			data["steering_angle"] = _carController.CurrentSteerAngle.ToString("N4");
		//			data["throttle"] = _carController.AccelInput.ToString("N4");
		//			data["speed"] = _carController.CurrentSpeed.ToString("N4");
		//			data["image"] = Convert.ToBase64String(CameraHelper.CaptureFrame(FrontFacingCamera));
		//			_socket.Emit("telemetry", new JSONObject(data));
		//		}
		//      
		////      
		//    });
	}
}