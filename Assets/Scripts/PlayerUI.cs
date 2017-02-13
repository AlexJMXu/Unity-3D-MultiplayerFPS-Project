using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	[SerializeField] RectTransform thrusterFuelFill;
	[SerializeField] RectTransform healthBarFill;
	[SerializeField] Text ammoText;

	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject scoreBoard;

	private PlayerController controller;
	private Player player;
	private WeaponManager weaponManager;

	void Start() {
		PauseMenu.isOn = false;
	}

	void Update() {
		SetFuelAmount(controller.GetThrusterFuelAmount());
		SetHealthAmount(player.GetHealthAmount());
		SetAmmoAmount(weaponManager.GetCurrentWeapon().bullets, weaponManager.GetCurrentWeapon().maxBullets);

		if (Input.GetKeyDown(KeyCode.Escape)) {
			TogglePauseMenu();
		}

		if (Input.GetKeyDown(KeyCode.Tab)) {
			scoreBoard.SetActive(true);
		} else if (Input.GetKeyUp(KeyCode.Tab)) {
			scoreBoard.SetActive(false);
		}
	}

	public void SetPlayer(Player _player) {
		player = _player;
		controller = player.GetComponent<PlayerController>();
		weaponManager = player.GetComponent<WeaponManager>();
	}

	void SetFuelAmount(float _amount) {
		thrusterFuelFill.localScale = new Vector3 (1f, _amount, 1f);
	}

	void SetHealthAmount(float _amount) {
		healthBarFill.localScale = new Vector3 (1f, _amount, 1f);
	}

	void SetAmmoAmount(int _amount, int _maxAmount) {
		ammoText.text = _amount.ToString() + "/" + _maxAmount.ToString();
	}

	public void TogglePauseMenu() {
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		PauseMenu.isOn = pauseMenu.activeSelf;
	}


}
