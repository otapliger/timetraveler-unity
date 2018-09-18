using UnityEngine;
using System.Collections.Generic;
using System;

public struct TimeFrame
{
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
	public Vector3 velocity;
	public Vector3 angularVelocity;
}

public class TimeTraveller : MonoBehaviour
{
	Rigidbody rb;

	int lastFrame;
	int currentFrame;
	bool isPaused = false;
	bool hasRigidbody = false;

	List<TimeFrame> timeFrames = new List<TimeFrame>();

	void Awake()
	{
		// Check if every child has the TimeTraveller component and add it if they don't
		foreach (Transform child in transform)
		{
			if (child.GetComponent<TimeTraveller>() == null)
			{
				child.gameObject.AddComponent<TimeTraveller>();
			}
		}

		// Check if the game object has a Rigidbody component and set true if it has
		rb = GetComponent<Rigidbody>();

		if (rb != null)
		{
			hasRigidbody = true;
		}
	}

	void Update()
	{
		if (Input.GetButtonDown("PauseTime"))
		{
			Pause();
		}
	}

	void FixedUpdate()
	{
		// Game is not paused
		if (!isPaused)
		{
			// Rewind: remove frames from the list
			if (timeFrames.Count >= 1 && Input.GetButton("RewindTime"))
			{
				TimeMachine.Instance.isRewinding = true;

				if (timeFrames.Count > 1)
				{
					timeFrames.RemoveAt(timeFrames.Count - 1);
				}

				transform.position = timeFrames[timeFrames.Count - 1].position;
				transform.rotation = timeFrames[timeFrames.Count - 1].rotation;
				transform.localScale = timeFrames[timeFrames.Count - 1].scale;

				if (hasRigidbody)
				{
					rb.velocity = timeFrames[timeFrames.Count - 1].velocity;
					rb.angularVelocity = timeFrames[timeFrames.Count - 1].angularVelocity;
				}
			}

			// Play: add frames to the list
			else
			{
				TimeMachine.Instance.isRewinding = false;

				var now = new TimeFrame();
				now.position = transform.position;
				now.rotation = transform.rotation;
				now.scale = transform.localScale;

				if (hasRigidbody)
				{
					now.velocity = rb.velocity;
					now.angularVelocity = rb.angularVelocity;
				}

				timeFrames.Add(now);

				// Remove old frames from list if it exceeds rewindTimeLimit
				if (TimeMachine.Instance.rewindTimeLimit != 0)
				{
					var fps = 1.0 / Time.deltaTime;

					if (timeFrames.Count / fps > TimeMachine.Instance.rewindTimeLimit)
					{
						int framesToRemove = (int) Math.Round(timeFrames.Count - TimeMachine.Instance.rewindTimeLimit * fps);
						timeFrames.RemoveRange(0, framesToRemove);
					}
				}
			}
		}

		// Game is paused
		else
		{
			// Ensure that rigidbody will not interfere 
			if (hasRigidbody)
			{
				rb.useGravity = false;
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
			}

			if (Input.GetButton("ForwardPause") && currentFrame < timeFrames.Count - 1)
			{
				currentFrame++;

				transform.position = timeFrames[currentFrame].position;
				transform.rotation = timeFrames[currentFrame].rotation;
				transform.localScale = timeFrames[currentFrame].scale;
			}

			else if (Input.GetButton("RewindPause") && currentFrame > 0)
			{
				currentFrame--;

				transform.position = timeFrames[currentFrame].position;
				transform.rotation = timeFrames[currentFrame].rotation;
				transform.localScale = timeFrames[currentFrame].scale;
			}
		}
	}

	public void Pause()
	{
		// Reverse the state
		isPaused = !isPaused;
		TimeMachine.Instance.isPaused = isPaused;

		// Every time the game is paused set the current frame
		if (isPaused)
		{
			currentFrame = lastFrame = timeFrames.Count - 1;
		}

		else
		{
			// Clear the list from the past future
			if (currentFrame < lastFrame)
			{
				if (currentFrame == 0)
				{
					timeFrames.Clear();
				}

				else
				{
					timeFrames.RemoveRange(currentFrame, lastFrame - currentFrame + 1);
				}
			}

			if (hasRigidbody)
			{
				if (currentFrame != 0)
				{
					rb.angularVelocity = timeFrames[timeFrames.Count - 1].angularVelocity;
					rb.velocity = timeFrames[timeFrames.Count - 1].velocity;
				}

				rb.useGravity = true;
			}
		}
	}
}
