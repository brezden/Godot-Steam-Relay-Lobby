#!/usr/bin/env bash
set -euo pipefail

branch="$(git symbolic-ref --quiet --short HEAD || echo detached)"
read -rp "GitHub Issue # (optional): # " ISSUE
read -rp "MINOR (0.x.0): " MINOR
read -rp "PATCH  (0.0.x): " PATCH

[[ "$MINOR" =~ ^[0-9]+$ ]] || { echo "minor must be number"; exit 1; }
[[ "$PATCH" =~ ^[0-9]+$ ]] || { echo "patch must be number"; exit 1; }

VERSION="v0.${MINOR}.${PATCH}-${branch}"

read -rp "Subject: " SUBJECT

if [[ -n "$ISSUE" ]]; then
  MSG="${VERSION} (#${ISSUE}) - ${SUBJECT}"
else
  MSG="${VERSION} - ${SUBJECT}"
fi

# add your staged changes and commit
git commit -m "$MSG"
