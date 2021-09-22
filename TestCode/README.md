# README #
This system uses ASP.Net Core Odata in order to produce JSON Files required by the code examination.

I chose this for the following reasons.

1. The development time is easily cut down in comparison with other frameworks.
2. It supports IQueryable wherein the client side could choose the filters for the query (basically enabling client side to do DML statements while adhering closely to the models and definitions defined on the context <see CodeTestContext under Data/CodeTestContext>).
3. It is clean enough to understand very quickly. Each of the 3 controllers has its simple CRUD. If you need to see the child
navigation properties of a model, there is no need for a LINQ include statement or another get function such as 
"GetAccountWithPayments". Just add $expand and you should be good.
4. It supports versioning


The connection string used is specified on the application/json /application.development.json.
The only requirement for the application to connect to the database is for these two files to have a valid connection string with the name "CodeTestConnection".

I will be adding unto this repository the sql file for the reproduction of the tables and data. you may add more data in if you like as the post and patch for each controller works.


1.Main Task - Account and its payments sorted by payment date and the account status
https://{Base_URL}/odata/Accounts?$expand=Payments($orderBy=PaymentDate desc),Status -- All Accounts
https://{Base_URL}/odata/Accounts(Id)?$expand=Payments($orderBy=PaymentDate desc),Status -- Single Account

Controllers Exposed
1.Accounts - to Manage Account / Account Status
2.Payments - to Manage Payment / Payment Status
3.Statuses - to Manage Statuses


Please see Images for Sample result sets
Its under TestCode/CodeTestImages


--Comments
1. The Created and Modified fields are a personal touch. always has been doing things like this as with role based systems,
these fields are supposed to be auto filled/updated (no need to include in the body unlike the one in the post sample)
upon patch and posts.
2. This was done on my personal machine however, to test out the repository, ill be waiting for my accenture laptop then clone
it there before submitting this.
3. The IsDeleted field is also a personal touch. its supposed to be a field for global query filters.
4. I forgot to add the field "AdditionalComment" early on so its not included in the examples. as you can see in the last commit its only the readme file and Payment Model that has been changed.


Deployment Instructions
1. Run SQL File Script on your Database
2. Change Connection string on appsettings.json / appsettings.development.json (if using debug mode)
3. Compile and Run




--Additional Inputs
1. Added simple cookie based authorization - all apis should return 404 when there is no cookie present (this is cause cookies return to the main page if its not found)
when trying to access APIs, please use https://{Base_url}/Auth/Login and https://{Base_url}/Auth/Logout to test this one.
2. Added simple unit tests for the task specified.