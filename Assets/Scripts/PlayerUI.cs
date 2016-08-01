using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField] RectTransform thrusterFuelFill;
	[SerializeField] RectTransform healthBarFill;

	private PlayerController controller;
	private Player player;

	public void SetController(PlayerController _controller) {
		controller = _controller;
	}

	public void SetPlayer(Player _player) {
		player = _player;
	}

	void SetFuelAmount(float _amount) {
		thrusterFuelFill.localScale = new Vector3 (1f, _amount, 1f);
	}

	void SetHealthAmount(float _amount) {
		healthBarFill.localScale = new Vector3 (1f, _amount, 1f);
	}

	void Update() {
		SetFuelAmount(controller.GetThrusterFuelAmount());
		SetHealthAmount(player.GetHealthAmount());
	}


}
