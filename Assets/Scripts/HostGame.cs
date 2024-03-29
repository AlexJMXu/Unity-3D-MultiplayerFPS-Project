﻿using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour {

	[SerializeField] private uint roomSize = 16;

	private string roomName;

	private NetworkManager networkManager;

	void Start() {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker();
		}
	}

	public void SetRoomName(string _name) {
		roomName = _name;
	}

	public void CreateRoom() {
		if (!string.IsNullOrEmpty(roomName)) {
			Debug.Log("Creating room: " + roomName + " with room size " + roomSize);
			networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "", "", "", 0, 0, networkManager.OnMatchCreate);
		} else {
			Debug.Log("Failed to create room");
		}
	}

}
