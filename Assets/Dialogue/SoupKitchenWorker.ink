VAR asked_sk_who = false
VAR asked_sk_services = false
VAR asked_sk_help = false
VAR asked_sk_stats = false


Soup Kitchen Worker: Hello, welcome to the Stonewake City soup kitchen. We're glad you're here.
*[Hello]
    -> Hello

==Hello==
Player: Hi. I'm trying to get housing at the Stonewake City Homeless Shelter and they sent me here to begin the process of getting a room.  

Soup Kitchen Worker: That’s great to hear! They sent you hear to learn more about this resource. To begin we serve anyone who needs a meal, no questions asked. If you have specific questions you want to ask about what we do, feel free to ask.

-> soup_kitchen_questions


=== soup_kitchen_questions ===

{asked_sk_who && asked_sk_services && asked_sk_help && asked_sk_stats:
    -> soup_kitchen_complete
}

Soup Kitchen Worker: What else would you like to know?

* {not asked_sk_who} [Who are you?]
    -> sk_who

* {not asked_sk_services} [What services does this place provide?]
    -> sk_services

* {not asked_sk_help} [How can someone help here?]
    -> sk_help

* {not asked_sk_stats} [Does a place like this really make an impact?]
    -> sk_stats


=== sk_who ===
~ asked_sk_who = true

Player: Who are you?

Soup Kitchen Worker: I’m one of the coordinators here. I help organize volunteers, manage food donations, and make sure meals are prepared safely. Most of us are volunteers, though — this place runs on community support.

-> soup_kitchen_questions


=== sk_services ===
~ asked_sk_services = true

Player: What services does this place provide?

Soup Kitchen Worker: We provide hot meals, groceries when available, bottled water, and basic hygiene supplies. Sometimes we partner with shelters and outreach teams to connect people with housing, healthcare, or job programs.

Soup Kitchen Worker: For many people, a meal here is the first step toward connecting with other resources.

-> soup_kitchen_questions


=== sk_help ===
~ asked_sk_help = true

Player: How can someone help here?

Soup Kitchen Worker: People can volunteer to prepare or serve meals, organize donations, or help clean up.  Local businesses and community members donate food and supplies.

Soup Kitchen Worker: I always encourage people to search for soup kitchens or food banks in their own communities and see how they can help.  Even a few hours of time or a small donation can make a difference.

-> soup_kitchen_questions


=== sk_stats ===
~ asked_sk_stats = true

Player: Does a place like this really make an impact?

Soup Kitchen Worker: Absolutely. Feeding America reports that food banks and meal programs help provide billions of meals each year across the country.

Soup Kitchen Worker: Millions of households experience food insecurity annually, and community meal programs help bridge that gap.

Soup Kitchen Worker: For many individuals facing homelessness, soup kitchens are one of the most immediate and reliable sources of daily support.

-> soup_kitchen_questions


=== soup_kitchen_complete ===

Soup Kitchen Worker: I’m glad you took the time to learn about us.

Soup Kitchen Worker: Here — take one of our flyers. It explains our services and how we partner with other community organizations and shows that you visited us.

Soup Kitchen Worker: Also take this meal.  It's a sandwich so you have something to eat later today.

# give_item:Flyer
# give_item:Sandwich
# unlock:npc4
*[Thank you]
    -> Next

==Next==
Player: Thank you. I appreciate it.

Soup Kitchen Worker: You’re welcome. Stay safe — and come back anytime you need a meal.

-> END


// SOURCES FOR STATISTICS (not spoken in dialogue):
// Feeding America – Impact Statistics:
// https://www.feedingamerica.org/hunger-in-america
//
// USDA – Food Security in the U.S. Reports:
// https://www.ers.usda.gov/topics/food-nutrition-assistance/food-security-in-the-u-s/