# BankApplicationUsingAdo.Net
This is primarily a Bank Application in which it consists of Multiple Class Libraries which Includes Models, Api, Repositories, Services as Main Projects.,This Application provides Entire Banking Facility for a Customer and the Data Base Used is SQL Server
This has a Console Application aswell which works on few Modifications.
Currently this application works Fine with Api's.

# How Applicagtion Works:
This Application has A Hierarchy which Includes Positions like,
1.Reserve Bank Manager
2.Bank Head Manager
3.Branch Manager 
4.Staff
5.Customers

1.Once the ReserveBank Manager Account is Available/Created he/she can create a Bank(ex.sbi) with a Name and multiple Head Managers for that respective Bank(sbi).
2.Once the Bank and Bank Head Manager(s) are Created.,Bank Head Manager can create a Branch(nlg) with a Name and multiple Branch Managers for that respective Branch(nlg).
3.Once the Branch and Branch Managers are created.,Branch Manager can/have rights to create a staff Members and customer with their Accounts.
4.Staff can also create customers Accounts.
5.Once the customer is created he/she can do Banking like withdraw,transfer,check account Balance,Print passbook details. and deposit can be done only using Branch Manager/Staff .

*So there is a Dependency in the Application can be seen like below:
ReserveBankManager-->Bank(Bank Manager)-->Branch(Branch Manager)-->Manager-->Staff or Customer.
So without Bank there won't be a Branch.

# Application Built Purpose.
This Application is Built as per the Requirements below . so understanding the requirements and comparing the code with requirements can give a clear Picture. 

It is console application that Simulate a bank account which supports creation of account, closing an account, withdrawals, deposits, transfer funds 

Use cases to be taken into consideration while developing the application: 

Setup new Bank:

Set Default RTGS and IMPS charges for same bank  
RTGS-0%  
IMPS-5%  

Set Default RTGS and IMPS charges for other bank  
RTGS- 2%   
IMPS- 6%  

Add default accepted currency as INR. 
Create a page where user will get options to login as account holder or bank staff.
If User is bank staff then he should be able to perform following actions.
Create new account and give username and password to account holder.
Update / Delete account at any time.
Add new Accepted currency with exchange rate.

Add service charge for same bank account 
RTGS 
IMPS 

Add service charge for other bank account 
RTGS 
IMPS 

Can view account transaction history 
Can revert any transaction 
If User is account holder he should be able to perform following actions:

Deposit amount (any currency but bank will convert it to INR and will accept the deposit) 
Withdraw amount (INR only) 
Transfer funds (INR only) 
Can view his transaction history 

NOTE: 
Bank ID pattern - Starting 3 letters of bank name + current date 
Account ID pattern -  Starting 3 letters of account holder name + current date +time 
Transaction ID Pattern – “TXN” + bank ID + Account ID + current date  

# Requirements:
Visual studio/ Visual Studio Code Installed.
Install Dot Net Core Version 7.
PostMan or Swagger to check the Api Calls Data.

