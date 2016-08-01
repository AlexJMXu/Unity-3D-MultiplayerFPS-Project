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
		status.text = "Loading...";
		networkManager.matchMaker.ListMatches(0, 20, "", OnMatchList);
	}

	public void OnMatchList(ListMatchResponse matchList) {
		status.text = "";
		if (matchList == null) {
			status.text = "Couldn't get room list.";
			return;
		}

		ClearRoomList();

		foreach (MatchDesc match in matchList.matches) {
			GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
			_roomListItemGO.transform.SetParent(roomListParent);
			Text _roomNameText = _roomListItemGO.transform.GetChild(0).GetComponent<Text>();
			_roomNameText.text = match.name + " (" 
								+ match.currentSize + "/" 
								+ match.maxSize + ")";
			// Have a component sit on the gameobject
			// that will take care of setting up the name/amount of users
			// as well as setting up a callback function that will join the game
			roomList.Add(_roomListItemGO);
			_roomListItemGO.GetComponent<Button>().onClick.AddListener(() 
				=> { JoinRoom(match.networkId); });
		}
	}

	private void ClearRoomList() {
		for (int i = 0; i < roomList.Count; i++) {
			Destroy(roomList[i]);
		}

		roomList.Clear();
	}

	public void JoinRoom(NetworkID _netId) {
		ClearRoomList();
		status.text = "Joining room...";
		networkManager.matchMaker.JoinMatch(_netId, "", OnJoinMatch);
	}

	private void OnJoinMatch(JoinMatchResponse matchJoin) {
        if (matchJoin.success) {
            MatchInfo hostInfo = new MatchInfo(matchJoin);
            NetworkManager.singleton.StartClient(hostInfo);
        } else {
            status.text = "Failed to join room.";
            StartCoroutine(WaitSeconds());
            RefreshRoomList();
        }
    }

    private IEnumerator WaitSeconds() {
    	yield return new WaitForSeconds(1f);
    }
}
