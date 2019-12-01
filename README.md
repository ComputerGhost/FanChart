# FanChart

Automatically tweet notifications of milestones for a fandom, pulled from multiple sources.

## Setup

When FanChart first runs, it prompts for all of the needed information.  This information can be obtained from obtained from Twitter, YouTube, and [evilarceus/Spotify-Playcount](https://github.com/evilarceus/Spotify-PlayCount).

Once all of the API connections are configured, resources to be monitored can be managed via the "List" menu options.

## Scheduling?

Yea, it doesn't schedule automatically.  I plan to use an external tool for that, but I still need to add support for running on load before that will work.  For now, it just has to be run manually from the menu.

## Future Updates

Leave a feature request if you have an idea!  I am doing this in my free time, so I have to ensure that my limited development time is spent on something wanted or needed.  :)

Currently on my list are:


### High Load Handling

At some point, I need to code the software to handle high load.  Currently, it should error out and needs re-run until completion.  I won't bother with this until I start getting errors under normal use.

### More Sources to Monitor

I will add more sources to monitor, prioritized per demand.

### Start on Run without GUI

Eventually, I want to run this software from an event scheduler.  This won't work right now, but I'll add this ability once I'm comfortable with a "set it and forget it" approach.  Right now, I just need to see it running correctly.

