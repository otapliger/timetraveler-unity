using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
	bool isPaused = false;

	void Update()
	{
		if (Input.GetButtonDown("PauseTime"))
		{
			isPaused = !isPaused;
		}

		if (isPaused)
		{
			if (Input.GetButton("ForwardPause"))
			{
				GetComponent<Text>().text = "Status: forward";
			}

			else if (Input.GetButton("RewindPause"))
			{
				GetComponent<Text>().text = "Status: rewind";
			}

			else
			{
				GetComponent<Text>().text = "Status: pause";
			}
		}

		else
		{
			if (Input.GetButton("RewindTime"))
			{
				GetComponent<Text>().text = "Status: rewind";
			}

			else
			{
				GetComponent<Text>().text = "Status: play";
			}
		}
	}
}
