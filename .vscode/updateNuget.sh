#!/bin/bash
regex='PackageReference Include="([^"]*)" Version="([^"]*)"'
find . -name "*.*proj" | while read -r proj; do
    while read -r line; do
        if [[ $line =~ $regex ]]; then
            name="${BASH_REMATCH[1]}"
            version="${BASH_REMATCH[2]}"
            if [[ $version != *-* ]]; then
                dotnet add "$proj" package "$name"
            fi
        fi
    done <"$proj"
done
