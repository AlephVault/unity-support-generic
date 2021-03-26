# Unity Support - Generic Types
This support package contains generic classes, mostly intended for authoring side (i.e. editor and fields for conversion).

# Install
To install this package you need to open the package manager in your project and:

  1. Add a scoped registry with:
     - "name": "AlephVault"
     - "url": "https://packages.alephvault.com/artifactory/api/npm/unity"
     - "scopes": ["com.alephvault"]
  2. Look for this package: `com.alephvault.unity.support.generic`.
  3. Install it.

# Usage
Two classes, intended to be component fields, are supported (at namespace `AlephVault.Unity.Support.Generic.Authoring`):

  1. `OrderedSet<T>`: A set keeping track of the first insertion index.
  2. `Dictionary<K, V>`: A dictionary.

Both are supported both in the editor and runtime.
