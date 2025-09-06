#!/usr/bin/env bash
set -euo pipefail

# Usage:
#   ./scripts/bump_version.sh prealpha --print-only
#   ./scripts/bump_version.sh prerc    --print-only
#   ./scripts/bump_version.sh finalize --print-only
#
# Prints the NEXT version (no leading 'v'). Does NOT modify files when --print-only is used.

VERSION_FILE="VERSION"

read_version() { [[ -f $VERSION_FILE ]] && tr -d '\n' < "$VERSION_FILE" || echo "0.1.0"; }

parse() {
  local v="$1"
  BASE="${v%%-*}"
  PRE="${v#"$BASE"}"
  PRE="${PRE#-}"     # "" or like alpha.3
}

bump_base() {
  local base="$1" part="$2"
  IFS='.' read -r MA MI PA <<<"$base"
  case "$part" in
    major) ((MA+=1)); MI=0; PA=0;;
    minor) ((MI+=1)); PA=0;;
    patch) ((PA+=1));;
    *) echo "bad part" >&2; exit 1;;
  esac
  echo "${MA}.${MI}.${PA}"
}

next_pre() {
  local base="$1" pre="$2" preid="$3"
  if [[ -n "$pre" ]]; then
    local pid="${pre%%.*}" num="${pre##*.}"
    if [[ "$pid" == "$preid" && "$num" =~ ^[0-9]+$ ]]; then
      echo "${base}-${preid}.$((num+1))"; return
    fi
  fi
  echo "${base}-${preid}.1"
}

replace_pre() {
  local base="$1" preid="$2"
  echo "${base}-${preid}.1"
}

LEVEL="${1:?level required: prealpha|prerc|finalize|patch|minor|major}"
MODE="${2:-}"  # allow --print-only

CUR="$(read_version)"; parse "$CUR"

case "$LEVEL" in
  prealpha)
    if [[ -z "$PRE" ]]; then
      BASE="$(bump_base "$BASE" patch)"
      NEXT="$(next_pre "$BASE" "" "alpha")"
    else
      NEXT="$(next_pre "$BASE" "$PRE" "alpha")"
    fi
    ;;
  prerc)
    NEXT="$(replace_pre "$BASE" "rc")"
    ;;
  finalize)
    NEXT="$BASE"
    ;;
  patch|minor|major)
    NEXT="$(bump_base "$BASE" "$LEVEL")"
    ;;
  *) echo "unknown level $LEVEL"; exit 1;;
esac

# Only print — hook will write VERSION and commit, if needed.
if [[ "$MODE" == "--print-only" ]]; then
  echo "$NEXT"
else
  echo "$NEXT" > "$VERSION_FILE"
fi

