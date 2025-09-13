#!/bin/sh
echo -ne '\033c\033]0;Godot-Peer-2-Peer-Steam-CSharp\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/BrezdenSteamGame.x86_64" "$@"

