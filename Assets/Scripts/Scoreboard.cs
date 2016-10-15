using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	[SerializeField] GameObject playerScoreboardItem;

	[SerializeField] Transform playerScoreboardList;

	void OnEnable() {
		Player[] players = GameManager.GetAllPlayers();

		for (int i = 0; i < players.Length; i++) {
			GameObject itemGO = (GameObject) Instantiate(playerScoreboardItem, playerScoreboardList);
			PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
			if (item != null) {
				item.Setup(players[i].username, players[i].kills, players[i].deaths);
			}
		}
	}

	void OnDisable() {
		foreach (Transform child in playerScoreboardList) {
			Destroy(child.gameObject);
		}
	}
}
