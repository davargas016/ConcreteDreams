VAR asked_hc_who = false
VAR asked_hc_services = false
VAR asked_hc_help = false
VAR asked_hc_stats = false
VAR has_health_flyer = false

Health Center Worker: Hello, welcome to the community health center. How can I help you today?
*[Hello]
    -> Hello

==Hello==
Player: Hi. I’m trying to learn more about what this place does for my residency process at the Stonewake Homeless Shelter.

Health Center Worker: Great! I’m glad you’re here today then. Community health centers provide care regardless of someone’s ability to pay. Feel free to ask anything you’d like to know.

-> health_center_questions


=== health_center_questions ===

{asked_hc_who && asked_hc_services && asked_hc_help && asked_hc_stats:
    -> health_center_complete
}

Health Center Worker: What else would you like to know?

* {not asked_hc_who} [Who are you?]
    -> hc_who

* {not asked_hc_services} [What services does this place provide?]
    -> hc_services

* {not asked_hc_help} [How can someone help here?]
    -> hc_help

* {not asked_hc_stats} [Does a health center like this make a difference?]
    -> hc_stats


=== hc_who ===
~ asked_hc_who = true

Player: Who are you?

Health Center Worker: I’m one of the healthcare providers here. We also have nurses, counselors, administrative staff, and volunteers. Community health centers rely on teams working together to provide accessible care.

-> health_center_questions


=== hc_services ===
~ asked_hc_services = true

Player: What services does this place provide?

Health Center Worker: We offer primary medical care, mental health services, preventative screenings, and sometimes dental care. We operate on a sliding fee scale, meaning cost is adjusted based on income.

Health Center Worker: We also connect patients to social services, housing referrals, and nutrition assistance when needed.

-> health_center_questions


=== hc_help ===
~ asked_hc_help = true

Player: How can someone help here?

Health Center Worker: Medical professionals can volunteer their services. Community members can support through donations or advocacy.

Health Center Worker: I encourage people to search for community health centers in their area and see what support they need.
Many rely on local partnerships and volunteers to stay accessible.

-> health_center_questions


=== hc_stats ===
~ asked_hc_stats = true

Player: Does a health center like this make a difference?

Health Center Worker: Yes. Community health centers serve over 30 million patients nationwide each year.

Health Center Worker: They significantly reduce emergency room visits by providing preventative and consistent care.

Health Center Worker: For individuals experiencing homelessness, accessible healthcare can be critical to long-term stability.

-> health_center_questions


=== health_center_complete ===

Health Center Worker: I’m glad you took the time to understand what we do.

Health Center Worker: Here take this souvenir pill back to the homeless shelter and turn it in to the rep and they will know you visited the health center and learned about our services.  

# give_item:Pill
# unlock:npc6

*[Thank you]
    -> Next

==Next==
Player: Thank you.

Health Center Worker: You’re welcome. Knowledge is a powerful first step.

-> END
