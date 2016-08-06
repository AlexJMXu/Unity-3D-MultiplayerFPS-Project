using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {

	public delegate void JoinRoomDelegate(MatchDesc _match);
	private JoinRoomDelegate joinRoomCallback;

	[SerializeField] private Text roomNameText;

	private MatchDesc match;

	public void Setup(MatchDesc _match, JoinRoomDelegate _joinRoomCallback) {
		match = _match;
		joinRoomCallback = _joinRoomCallback;

		roomNameText.text = match.name + " (" + match.currentSize + "/" + match.maxSize + ")";
	}

	public void JoinRoom() {
		joinRoomCallback.Invoke(match);
	}

	/*public void JoinRoom(NetworkID _netId) {
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
    }*/

}
