using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetManager : NetworkManager {

	// Use this for initialization
	void Start () {
        dontDestroyOnLoad = false;
        autoCreatePlayer = false;
        networkAddress = "192.168.0.101";
        networkPort = 7777;
        StartServer();	
	}
	
}
