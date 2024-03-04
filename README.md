# Movie Price Checker (Backend)

Movie Price Checker is a web application that allows users to compare movie prices from 2 diferent providers and display the cheapest price for a selected movie.  This is the backend of the web application.

## Problem Scenario

A provider has a set of web api to access a list of available movies as well as the details of each movie.  Just like in a real scenario, the APIs can have intermittent access issues.  For security purposes, the app should not include credentials and other information that may put the company at risks company. 

## Solution

The first issue that needed to be address is the intermitent response from the web api.  The app must be able to retry a few times incase the api does not respond.  A client policy was put in place to handle response from the web apis.  When a response becomes problematic, the hanlder will initiate another request after the failed attempt.  The number of attempts is dynamic and can be set from the class ClientPolicy but for this exercise it is set to just 5.  There were also 3 different strategies that was implemented and they are the following:

* ImmediateHttpRetry - This strategy is the simplest as it retries immediately right after a failed attempt. No time interval for each attempt.
* LinearHttpRetry - This strategy not only retries but will allow time interval between attempts.  The interval is dynamic and can be set on the ClientPolicy class. The time interval is consistent for every request which means if you set the time interval 2 seconds it will wait 2 seconds after each failed resquest attempt.
* ExponentialHttpRetry - This is the best of all the strategies but can also have its down side as it uses exponential time interval.  It means the time interval increases as the number of attempts increases.  This can be effective as we cannot predict when exactly the api becomes intermitent. Unfortunately it can take a lot longer and might encounter timeout issues.  This is the strategy used by default.

Credentials are stored as environment variables to keep sensitive information secure and were set in the server. 

There are 2 end points that are managed by this backend.  One for the list of aggregated movies from both providers.  The other fetches the details of the movie that has the cheapest price.

## Some Assumptions

The providers are assumed to be third party those the consistency of, for example, titles are not guarranteed. For example, one might have the title as "Phantom Manace" but the other will have the title as "The Phantom Manace". Both titles are the same movie and to identify that they are the same movie, LevenshteinDistance algorithm was used.  The same strategy was use to match the movies from the 2 different providers.  Initially, the implementation was done in the backend but that requires more than one requests from the front end. So to limit the number of requests, a lists was aggregated that contains list from both CinemaWorld and Filmworld. I decided to implement the movie matching using LevenshteinDistance in the frontend.