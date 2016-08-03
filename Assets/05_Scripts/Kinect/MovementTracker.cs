using UnityEngine;
using System.Collections.Generic;
using Windows.Kinect;

public class MovementTracker : MonoBehaviour
{

	public Transform movementObject1;
	public Transform movementObject2;
	public Transform movementObject3;
	public Windows.Kinect.JointType partToTrack;
	public float posFactor = 2;
	public float lowPassFactor = 0.1F;
	private Vector3 p1Pos;
	private Vector3 p2Pos;
	private Vector3 p3Pos;
	BodySourceManager bodySourceManager;
	private Dictionary<ulong, int> _players;
	private const int MAX_PLAYERS = 6;
	// Use this for initialization
	void Start()
	{

		bodySourceManager = GetComponent<BodySourceManager>();
		_players = new Dictionary<ulong, int>();
		p1Pos = Vector3.zero;
		p2Pos = Vector3.zero;
		p3Pos = Vector3.zero;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (bodySourceManager == null)
		{
			return;
		}

		Windows.Kinect.Body[] data = bodySourceManager.GetData();
		if (data == null)
		{
			return;
		}

		RegisterBodies(data);

		int counter = 0;
		int value;
		foreach (var body in data)
		{
			value = 0;
			counter++;
			if (body == null)
			{
				continue;
			}

			//   Debug.Log(counter);

			if (body.IsTracked)
			{

				// Debug.Log(data);
				// Debug.Log(body.TrackingId);
				//  Debug.Log(body);

				if (!_players.TryGetValue(body.TrackingId, out value))
				{
					//  Debug.Log("not found");
					return;
				}

				Debug.Log("player "+value+" is detected");
				var pos = body.Joints[partToTrack].Position;


				//decide which object will use it
				//cases from 1 to 6
				switch (value)
				{
				case 1:
					{
						CalcLowPassValues(new Vector3(pos.X, pos.Y, -pos.Z), ref p1Pos);
						movementObject1.position = p1Pos * posFactor;
						   Debug.Log("Player " + value + " moved");
						break;
					}

				case 2:
					{
						CalcLowPassValues(new Vector3(pos.X, pos.Y, -pos.Z), ref p2Pos);
						movementObject2.position = p2Pos * posFactor;
						   Debug.Log("Player " + value + " moved");
						break;
					}

				case 3:
					{
						CalcLowPassValues(new Vector3(pos.X, pos.Y, -pos.Z), ref p3Pos);
						movementObject3.position = p3Pos * posFactor;
						    Debug.Log("Player " + value + " moved");
						break;
					}
				}



			}

		}


	}

	/// <summary>
	/// Used to pair body ids with with player numbers from 1 to 6, incrementally
	/// </summary>
	/// <param name="bodyData"></param>
	void RegisterBodies(Body[] bodyData)
	{
		int i;//j;
		List<ulong> overflowPlayers;
		// List<ulong> missingPlayers;
		// List <int> freePlayers;

		bool overflow = false;

		if (_players.Count == 0)
		{
			for (i = 0; i < bodyData.Length; i++)
			{
				if (bodyData[i] != null && !_players.ContainsKey(bodyData[i].TrackingId))
				{
					_players.Add(bodyData[i].TrackingId, _players.Count);
				}

			}
			//after the first init we exit
			return;
		}
		else if (_players.Count > 0)
		{

			RefreshPlayers(bodyData);

			overflowPlayers = new List<ulong>(0);

			for (i = 0; i < bodyData.Length; i++)
			{
				if (bodyData[i] != null)
				{
					if (!_players.ContainsKey(bodyData[i].TrackingId) && _players.Count < MAX_PLAYERS)
					{
						_players.Add(bodyData[i].TrackingId, _players.Count);
						// add bodies to max , does not account for either filtering or players leaving
					}
					else if(!_players.ContainsKey(bodyData[i].TrackingId) && _players.Count == MAX_PLAYERS)
					{
						overflowPlayers.Add(bodyData[i].TrackingId);
						overflow = true;
					}

				}

			}

			if (overflow)
			{

				RefreshPlayers(bodyData, overflowPlayers);
				/*
                        freePlayers = new List<int>(0);
                        missingPlayers = new List<ulong>(0);
                        // find the missing players
                        foreach (KeyValuePair<ulong, int> pair in _players)
                        { 
                            for (j = 0; j < bodyData.Length; j++)
                            {
                               if(pair.Key == bodyData[j].TrackingId)
                                {
                                    break;
                                }

                               if(j == bodyData.Length-1)
                                {
                                    missingPlayers.Add(pair.Key);
                                }
                            }

                        }

                        //remove missing players
                        for(i = 0; i < missingPlayers.Count; i++)
                        {
                            int val = 0;
                            if (!_players.TryGetValue(missingPlayers[i], out val))
                            {
                                Debug.Log("Player error, player does not exist");
                                return;
                            }
                            else
                            {
                                freePlayers.Add(val);
                                _players.Remove(missingPlayers[i]);
                            }


                        }

                        freePlayers.Sort();

                        //add new players
                        for(i = 0; i < freePlayers.Count; i++)
                        {
                            _players.Add(overflowPlayers[i], freePlayers[i]);
                        }
         */
			}


		}
	}

	/// <summary>
	/// Check for missing players
	/// </summary>
	/// <param name="bodyData"></param>
	void RefreshPlayers(Body[] bodyData)
	{
		int i, j;
		List<int> freePlayers = new List<int>(0);
		List<ulong> missingPlayers = new List<ulong>(0);

		// find the missing players
		foreach (KeyValuePair<ulong, int> pair in _players)
		{
			for (j = 0; j < bodyData.Length; j++)
			{
				if (pair.Key == bodyData[j].TrackingId)
				{
					break;
				}

				if (j == bodyData.Length - 1)
				{
					missingPlayers.Add(pair.Key);
				}
			}

		}

		//remove missing players
		for (i = 0; i < missingPlayers.Count; i++)
		{
			int val = 0;
			if (!_players.TryGetValue(missingPlayers[i], out val))
			{
				return;
			}
			else
			{
				freePlayers.Add(val);
				_players.Remove(missingPlayers[i]);
			}


		}

		freePlayers.Sort();

		//add new players
		for (i = 0; i < freePlayers.Count; i++)
		{
			for (j = 0; j < bodyData.Length; j++)
			{
				if (!_players.ContainsKey(bodyData[j].TrackingId))
				{
					_players.Add(bodyData[j].TrackingId, freePlayers[i]);
					break;
				}
			}
		}
	}

	/// <summary>
	/// Check for missing/new players while max players
	/// </summary>
	/// <param name="bodyData"></param>
	/// <param name="newPlayers"></param>
	void RefreshPlayers(Body[] bodyData, List<ulong> newPlayers)
	{
		int i, j;
		List<int> freePlayers = new List<int>(0);
		List<ulong> missingPlayers = new List<ulong>(0);

		// find the missing players
		foreach (KeyValuePair<ulong, int> pair in _players)
		{
			for (j = 0; j < bodyData.Length; j++)
			{
				if (pair.Key == bodyData[j].TrackingId)
				{
					break;
				}

				if (j == bodyData.Length - 1)
				{
					missingPlayers.Add(pair.Key);
				}
			}

		}

		//remove missing players
		for (i = 0; i < missingPlayers.Count; i++)
		{
			int val = 0;
			if (!_players.TryGetValue(missingPlayers[i], out val))
			{
				return;
			}
			else
			{
				freePlayers.Add(val);
				_players.Remove(missingPlayers[i]);
			}


		}

		freePlayers.Sort();

		//add new players
		for (i = 0; i < freePlayers.Count; i++)
		{
			_players.Add(newPlayers[i], freePlayers[i]);
		}
	}

	void CalcLowPassValues(Vector3 newPos, ref Vector3 resultPos)
	{
		resultPos = Vector3.Lerp(resultPos, newPos, lowPassFactor);
	}

}