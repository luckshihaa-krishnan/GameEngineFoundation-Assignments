## Sprite Game


### Objective

The objective of this task is to create a simple Sprite Game using C# and OpenTK. 
This is not a complete game, but is a start to build a game using Sprites. In this 
project, it will show a character and that character will have some actions to do 
such as running, attacking, etc. I first cloned this repository from my professor 
(https://github.com/mouraleonardo/SpriteGameOpenTk), then upgraded the code so that 
it uses new sprite character and animations.


### What new movements you implemented

The character being used in this game is a soldier, which contains movements such as 
walking, running, shooting and attacking. I first got the sprite game character from 
this website (https://craftpix.net/categorys/sprites/) which contains many sprite 
characters. There are four .png images that represent each movement of the character.


### How your state machine works

There are a couple of states the character has such as walking, running, shooting, 
attacking and idle, which is the default state of when the character is not doing 
an action. When the application has started to run, the character is in idle state, 
facing the right side. To make the character walk across the frame, you can use the 
'right arrow' key to walk in the right direction and the 'left arrow' key to walk in 
the left direction. To make the character run across the frame, you would need to hold 
down the 'shift' key (either left or right shift key on your keyboard) and also use the
'right arrow' key or 'left arrow' key to run to the direction you want the character to 
run to. To make the character shoot, you would need to press and hold on the key 'S' 
to shoot. The shooting action is longer as it needs to display the position/stance of 
shooting and then display the shot color, so we need to press the key 'S' for a longer 
time so we can view the entire shooting action. If we press the key 'S' for a short time, 
we will not see the full shooting action. To make the character attack, you would need to 
press and hold on the key 'A' to attack. This is also the same as the shooting action, as
we need to press the key 'A' for longer time to view the attack action because otherwise, 
we will not see the full attack motion.


### Any challenges faced and how you solved them.

The first challenge I faced when starting this project is finding a good sprite image. 
I first went on Google and searched up 'sprite game', and there were some good images I 
can use for this project as it contains actions such as running, punching, etc. When I 
used one of those images and implemented one action, it did not come out as expected. 
I was able to see the background of the image, which was a white and grey pixel background. 
The action worked fine, but with the background being visible, it does not look professional 
and not a good start to a sprite game. I then decided to find some websites that might have 
better sprite images I can use, which I found this: https://craftpix.net/categorys/sprites/. 
After a successful download of the sprite image, I was able to successfully move the character 
around the frame with more adjustments.

Another challenge I faced was that after implementing the walk and run actions and when testing 
it out, I noticed that the character goes beyond the frame, which is not user friendly. 
The character would need to stop at the end of the frame so that the character is always 
visible by users. I resolved this issue by using the method 'Clamp'.
This is the line I added: _position.X = MathHelper.Clamp(_position.X, 100 - 100, Size.X - 100 + 60); 
This line of code clamps the character's position, which restricts them from leaving the frame.


### Resources
Professor's sample code I cloned: https://github.com/mouraleonardo/SpriteGameOpenTk   
Sprite image I used: https://craftpix.net/categorys/sprites/
