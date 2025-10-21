# OrdersMini API

## Description
This project provides an API to manage **orders** using **PostgreSQL** and **MongoDB**. It implements **Clean Architecture** and is built using **.NET 8 (or 6)**. The API uses **EF Core** for interacting with **PostgreSQL** and **MongoDB.Driver** for storing order history.

## Setup Instructions

### 1. **Prerequisites**

Ensure that the following are installed on your machine:
- **Docker**: To run the database containers.
- **.NET SDK**: To run the API project.

### 2. **Run Docker Containers**

To run **PostgreSQL** and **MongoDB** using **Docker Compose**, execute the following command:

```bash
docker-compose up -d