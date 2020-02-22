# FanChart

Automatically tweet notifications of milestones for a fandom, pulled from multiple sources.


## Setup

You will need a database and the useful files in the `setup` folder..  Create the database, then import the `database.sql` file to create the tables and views.  While you're there, go ahead and put in the milestones.  You can use the spreadsheet and regular expression magic like I do, or you can just run `milestones.sql` for some good defaults.

After the database is set up, run FanChart.  Since it's the first time running, it will prompt for some connection information.  You should be able to figure out the connection string.  And for the APIs, check out Twitter, YouTube, and [evilarceus/Spotify-Playcount](https://github.com/evilarceus/Spotify-PlayCount).

Once the database is created and all of the connections are configured, items to be monitored can be managed via the "List" menu options.


## Scheduling?

Yea, it doesn't schedule automatically.  I plan to use an external tool for that, but I still need to add support for running on load before that will work.  For now, it just has to be run manually from the menu.

