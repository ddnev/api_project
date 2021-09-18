## Introduction
The purpose of this project is to test an applicant's coding and software design abilities. To that end, you may use online documentation and resources just as you would in your day-to-day work. This project is designed to be completed within a few hours.

## Project Story
Our clients need to build a high-performance web user interface that contains an area/geography lookup with autocomplete functionality. You have been provided with `areas.sqlite3` in the project directory that contains a hierarchical representation of US geographies starting at a root (United States) and branching out to a census tract level across the US, with intermediate entries at State, FIPS (County), ZIP codes, MSA (metropolitan service areas), and multiple rollup paths within (Which may require stripping out duplicate entries or somehow differentiating them in your response).

The schema of this database is as follows:

    CREATE TABLE Areas(
      name TEXT,
      abbr TEXT,
      display_id TEXT,
      child INT,
      parent INT,
      aggregation_path INT,
      level INT
    );

It should be noted that `display_id`, `abbr` and `name ` are the columns with data most interesting to users. `child` is a column with an internal ID.

Your task is to write an endpoint for the provided skeleton API that allows the user to pass in a search predicate, queries the SQLite database, and returns a reasonable array of possible results to the user (that include both the internal ID and the name at a minimum) using the following endpoint:

    GET http://localhost:5000/areas?q=[predicate]

A proper API should include good logging, and this API is set up to allow dependency injection of an `ILogger<AreasController>` object (which for the sake of this project just logs to the console).

## Ideas for showing off your skills
* A well-maintained API has useful and comprehensive logging. Feel free to bring in a library for logging and make it into an API that would be easy to troubleshoot. We use Serilog in house.
* Your queries are going to query the database. SQL injection?
* Asynchronous operations
* Middleware for logging? Metric telemetry? CorrelationId enrichment?
* TLS/SSL is pretty standard these days. A self-signed cert is fine for demo purposes.

## Other requirements
* Project should run on Ubuntu 20.04 (Feel free to use Docker)
* You are welcome (and will need to) take a dependency on whatever nuget packages are necessary to accomplish the task, but be prepared to explain what they are doing.
* Provide clear instructions for how to compile/run your code, along with any required packages that are not installed by default on Debian Buster.
* If you find these requirements to be ambiguous, go with your best guess and document your decision.

## Submission Process

-   Create a repository on Github with your code and email us a link.
-   The repository should include a README.md that clearly describes how to compile and run your code.

## Review Criteria

In the code review we'll cover:

-   Simplicity (how complicated is your design?)
-   Clarity (how easy is it to understand  _what_  your code is doing and  _why_  it's doing it?)
-   Alternatives (what other approaches did you consider? Why didn't you go with one of them?)
-   Performance (the database is not long, and your queries should not be either)
