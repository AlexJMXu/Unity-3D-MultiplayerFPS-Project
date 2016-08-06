using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class JoinGame : MonoBehaviour {

	List<GameObject> roomList = new List<GameObject>();

	[SerializeField] private Text status;

	[SerializeField] private GameObject roomListItemPrefab;

	[SerializeField] private Transform roomListParent;

	private NetworkManager networkManager;

	void Start() {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker();
		}

		RefreshRoomList();
	}

	public void RefreshRoomList() {
		ClearRoomList();
		status.text = "Loading...";
		networkManager.matchMaker.ListMatches(0, 20, "", OnMatchList);
	}

	public void OnMatchList(ListMatchResponse matchList) {
		status.text = "";
		if (matchList == null) {
			status.text = "Couldn't get room list.";
			return;
		}

		foreach (MatchDesc match in matchList.matches) {
			GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
			_roomListItemGO.transform.SetParent(roomListParent);
			RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
			if (_roomListItem != null) {
				_roomListItem.Setup(match, JoinRoom);
			}
			roomList.Add(_roomListItemGO);
			//_roomListItemGO.GetComponent<Button>().onClick.AddListener(() 
			//	=> { JoinRoom(match.networkId); });
		}

		if (roomList.Count == 0) {
			status.text = "No rooms found.";
		}
	}

	private void ClearRoomList() {
		for (int i = 0; i < roomList.Count; i++) {
			Destroy(roomList[i]);
		}

		roomList.Clear();
	}

	public void JoinRoom(MatchDesc _match) {
		networkManager.matchMaker.JoinMatch(_match.networkId, "", networkManager.OnMatchJoined);
		ClearRoomList();
		status.Text("Joining...");
	}
}
