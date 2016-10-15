using UnityEngine;

public class PlayerUI : MonoBehaviour {

	[SerializeField] RectTransform thrusterFuelFill;
	[SerializeField] RectTransform healthBarFill;

	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject scoreBoard;

	private PlayerController controller;
	private Player player;

	void Start() {
		PauseMenu.isOn = false;
	}

	void Update() {
		SetFuelAmount(controller.GetThrusterFuelAmount());
		SetHealthAmount(player.GetHealthAmount());

		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			scoreBoard.SetActive(true);
		} else if (Input.GetKeyUp(KeyCode.Tab)) {
			scoreBoard.SetActive(false);
		}
	}

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

	public void TogglePauseMenu() {
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.isOn = pauseMenu.activeSelf;
	}


}
