VAR asked_fb_who = false
VAR asked_fb_services = false
VAR asked_fb_help = false
VAR asked_fb_stats = false
VAR has_foodbank_flyer = false

Food Bank Worker: Hello, welcome to the community food bank. How can I help you today?
*[Hello]
    -> Hello

==Hello==
Player: Hi. I’m trying to learn more about what this place does for my residency process at the Stonewake Homeless Shelter.

Food Bank Worker: I’m glad you’re here. Food banks help provide groceries and meals to individuals and families facing food insecurity. Feel free to ask anything you’d like to know.

-> foodbank_questions


=== foodbank_questions ===

{asked_fb_who && asked_fb_services && asked_fb_help && asked_fb_stats:
    -> foodbank_complete
}

Food Bank Worker: What else would you like to know?

* {not asked_fb_who} [Who are you?]
    -> fb_who

* {not asked_fb_services} [What services does this place provide?]
    -> fb_services

* {not asked_fb_help} [How can someone help here?]
    -> fb_help

* {not asked_fb_stats} [Does a food bank like this make a difference?]
    -> fb_stats


=== fb_who ===
~ asked_fb_who = true

Player: Who are you?

Food Bank Worker: I’m one of the coordinators here. We organize food donations, manage volunteers, and distribute groceries to community members.

Food Bank Worker: Most food banks rely heavily on volunteers and partnerships with local stores, farms, and community groups.

-> foodbank_questions


=== fb_services ===
~ asked_fb_services = true

Player: What services does this place provide?

Food Bank Worker: We distribute free groceries, fresh produce when available, and nonperishable food items.

Food Bank Worker: Some food banks also offer nutrition education, assistance applying for food benefits, and referrals to shelters or housing services.

Food Bank Worker: For many families, this support helps stretch limited budgets and prevents hunger.

-> foodbank_questions


=== fb_help ===
~ asked_fb_help = true

Player: How can someone help here?

Food Bank Worker: People can donate food, volunteer their time sorting and distributing items, or organize community food drives.

Food Bank Worker: I always encourage people to search for local food banks in their area and see what their specific needs are.

Food Bank Worker: Even small efforts — a few canned goods or a few hours of volunteering — can make a meaningful difference.

-> foodbank_questions


=== fb_stats ===
~ asked_fb_stats = true

Player: Does a food bank like this make a difference?

Food Bank Worker: Yes. Nationwide, food banks and partner agencies help provide billions of meals every year.

Food Bank Worker: Tens of millions of people experience food insecurity annually, including children and seniors.

Food Bank Worker: Food banks help bridge that gap and often serve as a first connection point to other support services.

-> foodbank_questions


=== foodbank_complete ===

Food Bank Worker: I’m glad you took the time to understand what we do.

Food Bank Worker: Here, have a can of food to take back to the homeless shelter. It shows that you visited and learned show we operate and how we partner with other community resources.

# give_item:Canned-Food
# unlock:npc8

*[Thank you]
    -> Next

==Next==
Player: Thank you.

Food Bank Worker: You’re welcome. Access to food is one of the foundations of stability.

-> END
