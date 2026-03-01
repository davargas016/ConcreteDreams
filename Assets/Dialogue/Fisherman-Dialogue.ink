VAR fish = false
VAR talked = false

Fisherman: Hello, may I help you?

* [Hello]
    -> Next
    
* [Just turning in fish]
    ->END

==Next==
Player: Hello sir, I was wondering where I can talk to the owner of this fish shop?

Fisherman: That's me, how can I help you?  Is there a specific fish you're looking for?

* [I'm here for a job]
    ->Next2

==Next2==
Player: No, I'm not here to buy any fish.  I am here to ask if you would be open to help.

Player: I can help go fishing for you and bring the catches here, in exhange for a small payment, if that's okay?

Fisherman: Hmmmm I'm not sure.  Have you ever gone fishing before?

* [Yes]
    -> Yes
    
* [No]
    -> No
    
    
==Yes==
~ fish = true
Player: Yea I know how to fish.

Fisherman: Hmmm... Well it would be nice to have an extra set of hands to help out since its just me fishing and my son Isaac working the counter...

Fisherman: But I don't know, I would prefer to hire someone the traditional way.

* [Explain Your Situation]
    -> Explanation


==No==
Player: No, I've never done it before.

Fisherman: I'm sorry then, if you don't know how to fish, I don't think I can hire you.

Fisherman: Even if I were to hire you, I wouldn't even have time to show you how to properly fish

* [Explain Your Situation]
    -> Explanation

==Explanation==
Player: Please sir, I need a job to get housing at a homeless shelter.  

Player: I've been homeless for a few years now and I'm close to getting housing for once.  They have a list of requirements for someone to get help there and having a source of income is one of the last ones.

{not fish:
Player: You won't need to teach me to fish, I'll learn on my own and figure it out so I can make sure I'm turning fish in to you regularly.
}

{fish: 
Player: I already know how to fish sir. I'll be able to start helping right away.  
}

Player: Also, I don't need an hourly wage.  You can pay me like contract work.  At the end of each day, you can just give me a small amount of money for the work I did that day.

Fisherman: Mmmm alright then. I'll use your help then. 

*[Continue]
    -> Continue
    
==Continue==

# give_item:Fishing-Rod

Fisherman: Here is a decent fishing rod that you can have.  If you make good use of it, I'm sure you'll make enough money to upgrade it one day.  

{not fish: 
Fisherman: I'll give you a quick rundown on how to use the fishing rod real quick.

* [Next]
    ->Next3
}

{fish:
Fisherman: I know you said you know how to fish already, but I'll still give you a refresher.

* [Next]
    ->Next3
}

==Next3==
Fisherman: First, you need to have bait in your inventory when you want to go fishing, if not, nothing will bite and so going out there will be a waste of time.

Fisherman: Next, you gotta go up to the water and when you get there, press F and wait for something to bite.

Fisherman: Sometimes, it can take a while to get a bite or it can take a second, just need to wait and find out. 

*[Next]
    ->Bite
    
==Bite==
Fisherman: Finally, when you feel something bite, press F again to reel it up and check your inventory to see what you caught.

*[Got it]
    -> Sell
    
==Sell==
Player: So where do I bring the fish that I catch?

Fisherman: You can sell them to Isaac.  He over at the counter for the shop and usually deals with the customers and stores the fish for me.  

Fisherman: However...

*[However what?]
    -> Quest
    
==Quest==
Fisherman: I will also give you a specific fish I want you to catch for me from time to time.  When you bring me a few of that specific fish, I'll reward you with more money than you would get by selling them to my son.  

Fisherman: Once you turn in one quest, I'll give you another.  Each time you turn in a type of fish, the next time I need that fish, I'll need more, but I'll give you more money in return.

*[Next]
    -> Amazed
    
==Amazed==
Player: Oh wow! That sounds like a great deal then.  Thank you for this opportunity and I'll get to work now then!

Fisherman: Great! Thanks for wanting to help and I hope this benefits the both of us!  Bye now!

*[Bye] Player: Bye now!
    ->END
