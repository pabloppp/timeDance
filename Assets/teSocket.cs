using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class teSocket : MonoBehaviour {

	SocketIOClient.Client socket;
	// Use this for initialization
	void Start () {
		socket = new SocketIOClient.Client("http://localhost:3000/");

		socket.On("connect", (fn) => {
			Debug.Log ("connect - socket");
			
			Dictionary<string, string> args = new Dictionary<string, string>();
			args.Add("msg", "what's up?");
			socket.Emit("SEND", args);
		});
		socket.Error+=(sender, e) => {
			Debug.Log("Error "+ e.Message.ToString());
		};
		socket.Connect();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnApplicationQuit(){
		socket.Close();
	}
}
