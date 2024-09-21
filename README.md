# Unity Support - Generic Types
This package contains extra generic types, mainly intended for authoring and standard games.

# Install

This package is not available in any UPM server. You must install it in your project like this:

1. In Unity, with your project open, open the Package Manager.
2. Either refer this Github project: https://github.com/AlephVault/unity-support-generic.git or clone it locally and refer it from disk.
3. Also, the following packages are dependencies you need to install accordingly (in the same way and also ensuring all the recursive dependencies are satisfied):

   - https://github.com/AlephVault/unity-support.git

# Usage

This section describes the features offered by this Support (Generic) package.

## Behaviours

All these classes are accessible at namespace: `AlephVault.Unity.Support.Generic.Authoring.Behaviours`.

### `SingletonBehaviour`

This is an **abstract** class. Inherit from it to create a Behaviour subclass that can be instantiated only once across your entire game or application.

```
public class MyBehaviour : AlephVault.Unity.Support.Generic.Authoring.Behaviours.SingletonBehaviour<MyBehaviour> {
    ...
    protected override void Awake()
    {
        base.Awake();
        // ... your logic here ...
    }
    ...
}
```

Your `MyBehaviour` class will have members:

- `public static MyBehaviour Instance`: Returns the only instance of `MyBehaviour`. Returns `null` if there's no current instance. Only one of these behaviours can exist at a given time, and is forgotten / released when the behaviour component is destroyed.

## Component-related Types

All these classes are accessible at namespace: `AlephVault.Unity.Support.Generic.Authoring.Types`.

### `Dictionary<K, V>`

This class behaves exactly as the `System.Collections.Generic.Dictionary<K, V>` class, except that sub-classes of this class will have their public / serialized fields of that type accessible through the Unity Editor for a given object. But, otherwise, they're used exactly as the standard Dictionary class in all their scope.

One example of usage is:

```
using System;
using UnityEngine;
using AlephVault.Unity.Support.Generic.Authoring.Types;

// First, define the type and tag it as Serializable.
[Serializable]
public class MyDictType : Dictionary<K, V> {}

// Then, define the Unity Object type making use of it.
public class MyBehaviour : MonoBehaviour {
    public MyDictType MyDict;
}
```

Provided that `K` and `V` types are primitive or serializable, your variables of type `MyDictType` will also be serializable.

### `Interfaced<T>`

This class can be used to declare component (MonoBehaviour / ScriptableObject) members that are both:

1. Intended to be stored / serialized.
2. Of a type being an `interface`, not a Behaviour, Scriptable Object or other type of Unity's `Object`.

The first thing to do is to declare the subtype like this:

```
using System;
using UnityEngine;
using AlephVault.Unity.Support.Generic.Authoring.Types;

// The interface type.
public interface ISomeInterface {
    ... whatever body you want it to have ...
}

// The subclass of Interfaced<T> for it. It MUST have
// the Serializable tag.
[Serializable]
public class MySomeInterfaceRef : Interfaced<ISomeInterface> {}

// Finally, the Unity Object type making use of it.
public class MyBehaviour : MonoBehaviour {
	// Then, your variable.
	public MySomeInterfaceRef Ref;
}
```

Then, when accessing the `Ref` property from a `MySomeInterfaceRef` behaviour, you just do `Ref.Result` to set or retrieve the underlying `ISomeInterface` object.

Also, when editing your `MyBehaviour` object in the inspector, you'll have a field that, on click, allows you to select one of your in-scene / in-assets objects that implement `ISomeInterface` or have components that implement `ISomeInterface`.

### `InterfacedList<V, IType>`

Related to the `Interfaced<IType` type defined previously, this does not hold a single reference but a **list** of references.

In order to make use of it you need:

1. To define your `ISomeInterface` interface.
2. To define your `MySomeInterfaceRef` property type.
3. To define your `MySomeInterfaceRefList` property type, inheriting `InterfacedList<MySomeInterfaceRef, ISomeInterface>`.

An example would be:

```
using System;
using UnityEngine;
using AlephVault.Unity.Support.Generic.Authoring.Types;

// The interface type.
public interface ISomeInterface {
    ... whatever body you want it to have ...
}

// The subclass of Interfaced<T> for it. It MUST have
// the Serializable tag.
[Serializable]
public class MySomeInterfaceRef : Interfaced<ISomeInterface> {}

// The subclass of InterfacedList<V, T> for it. It MUST have
// the Serializable tag.
[Serializable]
public class MySomeInterfaceRefList : InterfacedList<MySomeInterfaceRef, ISomeInterface> {}

// Finally, the Unity Object type making use of it.
public class MyBehaviour : MonoBehaviour {
	// Then, your variable.
	public MySomeInterfaceRefList RefList;
}
```

Like before, this class will have the following characteristics:

1. The `RefList` variable will be visible in the editor, and rendered as a list of objects that can each be picked from scene or assets
2. The `RefList` will work as a list of `ISomeInterface` objects (notice that it's not of `MySomeInterfaceRef` but instead directly of the interface type itself).

### `OrderedSet<T>`

This works like a generic `ICollection<T>` (with the same criteria of `K` type of `Dictionary<K, V>` types) that can add and remove objects, always being unique. The difference is that, on top of `ICollection<T>`, it has extra members:

- `public T First`: Returns the first element in the set.
- `public T Last`: Returns the last element in the set.
- `public T Shift()`: Extracts and returns the first element in the set.
- `public T Pop()`: Extracts and returns the last element in the set.

The other characteristic is that this class is also serializable, so you are able to edit its contents (as a list of elements) in the editor, provided the element type `T` itself is primitive or also serializable.

The example code is:

```
using System;
using UnityEngine;
using AlephVault.Unity.Support.Generic.Authoring.Types;

// First, define the type and tag it as Serializable.
[Serializable]
public class MySetType : OrderedSet<K> {}

// Then, define the Unity Object type making use of it.
public class MyBehaviour : MonoBehaviour {
    public MySetType MySet;
}
```

## General-Purpose Types

All these classes are accessible at namespace: `AlephVault.Unity.Support.Generic.Types`.

### `CollectionExtensions`

This is an extension over several collection types. Right now it adds some methods to the `IDictionary<K, V>` implementing classes. Two methods are added:

- `SetDefault(K key, V @default)`: If there's a value at the given `key`, it returns that value. Otherwise, it inserts the value `@default` at the key `key` and then returns this new value.
- `SetDefault(K key, Func<V> @default)`: The idea is the same, but the default value will be retrieved by invoking the passed callback instead.

_Future versions of this method might be added for asynchronous `Task<V>`-returning implementations as well, but that's currently not supported for now._

### `Sampling.Cyclic<T>`

This is a sampler class that takes a non-empty, non-null list of elements of type `T`. When you want to retrieve any of the elements, you'll get the next element of the provided list each time (when you retrieved the last one, the cycle resets to the beginning).

Example code:

```
using UnityEngine;
using AlephVault.Unity.Support.Generic.Types.Sampling;

var cycle = new Cyclic<string>(new string[]{"foo", "bar", "baz"});
for(int idx = 0; idx < 0; idx++) {
    Debug.Log(cycle.Get());
}

// Prints lines being: foo, bar, baz, foo, bar, baz, foo, bar.
```

### `Sampling.Random<T>`

This is a sampler class that takes a non-empty, non-null list of elements of type `T`. When you want to retrieve any of the elements, you'll get a random element.

Example code:

```
using UnityEngine;
using AlephVault.Unity.Support.Generic.Types.Sampling;

var cycle = new Random<string>(new string[]{"foo", "bar", "baz"});
Debug.Log(cycle.Get()); // Prints randomly either foo, bar or baz.
```
