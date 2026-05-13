# Contact Management System (N-Tier Architecture Practice)

A desktop application built using **C#** and **.NET Framework** to demonstrate the implementation of **N-Tier Architecture (4-Tiers)** and database interaction using **ADO.NET**.

## 🎯 Purpose of the Project
This repository is a practical educational project focused on "Separation of Concerns". It demonstrates how to build a scalable application by decoupling the User Interface, Business Logic, and Data Access layers.

---

## 🏗 Architecture Overview (4-Tier)

The project is structured into four distinct layers to ensure maintainability and clean code:

1.  **Presentation Layer (WinForms App)**: The UI layer where users interact with the system. It handles user inputs and displays data.
2.  **Business Layer (Class Library)**: Contains all business logic, validations, and orchestration of CRUD operations.
3.  **Data Access Layer (Class Library)**: Responsible for direct communication with the **SQL Server** database using **ADO.NET**.
4.  **Modules (Class Library)**: Contains the data models (POCO classes) representing the entities (`Contact`, `Country`). This layer is shared across all other layers.

### 🔗 Dependencies Flow
- **Presentation Layer** ➡️ depends on *Business Layer* & *Modules*.
- **Business Layer** ➡️ depends on *Data Access Layer* & *Modules*.
- **Data Access Layer** ➡️ depends on *Modules*.
- **Modules** ➡️ independent (Zero dependencies).

---

## 🛠 Technologies & Tools
- **Language**: C#
- **Framework**: .NET Framework
- **IDE**: Visual Studio 2022
- **Database**: SQL Server
- **Data Access**: ADO.NET (Command, Connection, DataReader, etc.)
- **UI**: Windows Forms (WinForms)

---

## 🗄 Database Schema

The system manages two main tables with the following structure:

### 1. Contacts Table
| Column | Type |
| :--- | :--- |
| `ContactID` | Primary Key (Identity) |
| `FirstName` | nvarchar(50) |
| `LastName` | nvarchar(50) |
| `Email` | nvarchar(50) |
| `Phone` | nvarchar(20) |
| `Address` | nvarchar(250) |
| `DateOfBirth` | Date |
| `ImagePath` | nvarchar(500) |
| `CountryID` | Foreign Key (References Countries) |

### 2. Countries Table
| Column | Type |
| :--- | :--- |
| `CountryID` | Primary Key (Identity) |
| `CountryName` | nvarchar(50) |
| `Code` | nvarchar(10) |
| `PhoneCode` | nvarchar(10) |

---

## 🚀 Key Features (CRUD Operations)
The application allows users to perform full CRUD operations on contacts:
- ✅ **Add**: Create new contact records with image support.
- ✅ **View**: Display all contacts in a modern list view.
- ✅ **Search**: Find specific contacts by various criteria.
- ✅ **Update**: Modify existing contact information.
- ✅ **Delete**: Safely remove records from the database.
- ✅ **Validation**: Ensuring data integrity before it reaches the database.

---
## ⚠️ Important Note
A script has been added to a folder called `Database` containing commands for creating the database and entering data for those who want to try the project.

---

## 👤 About the Author

**Ebrahim Hasan**
A passionate Software Developer with a solid foundation in Computer Science. My journey started with learning the basics of the C++ language (Functional programming), then mastering **Algorithms, Object Oriented Programming and Data Structures** in C++, then learning C#, .NET and SQL Server Database, which paved the way for building complex systems using **C# and .NET**.

* **Expertise:** Desktop Applications (WinForms), SQL Server Database Design, and Logic Automation.
* **Key Projects:** Driver License Management System, Code Generator Tool.
* **Current Goal:** Transitioning into Web Full-stack Development (C#/.NET Backend).

---
📫 **Connect with me:**
* 📧 **Email:** [ebrahim.hasan.dev@gmail.com](mailto:ebrahim.hasan.dev@gmail.com)
* 💼 **LinkedIn:** [Your Profile Name](https://linkedin.com/in/ebrahim-hasan-dev)
