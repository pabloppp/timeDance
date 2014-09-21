using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class teSocket : MonoBehaviour {

	SocketIOClient.Client socket;
	bool error = false;
	string id = "";
	// Use this for initialization
	void Start () {
		socket = new SocketIOClient.Client("http://localhost:3000/");

		socket.On("connect", (fn) => {
			Debug.Log ("connect - socket");
			
			//Dictionary<string, string> args = new Dictionary<string, string>();
			List<string> args = new List<string>();
			//args.Add("what's up?");
			//socket.Emit("SEND", args);
			//string valueout;
			//args.TryGetValue("msg", out valueout);
		});

		socket.On("keyPressed", (fn) => {
			Debug.Log ("keyRecived "+fn.Json.args[0] );
		});

		socket.Error+=(sender, e) => {
			Debug.Log("Error "+ e.Message.ToString());
			error = true;
		};

		if(!error) socket.Connect();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnApplicationQuit(){
		socket.Close();
	}

	public void emit(string channel, string message){
		socket.Emit (channel, message);
	}
}
