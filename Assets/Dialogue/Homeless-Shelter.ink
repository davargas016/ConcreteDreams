VAR asked_who = false
VAR asked_services = false
VAR asked_help = false
VAR asked_stats = false

Homeless Shelter Worker 1: Hi! welcome to the Stonewake City Homeless shelter, how can I help you?
*[Hello]
    -> Hello

==Hello==
Player: Hi… I’m trying to figure out how I can get help — maybe stay here at the shelter?

Homeless Shelter Worker 1: Of course. This place is here to help anyone who needs it. We do ask that anyone staying with us is willing to engage with the resources we provide and complete a set of requirements before staying here.
Homeless Shelter Worker 1: We use those same resources to help people move toward stability — not just a bed for the night.

*[Next]
    -> Next

==Next==
Player: What’s the first thing I should understand?

Homeless Shelter Worker 1: The shelter itself. We provide meals, safe sleeping space, case management, and connections to housing, healthcare, and job programs. If you’re staying here, you’ll learn how those systems work — because they’re meant to support you long-term.

-> shelter_questions


=== shelter_questions ===

{asked_who && asked_services && asked_help && asked_stats:
    -> shelter_end
}

Homeless Shelter Worker 1: What else would you like to know?

* {not asked_who} Who are you?
    -> ask_who

* {not asked_services} What services does this place offer?
    -> ask_services

* {not asked_help} How can someone help here?
    -> ask_help

* {not asked_stats} Do shelters really make a difference?
    -> ask_stats


=== ask_who ===
~ asked_who = true

Player: Who are you, exactly?

Homeless Shelter Worker 1: I’m a staff member here, but we also rely heavily on volunteers. Some of us coordinate intake and case management. Others help serve meals, organize donations, or assist with outreach.

Homeless Shelter Worker 1: Some of us are also representatives from other resources, and you'll get to meet them soon. 

Homeless Shelter Worker 1: It takes a lot of people to keep this place running.

-> shelter_questions


=== ask_services ===
~ asked_services = true

Player: What services does this place offer?

Homeless Shelter Worker 1: We provide: Emergency shelter and safe sleeping space, Hot meals and hygiene supplies, Case management and housing navigation, Referrals to healthcare and mental health services, Help applying for benefits and job programs

Homeless Shelter Worker 1: Our goal is stabilization — helping people take steps toward permanent housing and independence.

-> shelter_questions


=== ask_help ===
~ asked_help = true

Player: How can someone help here?

Homeless Shelter Worker 1: Volunteers can help with meals, organizing donations, mentoring, or administrative support. Some assist with employment workshops or housing application support.

Homeless Shelter Worker 1: I always encourage people to search for local shelters in their own communities and see how they can help. Every shelter has different needs — time, supplies, advocacy — and even small efforts make a difference.

-> shelter_questions


=== ask_stats ===
~ asked_stats = true

Player: Do shelters really make a difference?

Homeless Shelter Worker 1: Yes. On a single night in 2023, over 650,000 people in the United States were experiencing homelessness.

Homeless Shelter Worker 1: Hundreds of thousands rely on emergency and transitional shelters each year to access housing support, healthcare referrals, and employment assistance.

Homeless Shelter Worker 1: Shelters are often the first step toward permanent housing and long-term stability.

-> shelter_questions


=== shelter_end ===

*[Next]
    ->Next2

==Next2==
Homeless Shelter Worker 1: I’m glad you asked those questions. Understanding how this place works is the first step.

Homeless Shelter Worker 1: If you’re ready, we can begin the intake process.

*[I'm Ready]
    ->Ready

==Ready==
PLAYER: I’m ready.

Homeless Shelter Worker 1: There’s a few requirements as I mentioned before.

Homeless Shelter Worker 1: For each of them, you will need to talk to the representatives here at the shelter to learn more.

Homeless Shelter Worker 1: Essentially, we ask new residents to learn about the community resources we partner with.

Homeless Shelter Worker 1: You will prove that you visited each one by bringing an item from each resource and turn them in to the correct representative.  

*[Got it] PLAYER: Ok, that sounds easy.
    ->FinalReq
    
==FinalReq==
Homeless Shelter Worker 1: After completing those, you will speak to another representative for the Homeless Shelter and they will ask for proof that you some source of income or financial responsibility.

Homeless Shelter Worker 1: But take it one step at a time. The first place you’ll need to visit is a local soup kitchen.  You can speak to the representative that is in the bottom left of the room. 

Homeless Shelter Worker 1: Speak to them and learn more about what you need to do.  

# unlock:npc2

*[Got it]
    ->Bye
    
==Bye==
Player: Okay, I'll go right now and begin the process.  Thank you for your help!

*[End]
-> END