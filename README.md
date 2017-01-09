# CharacterControllerCollisionFix

This is my attempt to fix the issue with Unity's Character Controller object wherein it pops into the air 
whenever it comes in contact with a rigid body.

My solution is very rough as I'm a novice coder and am lost in the world of Vector math 0_0.

This is the default Character Controller behavior with dynamic colliders.

![alt tag](https://zippy.gfycat.com/FabulousPettyBasenji.gif)

Upon scouring the interwebs the only ray of hope I found was this article https://mariusgames.com/3d-character-controller-db7cd3a7b4df#.l7bmb9rr8

I tried to reproduce his work in my solution, to get the closest point on the colliders I used code from the SuperCollider class found here https://github.com/IronWarrior/SuperCharacterController

Here is how the Character Controller behaves with the Collison Fixer script
![alt tag](https://zippy.gfycat.com/AdmiredQuestionableGerenuk.gif)

Its rough but it works- if you can improve it- please do! Just add the script to the Character Controller Game Object.
![alt tag](http://i.imgur.com/QeCEyAc.png)

I've included the Unity test project which was made in 5.5- I tested with different types of moving objects.
(Animated, Physics, Translate)

The script alone is below as well. If you can improve upon it please do and send me a copy! ^_^
I'm @mdotstrange on twitter or mdotstrange@gmail.com  Thanks!
