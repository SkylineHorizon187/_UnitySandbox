using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class NetLobbyHook : LobbyHook 
{
	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		TankInit tank = gamePlayer.GetComponent<TankInit>();
		 
		tank.playerName = lobby.playerName;
		tank.playerColor = lobby.playerColor;
	}
}

