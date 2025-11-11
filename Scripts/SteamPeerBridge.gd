extends Node
var peer: MultiplayerPeer

func create_host(virtual_port:int = 0):
	peer = SteamMultiplayerPeer.new()
	peer.create_host(virtual_port)
	multiplayer.multiplayer_peer = peer

func create_client(host_steam_id:int, virtual_port:int = 0):
	peer = SteamMultiplayerPeer.new()
	peer.create_client(host_steam_id, virtual_port)
	multiplayer.multiplayer_peer = peer
