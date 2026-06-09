#!/usr/bin/env bash
set -euo pipefail

# Packs the PowerCSharp Features packages into this repo's local-feed/ so the template can
# consume them via the "powercsharp-local" NuGet source (see NuGet.Config).
#
# Usage: scripts/pack-local-feed.sh [path-to-PowerCSharp-repo]
#   Defaults to a sibling checkout at ../PowerCSharp.

REPO_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SRC="${1:-$REPO_ROOT/../PowerCSharp}"
FEED="$REPO_ROOT/local-feed"

PROJECTS=(
  "src/Features/PowerCSharp.Features.Abstractions"
  "src/Features/PowerCSharp.Features"
  "src/Features/PowerCSharp.BuiltInFeatures"
  "src/Features/PowerCSharp.Feature.Cache"
  "src/Features/PowerCSharp.Feature.Cache.BitFaster"
)

if [ ! -d "$SRC" ]; then
  echo "PowerCSharp repo not found at: $SRC" >&2
  echo "Pass the path explicitly: scripts/pack-local-feed.sh /path/to/PowerCSharp" >&2
  exit 1
fi

mkdir -p "$FEED"
for p in "${PROJECTS[@]}"; do
  dotnet build "$SRC/$p" -c Release --nologo
done
cp "$SRC"/src/Features/*/bin/Release/*.nupkg "$FEED/"

echo "Packed $(ls -1 "$FEED"/*.nupkg | wc -l | tr -d ' ') packages into $FEED"
