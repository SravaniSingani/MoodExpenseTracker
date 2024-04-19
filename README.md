# MoodExpenseTracker

## Overview

The Expense Tracking System is a comprehensive platform designed to help users monitor and manage their personal spending habits. By logging expenses, users can track their spending history, analyze patterns, and make informed financial decisions. The system offers a range of features to facilitate expense management, including the ability to categorize expenses, track spending on specific payment cards, and also based on their mood and weather.

## Collaborators
- Sravani Singani - [Expense Tracking System](https://github.com/SravaniSingani/ExpenseTrackerSystem2-SravaniSingani)
- Ameesa Bajwa - [Mood Diary Project](https://github.com/abajwa90/MoodDiaryProjectW2024)

## Technologies Used

##### Backend Technologies

- C# , ASP.NET, Entity Framework, Visual Studio

##### Frontend Technologies

- HTML, CSS, Bootstrap

##### Database Technology

- SQL Server

## Features

### Log Expenses

- Users can easily log new expenses, providing details such as the amount spent, date of purchase, category, payment method, their mood, weather and additional notes.
- The system ensures that all logged expenses are accurately recorded and stored securely in the database.

### Track Expenses by Card, Category, Mood and Weather

- Users can track their expenses by associating each expense with a specific payment card, mood and weather.
- This functionality enables users to monitor spending on individual cards and gain insights into their overall financial activity.

## CRUD Functionalities

### Card Entity

- **Create:** Users can add new payment cards to their account, providing details such as card name, and card type.
- **Read:** Users can view a list of their existing payment cards, along with associated expenses.
- **Update:** Users can update the details of their payment cards.
- **Delete:** Users can remove payment cards from their account, along with all associated expense records.

### Expense Entity

- **Create:** Users can add new expense entries, providing details such as the amount spent, date of purchase, category, payment method, their mood and weather, along with some additional description.
- **Read:** Users can view detailed information about each expense entry and any additional notes.
- **Update:** Users can edit and update existing expense entries to correct inaccuracies or provide additional information.
- **Delete:** Users can delete unwanted expense entries from their records, ensuring accurate expense tracking.

### Category Entity

- **Create:** Users can create new expense categories to organize their spending, such as groceries, bills, entertainment, etc.
- **Read:** Users can view a list of existing expense categories and the corresponding expenses associated with each category.
- **Update:** Users can update the details of existing expense categories, such as renaming a category.
- **Delete:** Users can delete expense categories from the system, along with all associated expense records.

### Mood Entity

- **Create:** Users can add new mood entries to their account, allowing them to track their emotional state at the time of each expense.
- **Read:** Users can view a list of their mood entries and the corresponding expenses associated with each mood.
- **Update:** Users can update the details of existing mood entries, such as changing the mood type.
- **Delete:** Users can delete mood entries from their account, along with all associated expense records.

### Weather Entity

- **Create:** Users can add new weather entries to their account, enabling them to track the weather conditions at the time of each expense.
- **Read:** Users can view a list of their weather entries and the corresponding expenses associated with each weather condition.
- **Update:** Users can update the details of existing weather entries, such as changing the weather type.
- **Delete:** Users can delete weather entries from their account, along with all associated expense records.

## Relational Database

### Entity Relation Diagram

The Expense Tracking System utilizes a relational database to store and manage data efficiently. The following entities are included in the database schema:

- **Expense Entity:** Represents individual expense entries logged by users.
- **Card Entity:** Represents payment cards associated with user accounts.
- **Category Entity:** Represents categories for organizing expenses.
- **Mood Entity:** Represents predefined mood scale entries associated with each expense.
- **Weather Entity:** Represents weather entries associated with each expense.

### Table Relationships

- **Expense Entity:** Each expense entry is associated with one payment card, one expense category, one mood entry, and one weather entry.
- **Card Entity:** Each payment card can have multiple associated expense entries.
- **Category Entity:** Each expense category can have multiple associated expense entries.
- **Mood Entity:** Each mood entry can have multiple associated expense entries.
- **Weather Entity:** Each weather entry can have multiple associated expense entries.

## Additional Features

In addition to the core functionalities, the Expense Tracking System offers several additional features to enhance user experience and data analysis:

### Authentication

- The system provides authentication mechanisms to ensure secure access to user data.
- Users are required to log in with valid credentials to access their expense records and perform actions such as logging expenses, updating entries, and deleting records.

### Search Functionality

- Users can easily search for specific expense entries using the built-in search functionality.
- The search feature allows users to find expense entries based on various criteria such as expense, category, mood, weather, payment card, etc.

### Expense Total Sum

- The system automatically calculates the total sum of expenses for each entity value, such as card, category, mood, and weather.
- Users can view the total expense amount associated with each payment card, expense category, mood type, and weather condition.
- Expense sum calculations provide users with insights into their overall spending across different categories, moods, and weather conditions.

## Future Improvements
- **Date Filtering:** Users will have the option to filter their expenses by date range to view spending trends over specific time periods.
- **Visualize Expenses:** Users will be able to visualize their expenses using bar plots or charts, providing a visual representation of their spending patterns and trends over time.

