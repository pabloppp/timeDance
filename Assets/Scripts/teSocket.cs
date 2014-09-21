using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class teSocket : MonoBehaviour {

	SocketIOClient.Client socket;
	bool error = false;
	string id = "";

	public int lag;

	public GameObject onlinePlayer;
	songManager manager;

	// Use this for initialization
	void Start () {
		manager = onlinePlayer.GetComponent<songManager> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void initialize(){
		socket = new SocketIOClient.Client("http://192.168.1.104:3000/");
		
		socket.On("connect", (fn) => {
			Debug.Log ("connect - socket");
			
			//Dictionary<string, string> args = new Dictionary<string, string>();
			//List<string> args = new List<string>();
			//args.Add("what's up?");
			//socket.Emit("SEND", args);
			//string valueout;
			//args.TryGetValue("msg", out valueout);
		});
		
		socket.On("keyPressed", (fn) => {

			string data =  (string)fn.Json.args[0];

			string[] dataArray = data.Split(',');
			int millis = int.Parse(dataArray[1]);

			int score = int.Parse(dataArray[2]);

			if(dataArray[0].Equals("0")) manager.reciveLeft(millis, score);
			if(dataArray[0].Equals("1")) manager.reciveDown(millis, score);
			if(dataArray[0].Equals("2")) manager.reciveUp(millis, score);
			if(dataArray[0].Equals("3")) manager.reciveRight(millis, score);
		});
		
		socket.Error+=(sender, e) => {
			Debug.Log("Error "+ e.Message.ToString());
			error = true;
		};
		
		if(!error) socket.Connect();
	}
	

	void OnApplicationQuit(){
		if(socket != null) socket.Close();
	}

	public void emit(string channel, string message){
		socket.Emit (channel, message);
	}
}
