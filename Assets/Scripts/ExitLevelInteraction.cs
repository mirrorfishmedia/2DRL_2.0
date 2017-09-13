using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "2DRL/ExitLevelInteraction")]
public class ExitLevelInteraction : Interaction {

	public override void RespondToInteraction (MapCell targetTile)
	{
		SceneManager.LoadScene ("Main");
	}
}
