# FanChart

Automatically tweet notifications of milestones for a fandom, pulled from multiple sources.


## Setup

You will need a database and the useful files in the `setup` folder..  Create the database, then import the `database.sql` file to create the tables and views.  While you're there, go ahead and put in the milestones.  You can use the spreadsheet and regular expression magic like I do, or you can just run `milestones.sql` for some good defaults.

After the database is set up, run FanChart.  Since it's the first time running, it will prompt for some connection information.  You should be able to figure out the connection string.  And for the APIs, check out Twitter, YouTube, and [evilarceus/sp-playcount-librespot](https://github.com/evilarceus/sp-playcount-librespot).

Once the database is created and all of the connections are configured, items to be monitored can be managed via the "List" menu options.


## Scheduling?

You can run at any time by clicking "Sync" then "Run Now" on the menu, but that's only good for debugging and special cases.  It's much more useful to have it all run on a schedule!

An external tool must be used for scheduling.  I use Windows' Task Scheduler.  Have it run FanChart with the "--run-now" flag, which will just run the sync and close without ever showing the GUI.

