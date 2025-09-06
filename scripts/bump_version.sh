#!/usr/bin/env bash
set -euo pipefail
VERSION_FILE="VERSION"

read_version() { [[ -f $VERSION_FILE ]] && cat "$VERSION_FILE" || echo "0.1.0"; }

write_version() {
  local v="$1"
  echo "$v" > "$VERSION_FILE"
  git add "$VERSION_FILE"
  git commit -m "chore(release): v${v}" -q || true
}

# Parse version into base + optional preid.N
parse() {
  local v="$1"
  BASE="${v%%-*}"
  PRE="${v#"$BASE"}"            # empty or like -alpha.3
  PRE="${PRE#-}"                # remove leading -
}

# Increment base x.y.z by part
bump_base() {
  local base="$1" part="$2"
  IFS='.' read -r MA MI PA <<<"$base"
  case "$part" in
    major) ((MA+=1)); MI=0; PA=0;;
    minor) ((MI+=1)); PA=0;;
    patch) ((PA+=1));;
    *) echo "bad part"; exit 1;;
  esac
  echo "${MA}.${MI}.${PA}"
}

# Next prerelease for a given preid (alpha|rc|beta)
next_pre() {
  local base="$1" pre="$2" preid="$3"
  if [[ -n "$pre" ]]; then
    # pre like alpha.3
    local pid="${pre%%.*}" num="${pre##*.}"
    if [[ "$pid" == "$preid" && "$num" =~ ^[0-9]+$ ]]; then
      echo "${base}-${preid}.$((num+1))"; return
    fi
  fi
  echo "${base}-${preid}.1"
}

# Replace any prerelease with a new one, preserving base
replace_pre() {
  local base="$1" preid="$2"
  echo "${base}-${preid}.1"
}

LEVEL="${1:?usage: bump_version.sh <prealpha|prerc|finalize|patch|minor|major>}"
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
    # move from alpha.* -> rc.1, or start rc.1 if there was no prerelease
    NEXT="$(replace_pre "$BASE" "rc")"
    ;;
  finalize)
    NEXT="$BASE"
    ;;
  patch|minor|major)
    NEXT="$(bump_base "$BASE" "$LEVEL")"
    ;;
  *)
    echo "unknown level $LEVEL"; exit 1;;
esac

write_version "$NEXT"
echo "v$NEXT"

