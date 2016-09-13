using UnityEngine;
using System;

public class DataTranslator : MonoBehaviour {

	private static string KILLS_TAG = "[KILLS]";
	private static string DEATHS_TAG = "[DEATHS]";

	public static string ValuesToData(int kills, int deaths) {
		return KILLS_TAG + kills + "/" + DEATHS_TAG + deaths;
	}

	public static int DataToKills(string data) {
		return int.Parse(DataToValue(data, KILLS_TAG));
	}

	public static int DataToDeaths(string data) {
		return int.Parse(DataToValue(data, DEATHS_TAG));
	}

	private static string DataToValue (string data, string tag) {
		string[] pieces = data.Split('/');
		for (int i = 0; i < pieces.Length; i++) {
			if (pieces[i].StartsWith(tag)) {
				return pieces[i].Substring(tag.Length);
			}
		}

		Debug.LogError(tag + " not found in data.");
		return "";
	}
}
