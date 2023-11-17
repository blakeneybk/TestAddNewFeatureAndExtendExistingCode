# Litmus Interview Project Template
This was a fictitious test to showcase my ability to work with existing api code, extend existing functionality, and add new functionality to support some anticipated front end functionality. The actual changes I made for the test can be found in an incomplete pull request from dev => main branch. The goal was not to refactor technical debt of which there is quite a bit in this existing app design. Any technical debt would noted in standup and worked into the backlog before addressing because hidden work is bad and rabbit holing down major refactors is bad team manners.

# Test Instructions
# Introduction

In order to get a sense for your programming abilities, we ask you to complete this sample project. It shouldn't take more than a couple of hours to complete. While this specific problem is rather small, we’d like to see an example of your best work. So please treat it as something you’d deliver to a production environment. This exercise is “open book”. Feel free to use Google, StackOverflow, or whatever other resources you might need to do research.

Below you will find two wireframes of a fictional web application. Each wireframe represents a page and includes a short description of the desired functionality of the page. We would like you to evaluate the wireframes and existing web API application. Update the existing application to build out all the endpoints required to support the features requested in the wireframe.

## Web API Consumer

The Litmus Infrastructure team primarily builds and operates services consumed by the Applications team’s Ruby on Rails application. For this project, you can assume a similar architecture. The API project will be the source of truth and interop layer a UI application will consume via RESTful JSON calls. You will need to ensure the UI layer has the data it needs to render the given wireframes, but you do not need to worry about the implementation of the rendering itself.

## Existing Sakila and Sakila.API Projects

In the solution, you will find the Sakila, Sakila.API, and Sakila.Test projects. We expect you to update and add to the existing codebase. Please be prepared to walk us through your decisions for extending existing endpoints vs creating new ones and how those decisions impact maintaining backwards compatibility with existing consumers.

## Sakila Database

The Sakila Database is a sample dataset used to demonstrate what a standard application schema and dataset may look like. It is available on multiple platforms, but originated on MySql. In this code project, we have used a Sqlite port to remove the need for having a MySql server. The first time you open a database application, the Sqlite Db will automatically be downloaded to your local machine.

## Project Scope

### Outstanding Rentals

We have been asked to add a store filter to the existing Outstanding Rentals page. This will include adding a dropdown with a list of stores. When a store is selected, the list of customers with outstanding rentals will only show customers with outstanding rentals for a specific store. The Count column will only include counts for the selected store.
![image](https://github.com/blakeneybk/TestAddNewFeatureAndExtendExistingCode/assets/15051699/9e06e75f-9a08-4e51-8b40-707af9ca6272)

### Customer Details Page

This will be a new page in the application which gives some details for a specific customer. It is safe to assume the frontend web app will have the Customer Id integer when making calls to load this page. The Outstanding Rentals section will list all the current movies a customer has not returned yet. This section includes a button to allow a user to mark a rental as returned.
![image](https://github.com/blakeneybk/TestAddNewFeatureAndExtendExistingCode/assets/15051699/a33ead60-4c02-41cf-804c-3f626d4ce23e)

## Building
We use use [CAKE](https://cakebuild.net/) at Litmus to keep our build process in source control and make this process available on our local machine.

To build this solution, you can run `dotnet cake` on the root directory of this project.


