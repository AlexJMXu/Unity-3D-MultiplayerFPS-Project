using UnityEngine;
using UnityEngine.UI;

public class PlayerNameplate : MonoBehaviour {

	[SerializeField] private Text usernameText;
	[SerializeField] RectTransform healthBarFill;

	[SerializeField] private Player player;

	void Update () {
		Camera cam = Camera.main;

		usernameText.text = player.username;
		healthBarFill.localScale = new Vector3(player.GetHealthAmount(), 1f, 1f);

		if (cam != null) 
			transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
				cam.transform.rotation * Vector3.up);
	}
}
