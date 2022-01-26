# 01: Introduction to Unity Scripting

## Scripts

A **script** is a small collection of code that can be attached to a Game Object inside Unity to perform some functionality. Scripts can hold variables that can be changed in the editor, detect player input and react to it, use physics, change values over time, and much more.

Unity scripts are written in **C#**, an object-oriented programming language developed by Microsoft.

**Resource:** <https://docs.unity3d.com/Manual/ScriptingSection.html>

## Starter project

Download the **starter project**, unzip it, and open it inside Unity.

### Assets

The Assets folder contains all the resources for the project, including the models, textures, and music. This is also where we will store our game scripts.

- **Materials** - these are applied to models to give them their appearance. Materials are a combination of **textures** (bitmap images) and **shaders** (graphical scripts that contain calculations and algorithms for determining how to render the texture, based on lighting and other configurations).
- **Models** - the individual base models of the game objects.
- **Music** - the music in the game.
- **Prefabs** - these are complete Game Objects with models, child objects, starting values... you combine multiple elements into a reusable object and save it as a Prefab.
- **Scenes** - the entire environment of this part of the game. Unity works on the premise of **scenes**, which house all of the Game Objects for a logical sequence of the game (could be the entire game). For example, the title screen is likely a separate scene from the main game.
- **Scripts** - where we will store our C# scripts.
- **Sounds** - sound effect files.
- **Sprites** - 2D image files. In 3D games, typically reserved for UI graphical elements.
- **Textures** - a bitmap image that can be applied to models via **materials**.

### Scene view

Open the **Game** scene. You can see all the Game Objects currently in the scene.

In the **hierarchy tab** you can see all the same Game Objects (and a few others), organised into a tree-like hierarchy.

- **Music** - an audio source playing the background music loop.
- **Main Camera** - the camera that points down at the scene to render everything to the player. To see anything upon running the game, you must have a camera.
- **Directional Light** - a single light source that illuminates the scene. Scenes in Unity need some sort of light source to be able to see anything when playing the game.
- **Scenery** - an empty Game Object to hold the ground and the windmills. Acts like a 'folder'.
- **Hay Machine** - this is the blue machine sitting on the rails. It is comprised of a few Game Objects to make it easy to customise later on.

Press the **play button** to run the game. Press the button again to stop the game and return to the scene view.

### Creating a script

We will now create a script to rotate the windmill blades. Right-click the **scripts** folder and select **Create > C# Script**. Name the script **Rotate**.

In the **hierarchy tab**, expand Scenery and double-click the Windmill Game Object to open the Prefab editor. We are now editing the **Windmill prefab** which is a collection of Game Objects. Click on the **Wheel** Game Object. In the **inspector tab** click the **Add Component** button and start typing "Rotate"... when the script appears in the list, select it.

To edit the script, you can double-click the script field of the Wheel component (where it now says Rotate). The script should open in Visual Studio. It should look like this:

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    
    }
    
    //Update is called once per frame
    void Update()
    {
    
    }
}
```

This script is a **class** that derives from **MonoBehaviour**, Unity's base class for components. By default, Unity components have a `Start` and `Update` method. The `Start` method is called the first time the component is initialised, and you would use it to set initial values, initial state, etc... The `Update` method is called **every frame** of the game, and is used to perform actions or update values as the game is running.

Add the following code inside of the `Update` method:

```csharp
transform.Rotate(0, 50 * Time.deltaTime, 0);
```

### let

`let` is used to declare a **block-scoped** **local** variable.

```javascript
let x = 5

if (typeof x === 'number') { 
    let x = 10
    console.log(x) // 10
}

console.log(x) // 5
```

### const

`const` is used to declare a **block-scoped** **local** variable. It can not be reassigned nor be redeclared. **Note:** An **object** or **array** properties or elements can be changed.

The `const` example is much like the `let` example.

```javascript
const x = 5

if (typeof x === 'number') { 
    const x = 10
    console.log(x) // 10
}

console.log(x) // 5
```

Here are a few examples of errors commonly associated with `const`.

```javascript
const x 
x = 5 // SyntaxError: missing = in const declaration
```

```javascript
const x = 5 
x = 10 // TypeError: invalid assignment to const 'x'
```

```javascript
const x = 5 
const x = 10 // SyntaxError: redeclaration of const x
```

:question: **Interview Question:** What is the difference between **mutability** and **immutability**?

**Resources:**

- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/var>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/let>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/const>

## Control flow

### if...else...else if

Here is a skeleton example:

```javascript
if (condition) { 
    blockOne
} else {
    blockTwo
}
```

- `condition` - an expression that is either `true` or `false`.
- `blockOne` - execute the **block** if `condition` is `true`.
- `blockTwo` - execute the **block** if `condition` is `false`.

Here is a practical example:

```javascript
const x = 'Hello, World!'

if (typeof x === 'number') { 
    console.log(`${x} is a number`) 
} else {
    console.log(`${x} is not a number`)
}
```

If you want to have multiple **conditions**, you can use `else if`.

```javascript
if (conditionOne) {
    blockOne
} else if (conditionTwo) {
    blockTwo
} else {
    blockThree
}
```

### switch

```javascript
switch (expression) {
    // Execute blockOne when the result of an expression matches one
    case one:
        blockOne
    break

    // Execute blockTwo when the result of an expression matches two
    case two:
        blockTwo
    break
    
    // Execute blockThree when the result of an expression does not match one or two
    default:
        blockThree
    break
}
```

- `expression` - an expression whose result matches `case`.
- `case` - if `expression` matches `case`, execute the **block** associated to `case`.
- `default` - if `expression` does not match any **cases**, execute the **block** associated to `default`.

**Resources:**

- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/if...else>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/switch>

## Iterations

### for

A `for` loop consists of three **optional** expressions.

Here is a skeleton example:

```javascript
for (initialisation; condition; expression) {
    block
}
```

Here is a practical example:

```javascript
let x = ''

for (let i = 0; i <= 10; i++) {
    x += i
}

console.log(x) // 012345678910
```

### while

A `while` loop executes a **block** as long as the condition evaluates to `true`.

Here is a skeleton example:

```javascript
while (condition) {
    block
}
```

Here is a practical example:

```javascript
let x = 0

while (x < 10) {
  x++
}

console.log(x) // 10
```

### do...while

A `do...while` loop executes a **block** as long as the condition evaluates to `false`.

Here is a skeleton example:

```javascript
do {
    block
} while (condition)
```

Here is a practical example:

```javascript
let result = ''
let x = 0

do {
  x++
  result += x
} while (x < 5)

console.log(result) // 12345
```

Also, look at `for...of` and `for...in`.

**Resources:**

- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/for>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/while>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/do...while>

## Functions

There are two ways to write a **function** in **JavaScript**:

```javascript
// Default function
function x () { block }

// or

// Arrow function
const x = () => { block }
```

Here is a practical example:

```javascript
function convertFahToCel(x) {
  return (x - 32) * 5 / 9
}

console.log(convertFahToCel(5)) // -15
```

:question: **Interview Question:** Convert `convertFahToCel` above into an **arrow** **function**.

**Resources:**

- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/function>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/return>

## Arrays

An **array** is a list-like object that enables you to perform traversal and mutation operations. The length of an **array** is not fixed and can change at any time. It means elements can be stored at non-contiguous locations in an **array**.

Here is an example of how to create an **array**:

```javascript
let fruits = ['Apple', 'Banana']
```

Here is an example of how to access an **array**:

```javascript
let fruits = ['Apple', 'Banana']

// First element in fruits
console.log(fruits[0]) // Apple

// Last element in fruits
console.log(fruits[fruits.length - 1]) // Banana
```

**Note:** An **array** can not be indexed using a **string**. You must use an **integer**.

:question: **Interview Question:** What type of **array** allows you to index with a **string**?

Here is an example of how to mutate an **array**:

```javascript
let fruits = ['Apple', 'Banana']

fruits[0] = 'Grape'

console.log(fruits) // Array ["Grape", "Banana"]
```

**Note:** There are many ways to mutate an **array**. You will look other ways soon.

Here is an example of how to iterate over an **array**:

```javascript
const fruits = ['Apple', 'Banana']

// or 

fruits.forEach((el, index) => {
    console.log(`${el} => ${index}`) // Apple => 0
                                       // Banana => 1 
})

// or

for (let i = 0; i < fruits.length; i++) {
    console.log(`${fruits[i]} => ${i}`) // Apple => 0
                                        // Banana => 1 
}
```

As you can see, there are many ways to iterate over an **array**.

:question: **Interview Question:** What do the following **array** operations do?

- `push`
- `pop`
- `shift`
- `unshift`
- `indexOf`

**Resource:** <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array>

## Objects

An **object** is a collection of properties where each property is an association between a **key** and a **value**. The concept of an **object** can be understood with real-life objects, i.e., a classroom of students is an **array** of student **objects**.

Here is an example of how to create an **object**:

```javascript
const fruit = {
    name: 'Apple',
    color: 'Red'
}

// or

const fruit = new Object()
fruit.name = 'Apple'
fruit.color = 'Red'
```

I **strongly** recommend using the first example.

Here is an example of how to access an **object**:

```javascript
const fruit = {
    name: 'Apple',
    color: 'Red'
}

console.log(fruit.name) // Apple

// or 

console.log(fruit['name']) // Apple
```

**Note:** An **object** is sometimes called an **associative array**.

:question: **Interview Question:** Provide an example of mutating and iterating over an **object**.

**Resources:**

- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object>
- <https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Working_with_Objects>
  
## Formative assessment

You can find today's in-class activity [here](https://github.com/otago-polytechnic-bit-courses/ID607001-intro-app-dev-concepts/blob/master/in-class-activities/in-class-activity-es6-basics-1.pdf). Carefully read the **Code Review** section before you start coding.
