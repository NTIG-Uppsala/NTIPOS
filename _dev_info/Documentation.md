# Documentation Point of Sale   

This is a wpf point of sale system for a kiosk that loads products from a database and generates receipts for every purchase. 

## Database

The program uses a local SQLite database to store products and receipts.  

The categories are stored in a table with the columns:
* id, int, auto generated primary key
* name, string, the name of the category
* color, string, the color of the category as a string

The products are stored in a table with the columns:
* id, int, auto generated primary key
* name, string, the product name shown to the user
* categoryId, int, foreign key from categories table, the id of the category for the product
* price, float, the product price
* amountSold, int, the amount of the product that has been sold in the system 
* category, string, the actual category retrieved from the categories table
* color, string, the color of the category retrieved from the categories table

The receipts are stored in a table with the colums:
* id, int, primary key and the receipt number
* time, string, formatted time for the creation of the receipt

The articles in the receipts are stored in a table with the columns:
* id, int, auto generated primary key
* name, string, name of the product
* price, float, the price of the product
* quantity, int, the ammount of the product bought for that transaction
* receiptId, int, foreign key from receipt table, the id of the receipt for the article

The program will automatically generate buttons in the GUI based on the products in the database.  
If there is no database when the program starts it will generate a database based on hard coded products.  
The database is stored in `AppData\Roaming\POS\databases`

### Test database

During tests, the test will generate a database that will take the place of the original database. 
The original database will be preserved and restored at the end of the test. 
The contents of the test database is hard coded in the test file. 

## Receipts

Every time a purchase is made the system will generate a receipt (both as an object in the program and as a pdf).
The pdf-receipts are stored in the users Document directory in under `POS\receipts`.  
The pdfs get a name containing their serial receipt number, the date and time they were printed and 
the date and time they were issued. 
Your branch is up to date with 'origin/main'.
Switched to branch 'main'
### The receipts view

You can see all of the receipts issued by the system in a view in the main window. There you can also generate a 
new copy of the receipt pdf that will be stored in the same directory as the original receipts.  
**OBS** When the software is restarted all of the receipts objects will be lost and the numbers will restart from 0. 
The pdfs will still be intact. 

---
[Go back to README](../README.md)
