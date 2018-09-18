using UnityEngine;
using System.Collections.Generic;

public class TimeMachine : Singleton<TimeMachine>
{
	[HideInInspector]
	public bool isPaused = false;

	[HideInInspector]
	public bool isRewinding = false;

	[Space(4)]
	public int rewindTimeLimit = 10;

	[Space(4)]
	public List<string> tagsToRewind;

	void Awake()
	{
		var emptyTags = new List<string>();

		// Check the setted tags and clear the list from empty the ones (if any)
		foreach (var t in tagsToRewind)
		{
			if (t != "")
			{
				// Search for every game object with the setted tags and add the Time Traveller component to them
				foreach (var go in GameObject.FindGameObjectsWithTag(t))
				{
					go.AddComponent<TimeTraveller>();
				}
			}

			else
			{
				emptyTags.Add(t);
			}
		}

		foreach (var t in emptyTags)
		{
			tagsToRewind.Remove(t);
		}
	}
}
