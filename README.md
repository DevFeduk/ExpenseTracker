# Expense Tracker Console App

A console-based application for tracking personal expenses with persistent data storage. It allows users to manage their finances efficiently through a simple and intuitive command-line interface.

## ✨ Features

- **Full CRUD Operations**: Add, view, update, and delete expense records.
- **Data Persistence**: All data is automatically saved and loaded from a local `expenses.json` file.
- **Search & Filtering**: Find specific expenses by ID or filter them by category.
- **Input Validation**: Safe parsing of user input prevents application crashes on invalid data (e.g., letters instead of numbers).
- **Statistics**: View total spending and the number of recorded transactions.

## 🛠️ Tech Stack

- **Language**: C# 
- **Framework**: .NET
- **Libraries**: `System.Text.Json` (for serialization/deserialization), `LINQ` (for data querying)
- **Version Control**: Git & GitHub

## 🚀 How to Run

1. Clone the repository or download the source code as a ZIP archive.
2. Open the solution file (`ExpenseList.sln`) in **Visual Studio**.
3. Build the project and run it by pressing `F5` (or `Ctrl + F5` to run without debugging).
4. Follow the on-screen menu prompts to manage your expenses.

---
*Built as a practical project to demonstrate core C# concepts, file I/O operations, and clean architecture principles.*